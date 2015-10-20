using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Utf8;
using LowAllocationServer;

namespace System.Net.Http.Buffered
{
    public struct HttpHeaders : IEnumerable<KeyValuePair<Utf8String, Utf8String>>
    {
        private readonly ByteSpan _bytes;

        private readonly List<KeyValuePair<Utf8String, Utf8String>?> _headers;

        public HttpHeaders(ByteSpan bytes)
        {
            _bytes = bytes;
            _headers = new List<KeyValuePair<Utf8String, Utf8String>?>();

            ParseHeaders();
        }

        private void ParseHeaders()
        {
            var bytes = _bytes;
            while (bytes.Length > 0)
            {
                int parsedBytes;
                var header = ParseHeaderLine(bytes, out parsedBytes);
                _headers.Add(header);
                bytes = bytes.Slice(parsedBytes);
            }
        }

        private KeyValuePair<Utf8String, Utf8String> ParseHeaderLine(ByteSpan bytes, out int parsedBytes)
        {
            int consumedBytes;
            var headerName = new Utf8String(bytes.SliceTo(':', out consumedBytes));
            parsedBytes = consumedBytes;

            bytes = bytes.Slice(consumedBytes);
            var headerValue = new Utf8String(bytes.SliceTo('\r', '\n', out consumedBytes));
            parsedBytes += consumedBytes;

            return new KeyValuePair<Utf8String, Utf8String>(headerName, headerValue);
        }

        public Utf8String this[string headerName]
        {
            get
            {
                var header = _headers.FirstOrDefault(h => h.HasValue && h.Value.Key == new Utf8String(headerName));
                
                return header?.Value ?? Utf8String.Empty;
            }
        }

        public int Count => _headers.Count;

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