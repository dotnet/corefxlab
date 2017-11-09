// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Utf8;

namespace System.Text.Http.SingleSegment
{
    public ref struct HttpHeadersSingleSegment
    {
        private readonly Utf8Span _headerString;
        private int _count;
        
        public HttpHeadersSingleSegment(ReadOnlySpan<byte> bytes)
        {
            _headerString = new Utf8Span(bytes);
            _count = -1;
        }

        public HttpHeadersSingleSegment(Utf8Span headerString)
        {
            _headerString = headerString;
            _count = -1;
        }

        public Utf8Span this[string headerName]
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

                return Utf8Span.Empty;
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

        private static Utf8Span ParseHeaderLine(Utf8Span headerString, out Utf8SpanPair header)
        {

            //TODO: this will be simplified once we have TrySubstringTo/From accepting strings            
            if (!headerString.TrySubstringTo((byte)':', out Utf8Span headerName))
            {
                throw new ArgumentException("headerString");
            }

            headerString.TrySubstringFrom((byte) ':', out headerString);
            if (!headerString.IsEmpty)
            {
                headerString = headerString.Substring(1);
            }
            
            if (!headerString.TrySubstringTo((byte)'\r', out Utf8Span headerValue))
            {
                throw new ArgumentException("headerString");
            }

            headerString.TrySubstringFrom((byte)'\n', out headerString);
            if (!headerString.IsEmpty)
            {
                headerString = headerString.Substring(1);
            }            
            
            header = new Utf8SpanPair(headerName, headerValue);

            return headerString;
        }

        public ref struct Enumerator 
        {
            private readonly Utf8Span _originalHeaderString;
            private Utf8Span _headerString;
            private Utf8SpanPair _current;

            internal Enumerator(Utf8Span originalHeaderString)
            {
                _originalHeaderString = originalHeaderString;
                _headerString = _originalHeaderString;
                _current = new Utf8SpanPair();
            }

            public bool MoveNext()
            {
                if (_headerString.IsEmpty)
                {
                    return false;
                }

                _headerString = ParseHeaderLine(_headerString, out _current);

                return true;
            }

            public Utf8SpanPair Current => _current;
        }

        public ref struct Utf8SpanPair
        {
            public readonly Utf8Span Key;
            public readonly Utf8Span Value;

            public Utf8SpanPair(Utf8Span first, Utf8Span second)
            {
                this.Key = first;
                this.Value = second;
            }
        }
    }
}
