using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Networking.Tls
{
    public class SecureChannel<T> : ISecurePipeline where T : ISecureContext
    {
        private IPipelineConnection _lowerChannel;
        private PipelineReaderWriter _outputChannel;
        private PipelineReaderWriter _inputChannel;
        private readonly T _contextToDispose;
        private TaskCompletionSource<ApplicationProtocols.ProtocolIds> _handShakeCompleted;

        internal SecureChannel(IPipelineConnection inChannel, PipelineFactory channelFactory, T secureContext)
        {
            _contextToDispose = secureContext;
            _lowerChannel = inChannel;
            _inputChannel = channelFactory.Create();
            _outputChannel = channelFactory.Create();
            StartWriting();
        }

        public IPipelineReader Input => _outputChannel;
        public IPipelineWriter Output => _inputChannel;
        public CipherInfo CipherInfo => _contextToDispose.CipherInfo;

        public Task<ApplicationProtocols.ProtocolIds> HandShakeAsync()
        {
            return _handShakeCompleted?.Task ?? DoHandShake();
        }

        private async Task<ApplicationProtocols.ProtocolIds> DoHandShake()
        {
            _handShakeCompleted = new TaskCompletionSource<ApplicationProtocols.ProtocolIds>();
            if (!_contextToDispose.IsServer)
            {
                //If it is a client we need to start by sending a client hello straight away
                await _contextToDispose.ProcessContextMessageAsync(_lowerChannel.Output);
            }
            try
            {
                while (true)
                {
                    var result = await _lowerChannel.Input.ReadAsync();
                    var buffer = result.Buffer;
                    try
                    {
                        if (buffer.IsEmpty && result.IsCompleted)
                        {
                            new InvalidOperationException("Connection closed before the handshake completed");
                        }
                        ReadableBuffer messageBuffer;
                        TlsFrameType frameType;
                        while (TryGetFrameType(ref buffer, out messageBuffer, out frameType))
                        {
                            if (frameType != TlsFrameType.Handshake && frameType != TlsFrameType.ChangeCipherSpec)
                            {
                                throw new InvalidOperationException("Received a token that was invalid during the handshake");
                            }

                            await _contextToDispose.ProcessContextMessageAsync(messageBuffer, _lowerChannel.Output);

                            if (_contextToDispose.ReadyToSend)
                            {
                                _handShakeCompleted.SetResult(_contextToDispose.NegotiatedProtocol);
                                return await _handShakeCompleted.Task;
                            }
                        }
                    }
                    finally
                    {
                        _lowerChannel.Input.Advance(buffer.Start, buffer.End);
                    }
                }
            }
            catch (Exception ex)
            {
                Dispose();
                _handShakeCompleted = new TaskCompletionSource<ApplicationProtocols.ProtocolIds>();
                _handShakeCompleted.SetException(ex);
                return await _handShakeCompleted.Task;
            }
            finally
            {
                StartReading();
            }

        }

        private async void StartReading()
        {
            try
            {
                while (true)
                {
                    var result = await _lowerChannel.Input.ReadAsync();
                    var buffer = result.Buffer;
                    try
                    {
                        if (buffer.IsEmpty && result.IsCompleted)
                        {
                            break;
                        }
                        ReadableBuffer messageBuffer;
                        TlsFrameType frameType;
                        while (TryGetFrameType(ref buffer, out messageBuffer, out frameType))
                        {
                            //We have app data or tokens at this point so slice out the message
                            //If we have app data, we will slice it out and process it
                            if (frameType == TlsFrameType.AppData)
                            {
                                await _contextToDispose.DecryptAsync(messageBuffer, _outputChannel);
                            }
                            else
                            {
                                throw new InvalidOperationException("Invalid frame type during the connection was sent");
                            }
                        }
                    }
                    finally
                    {
                        _lowerChannel.Input.Advance(buffer.Start, buffer.End);
                    }
                }
            }
            finally
            {
                try
                {
                    //Close down the lower channel
                    _lowerChannel.Input.Complete();
                    _lowerChannel.Output.Complete();
                    //Tell the upper consumer that we aren't sending any more data
                    _outputChannel.CompleteWriter();
                    _outputChannel.CompleteReader();
                    _inputChannel.CompleteReader();
                    _inputChannel.CompleteWriter();
                }
                catch {  /*nom nom */ }
            }
        }

        private async void StartWriting()
        {
            await HandShakeAsync();
            var maxBlockSize = (SecurityContext.BlockSize - _contextToDispose.HeaderSize - _contextToDispose.TrailerSize);
            try
            {
                while (true)
                {
                    var result = await _inputChannel.ReadAsync();

                    var buffer = result.Buffer;
                    if (buffer.IsEmpty && result.IsCompleted)
                    {
                        break;
                    }
                    try
                    {
                        while (buffer.Length > 0)
                        {
                            ReadableBuffer messageBuffer;
                            if (buffer.Length <= maxBlockSize)
                            {
                                messageBuffer = buffer;
                                buffer = buffer.Slice(buffer.End);
                            }
                            else
                            {
                                messageBuffer = buffer.Slice(0, maxBlockSize);
                                buffer = buffer.Slice(maxBlockSize);
                            }
                            await _contextToDispose.EncryptAsync(messageBuffer, _lowerChannel.Output);
                        }
                    }
                    finally
                    {
                        _inputChannel.Advance(buffer.End);
                    }
                }
            }
            finally
            {
                try
                {
                    //Close down the lower channel
                    _lowerChannel.Input.Complete();
                    _lowerChannel.Output.Complete();
                    //Tell the upper consumer that we aren't sending any more data
                    _outputChannel.CompleteWriter();
                    _outputChannel.CompleteReader();
                    _inputChannel.CompleteReader();
                    _inputChannel.CompleteWriter();
                }
                catch {  /*nom nom */ }
            }
        }

        /// <summary>
        /// Checks to see if we have enough data for a frame and if the basic frame header is valid.
        /// </summary>
        /// <param name="buffer">The input buffer, it will be returned with the frame sliced out if there is a complete frame found</param>
        /// <param name="messageBuffer">If a frame is found this contains that frame</param>
        /// <returns>The status of the check for frame</returns>
        internal static bool TryGetFrameType(ref ReadableBuffer buffer, out ReadableBuffer messageBuffer, out TlsFrameType frameType)
        {
            frameType = TlsFrameType.Incomplete;
            //Need at least 5 bytes to be useful
            if (buffer.Length < 5)
            {
                messageBuffer = default(ReadableBuffer);
                return false;
            }
            frameType = (TlsFrameType)buffer.ReadBigEndian<byte>();

            //Check it's a valid frametype for what we are expecting
            if (frameType != TlsFrameType.AppData && frameType != TlsFrameType.Alert
                && frameType != TlsFrameType.ChangeCipherSpec && frameType != TlsFrameType.Handshake)
            {
                throw new FormatException($"The tls frame type was invalid value was {frameType}");
            }
            //now we get the version
            var version = buffer.Slice(1).ReadBigEndian<ushort>();

            if (version < 0x300 || version >= 0x500)
            {
                messageBuffer = default(ReadableBuffer);
                throw new FormatException($"The tls frame type was invalid due to the version value was {frameType}");
            }
            var length = buffer.Slice(3).ReadBigEndian<ushort>();
            // If we have a full frame slice it out and move the original buffer forward
            if (buffer.Length >= (length + 5))
            {
                messageBuffer = buffer.Slice(0, length + 5);
                buffer = buffer.Slice(messageBuffer.End);
                return true;
            }
            messageBuffer = default(ReadableBuffer);
            return false;
        }

        public void Dispose()
        {
            _lowerChannel.Dispose();
            _contextToDispose?.Dispose();
        }
    }
}
