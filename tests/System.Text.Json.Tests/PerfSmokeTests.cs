// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.Json.Tests.Resources;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace System.Text.Json.Tests
{
    public class PerfSmokeTests
    {
        private static readonly Stopwatch Timer = new Stopwatch();

        // Keep < 10 before checking in. Otherwise tests take over 2 seconds to execute.
        private const int NumberOfIterations = 5;
        private const int NumberOfSamples = 3;
        private const int ExpectedMemoryBenchMark = 0;

        // ReSharper disable once ConvertToConstant.Local
        private static readonly bool OutputResults = true;

        [Fact(Skip = "The tests are flaky and the GC sometimes reports allocations")]
        public void ReadBasicJson()
        {
            Output("====== TEST ReadBasicJson ======");
            ReadJsonHelper(TestJson.BasicJson); // Do not test first iteration
            RunTest(TestJson.BasicJson);
        }

        [Fact(Skip = "The tests are flaky and the GC sometimes reports allocations")]
        public void ReadProjectLockJson()
        {
            Output("====== TEST ReadProjectLockJson ======");
            ReadJsonHelper(TestJson.ProjectLockJson); // Do not test first iteration
            RunTest(TestJson.ProjectLockJson);
        }

        [Fact(Skip = "The tests are flaky and the GC sometimes reports allocations")]
        public void ReadHeavyNestedJsonPerf()
        {
            Output("====== TEST ReadHeavyNestedJson ======");
            ReadJsonHelper(TestJson.HeavyNestedJson); // Do not test first iteration
            RunTest(TestJson.HeavyNestedJson);
        }

        private static void RunTest(string jsonStr)
        {
            var timeIterReadResults = new List<long>();
            var memoryIterReadResults = new List<long>();

            for (var j = 0; j < NumberOfSamples; j++)
            {
                GC.Collect(2);
                var memoryBefore = GC.GetTotalMemory(true);
                Timer.Restart();
                for (var i = 0; i < NumberOfIterations; i++)
                {
                    ReadJsonHelper(jsonStr);
                }
                var time = Timer.ElapsedMilliseconds;
                var memoryAfter = GC.GetTotalMemory(true);
                var consumption = memoryAfter - memoryBefore;
                timeIterReadResults.Add(time);
                memoryIterReadResults.Add(consumption);
                Assert.True(consumption <= ExpectedMemoryBenchMark, consumption.ToString());
            }
            Output(timeIterReadResults.Average().ToString(CultureInfo.InvariantCulture));
            Output((memoryIterReadResults.Average()).ToString(CultureInfo.InvariantCulture));
        }

        private static void ReadJsonHelper(string jsonStr)
        {
            var reader = new JsonReader(jsonStr.AsReadOnlySpan().AsBytes(), SymbolTable.InvariantUtf16);
            while (reader.Read())
            {
                var tokenType = reader.TokenType;
                switch (tokenType)
                {
                    case JsonTokenType.StartObject:
                    case JsonTokenType.EndObject:
                    case JsonTokenType.StartArray:
                    case JsonTokenType.EndArray:
                        break;
                    case JsonTokenType.PropertyName:
                        break;
                    case JsonTokenType.Value:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static void Output(string str)
        {
            if (!OutputResults) return;
            Console.WriteLine(str);
        }
    }
}
