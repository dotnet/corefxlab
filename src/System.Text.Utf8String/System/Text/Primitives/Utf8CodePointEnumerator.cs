// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Primitives
{
    public ref struct Utf8CodePointEnumerator
    {
        private ReadOnlySpan<byte> _utf8Bytes;
        private int _index;
        private int _currentLenCache;
        private const int ResetIndex = -Utf8Helper.MaxCodeUnitsPerCodePoint - 1;

        public Utf8CodePointEnumerator(ReadOnlySpan<byte> utf8Bytes) : this()
        {
            _utf8Bytes = utf8Bytes;
            Reset();
        }

        // TODO: Name TBD
        public int PositionInCodeUnits => IsOnResetPosition() ? -1 : _index;

        public uint Current
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

                bool succeeded = Utf8Helper.TryDecodeCodePoint(_utf8Bytes, _index, out uint codePoint, out _currentLenCache);

                if (!succeeded || _currentLenCache == 0)
                {
                    // TODO: Change exception type
                    throw new Exception("Invalid code point!");
                }

                return codePoint;
            }
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
                uint codePointDummy = Current;
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
        private void MoveToFirstPosition() => _index = 0;

        private bool IsOnResetPosition() => _index == ResetIndex;

        private bool HasValue()
        {
            if (IsOnResetPosition()) {
                return true;
            }

            return _index < _utf8Bytes.Length;
        }

        // This is different than MoveToFirstPosition, this actually goes before anything
        internal void Reset()
        {
            _index = ResetIndex;
            _currentLenCache = 0;
        }
    }
}

