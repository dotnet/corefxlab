using System.Collections;
using System.Collections.Generic;
using System.Text.Utf8;

namespace System.Net.Http.Buffered
{
    public struct HttpHeaders : IEnumerable<KeyValuePair<Utf8String, Utf8String>>
    {
        private ByteSpan _bytes;

        public HttpHeaders(ByteSpan bytes)
        {
            _bytes = bytes;
            Count = 1;

            ParseHeaders();
        }

        private void ParseHeaders()
        {            
            for (var i = 0; i < _bytes.Length; i++)
            {                
                if (_bytes[i] == '\r' && _bytes[i + 1] == '\n')
                {
                    Count++;
                }
            }
        }

        public Utf8String? this[string header]
        {
            get
            {
                return new Utf8String("HttpHeaders.get_this not implemented yet");
            }
        }

        public int Count { get; private set; }

        public Enumerator GetEnumerator()
        {
            return new Enumerator();
        }

        public struct Enumerator : IEnumerator<KeyValuePair<Utf8String, Utf8String>>
        {
            ByteSpan _bytes;
            int _index;

            internal Enumerator(ByteSpan bytes)
            {
                _bytes = bytes;
                _index = 0;
            }

            public bool MoveNext() {
                throw new NotImplementedException();
            }

            public KeyValuePair<Utf8String, Utf8String> Current { get { throw new NotImplementedException(); } }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            void IDisposable.Dispose()
            {
            }

            void IEnumerator.Reset()
            {
                _index = 0;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(_bytes);
        }

        IEnumerator<KeyValuePair<Utf8String, Utf8String>> IEnumerable<KeyValuePair<Utf8String, Utf8String>>.GetEnumerator()
        {
            return new Enumerator(_bytes);
        }
    }
}