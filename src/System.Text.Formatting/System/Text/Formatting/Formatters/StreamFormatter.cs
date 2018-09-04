// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Buffers;
using System.Buffers.Text;

namespace System.Text.Formatting
{
    public struct StreamFormatter : ITextBufferWriter, IDisposable
    {
        Stream _stream;
        SymbolTable _symbolTable;
        byte[] _buffer;
        ArrayPool<byte> _pool;

        public StreamFormatter(Stream stream, ArrayPool<byte> pool) : this(stream, SymbolTable.InvariantUtf16, pool)
        {
        }

        public StreamFormatter(Stream stream, SymbolTable symbolTable, ArrayPool<byte> pool, int bufferSize = 256)
        {
            _pool = pool;
            _buffer = null;
            if (bufferSize > 0)
            {
                _buffer = _pool.Rent(bufferSize);
            }
            _symbolTable = symbolTable;
            _stream = stream;
        }
        Memory<byte> IBufferWriter<byte>.GetMemory(int minimumLength)
        {
            if (minimumLength > _buffer.Length)
            {
                var newSize = _buffer.Length * 2;
                if(minimumLength != 0){
                    newSize = minimumLength;
                }
                var temp = _buffer;
                _buffer = _pool.Rent(newSize);
                _pool.Return(temp);
            }
            return _buffer;
        }

        Span<byte> IBufferWriter<byte>.GetSpan(int minimumLength)
        {
            if (minimumLength > _buffer.Length)
            {
                var newSize = _buffer.Length * 2;
                if (minimumLength != 0)
                {
                    newSize = minimumLength;
                }
                var temp = _buffer;
                _buffer = _pool.Rent(newSize);
                _pool.Return(temp);
            }
            return _buffer;
        }

        // ISSUE
        // I would like to lazy write to the stream, but unfortunatelly this seems to be exclusive with this type being a struct.
        // If the write was lazy, passing this struct by value could result in data loss.
        // A stack frame could write more data to the buffer, and then when the frame pops, the infroamtion about how much was written could be lost.
        // On the other hand, I cannot make this type a class and keep using it as it can be used today (i.e. pass streams around and create instances of this type on demand).
        // Too bad we don't support move semantics and stack only structs.
        void IBufferWriter<byte>.Advance(int bytes)
        {
            _stream.Write(_buffer, 0, bytes);
        }

        public int MaxBufferSize => Int32.MaxValue;

        SymbolTable ITextBufferWriter.SymbolTable
        {
            get {
                return _symbolTable;
            }
        }

        /// <summary>
        /// Returns buffers to the pool
        /// </summary>
        public void Dispose()
        {
            _pool.Return(_buffer);
            _buffer = null;
            _stream = null;
        }
    }
}
