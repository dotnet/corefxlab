// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;

namespace System.Text.Utf8
{
    partial struct Utf8String
    {
        // TODO: Name TBD
        public struct CodePointReverseEnumerator : IEnumerator<UnicodeCodePoint>, IEnumerator
        {
            // TODO: This object is heavier than string itself... Once we got ByteSpan runtime support, change it.
            // TODO: Reduce number of members when we get Span<byte> runtime support
            private ByteSpan _startBuffer;
            private ByteSpan _resetBufferPosition;
            private ByteSpan _buffer;

            private byte[] _bytes;
            private int _startIndex;
            private int _index;
            private int _endIndex;
            private const int ResetIndex = -UnicodeConstants.Utf8MaxCodeUnitsPerCodePoint - 1;

            private int _currentLenCache;

            public CodePointReverseEnumerator(byte[] bytes, int index, int length) : this()
            {
                if (index + length > bytes.Length)
                    throw new ArgumentOutOfRangeException("index");

                _bytes = bytes;
                _startIndex = index;
                _endIndex = index + length;

                Reset();
            }

            public unsafe CodePointReverseEnumerator(ByteSpan buffer) : this()
            {
                if (buffer.UnsafeBuffer == null && buffer.Length != 0)
                    throw new ArgumentNullException("buffer");

                // first MoveNext moves to a first byte
                _startBuffer = new ByteSpan(buffer.UnsafeBuffer, buffer.Length);
                _resetBufferPosition = new ByteSpan(buffer.UnsafeBuffer - 1, 0);

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
                    if (_bytes != null)
                    {
                        return _index - _startIndex;
                    }
                    else
                    {
                        unsafe
                        {
                            return checked((int)(_buffer.UnsafeBuffer - _startBuffer.UnsafeBuffer));
                        }
                    }
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

                    UnicodeCodePoint ret;
                    bool succeeded;
                    if (_bytes != null)
                    {
                        fixed (byte* pinnedBytes = _bytes)
                        {
                            ByteSpan buffer = new ByteSpan(pinnedBytes + _startIndex, _index - _startIndex);
                            succeeded = Utf8Encoder.TryDecodeCodePointBackwards(buffer, out ret, out _currentLenCache);
                        }
                    }
                    else
                    {
                        succeeded = Utf8Encoder.TryDecodeCodePointBackwards(_buffer, out ret, out _currentLenCache);
                    }

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

                if (_bytes != null)
                {
                    _index -= _currentLenCache;
                    _currentLenCache = 0;
                }
                else
                {
                    _buffer = _buffer.Slice(0, _buffer.Length - _currentLenCache);
                    _currentLenCache = 0;
                }

                return HasValue();
            }

            // This is different than Reset, it goes to the first element not before first
            private void MoveToFirstPosition()
            {
                if (_bytes != null)
                {
                    _index = _endIndex;
                }
                else
                {
                    _buffer = _startBuffer;
                }
            }

            private bool IsOnResetPosition()
            {
                if (_bytes != null)
                {
                    return _index == ResetIndex;
                }
                else
                {
                    return _buffer.ReferenceEquals(_resetBufferPosition);
                }
            }

            private bool HasValue()
            {
                if (IsOnResetPosition())
                {
                    return true;
                }

                if (_bytes != null)
                {
                    return _index > _startIndex;
                }
                else
                {
                    unsafe
                    {
                        return _buffer.Length != 0;
                    }
                }
            }

            // This is different than MoveToFirstPosition, this actually goes before anything
            public void Reset()
            {
                _index = ResetIndex;
                _buffer = _resetBufferPosition;
                _currentLenCache = 0;
            }
        }
    }
}
