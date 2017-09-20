// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.IO.Compression;
using System.IO.Pipelines.Compression;
using System.IO.Pipelines.File;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Samples
{
    public class CompressionSample : ISample
    {
        public Task Run()
        {
            using (var factory = new MemoryPool())
            {
                var filePath = Path.GetFullPath("Program.cs");

                // This is what Stream looks like
                //var fs = File.OpenRead(filePath);
                //var compressed = new MemoryStream();
                //var compressStream = new DeflateStream(compressed, CompressionMode.Compress);
                //fs.CopyTo(compressStream);
                //compressStream.Flush();
                //compressed.Seek(0, SeekOrigin.Begin);

                var input = factory.ReadFile(filePath)
                              .DeflateCompress(factory, CompressionLevel.Optimal)
                              .DeflateDecompress(factory);

                // Wrap the console in a pipeline writer

                var outputPipe = factory.Create();
                outputPipe.Reader.CopyToEndAsync(Console.OpenStandardOutput());

                // Copy from the file reader to the console writer
                input.CopyToAsync(outputPipe.Writer).GetAwaiter().GetResult();

                input.Complete();

                outputPipe.Writer.Complete();

                return Task.CompletedTask;
            }
        }
    }
}
