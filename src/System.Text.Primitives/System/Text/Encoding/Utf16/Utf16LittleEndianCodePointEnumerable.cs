using System.Collections;
using System.Collections.Generic;

namespace System.Text.Utf16
{
    // TODO: Should this and Utf8 code point enumerators/enumerable be subclasses of Utf8/16Encoder?
    public struct Utf16LittleEndianCodePointEnumerable : IEnumerable<UnicodeCodePoint>, IEnumerable
    {
        private string _s;

        public Utf16LittleEndianCodePointEnumerable(string s)
        {
            _s = s;
        }

        public Utf16LittleEndianCodePointEnumerator GetEnumerator()
        {
            return new Utf16LittleEndianCodePointEnumerator(_s);
        }

        IEnumerator<UnicodeCodePoint> IEnumerable<UnicodeCodePoint>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
