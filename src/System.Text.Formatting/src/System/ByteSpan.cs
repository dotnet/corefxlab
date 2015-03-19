// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System {

    // I wish I could use just Span<byte>. 
    // Unfortunatelly, Span<T> (as implementable currently) is too slow for some tight formatting loops.
    // We might at some point get a fast Span<T>, and remove this type. 
    // The main problem with Span<T> is that all indexer accessors need to do double pointer arrithmetic (offset from array and index into array)
    // ByteSpan accessor just needs one pointer offset.
    unsafe struct ByteSpan // Span<byte>
    {
        byte* _data;
        int _length;

        // Instances of this type will be created out of a memory buffer pool.
        internal ByteSpan(byte* data, int length)
        {
            _data = data;
            _length = length;
        }

        public byte this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                // the range checks are super expensive. we need to optimize them out just like we do for arrays
                //if (index >= Length) Environment.FailFast("index out of range");
                return *(_data + index);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                // the range checks are super expensive. we need to optimize them out just like we do for arrays
                //if (index >= Length) Environment.FailFast("index out of range");
                *(_data + index) = value;
            }
        }

        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _length;
            }
        }

        public ByteSpan Slice(int index)
        {
            Precondition.Require(index < Length);

            var data = _data + index;
            var length = _length - index;
            return new ByteSpan(data, length);
        }

        public ByteSpan Slice(int index, int count)
        {
            Precondition.Require(index + count < Length);

            var data = _data + index;
            return new ByteSpan(data, count);
        }
    }
}