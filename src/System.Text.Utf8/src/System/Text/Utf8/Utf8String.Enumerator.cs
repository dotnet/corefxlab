// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;

namespace System.Text.Utf8
{
    partial struct Utf8String
    {
        public struct Enumerator : IEnumerator<UnicodeCodePoint>, IEnumerator
        {
            // TODO: This object is heavier than string itself... Once we got ByteSpan runtime support, change it.
            // TODO: Reduce number of members when we get Span<byte> runtime support
            private ByteSpan _startBuffer;
            private ByteSpan _buffer;

            private byte[] _bytes;
            private int _startIndex;
            private int _index;
            private int _lastIndex;

            private int _currentLenCache;

            public Enumerator(byte[] bytes, int index, int length)
            {
                if (index + length > bytes.Length)
                    throw new ArgumentOutOfRangeException("index");

                _startBuffer = default(ByteSpan);
                _buffer = default(ByteSpan);

                _bytes = bytes;
                // first MoveNext moves to a first byte
                _startIndex = index - 1;
                _index = _startIndex;
                _lastIndex = index + length;

                _currentLenCache = 0;
            }

            public unsafe Enumerator(ByteSpan buffer)
            {
                if (buffer.UnsafeBuffer == null)
                    throw new ArgumentNullException("buffer");

                // first MoveNext moves to a first byte
                _startBuffer = new ByteSpan(buffer.UnsafeBuffer - 1, buffer.Length + 1);
                _buffer = _startBuffer;

                _bytes = default(byte[]);
                _startIndex = default(int);
                _index = default(int);
                _lastIndex = default(int);

                _currentLenCache = 0;
            }


            object IEnumerator.Current { get { return Current; } }

            public unsafe UnicodeCodePoint Current
            {
                get
                {
                    UnicodeCodePoint ret;
                    bool succeeded;
                    if (_bytes != null)
                    {
                        if (_index == _startIndex)
                        {
                            throw new InvalidOperationException("MoveNext() needs to be called at least once");
                        }

                        fixed (byte* pinnedBytes = _bytes)
                        {
                            ByteSpan buffer = new ByteSpan(pinnedBytes + _index, _lastIndex - _index);

                            succeeded = Utf8Encoder.TryDecodeCodePoint(buffer, out ret, out _currentLenCache);
                        }
                    }
                    else
                    {
                        if (_buffer.UnsafeBuffer == _startBuffer.UnsafeBuffer)
                        {
                            throw new InvalidOperationException("MoveNext() needs to be called at least once");
                        }
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
                if (IsOnStartingPosition())
                {
                    if (_bytes != null)
                    {
                        _index++;
                        return _index < _lastIndex;
                    }
                    else
                    {
                        _buffer = _buffer.Slice(1);
                        return _buffer.Length > 0;
                    }
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
                    return _index < _lastIndex;
                }
                else
                {
                    _buffer = _buffer.Slice(_currentLenCache);
                    _currentLenCache = 0;
                    return _buffer.Length > 0;
                }
            }

            private bool IsOnStartingPosition()
            {
                if (_bytes != null)
                {
                    return _index == _startIndex;
                }
                else
                {
                    return _buffer.Length == _startBuffer.Length;
                }
            }

            public void Reset()
            {
                _index = _startIndex;
                _buffer = _startBuffer;
                _currentLenCache = 0;
            }
        }
    }
}
