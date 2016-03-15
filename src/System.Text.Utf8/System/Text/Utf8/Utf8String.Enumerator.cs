// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;

namespace System.Text.Utf8
{
    partial struct Utf8String
    {
        public struct Enumerator : IEnumerator<Utf8CodeUnit>, IEnumerator
        {
            private ReadOnlySpan<byte>.Enumerator _enumerator;

            public Enumerator(ReadOnlySpan<byte> buffer)
            {
                _enumerator = buffer.GetEnumerator();
            }

            object IEnumerator.Current { get { return Current; } }

            public Utf8CodeUnit Current
            {
                get
                {
                    return (Utf8CodeUnit)_enumerator.Current;
                }
            }

            void IDisposable.Dispose()
            {
            }

            public bool MoveNext()
            {
                return _enumerator.MoveNext();
            }

            public void Reset()
            {
                _enumerator.Reset();
            }
        }
    }
}
