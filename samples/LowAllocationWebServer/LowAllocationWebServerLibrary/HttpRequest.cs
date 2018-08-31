// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text.Http.Parser;
using System.Collections.Sequences;
using static System.Buffers.Text.Encodings;

namespace Microsoft.Net
{
    public struct HttpRequest : IHttpRequestLineHandler, IHttpHeadersHandler
    {
        Http.Method _method;
        Http.Version _version;
        byte[] _target;
        byte[] _path;
        byte[] _query;
        byte[] _customMethod;
        ResizableArray<Header> _headers;

        public Http.Method Method => _method;
        public Http.Version Version => _version;

        public ReadOnlySpan<byte> TargetBytes => _target;
        public string Target => Ascii.ToUtf16String(_target);

        public ReadOnlySpan<byte> PathBytes => _path;
        public string Path => Ascii.ToUtf16String(_path);

        public ReadOnlySpan<byte> QueryBytes => _query;
        public string Query => Ascii.ToUtf16String(_query);

        public ReadOnlySpan<byte> CustomMethodBytes => _customMethod;
        public string CustomMethod => Ascii.ToUtf16String(_customMethod);

        public ReadOnlySpan<Header> Headers => _headers.Full;

        public void OnHeader(ReadOnlySpan<byte> name, ReadOnlySpan<byte> value)
        {
            if (_headers.Items == null) _headers.Items = new Header[4];
            _headers.Add(new Header(name.ToArray(), value.ToArray()));
        }

        public void OnStartLine(Http.Method method, Http.Version version, ReadOnlySpan<byte> target, ReadOnlySpan<byte> path, ReadOnlySpan<byte> query, ReadOnlySpan<byte> customMethod, bool pathEncoded)
        {
            _method = method;
            _version = version;
            _target = target.ToArray();
            _path = path.ToArray();
            _query = query.ToArray();
            _customMethod = customMethod.ToArray();
        }

        public struct Header
        {
            byte[] _name;
            byte[] _value;

            public Header(byte[] name, byte[] value)
            {
                _name = name;
                _value = value;
            }
            public ReadOnlySpan<byte> Name => _name;
            public ReadOnlySpan<byte> Value => _value;
        }
    }
}
