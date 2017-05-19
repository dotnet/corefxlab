// Licensed to the .NET Foundation under one or more agreements.
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
        private const int MinWindowBits = 10;
        private const int MaxWindowBits = 24;
        private const int DefaultBufferSize = 64 * 1024;
        private const int MinQuality = 0;
        private const int MaxQuality = 11;
        private MemoryStream _intermediateStream = new MemoryStream();
        private int BufferSize;
        private Stream _stream;
        private CompressionMode _mode;
        private nuint TotalOut { get; }
        //private nuint AvailOut;
        private uint Quality;
        private uint LGWin;
        private IntPtr AvailOut;
        private IntPtr NextOut = IntPtr.Zero;
        private nuint TotalIn;
        private IntPtr AvailIn = IntPtr.Zero;
        private IntPtr NextIn = IntPtr.Zero;
        private IntPtr State = IntPtr.Zero;
        private IntPtr BufferIn { get; set; }
        private IntPtr BufferOut { get; set; }
        private BrotliNative.BrotliDecoderResult LastDecoderResult = BrotliNative.BrotliDecoderResult.NeedsMoreInput;
        private bool LeaveOpen;
        private int totalWrote;
        IntPtr Dict;
        private int _readOffset = 0;

        public BrotliStream(Stream baseStream, CompressionMode mode, bool leaveOpen, uint windowSize, uint quality, int BuffSize)
        {
            if (baseStream == null) throw new ArgumentNullException("baseStream");
            Quality = quality;
            LGWin = windowSize;
            _mode = mode;
            _stream = baseStream;
            LeaveOpen = leaveOpen;
            if (_mode == CompressionMode.Compress)
            {
                State = BrotliNative.BrotliEncoderCreateInstance();
                if (State == IntPtr.Zero)
                {
                    throw new Exception();//TODO Create exception
                }
                SetQuality(quality);
                SetWindow(windowSize);
            }
            else
            {
                State = BrotliNative.BrotliDecoderCreateInstance();
                if (State == IntPtr.Zero)
                {
                    throw new Exception();//Create exception
                }
            }
            BufferSize = BuffSize;
            BufferIn = Marshal.AllocHGlobal(BufferSize);
            BufferOut = Marshal.AllocHGlobal(BufferSize);
            NextIn = BufferIn;
            NextOut = BufferOut;
            AvailOut = new IntPtr((uint)BuffSize);
            /*Dict=Marshal.AllocHGlobal(10);
            BrotliNative.BrotliEncoderSetCustomDictionary(State,10, Dict);*/
        }

        public BrotliStream(Stream baseStream, CompressionMode mode, bool leave_open, uint windowsize, uint quality) : this(baseStream, mode, leave_open, windowsize, quality, DefaultBufferSize) { }

        public BrotliStream(Stream baseStream, CompressionMode mode, bool leave_open) : this(baseStream, mode, leave_open, MaxWindowBits, MaxQuality) { }

        public BrotliStream(Stream baseStream, CompressionMode mode) : this(baseStream, mode, false) { }

        public void SetQuality(uint quality)
        {
            if (quality < MinQuality || quality > MaxQuality)
            {
                throw new ArgumentException();//TODO
            }
            BrotliNative.BrotliEncoderSetParameter(State, BrotliNative.BrotliEncoderParameter.Quality, quality);
        }

        public void SetWindow(uint window)
        {
            if (window < MinWindowBits || window > MaxWindowBits)
            {
                throw new ArgumentException();//TODO
            }
            BrotliNative.BrotliEncoderSetParameter(State, BrotliNative.BrotliEncoderParameter.LGWin, window);
        }
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

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_mode != CompressionMode.Decompress) throw new Exception(); // wrong mode
            int bytesRead = (int)(_intermediateStream.Length - _readOffset);
            uint totalCount = 0;
            Boolean endOfStream = false;
            Boolean errorDetected = false;
            Byte[] buf = new Byte[BufferSize];
            while (bytesRead < count)
            {
                while (true)
                {
                    if (LastDecoderResult == BrotliNative.BrotliDecoderResult.NeedsMoreInput)
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
                    else if (LastDecoderResult == BrotliNative.BrotliDecoderResult.NeedsMoreOutput)
                    {
                        Marshal.Copy(BufferOut, buf, 0, BufferSize);
                        _intermediateStream.Write(buf, 0, BufferSize);
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
                    LastDecoderResult = BrotliNative.BrotliDecoderDecompressStream(State, ref AvailIn, ref NextIn,
                        ref AvailOut, ref NextOut, out totalCount);
                    if (bytesRead >= count) break;
                }
                if (endOfStream && !BrotliNative.BrotliDecoderIsFinished(State))
                {
                    errorDetected = true;
                }
                if (LastDecoderResult == BrotliNative.BrotliDecoderResult.Error || errorDetected)
                {
                    var error = BrotliNative.BrotliDecoderGetErrorCode(State);
                    var text = BrotliNative.BrotliDecoderErrorString(error);
                    throw new Exception(); //error - unable  decode stream
                }
                if (endOfStream && !BrotliNative.BrotliDecoderIsFinished(State) && LastDecoderResult == BrotliNative.BrotliDecoderResult.NeedsMoreInput)
                {
                    throw new Exception();//wrong end
                }
                if (endOfStream && NextOut != BufferOut)
                {
                    int remainBytes = (int)(NextOut.ToInt64() - BufferOut.ToInt64());
                    bytesRead += remainBytes;
                    Marshal.Copy(BufferOut, buf, 0, remainBytes);
                    _intermediateStream.Write(buf, 0, remainBytes);
                    NextOut = BufferOut;
                }
                if (endOfStream) break;
            }
            if (_intermediateStream.Length - _readOffset >= count || endOfStream)
            {
                _intermediateStream.Seek(_readOffset, SeekOrigin.Begin);
                var bytesToRead = (int)(_intermediateStream.Length - _readOffset);
                if (bytesToRead > count) bytesToRead = count;
                _intermediateStream.Read(buffer, offset, bytesToRead);
                RemoveBytes(_intermediateStream, _readOffset + bytesToRead);
                _readOffset = 0;
                return bytesToRead;
            }
            return 0;
        }
        private void RemoveBytes(MemoryStream ms, int numberOfBytes)
        {
            ArraySegment<byte> buf;
            if (ms.TryGetBuffer(out buf))
            {
                Buffer.BlockCopy(buf.Array, numberOfBytes, buf.Array, 0, (int)ms.Length - numberOfBytes);
                ms.SetLength(ms.Length - numberOfBytes);
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (_mode != CompressionMode.Compress) throw new Exception();//WrongStreamMode Exeption
            State = BrotliNative.BrotliEncoderCreateInstance();
            SetQuality(11);
            SetWindow(22);
            totalWrote += count;
            //Console.WriteLine(String.Format("Write {0} bytes,total={1} bytes.", count, totalWrote));
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
                    if (!BrotliNative.BrotliEncoderCompressStream(State, bytesRemain <= 0 ? BrotliNative.BrotliEncoderOperation.Finish : BrotliNative.BrotliEncoderOperation.Process, ref AvailIn, ref NextIn, ref AvailOut,
                        ref NextOut, out totalOut)) throw new Exception(); //TODO
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
                if (BrotliNative.BrotliEncoderIsFinished(State)) break;
            }
        }
    }
    
}
