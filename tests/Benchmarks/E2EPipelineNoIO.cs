using System;
using System.Collections.Generic;
using Xunit;
using Microsoft.Xunit.Performance;
using System.Text;
using System.IO.Pipelines.Samples;

public partial class E2EPipelineTests
{
    [Benchmark]
    [InlineData(1000, 250)]
    private static void TechEmpowerHelloWorldNoIO(int numberOfRequests, int concurrentConnections)
    {
        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement())
            {
                RawInMemoryHttpServer.Run(numberOfRequests, concurrentConnections);
            }
        }
    }
}

