using System.IO;
using System.Reflection;
using Microsoft.Xunit.Performance.Api;

public class PerfHarness
{
    public static void Main(string[] args)
    {
        using (XunitPerformanceHarness harness = new XunitPerformanceHarness(args))
        {
            foreach(var testName in GetTestAssemblies())
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
            "System.Text.Primitives.Tests",
            "System.Text.Primitives.Encoding.Performance.Tests"
        };
    }
}
