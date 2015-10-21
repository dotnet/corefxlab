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
            // TODO: This object is heavier than string itself... Once we got ByteSpan runtime support, change it.
            // TODO: Reduce number of members when we get Span<byte> runtime support
            private ByteSpan _startBuffer;
            private ByteSpan _resetBufferPosition;
            private ByteSpan _buffer;

            private byte[] _bytes;
            private int _startIndex;
            private int _index;
            private int _lastIndex;

            private int _currentLenCache;

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

            public CodePointEnumerator(byte[] bytes, int index, int length) : this()
            {
                if (index + length > bytes.Length)
                    throw new ArgumentOutOfRangeException("index");

                _bytes = bytes;
                // first MoveNext moves to a first byte
                _startIndex = index;
                _lastIndex = index + length;

                Reset();
            }

            public unsafe CodePointEnumerator(ByteSpan buffer) : this()
            {
                if (buffer.UnsafeBuffer == null)
                    throw new ArgumentNullException("buffer");

                // first MoveNext moves to a first byte
                _startBuffer = new ByteSpan(buffer.UnsafeBuffer, buffer.Length);
                _resetBufferPosition = new ByteSpan(buffer.UnsafeBuffer - 1, 0);

                Reset();
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
                            ByteSpan buffer = new ByteSpan(pinnedBytes + _index, _lastIndex - _index);
                            succeeded = Utf8Encoder.TryDecodeCodePoint(buffer, out ret, out _currentLenCache);
                        }
                    }
                    else
                    {
                        succeeded = Utf8Encoder.TryDecodeCodePoint(_buffer, out ret, out _currentLenCache);
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
                if (IsOnResetPosition())
                {
                    MoveToFirstPosition();
                    return !IsOnLastPosition();
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
                    _index += _currentLenCache;
                    _currentLenCache = 0;
                }
                else
                {
                    _buffer = _buffer.Slice(_currentLenCache);
                    _currentLenCache = 0;
                }

                return !IsOnLastPosition();
            }

            // This is different than Reset, it goes to the first element not before first
            private void MoveToFirstPosition()
            {
                if (_bytes != null)
                {
                    _index = _startIndex;
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
                    return _index == -1;
                }
                else
                {
                    return _buffer.ReferenceEquals(_resetBufferPosition);
                }
            }

            private bool IsOnLastPosition()
            {
                if (_bytes != null)
                {
                    return _index == _lastIndex;
                }
                else
                {
                    unsafe
                    {
                        return _buffer.Length == 0 && _buffer.UnsafeBuffer != _resetBufferPosition.UnsafeBuffer;
                    }
                }
            }

            // This is different than MoveToStartPosition, this actually goes before anything
            public void Reset()
            {
                _index = -1;
                _buffer = _resetBufferPosition;
                _currentLenCache = 0;
            }
        }
    }
}
