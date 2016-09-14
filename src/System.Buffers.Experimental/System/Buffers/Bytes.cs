// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace System.Buffers
{
    /// <summary>
    /// Represents a Span<byte> factory</byte>
    /// </summary>
    /// <remarks>
    /// This struct is not safe for multithreaded access, i.e. it's subject to struct tearing that can result in unsafe memory access. 
    /// </remarks>
    public struct Bytes
    {
        ArraySegment<byte> _segment;
        unsafe byte* _memory;
        int _memoryLength;

        public Bytes(ArraySegment<byte> segment)
        {
            _segment = segment;
            unsafe { _memory = null; }
            _memoryLength = 0;
        }
        public unsafe Bytes(byte* pointer, int length)
        {
            unsafe { _memory = pointer; }
            _memoryLength = length;
            _segment = default(ArraySegment<byte>);
        }

        public static implicit operator Span<byte>(Bytes buffer)
        {
            if (buffer._segment.Array != null) return buffer._segment.Slice();
            else {
                unsafe
                {
                    return new Span<byte>(buffer._memory, buffer._memoryLength);
                }
            }
        }

        public int Length {
            get {
                return _segment.Array == null?_memoryLength:_segment.Count;
            }
        }
    }
}
