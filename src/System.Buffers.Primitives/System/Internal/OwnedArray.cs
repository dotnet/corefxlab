// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers.Internal
{
    internal sealed class OwnedArray<T> : OwnedBuffer<T>
    {
        public new T[] Array => base.Array;

        public static implicit operator T[](OwnedArray<T> owner) {
            return owner.Array;
        }

        public static implicit operator OwnedArray<T>(T[] array) {
            return new OwnedArray<T>(array);
        }

        public static implicit operator OwnedArray<T>(ArraySegment<T> segment)
        {
            return new OwnedArray<T>(segment);
        }

        public OwnedArray(int length) : base(new T[length], 0, length)
        {}

        public OwnedArray(T[] array) : base(array, 0, array.Length)
        {}

        public OwnedArray(ArraySegment<T> segment) : base(segment.Array, segment.Offset, segment.Count)
        { }
    }
}