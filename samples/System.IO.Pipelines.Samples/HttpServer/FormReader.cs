// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Generic;
using System.Collections.Sequences;
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

        public bool TryParse(ref ReadOnlyBuffer<byte> buffer)
        {
            if (buffer.IsEmpty || !_contentLength.HasValue)
            {
                return true;
            }

            while (!buffer.IsEmpty && _contentLength > 0)
            {
                var next = buffer;
                if (!next.TrySliceTo((byte)'=', out ReadOnlyBuffer<byte> key, out SequenceIndex delim))
                {
                    break;
                }

                next = next.Slice(delim).Slice(1);

                if (next.TrySliceTo((byte)'&', out ReadOnlyBuffer<byte> value, out delim))
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
                _data[key.GetUtf8Span()] = value.GetUtf8Span();
                _contentLength -= (buffer.Length - next.Length);
                buffer = next;
            }

            return _contentLength == 0;
        }
    }
}
