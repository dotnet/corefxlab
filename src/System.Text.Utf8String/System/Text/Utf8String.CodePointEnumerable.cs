// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace System.Text.Utf8
{
    partial class Utf8String
    {
        public ref struct CodePointEnumerable 
        {
            private byte[] _buffer;

            public CodePointEnumerable(byte[] bytes, int index, int length)
            {
                _buffer = new byte[length - index];
                for(int i = index; i < _buffer.Length; i++)
                {
                    _buffer[i] = bytes[index + i];
                }
            }

            public unsafe CodePointEnumerable(byte[] buffer)
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

            public int Count()
            {
                int result = 0;
                foreach (var cp in this)
                {
                    result++;
                }

                return result;
            }
        }
    }
}
