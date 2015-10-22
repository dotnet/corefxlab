using System.Collections;
using System.Collections.Generic;
using System.Text.Utf8;
using LowAllocationServer;

namespace System.Net.Http.Buffered
{
    public struct HttpHeaders : IEnumerable<KeyValuePair<Utf8String, Utf8String>>
    {
        private readonly Utf8String _headerString;
        private int _count;
        
        //TODO: consider adding a Utf8String constructor
        public HttpHeaders(ByteSpan bytes)
        {
            _headerString = new Utf8String(bytes);
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

        private static KeyValuePair<Utf8String, Utf8String> ParseHeaderLine(Utf8String headerString)
        {
            Utf8String headerName;
            Utf8String headerValue;

            headerString.TrySubstringTo((Utf8CodeUnit) (byte) ':', out headerName);
            headerString.TrySubstringFrom((Utf8CodeUnit) (byte) ':', out headerString);

            headerString.TrySubstringTo(new Utf8String("\r\n"), out headerValue);
            headerString.TrySubstringFrom(new Utf8String("\r\n"), out headerString);
            
            return new KeyValuePair<Utf8String, Utf8String>(headerName, headerValue);
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

                _current = ParseHeaderLine(_headerString);

                return true;
            }

            public KeyValuePair<Utf8String, Utf8String> Current
            {
                get
                {
                    return _current;
                }
            }

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