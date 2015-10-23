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
            private ByteSpan _buffer;

            private byte[] _bytes;
            private int _index;
            private int _length;

            public CodePointEnumerable(byte[] bytes, int index, int length)
            {
                _buffer = default(ByteSpan);

                _bytes = bytes;
                _index = index;
                _length = length;
            }

            public unsafe CodePointEnumerable(ByteSpan buffer)
            {
                _buffer = buffer;

                _bytes = default(byte[]);
                _index = default(int);
                _length = default(int);
            }

            public CodePointEnumerator GetEnumerator()
            {
                if (_bytes != null)
                {
                    return new CodePointEnumerator(_bytes, _index, _length);
                }
                else
                {
                    return new CodePointEnumerator(_buffer);
                }
            }

            public CodePointReverseEnumerator GetReverseEnumerator()
            {
                if (_bytes != null)
                {
                    return new CodePointReverseEnumerator(_bytes, _index, _length);
                }
                else
                {
                    return new CodePointReverseEnumerator(_buffer);
                }
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
