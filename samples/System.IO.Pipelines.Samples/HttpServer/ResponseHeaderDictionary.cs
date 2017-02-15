// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.Formatting;
using System.IO.Pipelines;
using System.IO.Pipelines.Text.Primitives;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Internal.Http;
using Microsoft.Extensions.Primitives;

namespace System.IO.Pipelines.Samples.Http
{
    public class ResponseHeaderDictionary : IHeaderDictionary
    {
        private static readonly DateHeaderValueManager _dateHeaderValueManager = new DateHeaderValueManager();
        private static readonly byte[] _serverHeaderBytes = Encoding.UTF8.GetBytes("\r\nServer: Pipelines");
        private static readonly byte[] _chunkedHeaderBytes = Encoding.UTF8.GetBytes("\r\nTransfer-Encoding: chunked");

        private static readonly byte[] _headersStartBytes = Encoding.UTF8.GetBytes("\r\n");
        private static readonly byte[] _headersSeperatorBytes = Encoding.UTF8.GetBytes(": ");
        private static readonly byte[] _headersEndBytes = Encoding.UTF8.GetBytes("\r\n\r\n");

        private readonly HeaderDictionary _headers = new HeaderDictionary();

        public StringValues this[string key]
        {
            get
            {
                return _headers[key];
            }

            set
            {
                _headers[key] = value;
            }
        }

        public int Count => _headers.Count;

        public bool IsReadOnly => false;

        public ICollection<string> Keys => _headers.Keys;

        public ICollection<StringValues> Values => _headers.Values;

        public void Add(KeyValuePair<string, StringValues> item) => _headers.Add(item);

        public void Add(string key, StringValues value) => _headers.Add(key, value);

        public void Clear()
        {
            _headers.Clear();
        }

        public bool Contains(KeyValuePair<string, StringValues> item)
        {
            return _headers.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return _headers.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, StringValues>[] array, int arrayIndex)
        {
            _headers.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, StringValues>> GetEnumerator()
        {
            return _headers.GetEnumerator();
        }

        public bool Remove(KeyValuePair<string, StringValues> item)
        {
            return _headers.Remove(item);
        }

        public bool Remove(string key)
        {
            return _headers.Remove(key);
        }

        public bool TryGetValue(string key, out StringValues value)
        {
            return _headers.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void CopyTo(bool chunk, WritableBuffer buffer)
        {
            foreach (var header in _headers)
            {
                buffer.Write(_headersStartBytes);
                buffer.Append(header.Key, TextEncoder.Utf8);
                buffer.Write(_headersSeperatorBytes);
                buffer.Append(header.Value.ToString(), TextEncoder.Utf8);
            }

            if (chunk)
            {
                buffer.Write(_chunkedHeaderBytes);
            }

            buffer.Write(_serverHeaderBytes);
            var date = _dateHeaderValueManager.GetDateHeaderValues().Bytes;
            buffer.Write(date);

            buffer.Write(_headersEndBytes);
        }

        public void Reset() => _headers.Clear();
    }
}
