using System.Collections;
using System.Collections.Generic;
using System.Text.Utf8;

namespace System.Text.Http
{
    public struct HttpHeaders : IEnumerable<KeyValuePair<Utf8String, Utf8String>>
    {
        private readonly Utf8String _headerString;
        private int _count;
        
        public HttpHeaders(ReadOnlySpan<byte> bytes)
        {
            _headerString = new Utf8String(bytes);
            _count = -1;
        }

        public HttpHeaders(Utf8String headerString)
        {
            _headerString = headerString;
            _count = -1;
        }

        public Utf8String this[string headerName]
        {
            get
            {
                foreach (var header in this)
                {
                    if (header.Key == headerName)
                    {
                        return header.Value;
                    }
                }

                return Utf8String.Empty;
            }
        }

        public int Count
        {
            get
            {
                if (_count != -1)
                {
                    return _count;
                }

                _count = 0;
                foreach (var header in this)
                {
                    _count++;
                }

                return _count;
            }
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(_headerString);
        }        

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<KeyValuePair<Utf8String, Utf8String>> IEnumerable<KeyValuePair<Utf8String, Utf8String>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static Utf8String ParseHeaderLine(Utf8String headerString, out KeyValuePair<Utf8String, Utf8String> header)
        {
            Utf8String headerName;
            Utf8String headerValue;

            //TODO: this will be simplified once we have TrySubstringTo/From accepting strings            
            if (!headerString.TrySubstringTo((Utf8CodeUnit) (byte) ':', out headerName))
            {
                throw new ArgumentException("headerString");
            }

            headerString.TrySubstringFrom((Utf8CodeUnit) (byte) ':', out headerString);
            if (headerString.Length > 0)
            {
                headerString = headerString.Substring(1);
            }
            
            if (!headerString.TrySubstringTo((Utf8CodeUnit)(byte)'\r', out headerValue))
            {
                throw new ArgumentException("headerString");
            }

            headerString.TrySubstringFrom((Utf8CodeUnit)(byte)'\n', out headerString);
            if (headerString.Length > 0)
            {
                headerString = headerString.Substring(1);
            }            
            
            header = new KeyValuePair<Utf8String, Utf8String>(headerName, headerValue);

            return headerString;
        }

        public struct Enumerator : IEnumerator<KeyValuePair<Utf8String, Utf8String>>
        {
            private readonly Utf8String _originalHeaderString;
            private Utf8String _headerString;
            private KeyValuePair<Utf8String, Utf8String> _current;

            internal Enumerator(Utf8String originalHeaderString)
            {
                _originalHeaderString = originalHeaderString;
                _headerString = _originalHeaderString;
                _current = new KeyValuePair<Utf8String, Utf8String>();
            }

            public bool MoveNext()
            {
                if (_headerString.Length == 0)
                {
                    return false;
                }

                _headerString = ParseHeaderLine(_headerString, out _current);

                return true;
            }

            public KeyValuePair<Utf8String, Utf8String> Current => _current;

            object IEnumerator.Current => Current;

            void IDisposable.Dispose()
            {
            }

            void IEnumerator.Reset()
            {
                _headerString = _originalHeaderString;
                _current = new KeyValuePair<Utf8String, Utf8String>();
            }
        }
    }
}