// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Collections.Sequences;
using System.Runtime.CompilerServices;

namespace System.Text.Formatting
{
    public struct ArrayFormatterWrapper : ITextBufferWriter, IDisposable
    {
        private ArrayFormatter _arrayFormatter;

        public ArrayFormatterWrapper(int capacity, SymbolTable symbolTable, ArrayPool<byte> pool = null)
        {
            _arrayFormatter = new ArrayFormatter(capacity, symbolTable, pool);
        }

        public int CommitedByteCount => _arrayFormatter.CommitedByteCount;

        public void Clear() => _arrayFormatter.Clear();

        public ArraySegment<byte> Free => _arrayFormatter.Free;

        public ArraySegment<byte> Formatted => _arrayFormatter.Formatted;

        public SymbolTable SymbolTable => _arrayFormatter.SymbolTable;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Memory<byte> GetMemory(int minimumLength = 0) => _arrayFormatter.GetMemory(minimumLength);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<byte> GetSpan(int minimumLength = 0) => _arrayFormatter.GetSpan(minimumLength);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Advance(int bytes) => _arrayFormatter.Advance(bytes);

        public void Dispose() => _arrayFormatter.Dispose();
    }

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

        public void Clear()
        {
            _buffer.Count = 0;
        }

        public ArraySegment<byte> Free => _buffer.Free;

        public ArraySegment<byte> Formatted => _buffer.Full;

        public SymbolTable SymbolTable => _symbolTable;

        public Memory<byte> GetMemory(int minimumLength = 0)
        {
            if (minimumLength < 1) minimumLength = 1;
            if (minimumLength > _buffer.FreeCount)
            {
                int doubleCount = _buffer.FreeCount * 2;
                int newSize = minimumLength > doubleCount ? minimumLength : doubleCount;
                byte[] newArray = _pool.Rent(newSize + _buffer.Count);
                byte[] oldArray = _buffer.Resize(newArray);
                _pool.Return(oldArray);
            }
            return _buffer.FreeMemory;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<byte> GetSpan(int minimumLength = 0)
        {
            if (minimumLength < 1) minimumLength = 1;
            if (minimumLength > _buffer.FreeCount)
            {
                int doubleCount = _buffer.FreeCount * 2;
                int newSize = minimumLength > doubleCount ? minimumLength : doubleCount;
                byte[] newArray = _pool.Rent(newSize + _buffer.Count);
                byte[] oldArray = _buffer.Resize(newArray);
                _pool.Return(oldArray);
            }
            return _buffer.FreeSpan;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Advance(int bytes)
        {
            _buffer.Count += bytes;
            if (_buffer.Count > _buffer.Capacity)
            {
                FormatterThrowHelper.ThrowInvalidOperationException("More bytes commited than returned from FreeBuffer");
            }
        }

        public void Dispose()
        {
            byte[] array = _buffer.Items;
            _buffer.Items = null;
            _pool.Return(array);
        }
    }
}
