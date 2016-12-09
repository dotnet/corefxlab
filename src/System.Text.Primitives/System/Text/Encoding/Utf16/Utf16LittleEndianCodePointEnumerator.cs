using System.Collections;
using System.Collections.Generic;

namespace System.Text.Utf16
{
    public struct Utf16LittleEndianCodePointEnumerator : IEnumerator<UnicodeCodePoint>, IEnumerator
    {
        string _s;
        int _index;

        int _encodedChars;
        UnicodeCodePoint _codePoint;

        public Utf16LittleEndianCodePointEnumerator(string s)
        {
            _s = s;
            _index = -1;
            _encodedChars = 0;
            _codePoint = default(UnicodeCodePoint);
        }

        public UnicodeCodePoint Current
        {
            get
            {
                if (_encodedChars != 0)
                {
                    return _codePoint;
                }

                if (_index < 0 || _index >= _s.Length)
                {
                    throw new InvalidOperationException("Enumerator is on invalid position");
                }

                if (!Utf16LittleEndianEncoder.TryDecodeCodePointFromString(_s, _index, out _codePoint, out _encodedChars))
                {
                    _codePoint = default(UnicodeCodePoint);
                    _encodedChars = 0;
                    // or index outside of string
                    throw new InvalidOperationException("Invalid characters in the string");
                }

                if (_encodedChars <= 0)
                {
                    // TODO: Change exception type
                    throw new Exception("Internal error: CodePoint is decoded but number of characters read is 0 or negative");
                }

                return _codePoint;
            }
        }

        public void Reset()
        {
            _index = -1;
            _encodedChars = 0;
            _codePoint = default(UnicodeCodePoint);
        }

        public bool MoveNext()
        {
            if (_index == -1)
            {
                _index = 0;
                _encodedChars = 0;
            }
            else
            {
                UnicodeCodePoint dummy = Current;
                _index += _encodedChars;
                _encodedChars = 0;
            }

            return _index < _s.Length;
        }

        object IEnumerator.Current { get { return Current; } }

        void IDisposable.Dispose() { }
    }
}
