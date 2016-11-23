// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;

namespace System.Text.Utf8
{
    partial struct Utf8String
    {
        public struct CodePointEnumerable : IEnumerable<UnicodeCodePoint>, IEnumerable
        {
            private ReadOnlySpan<byte> _buffer;

            public CodePointEnumerable(byte[] bytes, int index, int length)
            {
                _buffer = new ReadOnlySpan<byte>(bytes, index, length);
            }

            public unsafe CodePointEnumerable(ReadOnlySpan<byte> buffer)
            {
                _buffer = buffer;
            }

            public CodePointEnumerator GetEnumerator()
            {
                return new CodePointEnumerator(_buffer);
            }

            public CodePointReverseEnumerator GetReverseEnumerator()
            {
                return new CodePointReverseEnumerator(_buffer);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            IEnumerator<UnicodeCodePoint> IEnumerable<UnicodeCodePoint>.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
