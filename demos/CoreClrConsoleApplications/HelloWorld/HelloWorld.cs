// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        foreach (var arg in args)
        {
            Console.Write("Hello ");
            Console.Write(arg);
            Console.WriteLine("!");
        }
        Console.WriteLine("Press ENTER to exit ...");
        Console.ReadLine();
    }
}