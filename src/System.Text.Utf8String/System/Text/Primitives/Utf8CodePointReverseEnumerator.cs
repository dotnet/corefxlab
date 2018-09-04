// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Primitives
{
    internal ref struct Utf8CodePointReverseEnumerator
    {
        private ReadOnlySpan<byte> _buffer;
        private int _index;
        private int _currentLenCache;
        private const int ResetIndex = -Utf8Helper.MaxCodeUnitsPerCodePoint - 1;

        public Utf8CodePointReverseEnumerator(ReadOnlySpan<byte> buffer) : this()
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

                ReadOnlySpan<byte> buffer = _buffer.Slice(0, _index);
                bool succeeded = Utf8Helper.TryDecodeCodePointBackwards(buffer, out uint ret, out _currentLenCache);

                if (!succeeded || _currentLenCache == 0)
                {
                    // TODO: Change exception type
                    throw new Exception("Invalid code point!");
                }

                return ret;
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

            _index -= _currentLenCache;
            _currentLenCache = 0;

            return HasValue();
        }

        // This is different than Reset, it goes to the first element not before first
        private void MoveToFirstPosition() => _index = _buffer.Length;

        private bool IsOnResetPosition() => _index == ResetIndex;

        private bool HasValue()
        {
            if (IsOnResetPosition())
            {
                return true;
            }

            return _index > 0;
        }

        // This is different than MoveToFirstPosition, this actually goes before anything
        internal void Reset()
        {
            _index = ResetIndex;
            _currentLenCache = 0;
        }
    }  
}
