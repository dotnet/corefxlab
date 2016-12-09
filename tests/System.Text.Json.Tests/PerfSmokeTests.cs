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

        [Fact, ActiveIssue(411)]
        public void ReadBasicJson()
        {
            Output("====== TEST ReadBasicJson ======");
            ReadJsonHelper(TestJson.BasicJson); // Do not test first iteration
            RunTest(TestJson.BasicJson);
        }

        [Fact, ActiveIssue(411)]
        public void ReadProjectLockJson()
        {
            Output("====== TEST ReadProjectLockJson ======");
            ReadJsonHelper(TestJson.ProjectLockJson); // Do not test first iteration
            RunTest(TestJson.ProjectLockJson);
        }

        [Fact]
        public void ReadHeavyNestedJson()
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
            var reader = new JsonReader(jsonStr);
            while (reader.Read())
            {
                var tokenType = reader.TokenType;
                switch (tokenType)
                {
                    case JsonReader.JsonTokenType.ObjectStart:
                    case JsonReader.JsonTokenType.ObjectEnd:
                    case JsonReader.JsonTokenType.ArrayStart:
                    case JsonReader.JsonTokenType.ArrayEnd:
                        break;
                    case JsonReader.JsonTokenType.Property:
                        // ReSharper disable once UnusedVariable
                        var name = reader.GetName();
                        var value = reader.GetValue();
                        break;
                    case JsonReader.JsonTokenType.Value:
                        value = reader.GetValue();
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