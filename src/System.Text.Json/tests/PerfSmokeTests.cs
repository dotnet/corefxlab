using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Tests.Resources;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace System.Text.Json.Tests
{
    public class PerfSmokeTests
    {
        private static readonly Stopwatch Timer = new Stopwatch();
        private const bool Collect = false;
        private const int MemoryToleranceFactor = 2;
        private const int IncreaseIterationsBy = 100;
        private const int ExpectedMemoryBenchMark = 3000000;

        [Fact]
        public void ReadBasicJson()
        {
            Console.WriteLine("====== TEST ReadBasicJson ======");
            RunTest(TestJson.BasicJson, 5, 3, 1, ExpectedMemoryBenchMark);
        }

        [Fact]
        public void ReadProjectLockJson()
        {
            Console.WriteLine("====== TEST ReadProjectLockJson ======");
            RunTest(TestJson.ProjectLockJson, 5, 3, 1000, ExpectedMemoryBenchMark*2);
        }

        [Fact]
        public void ReadHeavyNestedJson()
        {
            Console.WriteLine("====== TEST ReadHeavyNestedJson ======");
            RunTest(TestJson.HeavyNestedJson, 5, 3, 15, ExpectedMemoryBenchMark*2);
        }

        private static void RunTest(string jsonStr, int numIncrements, int numSamples, int runTimeFactor,
            int memoryBenchmark)
        {
            var timeResultsRead = new List<double>();
            var memoryResultsRead = new List<double>();

            for (var k = 0; k < numIncrements; k++)
            {
                var numIterations = IncreaseIterationsBy*(k + 1);
                var readTimeBenchMark = numIterations*runTimeFactor;

                var timeIterReadResults = new List<long>();
                var memoryIterReadResults = new List<long>();

                for (var j = 0; j < numSamples; j++)
                {
                    Timer.Restart();
                    for (var i = 0; i < numIterations; i++)
                    {
                        // ReSharper disable once UnusedVariable
                        var json = ReadJson(jsonStr);
                    }
                    var time = Timer.ElapsedMilliseconds;
                    var memory = GC.GetTotalMemory(Collect);
                    timeIterReadResults.Add(time);
                    memoryIterReadResults.Add(memory);
                    Assert.True(time < readTimeBenchMark);
                    Assert.True(memory < memoryBenchmark*MemoryToleranceFactor);
                }

                timeResultsRead.Add(timeIterReadResults.Average());
                memoryResultsRead.Add(memoryIterReadResults.Average());
            }

            foreach (var res in timeResultsRead)
            {
                Console.WriteLine(res);
            }

            foreach (var res in memoryResultsRead)
            {
                Console.WriteLine(res);
            }
        }

        private static Json ReadJson(string jsonString)
        {
            var json = new Json();
            if (string.IsNullOrEmpty(jsonString))
            {
                return json;
            }

            var jsonReader = new JsonReader(jsonString);
            var jsonObjectMain = new Object();
            var jsonMembersMain = new List<Members>();

            ReadJsonHelper(jsonReader, jsonMembersMain);

            jsonObjectMain.Members = jsonMembersMain;
            json.Object = jsonObjectMain;

            return json;
        }

        private static void ReadJsonHelper(JsonReader jsonReader, ICollection<Members> jsonMembersMain)
        {
            Array jsonArray = null;
            List<Pair> jsonPairs = null;
            List<Value> jsonValues = null;

            while (jsonReader.Read())
            {
                switch (jsonReader.TokenType)
                {
                    case JsonReader.JsonTokenType.Start:
                        break;
                    case JsonReader.JsonTokenType.ObjectStart:
                        jsonPairs = new List<Pair>();
                        break;
                    case JsonReader.JsonTokenType.ObjectEnd:
                        if (jsonPairs != null)
                        {
                            var jsonMembers = new Members {Pairs = jsonPairs};
                            jsonMembersMain.Add(jsonMembers);
                        }
                        break;
                    case JsonReader.JsonTokenType.ArrayStart:
                        jsonArray = new Array();
                        jsonValues = new List<Value>();
                        break;
                    case JsonReader.JsonTokenType.ArrayEnd:
                        if (jsonArray != null)
                        {
                            var jsonElements = new Elements {Values = jsonValues};
                            jsonArray.Elements = new List<Elements> {jsonElements};
                        }
                        break;
                    case JsonReader.JsonTokenType.Pair:
                        var pair = new Pair
                        {
                            Name = (string) jsonReader.ReadString(),
                            Value = GetValue(ref jsonReader)
                        };
                        if (jsonPairs != null) jsonPairs.Add(pair);
                        break;
                    case JsonReader.JsonTokenType.Value:
                        if (jsonValues != null) jsonValues.Add(GetValue(ref jsonReader));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static Value GetValue(ref JsonReader jsonReader)
        {
            var value = new Value {Type = (Value.ValueType) jsonReader.GetValueType()};
            var obj = jsonReader.ReadValue();
            switch (value.Type)
            {
                case Value.ValueType.String:
                    value.StringValue = obj.ToString();
                    break;
                case Value.ValueType.Number:
                    value.NumberValue = (double) obj;
                    break;
                case Value.ValueType.True:
                    break;
                case Value.ValueType.False:
                    break;
                case Value.ValueType.Null:
                    break;
                case Value.ValueType.Object:
                    value.ObjectValue = ReadObject(ref jsonReader);
                    break;
                case Value.ValueType.Array:
                    value.ArrayValue = ReadArray(ref jsonReader);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return value;
        }

        private static Object ReadObject(ref JsonReader jsonReader)
        {
            var jsonObject = new Object();
            var jsonMembersList = new List<Members>();
            List<Pair> jsonPairs = null;

            while (jsonReader.Read())
            {
                switch (jsonReader.TokenType)
                {
                    case JsonReader.JsonTokenType.Start:
                        break;
                    case JsonReader.JsonTokenType.ObjectStart:
                        jsonPairs = new List<Pair>();
                        break;
                    case JsonReader.JsonTokenType.ObjectEnd:
                        if (jsonPairs != null)
                        {
                            var jsonMembers = new Members {Pairs = jsonPairs};
                            jsonMembersList.Add(jsonMembers);
                            jsonObject.Members = jsonMembersList;
                            return jsonObject;
                        }
                        break;
                    case JsonReader.JsonTokenType.ArrayStart:
                        break;
                    case JsonReader.JsonTokenType.ArrayEnd:
                        break;
                    case JsonReader.JsonTokenType.Pair:
                        var pair = new Pair
                        {
                            Name = (string) jsonReader.ReadString(),
                            Value = GetValue(ref jsonReader)
                        };
                        if (jsonPairs != null) jsonPairs.Add(pair);
                        break;
                    case JsonReader.JsonTokenType.Value:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            throw new FormatException("Json object was started but never ended.");
        }

        private static Array ReadArray(ref JsonReader jsonReader)
        {
            Array jsonArray = null;
            List<Value> jsonValues = null;

            while (jsonReader.Read())
            {
                switch (jsonReader.TokenType)
                {
                    case JsonReader.JsonTokenType.Start:
                        break;
                    case JsonReader.JsonTokenType.ObjectStart:
                        break;
                    case JsonReader.JsonTokenType.ObjectEnd:
                        break;
                    case JsonReader.JsonTokenType.ArrayStart:
                        jsonArray = new Array();
                        jsonValues = new List<Value>();
                        break;
                    case JsonReader.JsonTokenType.ArrayEnd:
                        if (jsonArray != null)
                        {
                            var jsonElements = new Elements {Values = jsonValues};
                            jsonArray.Elements = new List<Elements> {jsonElements};
                            return jsonArray;
                        }
                        break;
                    case JsonReader.JsonTokenType.Pair:
                        break;
                    case JsonReader.JsonTokenType.Value:
                        if (jsonValues != null) jsonValues.Add(GetValue(ref jsonReader));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            throw new FormatException("Json array was started but never ended.");
        }
    }
}
