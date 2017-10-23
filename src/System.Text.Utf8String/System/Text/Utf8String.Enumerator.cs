﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace System.Text.Utf8
{
    partial class Utf8String
    {
        public ref struct Enumerator
        {
            private readonly byte[] _buffer;
            private readonly int _length;
            private int _index;

            internal Enumerator(byte[] buffer)
            {
                _buffer = buffer;
                _length = buffer.Length;
                _index = -1;
            }

            public byte Current
            {
                get
                {
                    return _buffer[_index];
                }
            }

            public bool MoveNext()
            {
                return ++_index < _length;
            }

            public void Reset()
            {
                _index = -1;
            }
        }
    }
}
