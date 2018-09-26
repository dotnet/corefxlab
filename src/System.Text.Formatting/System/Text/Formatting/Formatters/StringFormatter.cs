// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Collections.Sequences;

namespace System.Text.Formatting
{
    public class StringFormatter : ITextBufferWriter, IDisposable
    {
        ResizableArray<byte> _buffer;
        ArrayPool<byte> _pool;
        public SymbolTable SymbolTable { get; set; } = SymbolTable.InvariantUtf16;

        public StringFormatter(int characterCapacity = 32, ArrayPool<byte> pool = null)
        {
            if (pool == null) _pool = ArrayPool<byte>.Shared;
            else _pool = pool;
            _buffer = new ResizableArray<byte>(_pool.Rent(characterCapacity * 2));
        }

        public void Dispose()
        {
            _pool.Return(_buffer.Items);
            _buffer.Count = 0;
        }

        public void Append(char character) {
            _buffer.Add((byte)character);
            _buffer.Add((byte)(character >> 8));
        }

        //TODO: this should use Span<byte>
        public void Append(string text)
        {
            foreach (char character in text)
            {
                Append(character);
            }
        }

        //TODO: this should use Span<byte>
        public void Append(ReadOnlySpan<char> substring)
        {
            for (int i = 0; i < substring.Length; i++)
            {
                Append(substring[i]);
            }
        }

        public void Clear()
        {
            _buffer.Clear();
        }

        public override string ToString()
        {
            var text = Encoding.Unicode.GetString(_buffer.Items, 0, _buffer.Count);
            return text;
        }

        Memory<byte> IBufferWriter<byte>.GetMemory(int minimumLength)
        {
            if (minimumLength < 1) minimumLength = 1;
            if (_buffer.Free.Count < minimumLength)
            {
                var doubleCount = _buffer.Free.Count * 2;
                int newSize = minimumLength > doubleCount ? minimumLength : doubleCount;
                var newArray = _pool.Rent(newSize + _buffer.Count);
                var oldArray = _buffer.Resize(newArray);
                _pool.Return(oldArray);
            }
            return _buffer.Free;
        }

        Span<byte> IBufferWriter<byte>.GetSpan(int minimumLength)
        {
            if (minimumLength < 1) minimumLength = 1;
            if (_buffer.Free.Count < minimumLength)
            {
                var doubleCount = _buffer.Free.Count * 2;
                int newSize = minimumLength > doubleCount ? minimumLength : doubleCount;
                var newArray = _pool.Rent(newSize + _buffer.Count);
                var oldArray = _buffer.Resize(newArray);
                _pool.Return(oldArray);
            }
            return _buffer.Free;
        }

        void IBufferWriter<byte>.Advance(int bytes)
        {
            _buffer.Count += bytes;
        }

        public int MaxBufferSize { get; } = Int32.MaxValue;
    }
}
