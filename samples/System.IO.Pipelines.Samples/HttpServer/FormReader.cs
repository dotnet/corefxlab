// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Numerics;
using System.IO.Pipelines.Text.Primitives;
using Microsoft.Extensions.Primitives;

namespace System.IO.Pipelines.Samples.Http
{
    public class FormReader
    {
        private Dictionary<string, StringValues> _data = new Dictionary<string, StringValues>();
        private long? _contentLength;

        public FormReader(long? contentLength)
        {
            _contentLength = contentLength;
        }

        public Dictionary<string, StringValues> FormValues => _data;

        public bool TryParse(ref ReadableBuffer buffer)
        {
            if (buffer.IsEmpty || !_contentLength.HasValue)
            {
                return true;
            }

            while (!buffer.IsEmpty && _contentLength > 0)
            {
                var next = buffer;
                ReadCursor delim;
                ReadableBuffer key;
                if (!next.TrySliceTo((byte)'=', out key, out delim))
                {
                    break;
                }

                next = next.Slice(delim).Slice(1);

                ReadableBuffer value;
                if (next.TrySliceTo((byte)'&', out value, out delim))
                {
                    next = next.Slice(delim).Slice(1);
                }
                else
                {

                    var remaining = _contentLength - buffer.Length;

                    if (remaining == 0)
                    {
                        value = next;
                        next = next.Slice(next.End);
                    }
                    else
                    {
                        break;
                    }
                }

                // TODO: Combine multi value keys
                _data[key.GetUtf8String()] = value.GetUtf8String();
                _contentLength -= (buffer.Length - next.Length);
                buffer = next;
            }

            return _contentLength == 0;
        }
    }
}
