// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using System.Linq;

namespace System.IO.Pipelines.Samples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var sampleTypes = typeof(Program).GetTypeInfo().Assembly.GetTypes()
                    .Where(t => (typeof(ISample)).IsAssignableFrom(t) && !t.GetTypeInfo().IsAbstract)
                    .Where(t => t.Name.ToLower().StartsWith(args[0].ToLower())).ToList();

                if (sampleTypes.Count == 1)
                {
                    Console.WriteLine("Starting " + sampleTypes[0].Name);
                    var instance = (ISample)Activator.CreateInstance(sampleTypes[0]);
                    instance.Run().Wait();
                }
                else
                {
                    Console.WriteLine($"Multiple samples starting with found, " + string.Join(", ", sampleTypes.Select(t => t.Name)));
                    PrintHelp();
                }
            }
            else
            {
                PrintHelp();
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Supply the start of name of the sample you want to run.");
            Console.WriteLine("");
            Console.WriteLine("Available samples:");
            foreach (var t in typeof(Program).GetTypeInfo().Assembly.GetTypes().Where(t => (typeof(ISample)).IsAssignableFrom(t) && !t.GetTypeInfo().IsAbstract))
            {
                Console.WriteLine(t.Name);
            }
        }
    }
}
