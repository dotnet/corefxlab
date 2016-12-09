using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Pipelines.Samples.Framing;
using System.IO.Pipelines.Text.Primitives;

namespace System.IO.Pipelines.Samples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // AspNetHttpServerSample.Run();
            RawLibuvHttpServerSample.Run();
            // ProtocolHandling.Run();
        }
    }
}
