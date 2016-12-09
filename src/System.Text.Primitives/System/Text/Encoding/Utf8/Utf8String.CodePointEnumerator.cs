// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;

namespace System.Text.Utf8
{
    partial struct Utf8String
    {
        public struct CodePointEnumerator : IEnumerator<UnicodeCodePoint>, IEnumerator
        {
            private ReadOnlySpan<byte> _buffer;
            private int _index;
            private int _currentLenCache;
            private const int ResetIndex = -UnicodeConstants.Utf8MaxCodeUnitsPerCodePoint - 1;

            public unsafe CodePointEnumerator(ReadOnlySpan<byte> buffer) : this()
            {
                _buffer = buffer;

                Reset();
            }

            // TODO: Name TBD
            public int PositionInCodeUnits
            {
                get
                {
                    if (IsOnResetPosition())
                    {
                        return -1;
                    }

                    return _index;
                }
            }

            object IEnumerator.Current { get { return Current; } }

            public unsafe UnicodeCodePoint Current
            {
                get
                {
                    if (IsOnResetPosition())
                    {
                        throw new InvalidOperationException("MoveNext() needs to be called at least once");
                    }

                    if (!HasValue())
                    {
                        throw new InvalidOperationException("Current does not exist");
                    }

                    ReadOnlySpan<byte> buffer = _buffer.Slice(_index);
                    UnicodeCodePoint ret;
                    bool succeeded = Utf8Encoder.TryDecodeCodePoint(buffer, out ret, out _currentLenCache);

                    if (!succeeded || _currentLenCache == 0)
                    {
                        // TODO: Change exception type
                        throw new Exception("Invalid code point!");
                    }

                    return ret;
                }
            }

            void IDisposable.Dispose()
            {
            }

            public bool MoveNext()
            {
                if (!HasValue())
                {
                    return false;
                }
                if (IsOnResetPosition())
                {
                    MoveToFirstPosition();
                    return HasValue();
                }

                if (_currentLenCache == 0)
                {
                    UnicodeCodePoint codePointDummy = Current;
                    if (_currentLenCache == 0)
                    {
                        throw new Exception("Invalid UTF-8 character (badly encoded)");
                    }
                }

                _index += _currentLenCache;
                _currentLenCache = 0;

                return HasValue();
            }

            // This is different than Reset, it goes to the first element not before first
            private void MoveToFirstPosition()
            {
                _index = 0;
            }

            private bool IsOnResetPosition()
            {
                return _index == ResetIndex;
            }

            private bool HasValue()
            {
                if (IsOnResetPosition())
                {
                    return true;
                }

                return _index < _buffer.Length;
            }

            // This is different than MoveToFirstPosition, this actually goes before anything
            public void Reset()
            {
                _index = ResetIndex;
                _currentLenCache = 0;
            }
        }
    }
}
