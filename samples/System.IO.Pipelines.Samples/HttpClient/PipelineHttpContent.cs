// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Samples
{
    public class PipelineHttpContent : HttpContent
    {
        private readonly IPipeReader _output;

        public PipelineHttpContent(IPipeReader output)
        {
            _output = output;
        }

        public int ContentLength { get; set; }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            int remaining = ContentLength;

            while (remaining > 0)
            {
                var result = await _output.ReadAsync();
                var inputBuffer = result.Buffer;

                var fin = result.IsCompleted;

                var consumed = inputBuffer.Start;

                try
                {
                    if (inputBuffer.IsEmpty && fin)
                    {
                        return;
                    }

                    var data = inputBuffer.Slice(0, remaining);

                    foreach (var memory in data)
                    {
                        ArraySegment<byte> buffer;

                        unsafe
                        {
                            if (!memory.TryGetArray(out buffer))
                            {
                                // Fall back to copies if this was native memory and we were unable to get
                                //  something we could write
                                buffer = new ArraySegment<byte>(memory.Span.ToArray());
                            }
                        }

                        await stream.WriteAsync(buffer.Array, buffer.Offset, buffer.Count);
                    }

                    consumed = data.End;
                    remaining -= data.Length;
                }
                finally
                {
                    _output.Advance(consumed);
                }
            }
        }

        protected override bool TryComputeLength(out long length)
        {
            length = ContentLength;
            return true;
        }
    }
}
