using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Libuv;
using System.Threading.Tasks;

namespace LibuvClientUDP
{
    public class Program
    {
        static void Main(string[] args)
        {
            var loop = new UVLoop();
            var endpoint = new IPEndPoint( IPAddress.Any, 6877);

            var udpClient = new UdpClient(
                endpoint.Address.ToString(), 
                (ushort)endpoint.Port , 
                loop
            );

            udpClient.ReceiveCompleted += (ip,port,data) =>
            {

                var sender = new IPEndPoint( IPAddress.Parse(ip), (int) port );
                Console.WriteLine("UDP: {0}",sender.ToString());
                Console.WriteLine(data);
                
                // udpClient.Send(sender.Address, 6877, data );
            };
            udpClient.Listen( );

            loop.Run();
        }
    }
}
