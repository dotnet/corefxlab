using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Abstractions;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets;

namespace ProxyService
{
    public class Program
    {
        public static string ListenUrl = "http://0.0.0.0:5001";
        public static string ForwardUrl = "http://localhost:5000";

        public static void Main(string[] args)
        {
            Console.WriteLine("Starting Program forwarding from port {0} to {1}", ListenUrl, ForwardUrl);
            var host = new WebHostBuilder()
               .UseKestrel(options =>
               {
                   // Run callbacks on the transport thread
                   options.ApplicationSchedulingMode = SchedulingMode.Inline;
               })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls(ListenUrl)
                .UseSockets()
                .Build();

            host.Run();
            Console.WriteLine("Proxy Service Ending");
        }
    }
}
