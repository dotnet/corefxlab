// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Text.JsonLab
{
    internal ref struct CustomStack
    {
        private Span<byte> _stackSpace;
        private int _topOfStack;

        public CustomStack(Span<byte> stackSpace)
        {
            _stackSpace = stackSpace;
            _topOfStack = stackSpace.Length;
        }

        public bool TryPush(StackRow row)
        {
            if (_topOfStack >= StackRow.Size)
            {
                MemoryMarshal.Write(_stackSpace.Slice(_topOfStack - StackRow.Size), ref row);
                bool hasChildren = true;
                if (_topOfStack < _stackSpace.Length)
                    MemoryMarshal.Write(_stackSpace.Slice(_topOfStack + 5), ref hasChildren);
                _topOfStack -= StackRow.Size;
                return true;
            }
            return false;
        }

        public StackRow Pop()
        {
            StackRow row = Peek();
            _topOfStack += StackRow.Size;
            return row;
        }

        public StackRow Peek()
        {
            if (_topOfStack > _stackSpace.Length - StackRow.Size)
            {
                JsonThrowHelper.ThrowInvalidOperationException();
            }

            return MemoryMarshal.Read<StackRow>(_stackSpace.Slice(_topOfStack));
        }

        public bool IsTopArray()
        {
            if (_topOfStack > _stackSpace.Length - StackRow.Size)
            {
                return false;
            }
            return _stackSpace[_topOfStack] == 1;
        }

        private void Reset()
        {
            _stackSpace.Slice(_topOfStack - StackRow.Size, StackRow.Size).Fill(255);
        }

        public void Resize(Span<byte> newStackMemory)
        {
            Debug.Assert(newStackMemory.Length > _stackSpace.Length);
            int stackGrowth = newStackMemory.Length - _stackSpace.Length;
            _stackSpace.CopyTo(newStackMemory.Slice(stackGrowth));
            _topOfStack += stackGrowth;
            _stackSpace = newStackMemory;
        }

        public string PrintStacks()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("IsArray" + "\t" + "Length" + "\r\n");
            for (int i = _stackSpace.Length - StackRow.Size; i >= StackRow.Size; i -= StackRow.Size)
            {
                StackRow row = MemoryMarshal.Read<StackRow>(_stackSpace.Slice(i));
                sb.Append(row.IsArray + "\t" + row.Length + "\r\n");
            }
            return sb.ToString();
        }
    }
}
