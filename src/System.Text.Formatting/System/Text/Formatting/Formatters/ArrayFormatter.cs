// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Collections.Sequences;

namespace System.Text.Formatting
{
    public class ArrayFormatter : ITextBufferWriter, IDisposable
    {
        ResizableArray<byte> _buffer;
        SymbolTable _symbolTable;
        ArrayPool<byte> _pool;

        public ArrayFormatter(int capacity, SymbolTable symbolTable, ArrayPool<byte> pool = null)
        {
            _pool = pool ?? ArrayPool<byte>.Shared;
            _symbolTable = symbolTable;
            _buffer = new ResizableArray<byte>(_pool.Rent(capacity));
        }

        public int CommitedByteCount => _buffer.Count;

        public void Clear() {
            _buffer.Count = 0;
        }

        public ArraySegment<byte> Free => _buffer.Free;
        public ArraySegment<byte> Formatted => _buffer.Full;

        public SymbolTable SymbolTable => _symbolTable;

        public Memory<byte> GetMemory(int minimumLength = 0)
        {
            if (minimumLength < 1) minimumLength = 1;
            if (minimumLength > _buffer.Free.Count)
            {
                var doubleCount = _buffer.Free.Count * 2;
                int newSize = minimumLength>doubleCount?minimumLength:doubleCount;
                var newArray = _pool.Rent(newSize + _buffer.Count);
                var oldArray = _buffer.Resize(newArray);
                _pool.Return(oldArray);
            }
            return _buffer.Free;
        }

        public Span<byte> GetSpan(int minimumLength) => GetMemory(minimumLength).Span;

        public void Advance(int bytes)
        {
            _buffer.Count += bytes;
            if(_buffer.Count > _buffer.Count)
            {
                throw new InvalidOperationException("More bytes commited than returned from FreeBuffer");
            }
        }

        public void Dispose()
        {
            var array = _buffer.Items;
            _buffer.Items = null;
            _pool.Return(array);
        }
    }
}
