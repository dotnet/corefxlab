// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Reflection;
using Microsoft.Xunit.Performance.Api;

public class PerfHarness
{
    public static void Main(string[] args)
    {
        string[] assemblies = GetTestAssemblies();
        
        int pos =  Array.IndexOf(args, "--assembly");

        using (XunitPerformanceHarness harness = new XunitPerformanceHarness(args))
        {
            if (pos > -1 && args.Length > pos + 1)
            {
                pos = Array.IndexOf(assemblies, args[pos + 1]);
                if (pos > -1)
                {
                    harness.RunBenchmarks(GetTestAssembly(assemblies[pos]));
                    return;
                }
            }

            foreach(var testName in assemblies)
            {
                harness.RunBenchmarks(GetTestAssembly(testName));
            }
        }
    }

    private static string GetTestAssembly(string testName)
    {
        // Assume test assemblies are colocated/restored next to the PerfHarness.
        return Path.Combine(
            Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
            $"{testName}.dll"
        );
    }

    private static string[] GetTestAssemblies()
    {
        return new [] {
            "Benchmarks",
            "System.Binary.Base64.Tests",
            "System.Text.Primitives.Performance.Tests",
            "System.Slices.Tests"
        };
    }
}
