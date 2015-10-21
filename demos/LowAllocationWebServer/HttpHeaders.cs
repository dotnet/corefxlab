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
            var bytes = _bytes;
            while (bytes.Length > 0)
            {
                int parsedBytes;
                var header = ParseHeaderLine(bytes, out parsedBytes);
                bytes = bytes.Slice(parsedBytes);

                yield return header;
            }
        }        

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<KeyValuePair<Utf8String, Utf8String>> IEnumerable<KeyValuePair<Utf8String, Utf8String>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static KeyValuePair<Utf8String, Utf8String> ParseHeaderLine(ByteSpan bytes, out int parsedBytes)
        {
            int consumedBytes;
            var headerName = new Utf8String(bytes.SliceTo(':', out consumedBytes));
            parsedBytes = consumedBytes;

            bytes = bytes.Slice(consumedBytes);
            var headerValue = new Utf8String(bytes.SliceTo('\r', '\n', out consumedBytes));
            parsedBytes += consumedBytes;

            return new KeyValuePair<Utf8String, Utf8String>(headerName, headerValue);
        }
    }
}