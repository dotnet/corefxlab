using System.IO.Pipelines.Networking.Tls.Internal.Sspi;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Networking.Tls
{
    internal unsafe class SecureConnectionContext : ISecureContext
    {
        private readonly SecurityContext _securityContext;
        private static readonly Task CachedTask = Task.FromResult(0);
        private SSPIHandle _contextPointer;
        private int _headerSize = 5; //5 is the minimum (1 for frame type, 2 for version, 2 for frame size)
        private int _trailerSize = 16;
        private bool _readyToSend;
        private ApplicationProtocols.ProtocolIds _negotiatedProtocol;

        public SecureConnectionContext(SecurityContext securityContext)
        {
            _securityContext = securityContext;
        }

        public bool ReadyToSend => _readyToSend;
        public ApplicationProtocols.ProtocolIds NegotiatedProtocol => _negotiatedProtocol;

        public int HeaderSize
        {
            get { return _headerSize; }
            set { _headerSize = value; }
        }

        public int TrailerSize
        {
            get { return _trailerSize; }
            set { _trailerSize = value; }
        }

        public SSPIHandle ContextHandle => _contextPointer;
        public bool IsServer => _securityContext.IsServer;
        public CipherInfo CipherInfo => Interop.GetCipherInfo(_contextPointer);

        /// <summary>
        /// Without a payload from the client the server will just return straight away.
        /// </summary>
        /// <param name="writeBuffer"></param>
        public Task ProcessContextMessageAsync(IPipelineWriter writeBuffer)
        {
            return ProcessContextMessageAsync(default(ReadableBuffer), writeBuffer);
        }

        /// <summary>
        /// Processes the tokens or cipher change messages from a client and can then return server messages for the client
        /// </summary>
        /// <param name="readBuffer"></param>
        /// <param name="writePipeline"></param>
        public Task ProcessContextMessageAsync(ReadableBuffer readBuffer, IPipelineWriter writePipeline)
        {
            var handleForAllocation = default(GCHandle);
            try
            {
                var output = new SecurityBufferDescriptor(2);
                var outputBuff = stackalloc SecurityBuffer[2];
                outputBuff[0] = new SecurityBuffer(null, 0, SecurityBufferType.Token);
                outputBuff[1] = new SecurityBuffer(null, 0, SecurityBufferType.Alert);
                output.UnmanagedPointer = outputBuff;

                var handle = _securityContext.CredentialsHandle;
                SSPIHandle localhandle = _contextPointer;
                void* contextptr;
                void* newContextptr;
                if (_contextPointer.handleHi == IntPtr.Zero && _contextPointer.handleLo == IntPtr.Zero)
                {
                    contextptr = null;
                    newContextptr = &localhandle;
                }
                else
                {
                    contextptr = &localhandle;
                    newContextptr = null;
                }
                var unusedAttributes = default(ContextFlags);
                SecurityBufferDescriptor* pointerToDescriptor = null;

                if (readBuffer.Length > 0)
                {
                    var input = new SecurityBufferDescriptor(2);
                    var inputBuff = stackalloc SecurityBuffer[2];
                    inputBuff[0].size = readBuffer.Length;
                    inputBuff[0].type = SecurityBufferType.Token;

                    if (readBuffer.IsSingleSpan)
                    {
                        void* arrayPointer;
                        readBuffer.First.TryGetPointer(out arrayPointer);
                        inputBuff[0].tokenPointer = arrayPointer;
                    }
                    else
                    {
                        if (readBuffer.Length <= SecurityContext.MaxStackAllocSize)
                        {
                            var tempBuffer = stackalloc byte[readBuffer.Length];
                            var tmpSpan = new Span<byte>(tempBuffer, readBuffer.Length);
                            readBuffer.CopyTo(tmpSpan);
                            inputBuff[0].tokenPointer = tempBuffer;
                        }
                        else
                        {
                            //We have to allocate... sorry
                            var tempBuffer = readBuffer.ToArray();
                            handleForAllocation = GCHandle.Alloc(tempBuffer, GCHandleType.Pinned);
                            inputBuff[0].tokenPointer = (void*)handleForAllocation.AddrOfPinnedObject();
                        }
                    }
                    //If we have APLN extensions to send use the last buffer
                    if (_securityContext.AplnRequired)
                    {
                        inputBuff[1] = _securityContext.AplnBuffer;
                    }
                    input.UnmanagedPointer = inputBuff;
                    pointerToDescriptor = &input;
                }
                else
                {
                    //Only build an input buffer if we have to send APLN extensions
                    if (_securityContext.AplnRequired)
                    {
                        var input = new SecurityBufferDescriptor(1);
                        var inputBuff = stackalloc SecurityBuffer[1];
                        inputBuff[0] = _securityContext.AplnBuffer;
                        input.UnmanagedPointer = inputBuff;
                        pointerToDescriptor = &input;
                    }
                }
                //We call accept security context for a server (as it is initiated by the client) and for the client we call Initialize
                long timestamp = 0;
                SecurityStatus errorCode;
                if (_securityContext.IsServer)
                {
                    errorCode = Interop.AcceptSecurityContext(credentialHandle: ref handle, inContextPtr: contextptr,
                        inputBuffer: pointerToDescriptor, inFlags: SecurityContext.ServerRequiredFlags,
                        endianness: Endianness.Native
                        , newContextPtr: newContextptr, outputBuffer: &output, attributes: ref unusedAttributes,
                        timeStamp: out timestamp);
                }
                else
                {
                    errorCode = Interop.InitializeSecurityContextW(credentialHandle: ref handle,
                        inContextPtr: contextptr, targetName: _securityContext.HostName
                        , inFlags: SecurityContext.RequiredFlags | ContextFlags.InitManualCredValidation, reservedI: 0,
                        endianness: Endianness.Native, inputBuffer: pointerToDescriptor
                        , reservedII: 0, newContextPtr: newContextptr, outputBuffer: &output,
                        attributes: ref unusedAttributes, timeStamp: out timestamp);
                }

                _contextPointer = localhandle;
                if (errorCode == SecurityStatus.ContinueNeeded || errorCode == SecurityStatus.OK)
                {
                    if (errorCode == SecurityStatus.OK)
                    {
                        ContextStreamSizes ss;
                        //We have a valid context so lets query it for the size of the header and trailer
                        Interop.QueryContextAttributesW(ref _contextPointer, ContextAttribute.StreamSizes, out ss);
                        _headerSize = ss.header;
                        _trailerSize = ss.trailer;
                        //If we needed APLN this should now be set
                        if (_securityContext.AplnRequired)
                        {
                            _negotiatedProtocol = ApplicationProtocols.FindNegotiatedProtocol(_contextPointer);
                        }
                        _readyToSend = true;
                    }
                    if (outputBuff[0].size > 0)
                    {
                        var writeBuffer = writePipeline.Alloc();
                        writeBuffer.Write(new Span<byte>(outputBuff[0].tokenPointer, outputBuff[0].size));
                        Interop.FreeContextBuffer((IntPtr)outputBuff[0].tokenPointer);
                        return writeBuffer.FlushAsync();
                    }
                    return CachedTask;
                }
                throw new InvalidOperationException($"An error occured trying to negoiate a session {errorCode}");
            }
            finally
            {
                if (handleForAllocation.IsAllocated)
                {
                    handleForAllocation.Free();
                }
            }
        }

        /// <summary>
        /// Encrypts by allocating a single block on the out buffer to contain the message, plus the trailer and header. Then uses SSPI to write directly onto the output
        /// </summary>
        /// <param name="unencrypted">The secure context that holds the information about the current connection</param>
        /// <param name="encryptedDataPipeline">The buffer to write the encryption results to</param>
        public Task EncryptAsync(ReadableBuffer unencrypted, IPipelineWriter encryptedDataPipeline)
        {
            var encryptedData = encryptedDataPipeline.Alloc();
            encryptedData.Ensure(_trailerSize + _headerSize + unencrypted.Length);
            void* outBufferPointer;
            encryptedData.Memory.TryGetPointer(out outBufferPointer);

            //Copy the unencrypted across to the encrypted pipeline, it will be updated in place and destroyed
            unencrypted.CopyTo(encryptedData.Memory.Slice(_headerSize, unencrypted.Length).Span);

            var securityBuff = stackalloc SecurityBuffer[4];
            SecurityBufferDescriptor sdcInOut = new SecurityBufferDescriptor(4);
            securityBuff[0] = new SecurityBuffer(outBufferPointer, _headerSize, SecurityBufferType.Header);
            securityBuff[1] = new SecurityBuffer((byte*)outBufferPointer + _headerSize, unencrypted.Length,
                SecurityBufferType.Data);
            securityBuff[2] = new SecurityBuffer((byte*)securityBuff[1].tokenPointer + unencrypted.Length, _trailerSize,
                SecurityBufferType.Trailer);

            sdcInOut.UnmanagedPointer = securityBuff;

            var handle = _contextPointer;
            var result = Interop.EncryptMessage(ref handle, 0, sdcInOut, 0);
            if (result == 0)
            {
                var totalSize = securityBuff[0].size + securityBuff[1].size + securityBuff[2].size;
                encryptedData.Advance(totalSize);
                return encryptedData.FlushAsync();
            }
            //Zero out the output buffer before throwing the exception to stop any data being sent in the clear
            //By a misbehaving underlying pipeline we will allocate here simply because it is a rare occurance and not
            //worth risking a stack overflow over
            var memoryToClear = new Span<byte>(outBufferPointer, _headerSize + _trailerSize + unencrypted.Length);
            var empty = new Span<byte>(new byte[_headerSize + _trailerSize + unencrypted.Length]);
            empty.CopyTo(memoryToClear);
            encryptedData.Commit();
            throw new InvalidOperationException($"There was an issue encrypting the data {result}");
        }

        /// <summary>
        /// Decrypts the data that comes from a readable buffer. If it is in a single span it will be decrypted in place. Next we will attempt to use the stack. If it is
        /// too big for that we will allocate.
        /// </summary>
        /// <param name="encryptedData">The buffer that will provide the bytes to be encrypted</param>
        /// <param name="decryptedDataPipeline">The buffer to write the encryption results to</param>
        /// <returns></returns>
        public unsafe Task DecryptAsync(ReadableBuffer encryptedData, IPipelineWriter decryptedDataPipeline)
        {
            GCHandle handle = default(GCHandle);
            try
            {
                void* pointer;
                if (encryptedData.IsSingleSpan)
                {
                    encryptedData.First.TryGetPointer(out pointer);
                }
                else
                {
                    if (encryptedData.Length <= SecurityContext.MaxStackAllocSize)
                    {
                        var tmpBuffer = stackalloc byte[encryptedData.Length];
                        encryptedData.CopyTo(new Span<byte>(tmpBuffer, encryptedData.Length));
                        pointer = tmpBuffer;
                    }
                    else
                    {
                        var tmpBuffer = new byte[encryptedData.Length];
                        encryptedData.CopyTo(tmpBuffer);
                        handle = GCHandle.Alloc(tmpBuffer, GCHandleType.Pinned);
                        pointer = (void*)handle.AddrOfPinnedObject();
                    }
                }
                int offset = 0;
                int count = encryptedData.Length;

                DecryptMessage(pointer, ref offset, ref count);
                if (encryptedData.IsSingleSpan)
                {
                    //The data was always in a single continous buffer so we can just append the decrypted data to the output
                    var decryptedData = decryptedDataPipeline.Alloc();
                    encryptedData = encryptedData.Slice(offset, count);
                    decryptedData.Append(encryptedData);
                    return decryptedData.FlushAsync();
                }
                else
                {
                    //The data was multispan so we had to copy it out into either a stack pointer or an allocated and pinned array
                    //so now we need to copy it out to the output
                    var decryptedData = decryptedDataPipeline.Alloc();
                    decryptedData.Write(new Span<byte>(pointer, encryptedData.Length).Slice(offset, count));
                    return decryptedData.FlushAsync();
                }
            }
            finally
            {
                if (handle.IsAllocated)
                {
                    handle.Free();
                }
            }
        }

        private void DecryptMessage(void* buffer, ref int offset, ref int count)
        {
            var securityBuff = stackalloc SecurityBuffer[4];
            SecurityBufferDescriptor sdcInOut = new SecurityBufferDescriptor(4);
            securityBuff[0] = new SecurityBuffer(buffer, count, SecurityBufferType.Data);
            sdcInOut.UnmanagedPointer = securityBuff;

            var errorCode = Interop.DecryptMessage(ref _contextPointer, sdcInOut, 0, null);

            if (errorCode != 0)
            {
                throw new InvalidOperationException($"There was an error decrypting the data {errorCode.ToString()}");
            }
            for (var i = 0; i < 4; i++)
            {
                if (securityBuff[i].type != SecurityBufferType.Data)
                {
                    continue;
                }
                //we have found the data lets find the offset
                offset = (int)((byte*)securityBuff[i].tokenPointer - (byte*)buffer);
                if (offset > (count - 1))
                {
                    throw new OverflowException();
                }
                count = securityBuff[i].size;
                return;
            }
            throw new InvalidOperationException($"There was an error decrypting the data {errorCode}");
        }

        public void Dispose()
        {
            if (_contextPointer.IsValid)
            {
                Interop.DeleteSecurityContext(ref _contextPointer);
                _contextPointer = default(SSPIHandle);
            }
            GC.SuppressFinalize(this);
        }

        ~SecureConnectionContext()
        {
            Dispose();
        }
    }
}
