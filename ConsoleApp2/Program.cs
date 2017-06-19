using System;
using System.IO;
using System.Collections.Generic;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks;


namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            byte[] bytes = File.ReadAllBytes("ptt5.txt");
            FileStream fileStreams = File.Create("output.br");
            BrotliStream brotliStream = new BrotliStream(fileStreams, CompressionMode.Compress,false,65535,CompressionLevel.Optimal);
            Console.WriteLine("Init OK");
            brotliStream.Write(bytes, 0, bytes.Length);
            Console.WriteLine("Write NO error");
            brotliStream.Flush();
            Console.WriteLine("Flush NO error");
            
        }
    }
}
