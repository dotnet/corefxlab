// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO.Pipelines.Networking.Windows.RIO.Internal;
using System.IO.Pipelines.Networking.Windows.RIO.Internal.Winsock;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Networking.Windows.RIO
{
    public sealed class RioTcpConnection : IPipeConnection
    {
        private const RioSendFlags MessagePart = RioSendFlags.Defer | RioSendFlags.DontNotify;
        private const RioSendFlags MessageEnd = RioSendFlags.None;

        private const long PartialSendCorrelation = -1;
        private const long RestartSendCorrelations = -2;

        private readonly static Task _completedTask = Task.FromResult(0);

        private readonly long _connectionId;
        private readonly IntPtr _socket;
        private readonly IntPtr _requestQueue;
        private readonly RegisteredIO _rio;
        private readonly RioThread _rioThread;
        private bool _disposedValue;

        private long _previousSendCorrelation = RestartSendCorrelations;

        private readonly IPipe _input;
        private readonly IPipe _output;

        private readonly SemaphoreSlim _outgoingSends = new SemaphoreSlim(RioTcpServer.MaxWritesPerSocket);
        private readonly SemaphoreSlim _previousSendsComplete = new SemaphoreSlim(1);

        private Task _sendTask;

        private PreservedBuffer _sendingBuffer;
        private WritableBuffer _buffer;

        internal RioTcpConnection(IntPtr socket, long connectionId, IntPtr requestQueue, RioThread rioThread, RegisteredIO rio)
        {
            _socket = socket;
            _connectionId = connectionId;
            _rio = rio;
            _rioThread = rioThread;

            _input = rioThread.PipeFactory.Create();
            _output = rioThread.PipeFactory.Create();

            _requestQueue = requestQueue;

            rioThread.AddConnection(connectionId, this);

            ProcessReceives();
            _sendTask = ProcessSends();
        }

        public IPipeReader Input => _input.Reader;
        public IPipeWriter Output => _output.Writer;

        private void ProcessReceives()
        {
            _buffer = _input.Writer.Alloc(2048);
            var receiveBufferSeg = _rioThread.GetSegmentFromMemory(_buffer.Buffer);

            if (!_rio.RioReceive(_requestQueue, ref receiveBufferSeg, 1, RioReceiveFlags.None, 0))
            {
                ThrowError(ErrorType.Receive);
            }
        }

        private async Task ProcessSends()
        {
            while (true)
            {
                var result = await _output.Reader.ReadAsync();
                var buffer = result.Buffer;

                if (buffer.IsEmpty && result.IsCompleted)
                {
                    break;
                }

                var enumerator = buffer.GetEnumerator();

                if (enumerator.MoveNext())
                {
                    var current = enumerator.Current;

                    while (enumerator.MoveNext())
                    {
                        var next = enumerator.Current;

                        await SendAsync(current, endOfMessage: false);
                        current = next;
                    }

                    await PreviousSendingComplete;

                    _sendingBuffer = buffer.Preserve();

                    await SendAsync(current, endOfMessage: true);
                }

                _output.Reader.Advance(buffer.End);
            }

            _output.Reader.Complete();
        }

        private Task SendAsync(Buffer<byte> memory, bool endOfMessage)
        {
            if (!IsReadyToSend)
            {
                return SendAsyncAwaited(memory, endOfMessage);
            }

            var flushSends = endOfMessage || MaxOutstandingSendsReached;

            Send(_rioThread.GetSegmentFromMemory(memory), flushSends);

            if (flushSends && !endOfMessage)
            {
                return AwaitReadyToSend();
            }

            return _completedTask;
        }

        private async Task SendAsyncAwaited(Buffer<byte> memory, bool endOfMessage)
        {
            await ReadyToSend;

            var flushSends = endOfMessage || MaxOutstandingSendsReached;

            Send(_rioThread.GetSegmentFromMemory(memory), flushSends);

            if (flushSends && !endOfMessage)
            {
                await ReadyToSend;
            }
        }

        private async Task AwaitReadyToSend()
        {
            await ReadyToSend;
        }

        private void Send(RioBufferSegment segment, bool flushSends)
        {
            var sendCorrelation = flushSends ? CompleteSendCorrelation() : PartialSendCorrelation;
            var sendFlags = flushSends ? MessageEnd : MessagePart;

            if (!_rio.Send(_requestQueue, ref segment, 1, sendFlags, sendCorrelation))
            {
                ThrowError(ErrorType.Send);
            }
        }

        private Task PreviousSendingComplete => _previousSendsComplete.WaitAsync();

        private Task ReadyToSend => _outgoingSends.WaitAsync();

        private bool IsReadyToSend => _outgoingSends.Wait(0);

        private bool MaxOutstandingSendsReached => _outgoingSends.CurrentCount == 0;

        private void CompleteSend() => _outgoingSends.Release();

        private void CompletePreviousSending()
        {
            _sendingBuffer.Dispose();
            _previousSendsComplete.Release();
        }

        private long CompleteSendCorrelation()
        {
            var sendCorrelation = _previousSendCorrelation;
            if (sendCorrelation == int.MinValue)
            {
                _previousSendCorrelation = RestartSendCorrelations;
                return RestartSendCorrelations;
            }

            _previousSendCorrelation = sendCorrelation - 1;
            return sendCorrelation - 1;
        }

        public void SendComplete(long sendCorrelation)
        {
            CompleteSend();

            if (sendCorrelation == _previousSendCorrelation)
            {
                CompletePreviousSending();
            }
        }

        public void ReceiveBeginComplete(uint bytesTransferred)
        {
            if (bytesTransferred == 0)
            {
                _input.Writer.Complete();
            }
            else
            {
                _buffer.Advance((int)bytesTransferred);
                _buffer.Commit();

                ProcessReceives();
            }
        }

        public void ReceiveEndComplete()
        {
            _buffer.FlushAsync();
        }

        private static void ThrowError(ErrorType type)
        {
            var errorNo = RioImports.WSAGetLastError();

            string errorMessage;
            switch (errorNo)
            {
                case 10014: // WSAEFAULT
                    errorMessage = $"{type} failed: WSAEFAULT - The system detected an invalid pointer address in attempting to use a pointer argument in a call.";
                    break;
                case 10022: // WSAEINVAL
                    errorMessage = $"{type} failed: WSAEINVAL -  the SocketQueue parameter is not valid, the Flags parameter contains an value not valid for a send operation, or the integrity of the completion queue has been compromised.";
                    break;
                case 10055: // WSAENOBUFS
                    errorMessage = $"{type} failed: WSAENOBUFS - Sufficient memory could not be allocated, the I/O completion queue associated with the SocketQueue parameter is full.";
                    break;
                case 997: // WSA_IO_PENDING
                    errorMessage = $"{type} failed? WSA_IO_PENDING - The operation has been successfully initiated and the completion will be queued at a later time.";
                    break;
                case 995: // WSA_OPERATION_ABORTED
                    errorMessage = $"{type} failed. WSA_OPERATION_ABORTED - The operation has been canceled while the receive operation was pending.";
                    break;
                default:
                    errorMessage = $"{type} failed:  WSA error code {errorNo}";
                    break;
            }

            throw new InvalidOperationException(errorMessage);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                _rioThread.RemoveConnection(_connectionId);
                RioImports.closesocket(_socket);

                _disposedValue = true;
            }
        }

        ~RioTcpConnection()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private enum ErrorType
        {
            Send,
            Receive
        }
    }
}
