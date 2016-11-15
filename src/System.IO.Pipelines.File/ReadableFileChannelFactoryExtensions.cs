using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.IO.Pipelines.File
{
    public static class ReadableFileChannelFactoryExtensions
    {
        public static IPipelineReader ReadFile(this PipelineFactory factory, string path)
        {
            var channel = factory.Create();

            var file = new ReadableFileChannel(channel);
            file.OpenReadFile(path);
            return file;
        }
    }
}
