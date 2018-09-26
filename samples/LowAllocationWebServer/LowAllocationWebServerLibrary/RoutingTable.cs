// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Text.Http.Parser;
using System.Text.Utf8;

namespace Microsoft.Net
{
    public class ApiRoutingTable<TRequestId>
    {
        const int tablecapacity = 20;
        byte[][] _uris = new byte[tablecapacity][];
        TRequestId[] _requestIds = new TRequestId[tablecapacity];
        Http.Method[] _verbs = new Http.Method[tablecapacity];
        Action<HttpRequest, ReadOnlySequence<byte>, TcpConnectionFormatter>[] _handlers = new Action<HttpRequest, ReadOnlySequence<byte>, TcpConnectionFormatter>[tablecapacity];
        int _count;

        public TRequestId GetRequestId(HttpRequest request)
        {
            for(int i=0; i<_count; i++) {
                if (request.PathBytes.SequenceEqual(_uris[i]) && request.Method == _verbs[i]) return _requestIds[i];
            }
            return default;
        }

        public bool TryHandle(HttpRequest request, ReadOnlySequence<byte> body, TcpConnectionFormatter response)
        {
            var path = new Utf8Span(request.PathBytes);
            for (int i = 0; i < _count; i++)
            {
                // TODO: this should check the verb too
                if (path.Equals(new Utf8Span(_uris[i])))
                {
                    _handlers[i](request, body, response);
                    return true;
                }
            }
            return false;
        }

        public void Add(Http.Method method, string requestUri, TRequestId requestId, Action<HttpRequest, ReadOnlySequence<byte>, TcpConnectionFormatter> handler = null)
        {
            if (_count == tablecapacity) throw new NotImplementedException("ApiReoutingTable does not resize yet.");
            _uris[_count] = new Utf8Span(requestUri).Bytes.ToArray();
            _requestIds[_count] = requestId;
            _verbs[_count] = method;
            _handlers[_count] = handler;
            _count++;
        }
    }
}
