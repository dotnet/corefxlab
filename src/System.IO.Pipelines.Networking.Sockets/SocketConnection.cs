// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO.Pipelines.Networking.Sockets.Internal;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Networking.Sockets
{
    /// <summary>
    /// Represents an <see cref="IPipeConnection"/> implementation using the async Socket API
    /// </summary>
    public class SocketConnection : IPipeConnection
    {
        private static readonly EventHandler<SocketAsyncEventArgs> _asyncCompleted = OnAsyncCompleted;

        private static readonly Queue<SocketAsyncEventArgs> _argsPool = new Queue<SocketAsyncEventArgs>();

        private static readonly MicroBufferPool _smallBuffers;

        internal static int SmallBuffersInUse = _smallBuffers?.InUse ?? 0;

        const int SmallBufferSize = 8;

        // track the state of which strategy to use; need to use a known-safe
        // strategy until we can decide which to use (by observing behavior)
        private static BufferStyle _bufferStyle;
        private static bool _seenReceiveZeroWithAvailable, _seenReceiveZeroWithEOF;

        private static readonly byte[] _zeroLengthBuffer = new byte[0];


        private readonly bool _ownsFactory;
        private PipeFactory _factory;
        private IPipe _input, _output;
        private Socket _socket;
        private Task _receiveTask;
        private Task _sendTask;
        private volatile bool _stopping;
        private bool _disposed;

        static SocketConnection()
        {

            // validated styles for known OSes
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // zero-length receive works fine
                _bufferStyle = BufferStyle.UseZeroLengthBuffer;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // zero-length receive is unreliable
                _bufferStyle = BufferStyle.UseSmallBuffer;
            }
            else
            {
                // default to "figure it out based on what happens"
                _bufferStyle = BufferStyle.Unknown;
            }

            if (_bufferStyle != BufferStyle.UseZeroLengthBuffer)
            {
                // we're going to need to use small buffers for awaiting input
                _smallBuffers = new MicroBufferPool(SmallBufferSize, ushort.MaxValue);
            }
        }

        internal SocketConnection(Socket socket, PipeFactory factory)
        {
            socket.NoDelay = true;
            _socket = socket;
            if (factory == null)
            {
                _ownsFactory = true;
                factory = new PipeFactory();
            }
            _factory = factory;

            // TODO: Make this configurable
            // Dispatch to avoid deadlocks
            _input = PipeFactory.Create(new PipeOptions
            {
                ReaderScheduler = TaskRunScheduler.Default,
                WriterScheduler = TaskRunScheduler.Default
            });

            _output = PipeFactory.Create(new PipeOptions
            {
                ReaderScheduler = TaskRunScheduler.Default,
                WriterScheduler = TaskRunScheduler.Default
            });

            _receiveTask = ReceiveFromSocketAndPushToWriterAsync();
            _sendTask = ReadFromReaderAndWriteToSocketAsync();
        }

        /// <summary>
        /// Provides access to data received from the socket
        /// </summary>
        public IPipeReader Input => _input.Reader;

        /// <summary>
        /// Provides access to write data to the socket
        /// </summary>
        public IPipeWriter Output => _output.Writer;

        private PipeFactory PipeFactory => _factory;

        private Socket Socket => _socket;

        /// <summary>
        /// Begins an asynchronous connect operation to the designated endpoint
        /// </summary>
        /// <param name="endPoint">The endpoint to which to connect</param>
        /// <param name="factory">Optionally allows the underlying <see cref="PipeFactory"/> (and hence memory pool) to be specified; if one is not provided, a <see cref="PipeFactory"/> will be instantiated and owned by the connection</param>
        public static Task<SocketConnection> ConnectAsync(IPEndPoint endPoint, PipeFactory factory = null)
        {
            var args = new SocketAsyncEventArgs();
            args.RemoteEndPoint = endPoint;
            args.Completed += _asyncCompleted;
            var tcs = new TaskCompletionSource<SocketConnection>(factory);
            args.UserToken = tcs;
            if (!Socket.ConnectAsync(SocketType.Stream, ProtocolType.Tcp, args))
            {
                OnConnect(args); // completed sync - usually means failure
            }
            return tcs.Task;
        }
        /// <summary>
        /// Releases all resources owned by the connection
        /// </summary>
        public void Dispose() => Dispose(true);

        internal static SocketAsyncEventArgs GetOrCreateSocketAsyncEventArgs()
        {
            SocketAsyncEventArgs args = null;
            lock (_argsPool)
            {
                if (_argsPool.Count != 0)
                {
                    args = _argsPool.Dequeue();
                }
            }
            if (args == null)
            {
                args = new SocketAsyncEventArgs();
                args.Completed += _asyncCompleted; // only for new, otherwise multi-fire
            }
            if (args.UserToken is Signal)
            {
                ((Signal)args.UserToken).Reset();
            }
            else
            {
                args.UserToken = new Signal();
            }
            return args;
        }

        internal static void RecycleSocketAsyncEventArgs(SocketAsyncEventArgs args)
        {
            if (args != null)
            {
                args.SetBuffer(null, 0, 0); // make sure we don't keep a slab alive
                lock (_argsPool)
                {
                    if (_argsPool.Count < 2048)
                    {
                        _argsPool.Enqueue(args);
                    }
                }
            }
        }

        public async Task DisposeAsync()
        {
            if (!_disposed)
            {
                _disposed = true;
                _stopping = true;
                _output.Reader.CancelPendingRead();

                await Task.WhenAll(_sendTask, _receiveTask);

                _output.Writer.Complete();
                _input.Reader.Complete();

                _socket?.Dispose();
                _socket = null;
                if (_ownsFactory) { _factory?.Dispose(); }
                _factory = null;
            }
        }

        /// <summary>
        /// Releases all resources owned by the connection
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.SuppressFinalize(this);

                DisposeAsync().GetAwaiter().GetResult();
            }
        }

        private static void OnAsyncCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                switch (e.LastOperation)
                {
                    case SocketAsyncOperation.Connect:
                        OnConnect(e);
                        break;

                    case SocketAsyncOperation.Send:
                    case SocketAsyncOperation.Receive:
                        ReleasePending(e);
                        break;
                }
            }
            catch { }
        }

        private static void OnConnect(SocketAsyncEventArgs e)
        {
            var tcs = (TaskCompletionSource<SocketConnection>)e.UserToken;
            try
            {
                if (e.SocketError == SocketError.Success)
                {
                    tcs.TrySetResult(new SocketConnection(e.ConnectSocket, (PipeFactory)tcs.Task.AsyncState));
                }
                else
                {
                    tcs.TrySetException(new SocketException((int)e.SocketError));
                }
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
        }

        private static void ReleasePending(SocketAsyncEventArgs e)
        {
            var pending = (Signal)e.UserToken;
            pending.Set();
        }

        private enum BufferStyle
        {
            Unknown,
            UseZeroLengthBuffer,
            UseSmallBuffer
        }

        private async Task ReceiveFromSocketAndPushToWriterAsync()
        {
            SocketAsyncEventArgs args = null;
            try
            {
                // wait for someone to be interested in data before we
                // start allocating buffers and probing the socket
                args = GetOrCreateSocketAsyncEventArgs();
                while (!_stopping)
                {
                    bool haveWriteBuffer = false;
                    WritableBuffer buffer = default(WritableBuffer);
                    var initialSegment = default(ArraySegment<byte>);

                    try
                    {

                        int bytesFromInitialDataBuffer = 0;

                        if (Socket.Available == 0)
                        {
                            // now, this gets a bit messy unfortunately, because support for the ideal option
                            // (zero-length reads) is platform dependent
                            switch (_bufferStyle)
                            {
                                case BufferStyle.Unknown:
                                    try
                                    {
                                        initialSegment = await ReceiveInitialDataUnknownStrategyAsync(args);
                                    }
                                    catch
                                    {
                                        initialSegment = default(ArraySegment<byte>);
                                    }
                                    if (initialSegment.Array == null)
                                    {
                                        continue; // redo from start
                                    }
                                    break;
                                case BufferStyle.UseZeroLengthBuffer:
                                    // if we already have a buffer, use that (but: zero count); otherwise use a shared
                                    // zero-length; this avoids constantly changing the buffer that the args use, which
                                    // avoids some overheads
                                    args.SetBuffer(args.Buffer ?? _zeroLengthBuffer, 0, 0);

                                    // await async for the io work to be completed
                                    await Socket.ReceiveSignalAsync(args);
                                    break;
                                case BufferStyle.UseSmallBuffer:
                                    // We need  to do a speculative receive with a *cheap* buffer while we wait for input; it would be *nice* if
                                    // we could do a zero-length receive, but this is not supported equally on all platforms (fine on Windows, but
                                    // linux hates it). The key aim here is to make sure that we don't tie up an entire block from the memory pool
                                    // waiting for input on a socket; fine for 1 socket, not so fine for 100,000 sockets

                                    // do a short receive while we wait (async) for data
                                    initialSegment = LeaseSmallBuffer();
                                    args.SetBuffer(initialSegment.Array, initialSegment.Offset, initialSegment.Count);

                                    // await async for the io work to be completed
                                    await Socket.ReceiveSignalAsync(args);
                                    break;
                            }
                            if (args.SocketError != SocketError.Success)
                            {
                                throw new SocketException((int)args.SocketError);
                            }

                            // note we can't check BytesTransferred <= 0, as we always
                            // expect 0; but if we returned, we expect data to be
                            // buffered *on the socket*, else EOF
                            if ((bytesFromInitialDataBuffer = args.BytesTransferred) <= 0)
                            {
                                if (ReferenceEquals(initialSegment.Array, _zeroLengthBuffer))
                                {
                                    // sentinel value that means we should just
                                    // consume sync (we expect there to be data)
                                    initialSegment = default(ArraySegment<byte>);
                                }
                                else
                                {
                                    // socket reported EOF
                                    RecycleSmallBuffer(ref initialSegment);
                                }
                                if (Socket.Available == 0)
                                {
                                    // yup, definitely an EOF
                                    break;
                                }
                            }
                        }

                        // note that we will try to coalesce things here to reduce the number of flushes; we
                        // certainly want to coalesce the initial buffer (from the speculative receive) with the initial
                        // data, but we probably don't want to buffer indefinitely; for now, it will buffer up to 4 pages
                        // before flushing (entirely arbitrarily) - might want to make this configurable later
                        buffer = _input.Writer.Alloc(SmallBufferSize * 2);
                        haveWriteBuffer = true;

                        const int FlushInputEveryBytes = 4 * MemoryPool.MaxPooledBlockLength;

                        if (initialSegment.Array != null)
                        {
                            // need to account for anything that we got in the speculative receive
                            if (bytesFromInitialDataBuffer != 0)
                            {
                                buffer.Write(new Span<byte>(initialSegment.Array, initialSegment.Offset, bytesFromInitialDataBuffer));
                            }
                            // make the small buffer available to other consumers
                            RecycleSmallBuffer(ref initialSegment);
                        }

                        bool isEOF = false;
                        while (Socket.Available != 0 && buffer.BytesWritten < FlushInputEveryBytes)
                        {
                            buffer.Ensure(); // ask for *something*, then use whatever is available (usually much much more)
                            SetBuffer(buffer.Buffer, args);
                            // await async for the io work to be completed
                            await Socket.ReceiveSignalAsync(args);

                            // either way, need to validate
                            if (args.SocketError != SocketError.Success)
                            {
                                throw new SocketException((int)args.SocketError);
                            }
                            int len = args.BytesTransferred;
                            if (len <= 0)
                            {
                                // socket reported EOF
                                isEOF = true;
                                break;
                            }

                            // record what data we filled into the buffer
                            buffer.Advance(len);
                        }
                        if (isEOF)
                        {
                            break;
                        }
                    }
                    finally
                    {
                        RecycleSmallBuffer(ref initialSegment);
                        if (haveWriteBuffer)
                        {
                            _stopping = (await buffer.FlushAsync()).IsCompleted;
                        }
                    }
                }
                _input.Writer.Complete();
            }
            catch (Exception ex)
            {
                // don't trust signal after an error; someone else could
                // still have it and invoke Set
                if (args != null)
                {
                    args.UserToken = null;
                }
                _input?.Writer.Complete(ex);
            }
            finally
            {
                try
                {
                    Socket.Shutdown(SocketShutdown.Receive);
                }
                catch { }

                RecycleSocketAsyncEventArgs(args);
            }
        }


        private static ArraySegment<byte> LeaseSmallBuffer()
        {
            ArraySegment<byte> result;
            if (!_smallBuffers.TryTake(out result))
            {
                // use a throw-away buffer as a fallback
                result = new ArraySegment<byte>(new byte[_smallBuffers.BytesPerItem]);
            }
            return result;
        }
        private void RecycleSmallBuffer(ref ArraySegment<byte> buffer)
        {
            if (buffer.Array != null)
            {
                _smallBuffers?.Recycle(buffer);
            }
            buffer = default(ArraySegment<byte>);
        }

        /// returns null if the caller should redo from start; returns
        /// a non-null result to preocess the data
        private async Task<ArraySegment<byte>> ReceiveInitialDataUnknownStrategyAsync(SocketAsyncEventArgs args)
        {
            // to prove that it works OK, we need (after a read):
            // - have seen return 0 and Available > 0
            // - have reen return <= 0 and Available == 0 and is true EOF
            //
            // if we've seen both, we can switch to the simpler approach;
            // until then, if we just see return 0 and Available > 0, well...
            // we're happy
            //
            // note: if we see return 0 and available == 0 and not EOF,
            // then we know that zero-length receive is not supported

            try
            {
                args.SetBuffer(_zeroLengthBuffer, 0, 0);
                // we'll do a receive and see what happens
                await Socket.ReceiveSignalAsync(args);
            }
            catch
            {
                // well, it didn't like that... switch to small buffers
                _bufferStyle = BufferStyle.UseSmallBuffer;
                return default(ArraySegment<byte>);
            }
            if (args.SocketError != SocketError.Success)
            {   // let the calling code explode
                return new ArraySegment<byte>(_zeroLengthBuffer);
            }

            if (Socket.Available > 0)
            {
                _seenReceiveZeroWithAvailable = true;
                if (_seenReceiveZeroWithEOF)
                {
                    _bufferStyle = BufferStyle.UseZeroLengthBuffer;
                }
                // we'll let the calling method pull the data out
                return new ArraySegment<byte>(_zeroLengthBuffer);
            }

            // so now we need to detect if this is a genuine EOF; if it isn't,
            // that isn't conclusive, because could just be timing; but if it is: great
            var buffer = LeaseSmallBuffer();
            try
            {
                args.SetBuffer(buffer.Array, buffer.Offset, buffer.Count);
                // we'll do a receive and see what happens
                await Socket.ReceiveSignalAsync(args);

                if (args.SocketError != SocketError.Success)
                {   // we can't actually conclude  anything
                    RecycleSmallBuffer(ref buffer);
                    throw new SocketException((int)args.SocketError);
                }
                if (args.BytesTransferred <= 0)
                {
                    RecycleSmallBuffer(ref buffer);
                    _seenReceiveZeroWithEOF = true;
                    if (_seenReceiveZeroWithAvailable)
                    {
                        _bufferStyle = BufferStyle.UseZeroLengthBuffer;
                    }
                    // we'll let the calling method shut everything down
                    return new ArraySegment<byte>(_zeroLengthBuffer);
                }

                // otherwise, we got something that looked like an EOF from receive,
                // but which wasn't really; we'll have to do things the hard way :(
                _bufferStyle = BufferStyle.UseSmallBuffer;
                return buffer;
            }
            catch
            {
                // already recycled (or not) correctly in the success cases
                RecycleSmallBuffer(ref buffer);
                throw;
            }
        }

        private async Task ReadFromReaderAndWriteToSocketAsync()
        {
            SocketAsyncEventArgs args = null;
            try
            {
                args = GetOrCreateSocketAsyncEventArgs();

                while (!_stopping)
                {
                    var result = await _output.Reader.ReadAsync();
                    var buffer = result.Buffer;
                    try
                    {
                        if (buffer.IsEmpty && result.IsCompleted)
                        {
                            break;
                        }

                        foreach (var memory in buffer)
                        {
                            int remaining = memory.Length;
                            while (remaining != 0)
                            {
                                SetBuffer(memory, args, memory.Length - remaining);

                                // await async for the io work to be completed
                                await Socket.SendSignalAsync(args);

                                // either way, need to validate
                                if (args.SocketError != SocketError.Success)
                                {
                                    throw new SocketException((int)args.SocketError);
                                }

                                remaining -= args.BytesTransferred;
                            }
                        }
                    }
                    finally
                    {
                        _output.Reader.Advance(buffer.End);
                    }
                }
                _output.Reader.Complete();
            }
            catch (Exception ex)
            {
                // don't trust signal after an error; someone else could
                // still have it and invoke Set
                if (args != null)
                {
                    args.UserToken = null;
                }
                _output?.Reader.Complete(ex);
            }
            finally
            {
                try // we're not going to be sending anything else
                {
                    Socket.Shutdown(SocketShutdown.Send);
                }
                catch { }
                RecycleSocketAsyncEventArgs(args);
            }
        }

        // unsafe+async not good friends
        private unsafe void SetBuffer(Buffer<byte> memory, SocketAsyncEventArgs args, int ignore = 0)
        {
            ArraySegment<byte> segment;
            if (!memory.TryGetArray(out segment))
            {
                throw new InvalidOperationException("Memory is not backed by an array; oops!");
            }
            args.SetBuffer(segment.Array, segment.Offset + ignore, segment.Count - ignore);
        }

        private void Shutdown()
        {
            Socket?.Shutdown(SocketShutdown.Both);
        }
    }
}