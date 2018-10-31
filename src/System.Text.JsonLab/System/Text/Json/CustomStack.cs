// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text.JsonLab
{
    internal ref struct CustomStack
    {
        private static byte[] _initStack = new byte[JsonUtf8Reader.StackFreeMaxDepth * StackRow.Size];
        private byte[] _rentedBuffer;
        private Span<byte> _stackSpace;
        private int _topOfStack;

        public CustomStack(int initialSize)
        {
            _rentedBuffer = initialSize <= JsonUtf8Reader.StackFreeMaxDepth * StackRow.Size
                ? _initStack
                : ArrayPool<byte>.Shared.Rent(initialSize);
            _stackSpace = _rentedBuffer;
            _topOfStack = _stackSpace.Length;
        }

        public void Dispose()
        {
            if (_rentedBuffer.Length > JsonUtf8Reader.StackFreeMaxDepth * StackRow.Size)
                ArrayPool<byte>.Shared.Return(_rentedBuffer);
            _stackSpace = Span<byte>.Empty;
            _topOfStack = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Push(StackRow row)
        {
            if (_topOfStack < StackRow.Size)
                Enlarge();
            _topOfStack -= StackRow.Size;
            MemoryMarshal.Write(_stackSpace.Slice(_topOfStack), ref row);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StackRow Pop()
        {
            StackRow row = Peek();
            _topOfStack += StackRow.Size;
            return row;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StackRow Peek()
        {
            if (_topOfStack > _stackSpace.Length - StackRow.Size)
            {
                JsonThrowHelper.ThrowInvalidOperationException();
            }

            return MemoryMarshal.Read<StackRow>(_stackSpace.Slice(_topOfStack));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Enlarge()
        {
            int size = _rentedBuffer.Length * 2;
            byte[] newArray = ArrayPool<byte>.Shared.Rent(size);

            Span<byte> newStackSpace = newArray;
            _stackSpace.CopyTo(newStackSpace.Slice(_rentedBuffer.Length));
            _topOfStack += _rentedBuffer.Length;

            ArrayPool<byte>.Shared.Return(_rentedBuffer);
            _rentedBuffer = newArray;
            _stackSpace = _rentedBuffer;
        }

        public string PrintStack()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(nameof(StackRow.SizeOrLength) + "\t" + nameof(StackRow.NumberOfRows) + "\r\n");
            for (int i = _stackSpace.Length - StackRow.Size; i >= StackRow.Size; i -= StackRow.Size)
            {
                StackRow row = MemoryMarshal.Read<StackRow>(_stackSpace.Slice(i));
                sb.Append(row.SizeOrLength + "\t" + row.NumberOfRows + "\r\n");
            }
            return sb.ToString();
        }
    }
}
