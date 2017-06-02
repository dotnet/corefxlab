﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.IO;
using System.Runtime.InteropServices;

#if BIT64
    using nuint = System.UInt64;
#else
using nuint = System.UInt32;
#endif 
namespace System.IO.Compression
{
    public partial class BrotliStream : Stream
    {

        private const int DefaultBufferSize = 64 * 1024;
        private int BufferSize;
        public Stream _stream;
        private CompressionMode _mode;
        private nuint TotalOut { get; }
        private IntPtr AvailOut;
        private IntPtr NextOut = IntPtr.Zero;
        private IntPtr AvailIn = IntPtr.Zero;
        private IntPtr NextIn = IntPtr.Zero;
        private IntPtr BufferIn { get; set; }
        private IntPtr BufferOut { get; set; }
        private bool LeaveOpen;
        private int totalWrote;
        IntPtr Dict;//TODO
        private int _readOffset = 0;
        Decoder _decoder;
        Encoder _encoder;
        public BrotliStream(Stream baseStream, CompressionMode mode, bool leaveOpen, int BuffSize, uint windowSize, uint quality) : this(baseStream, mode, leaveOpen, BuffSize) {
            if (_mode == CompressionMode.Decompress) throw new System.IO.IOException("quality and windowsize is ambitious for Decompress mode");
            else
            {
                _encoder.SetQuality(quality);
                _encoder.SetWindow(windowSize);
            }
        }
        public BrotliStream(Stream baseStream, CompressionMode mode, bool leaveOpen, int BuffSize)
        {
            if (baseStream == null) throw new ArgumentNullException("baseStream");
            _mode = mode;
            _stream = baseStream;
            LeaveOpen = leaveOpen;
            if (_mode == CompressionMode.Compress)
            {
                _encoder = new Encoder();
                _encoder.SetQuality();
                _encoder.SetWindow();
            }
            else
            {
                _decoder = new Decoder();
            }
            BufferSize = BuffSize;
            BufferIn = Marshal.AllocHGlobal(BufferSize);
            BufferOut = Marshal.AllocHGlobal(BufferSize);
            NextIn = BufferIn;
            NextOut = BufferOut;
            AvailOut = new IntPtr((uint)BuffSize);
        }
        public BrotliStream(Stream baseStream, CompressionMode mode, bool leaveOpen) : this(baseStream, mode, leaveOpen, DefaultBufferSize) { }

        public BrotliStream(Stream baseStream, CompressionMode mode) : this(baseStream, mode, false) { }

        public override bool CanRead
        {
            get
            {
                if (_stream == null)
                {
                    return false;
                }
                return (_mode == CompressionMode.Decompress && _stream.CanRead);
            }
        }

        public override bool CanWrite
        {
            get
            {
                if (_stream == null)
                {
                    return false;
                }
                return (_mode == CompressionMode.Compress && _stream.CanWrite);
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }
        public override bool CanSeek => false;

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        protected virtual void FlushEncoder(Boolean finished)
        {
            if (_encoder.State == IntPtr.Zero) return;
            if (BrotliNative.BrotliEncoderIsFinished(_encoder.State)) return;
            BrotliNative.BrotliEncoderOperation op = finished ? BrotliNative.BrotliEncoderOperation.Finish : BrotliNative.BrotliEncoderOperation.Flush;
            UInt32 totalOut = 0;
            while (true)
            {
                if (!BrotliNative.BrotliEncoderCompressStream(_encoder.State, op, ref AvailIn, ref NextIn, ref AvailOut, ref NextOut, out totalOut)) throw new Exception();// unable encode
                var extraData = (nuint)AvailOut != BufferSize;
                if (extraData)
                {
                    var bytesWrote = (int)(BufferSize - (nuint)AvailOut);
                    Byte[] buf = new Byte[bytesWrote];
                    Marshal.Copy(BufferOut, buf, 0, bytesWrote);
                    _stream.Write(buf, 0, bytesWrote);
                    AvailOut = (IntPtr)BufferSize;
                    NextOut = BufferOut;
                }
                if (BrotliNative.BrotliEncoderIsFinished(_encoder.State)) break;
                if (!extraData) break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _stream != null && _mode == CompressionMode.Compress)
            {
                FlushEncoder(true);
            }
            try
            {
                if (BufferIn != IntPtr.Zero) Marshal.FreeHGlobal(BufferIn);
                if (BufferOut != IntPtr.Zero) Marshal.FreeHGlobal(BufferOut);
                BufferIn = IntPtr.Zero;
                BufferOut = IntPtr.Zero;
                if (disposing && !LeaveOpen) _stream?.Dispose();
            }
            finally
            {
                _stream = null;
                try
                {
                    _decoder?.Dispose();
                    _encoder?.Dispose();
                }
                finally
                {
                    _encoder = null;
                    _decoder = null;
                }
                base.Dispose(disposing);
            }
            
        }     
    
        public override void Flush()
        {
            EnsureNotDisposed();
            if (_mode == CompressionMode.Compress)
            {
                FlushEncoder(false);
            }
        }

        private void ValidateParameters(byte[] array, int offset, int count)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (array.Length - offset < count)
                throw new ArgumentException("InvalidArgument","OffsetCount");
        }

        private void EnsureDecompressionMode()
        {
            if (_mode != CompressionMode.Decompress)
                throw new System.InvalidOperationException("Wrong stream mode. Expect: Decompress");
        }

        private void EnsureNotDisposed()
        {
            if (_stream == null)
                throw new ObjectDisposedException("Stream");
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            EnsureDecompressionMode();
            ValidateParameters(buffer, offset, count);
            EnsureNotDisposed();
            
            int bytesRead = (int)(_decoder.BufferStream.Length - _readOffset);
            uint totalCount = 0;
            Boolean endOfStream = false;
            Boolean errorDetected = false;
            Byte[] buf = new Byte[BufferSize];
            while (bytesRead < count)
            {
                while (true)
                {
                    if (_decoder.LastDecoderResult == BrotliNative.BrotliDecoderResult.NeedsMoreInput)
                    {
                        AvailIn = (IntPtr)_stream.Read(buf, 0, (int)BufferSize);
                        NextIn = BufferIn;
                        if ((int)AvailIn <= 0)
                        {
                            endOfStream = true;
                            break;
                        }
                        Marshal.Copy(buf, 0, BufferIn, (int)AvailIn);
                    }
                    else if (_decoder.LastDecoderResult == BrotliNative.BrotliDecoderResult.NeedsMoreOutput)
                    {
                        Marshal.Copy(BufferOut, buf, 0, BufferSize);
                        _decoder.BufferStream.Write(buf, 0, BufferSize);
                        bytesRead += BufferSize;
                        AvailOut = new IntPtr((uint)BufferSize);
                        NextOut = BufferOut;
                    }
                    else
                    {
                        //Error or OK
                        endOfStream = true;
                        break;
                    }
                    _decoder.LastDecoderResult = BrotliNative.BrotliDecoderDecompressStream(_decoder.State, ref AvailIn, ref NextIn,
                        ref AvailOut, ref NextOut, out totalCount);
                    if (bytesRead >= count) break;
                }
                if (endOfStream && !BrotliNative.BrotliDecoderIsFinished(_decoder.State))
                {
                    errorDetected = true;
                }
                if (_decoder.LastDecoderResult == BrotliNative.BrotliDecoderResult.Error || errorDetected)
                {
                    var error = BrotliNative.BrotliDecoderGetErrorCode(_decoder.State);
                    var text = BrotliNative.BrotliDecoderErrorString(error);
                    throw new System.IO.IOException(text+"- unable to decode stream");
                }
                if (endOfStream && !BrotliNative.BrotliDecoderIsFinished(_decoder.State) && _decoder.LastDecoderResult == BrotliNative.BrotliDecoderResult.NeedsMoreInput)
                {
                    throw new System.IO.IOException("Bad finish");
                }
                if (endOfStream && NextOut != BufferOut)
                {
                    int remainBytes = (int)(NextOut.ToInt64() - BufferOut.ToInt64());
                    bytesRead += remainBytes;
                    Marshal.Copy(BufferOut, buf, 0, remainBytes);
                    _decoder.BufferStream.Write(buf, 0, remainBytes);
                    NextOut = BufferOut;
                }
                if (endOfStream) break;
            }
            if (_decoder.BufferStream.Length - _readOffset >= count || endOfStream)
            {
                _decoder.BufferStream.Seek(_readOffset, SeekOrigin.Begin);
                var bytesToRead = (int)(_decoder.BufferStream.Length - _readOffset);
                if (bytesToRead > count) bytesToRead = count;
                _decoder.BufferStream.Read(buffer, offset, bytesToRead);
                _decoder.RemoveBytes(_readOffset + bytesToRead);
                _readOffset = 0;
                return bytesToRead;
            }
            return 0;
        }
       
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }
        private void EnsureCompressionMode()
        {
            if (_mode != CompressionMode.Compress)
                throw new System.InvalidOperationException("Wrong stream mode. Expect: Compress");
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            EnsureCompressionMode();
            ValidateParameters(buffer, offset, count);
            EnsureNotDisposed();
            if (_mode != CompressionMode.Compress) 
            totalWrote += count;
            nuint totalOut = 0;
            int bytesRemain = count;
            int currentOffset = offset;
            int copyLen;
            while (bytesRemain > 0)
            {
                copyLen = bytesRemain > BufferSize ? BufferSize : bytesRemain;
                Marshal.Copy(buffer, currentOffset, BufferIn, copyLen);
                bytesRemain -= copyLen;
                currentOffset += copyLen;
                AvailIn = (IntPtr)copyLen;
                NextIn = BufferIn;
                while ((int)AvailIn > 0)
                {
                    if (!BrotliNative.BrotliEncoderCompressStream(_encoder.State, BrotliNative.BrotliEncoderOperation.Process, ref AvailIn, ref NextIn, ref AvailOut,
                        ref NextOut, out totalOut)) throw new System.IO.IOException("Unable compress stream"); 
                    if ((nuint)AvailOut != BufferSize)
                    {
                        var bytesWrote = (int)(BufferSize - (nuint)AvailOut);
                        Byte[] buf = new Byte[bytesWrote];
                        Marshal.Copy(BufferOut, buf, 0, bytesWrote);
                        _stream.Write(buf, 0, bytesWrote);
                        AvailOut = new IntPtr((uint)BufferSize);
                        NextOut = BufferOut;
                    }
                }
                if (BrotliNative.BrotliEncoderIsFinished(_encoder.State)) break;
            }
        }
    }
    
}
