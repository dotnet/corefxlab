// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;
using System.Text.Utf8;

namespace System.Text.Http.SingleSegment
{
    public ref struct HttpHeadersSingleSegment
    {
        private readonly Utf8String _headerString;
        private int _count;
        
        public HttpHeadersSingleSegment(ReadOnlySpan<byte> bytes)
        {
            _headerString = new Utf8String(bytes);
            _count = -1;
        }

        public HttpHeadersSingleSegment(Utf8String headerString)
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

        private static Utf8String ParseHeaderLine(Utf8String headerString, out Utf8StringPair header)
        {
            Utf8String headerName;
            Utf8String headerValue;

            //TODO: this will be simplified once we have TrySubstringTo/From accepting strings            
            if (!headerString.TrySubstringTo((byte) ':', out headerName))
            {
                throw new ArgumentException("headerString");
            }

            headerString.TrySubstringFrom((byte) ':', out headerString);
            if (headerString.Length > 0)
            {
                headerString = headerString.Substring(1);
            }
            
            if (!headerString.TrySubstringTo((byte)'\r', out headerValue))
            {
                throw new ArgumentException("headerString");
            }

            headerString.TrySubstringFrom((byte)'\n', out headerString);
            if (headerString.Length > 0)
            {
                headerString = headerString.Substring(1);
            }            
            
            header = new Utf8StringPair(headerName, headerValue);

            return headerString;
        }

        public ref struct Enumerator 
        {
            private readonly Utf8String _originalHeaderString;
            private Utf8String _headerString;
            private Utf8StringPair _current;

            internal Enumerator(Utf8String originalHeaderString)
            {
                _originalHeaderString = originalHeaderString;
                _headerString = _originalHeaderString;
                _current = new Utf8StringPair();
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

            public Utf8StringPair Current => _current;
        }

        public ref struct Utf8StringPair
        {
            public readonly Utf8String Key;
            public readonly Utf8String Value;

            public Utf8StringPair(Utf8String first, Utf8String second)
            {
                this.Key = first;
                this.Value = second;
            }
        }
    }
}