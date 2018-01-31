// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers.Native;
using System.Buffers.Text;
using System.Linq;
using System.Net.Libuv;
using System.Text.Formatting;
using System.Text.Utf8;
using System.Threading.Tasks;

namespace LibuvWithNonAllocatingFormatters
{
    public class Program
    {
        static string s_ipAddress;
        static int s_port;

        public static void Main(string[] args)
        {
            if (args.Length < 1 || args[0].Substring(0, 4) != "/ip:")
            {
                Usage();
                return;
            }

            var options = args[0].Substring(4).Split(':');
            if (options.Length != 2)
            {
                Usage();
                return;
            }
            s_ipAddress = options[0];
            s_port = Int32.Parse(options[1]);

            Console.WriteLine("browse to http://{0}:{1}", s_ipAddress, s_port);

            bool log = false;
            if (args.Length > 0 && args[0] == "/log")
            {
                log = true;
            }

            var numberOfLoops = Environment.ProcessorCount;
            var loops = new Task[numberOfLoops];
            for (int i = 0; i < numberOfLoops; i++)
            {
                var loop = Task.Run(() => RunLoop(log));
                loops[i] = loop;
            }

            Task.WaitAll(loops);
        }

        private static void Usage()
        {
            Console.WriteLine("Usage: {0} /ip:<ip_address>:<port>", Environment.GetCommandLineArgs()[0]);
        }

        static void RunLoop(bool log)
        {
            var loop = new UVLoop();

            var listener = new TcpListener(s_ipAddress, s_port, loop);

            listener.ConnectionAccepted += (Tcp connection) =>
            {
                if (log)
                {
                    Console.WriteLine("connection accepted");
                }

                connection.ReadCompleted += (data) =>
                {
                    if (log)
                    {
                        unsafe
                        {
                            var requestString = new Utf8Span(data.Span);
                            Console.WriteLine("*REQUEST:\n {0}", requestString.ToString());
                        }
                    }

                    var formatter = new ArrayFormatter(512, SymbolTable.InvariantUtf8);
                    formatter.Clear();
                    formatter.Append("HTTP/1.1 200 OK");
                    formatter.Append("\r\n\r\n");
                    formatter.Append("Hello World!");
                    if (log)
                    {
                        formatter.Format(" @ {0:O}", DateTime.UtcNow);
                    }

                    var segment = formatter.Formatted;
                    using (var memory = new OwnedPinnedBuffer<byte>(segment.Array))
                    {
                        connection.TryWrite(memory.Memory.Slice(segment.Offset, segment.Count));
                        connection.Dispose();
                    }
                };

                connection.ReadStart();
            };

            listener.Listen();
            loop.Run();
        }
    }
}
