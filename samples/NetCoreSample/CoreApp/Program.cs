// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
