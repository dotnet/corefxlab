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
        private const int MemoryToleranceFactor = 5;

        // Keep < 10 before checking in. Otherwise tests take over 2 seconds to execute.
        private const int NumberOfIterations = 5;

        private const int NumberOfSamples = 3;
        private const int ExpectedMemoryBenchMark = 1000000;

        // ReSharper disable once ConvertToConstant.Local
        private static readonly bool OutputResults = true;

        [Fact]
        public void ReadBasicJson()
        {
            Output("====== TEST ReadBasicJson ======");
            ReadJsonHelper(TestJson.BasicJson);   // Do not test first iteration
            RunTest(TestJson.BasicJson, 10 + NumberOfIterations, ExpectedMemoryBenchMark*MemoryToleranceFactor);
        }

        [Fact]
        public void ReadProjectLockJson()
        {
            Output("====== TEST ReadProjectLockJson ======");
            ReadJsonHelper(TestJson.ProjectLockJson);   // Do not test first iteration
            RunTest(TestJson.ProjectLockJson, 10 + NumberOfIterations*100, ExpectedMemoryBenchMark*MemoryToleranceFactor);
        }

        [Fact]
        public void ReadHeavyNestedJson()
        {
            Output("====== TEST ReadHeavyNestedJson ======");
            ReadJsonHelper(TestJson.HeavyNestedJson);   // Do not test first iteration
            RunTest(TestJson.HeavyNestedJson, 10 + NumberOfIterations*2, ExpectedMemoryBenchMark*MemoryToleranceFactor);
        }

        private static void RunTest(string jsonStr, int timeBenchmark, int memoryBenchmark)
        {
            GC.Collect();
            var timeIterReadResults = new List<long>();
            var memoryIterReadResults = new List<long>();

            for (var j = 0; j < NumberOfSamples; j++)
            {
                Timer.Restart();
                for (var i = 0; i < NumberOfIterations; i++)
                {
                    ReadJsonHelper(jsonStr);
                }
                var time = Timer.ElapsedMilliseconds;
                var memory = GC.GetTotalMemory(false);
                timeIterReadResults.Add(time);
                memoryIterReadResults.Add(memory);
                Assert.True(time < timeBenchmark);
                Assert.True(memory < memoryBenchmark);
            }
            Output(timeIterReadResults.Average().ToString(CultureInfo.InvariantCulture));
            Output((memoryIterReadResults.Average()/1000).ToString(CultureInfo.InvariantCulture));
            GC.Collect();
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
                        OutputValue(ref reader);
                        break;
                    case JsonReader.JsonTokenType.Value:
                        OutputValue(ref reader);
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

        private static void OutputValue(ref JsonReader reader)
        {
            switch (reader.GetJsonValueType())
            {
                case JsonReader.JsonValueType.String:
                case JsonReader.JsonValueType.Number:
                case JsonReader.JsonValueType.True:
                case JsonReader.JsonValueType.False:
                case JsonReader.JsonValueType.Null:
                    // ReSharper disable once UnusedVariable
                    var value = reader.GetValue();
                    break;
                case JsonReader.JsonValueType.Object:
                    break;
                case JsonReader.JsonValueType.Array:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}