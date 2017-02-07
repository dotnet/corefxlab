using System;
using System.IO;
using System.IO.Compression;
using System.IO.Pipelines.Compression;
using System.IO.Pipelines.File;

namespace System.IO.Pipelines.Samples
{
    public class CompressionSample
    {
        public static void Run()
        {
            using (var factory = new PipeFactory())
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
                var output = factory.CreateWriter(Console.OpenStandardOutput());

                // Copy from the file reader to the console writer
                input.CopyToAsync(output).GetAwaiter().GetResult();

                input.Complete();

                output.Complete();
            }
        }
    }
}
