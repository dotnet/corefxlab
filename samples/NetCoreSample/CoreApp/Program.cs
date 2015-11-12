using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace coreApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"{NetCoreLibrary.Class1.SayHello()} world");
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
        }
    }
}
