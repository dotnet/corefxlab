using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Utf8;
using Newtonsoft.Json;
using JsonReader = System.Text.Json.JsonReader;

namespace Json.Net.Tests
{
    internal class JsonNetComparison
    {
        private const int NumberOfIterations = 100;
        private const int NumberOfSamples = 10;

        private const bool OutputResults = true;
        private const bool OutputJsonData = false;

        private static readonly Stopwatch Timer = new Stopwatch();
        private static readonly List<long> TimingResultsJsonNet = new List<long>();
        private static readonly List<long> TimingResultsJsonReader = new List<long>();

        private static void Main()
        {
            for (var i = 0; i < NumberOfSamples; i++)
            {
                RunTest();
            }
            
            foreach (var res in TimingResultsJsonNet)
            {
                Output(res.ToString());
            }

            foreach (var res in TimingResultsJsonReader)
            {
                Output(res.ToString());
            }
            //Console.Read();
        }

        private static void RunTest()
        {
            Output("====== TEST ReadProjectLockJson ======");
            JsonNetReaderHelper(new StringReader(TestJson.ProjectLockJson), OutputJsonData);
                // Do not test first iteration
            RunTestJsonNet(TestJson.ProjectLockJson, OutputJsonData);
            JsonReaderHelper(new Utf8String(TestJson.ProjectLockJson), OutputJsonData); // Do not test first iteration
            RunTestJsonReader(TestJson.ProjectLockJson, OutputJsonData);
        }

        private static void RunTestJsonNet(string str, bool output)
        {
            var utf8Str = new Utf8String(str);
            Timer.Restart();
            for (var i = 0; i < NumberOfIterations; i++)
            {
                JsonNetReaderHelper(new StringReader(utf8Str.ToString()), output);
            }
            TimingResultsJsonNet.Add(Timer.ElapsedMilliseconds);
        }

        private static void JsonNetReaderHelper(TextReader str, bool output)
        {
            var reader = new JsonTextReader(str);

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    // ReSharper disable once UnusedVariable
                    var x = reader.TokenType;
                    // ReSharper disable once UnusedVariable
                    var y = reader.Value;
                    if (output) Console.WriteLine(y);
                }
                else
                {
                    // ReSharper disable once UnusedVariable
                    var z = reader.TokenType;
                    if (output) Console.WriteLine(z);
                }
            }
        }

        private static void RunTestJsonReader(string str, bool output)
        {
            var utf8Str = new Utf8String(str);
            Timer.Restart();
            for (var i = 0; i < NumberOfIterations; i++)
            {
                JsonReaderHelper(utf8Str, output);
            }
            TimingResultsJsonReader.Add(Timer.ElapsedMilliseconds);
        }
        
        private static void JsonReaderHelper(Utf8String str, bool output)
        {
            var reader = new JsonReader(str);
            while (reader.Read())
            {
                var tokenType = reader.TokenType;
                switch (tokenType)
                {
                    case JsonReader.JsonTokenType.ObjectStart:
                    case JsonReader.JsonTokenType.ObjectEnd:
                    case JsonReader.JsonTokenType.ArrayStart:
                    case JsonReader.JsonTokenType.ArrayEnd:
                        if (output) Console.WriteLine(tokenType);
                        break;
                    case JsonReader.JsonTokenType.Property:
                        var name = reader.GetName();
                        if (output) Console.WriteLine(name);
                        var x = reader.GetValue();
                        if (output) Console.WriteLine(x);
                        break;
                    case JsonReader.JsonTokenType.Value:
                        x = reader.GetValue();
                        if (output) Console.WriteLine(x);
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
