// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.IO.Pipelines.Text.Primitives;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace System.IO.Pipelines.Samples.Http
{
    public class RequestHeaderDictionary : IHeaderDictionary
    {
        private static readonly byte[] ContentLengthKeyBytes = Encoding.ASCII.GetBytes("CONTENT-LENGTH");
        private static readonly byte[] ContentTypeKeyBytes = Encoding.ASCII.GetBytes("CONTENT-TYPE");
        private static readonly byte[] AcceptBytes = Encoding.ASCII.GetBytes("ACCEPT");
        private static readonly byte[] AcceptLanguageBytes = Encoding.ASCII.GetBytes("ACCEPT-LANGUAGE");
        private static readonly byte[] AcceptEncodingBytes = Encoding.ASCII.GetBytes("ACCEPT-ENCODING");
        private static readonly byte[] HostBytes = Encoding.ASCII.GetBytes("HOST");
        private static readonly byte[] ConnectionBytes = Encoding.ASCII.GetBytes("CONNECTION");
        private static readonly byte[] CacheControlBytes = Encoding.ASCII.GetBytes("CACHE-CONTROL");
        private static readonly byte[] UserAgentBytes = Encoding.ASCII.GetBytes("USER-AGENT");
        private static readonly byte[] UpgradeInsecureRequests = Encoding.ASCII.GetBytes("UPGRADE-INSECURE-REQUESTS");

        private Dictionary<string, HeaderValue> _headers = new Dictionary<string, HeaderValue>(10, StringComparer.OrdinalIgnoreCase);

        public StringValues this[string key]
        {
            get
            {
                TryGetValue(key, out StringValues values);
                return values;
            }

            set
            {
                SetHeader(key, value);
            }
        }

        public int Count => _headers.Count;

        public bool IsReadOnly => false;

        public ICollection<string> Keys => _headers.Keys;

        public ICollection<StringValues> Values => _headers.Values.Select(v => v.GetValue()).ToList();

        public void SetHeader(ref ReadOnlySequence<byte> key, ref ReadOnlySequence<byte> value)
        {
            string headerKey = GetHeaderKey(ref key);
            _headers[headerKey] = new HeaderValue
            {
                Raw = value.ToArray()
            };
        }

        public ReadOnlySequence<byte> GetHeaderRaw(string key)
        {
            if (_headers.TryGetValue(key, out HeaderValue value))
            {
                return new ReadOnlySequence<byte>(value.Raw);
            }
            return default;
        }

        private string GetHeaderKey(ref ReadOnlySequence<byte> key)
        {
            if (EqualsIgnoreCase(ref key, AcceptBytes))
            {
                return "Accept";
            }

            if (EqualsIgnoreCase(ref key, AcceptEncodingBytes))
            {
                return "Accept-Encoding";
            }

            if (EqualsIgnoreCase(ref key, AcceptLanguageBytes))
            {
                return "Accept-Language";
            }

            if (EqualsIgnoreCase(ref key, HostBytes))
            {
                return "Host";
            }

            if (EqualsIgnoreCase(ref key, UserAgentBytes))
            {
                return "User-Agent";
            }

            if (EqualsIgnoreCase(ref key, CacheControlBytes))
            {
                return "Cache-Control";
            }

            if (EqualsIgnoreCase(ref key, ConnectionBytes))
            {
                return "Connection";
            }

            if (EqualsIgnoreCase(ref key, UpgradeInsecureRequests))
            {
                return "Upgrade-Insecure-Requests";
            }

            return key.GetAsciiString();
        }

        private bool EqualsIgnoreCase(ref ReadOnlySequence<byte> key, byte[] buffer)
        {
            if (key.Length != buffer.Length)
            {
                return false;
            }

            return key.EqualsTo(buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsAlpha(byte b)
        {
            return b >= 'a' && b <= 'z' || b >= 'A' && b <= 'Z';
        }

        private void SetHeader(string key, StringValues value)
        {
            _headers[key] = new HeaderValue
            {
                Value = value
            };
        }

        public void Add(KeyValuePair<string, StringValues> item)
        {
            SetHeader(item.Key, item.Value);
        }

        public void Add(string key, StringValues value)
        {
            SetHeader(key, value);
        }

        public void Clear()
        {
            _headers.Clear();
        }

        public bool Contains(KeyValuePair<string, StringValues> item)
        {
            return false;
        }

        public bool ContainsKey(string key)
        {
            return _headers.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, StringValues>[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        public void Reset()
        {
            _headers.Clear();
        }

        public IEnumerator<KeyValuePair<string, StringValues>> GetEnumerator()
        {
            return _headers.Select(h => new KeyValuePair<string, StringValues>(h.Key, h.Value.GetValue())).GetEnumerator();
        }

        public bool Remove(KeyValuePair<string, StringValues> item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            return _headers.Remove(key);
        }

        public bool TryGetValue(string key, out StringValues value)
        {
            if (_headers.TryGetValue(key, out HeaderValue headerValue))
            {
                value = headerValue.GetValue();
                return true;
            }

            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private struct HeaderValue
        {
            public byte[] Raw;
            public StringValues? Value;

            public StringValues GetValue()
            {
                if (!Value.HasValue)
                {
                    if (Raw == null)
                    {
                        return StringValues.Empty;
                    }

                    Value = Encoding.ASCII.GetString(Raw);
                }

                return Value.Value;
            }
        }
    }
}
