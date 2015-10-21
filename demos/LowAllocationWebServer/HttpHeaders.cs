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
        
        public HttpHeaders(ByteSpan bytes)
        {
            _bytes = bytes;            
        }                

        public Utf8String this[string headerName]
        {
            get
            {
                var headers = this.Where(h => h.Key == headerName);

                return headers.Any() ? headers.First().Value : Utf8String.Empty;
            }
        }

        public int Count => this.Count();

        public IEnumerator<KeyValuePair<Utf8String, Utf8String>> GetEnumerator()
        {
            return new Enumerator(_bytes);
        }        

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<KeyValuePair<Utf8String, Utf8String>> IEnumerable<KeyValuePair<Utf8String, Utf8String>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static KeyValuePair<Utf8String, Utf8String> ParseHeaderLine(ByteSpan bytes, int start, out int parsedBytes)
        {
            int consumedBytes;
            var headerName = new Utf8String(bytes.SliceTo(start, ':', out consumedBytes));
            parsedBytes = start + consumedBytes;

            var headerValue = new Utf8String(bytes.SliceTo(start + consumedBytes, '\r', '\n', out consumedBytes));
            parsedBytes += consumedBytes;

            return new KeyValuePair<Utf8String, Utf8String>(headerName, headerValue);
        }

        public struct Enumerator : IEnumerator<KeyValuePair<Utf8String, Utf8String>>
        {
            ByteSpan _bytes;
            private int _readBytes;

            internal Enumerator(ByteSpan bytes)
            {
                _bytes = bytes;
                _readBytes = -1;
            }

            public bool MoveNext()
            {                
                if (_readBytes == -1)
                {
                    _readBytes++;                    
                }
                else
                {
                    ParseHeaderLine(_bytes, _readBytes, out _readBytes);
                    if (_readBytes >= _bytes.Length)
                    {
                        return false;
                    }
                }

                return true;
            }

            public KeyValuePair<Utf8String, Utf8String> Current
            {
                get
                {
                    if (_readBytes == -1)
                    {
                        return new KeyValuePair<Utf8String, Utf8String>();
                    }

                    int readBytes;
                    return ParseHeaderLine(_bytes, _readBytes, out readBytes);
                }
            }

            object IEnumerator.Current => Current;

            void IDisposable.Dispose()
            {
            }

            void IEnumerator.Reset()
            {
                _readBytes = -1;
            }
        }
    }
}