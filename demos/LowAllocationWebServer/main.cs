// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Net;

class Program
{
    static void Main()
    {
        Console.WriteLine("Sample Rest Server Started");
        Console.WriteLine("The server implements /time REST method.");
        Console.WriteLine("Browse to http://localhost:9999/time to test it.\n");

        var log = new ConsoleLog((Log.Level.Off));
        var address = new IPAddress(new byte[] { 127, 0, 0, 1 }); 
        var restServer = new SampleRestServer(log, address, 9999); 
        restServer.StartAsync();

        Console.WriteLine("Press ENTER to exit ...");
        Console.ReadLine();

        restServer.Stop();
    }
}

