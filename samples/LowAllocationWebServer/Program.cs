using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LowAllocationWebServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Sample Rest Server Started");
            Console.WriteLine("The server implements /time REST method.");
            Console.WriteLine("Browse to http://<host>:8080/time or http://<host>:8080/plaintext to test it.\n");

            var log = new ConsoleLog((Log.Level.Error));
            var restServer = new SampleRestServer(log, 8080, 0, 0, 0, 0);
            restServer.StartAsync();

            Console.WriteLine("Press ENTER to exit ...");
            Console.ReadLine();

            restServer.Stop();
        }
    }
}
