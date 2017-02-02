using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LowAllocationWebServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Sample Rest Server Started");
            Console.WriteLine("Browse to http://<host>:8080/time, http://<host>:8080/plaintext, or http://<host>:8080/json to test it.\n");

            var log = new ConsoleLog((Log.Level.Verbose));
            var cancellation = new CancellationTokenSource();

            var restServer = new SampleRestServer(cancellation.Token, log, 8080, 0, 0, 0, 0);
            restServer.StartAsync();

            Console.WriteLine("Press ENTER to exit ...");
            Console.ReadLine();

            cancellation.Cancel();
        }
    }
}
