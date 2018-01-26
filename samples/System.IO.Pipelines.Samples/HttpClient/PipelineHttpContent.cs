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
        private readonly PipeReader _output;

        public PipelineHttpContent(PipeReader output)
        {
            _output = output;
        }

        public int ContentLength { get; set; }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            long remaining = ContentLength;

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
                        await stream.WriteAsync(memory);
                    }

                    consumed = data.End;
                    remaining -= data.Length;
                }
                finally
                {
                    _output.AdvanceTo(consumed);
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
