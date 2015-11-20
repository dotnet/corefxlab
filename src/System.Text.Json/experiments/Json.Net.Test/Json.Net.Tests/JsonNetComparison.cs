using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Utf8;
using Newtonsoft.Json;
using JsonReader = System.Text.Json.JsonReader;
using JsonWriter = System.Text.Json.JsonWriter;
using System.Text.Formatting;
using System.Text;
using System.IO;

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
            RunWriterTest();
            RunReaderTest();
            //Console.Read();
        }

        private static void RunWriterTest()
        {
            Output("====== TEST Write ======");
            for (var i = 0; i < NumberOfSamples; i++)
            {
                JsonNetWriterHelper(OutputJsonData);          // Do not test first iteration
                RunWriterTestJsonNet();

                JsonWriterHelper(OutputJsonData);             // Do not test first iteration
                RunWriterTestJson();
            }

            Output("Json.NET Timing Results");
            foreach (var res in TimingResultsJsonNet)
            {
                Output(res.ToString());
            }

            Output("System.Text.Json Timing Results");
            foreach (var res in TimingResultsJsonReader)
            {
                Output(res.ToString());
            }

            TimingResultsJsonNet.Clear();
            TimingResultsJsonReader.Clear();
        }

        private static void RunWriterTestJsonNet()
        {
            Timer.Restart();
            for (var i = 0; i < NumberOfIterations*NumberOfIterations; i++)
            {
                JsonNetWriterHelper(OutputJsonData);
            }
            TimingResultsJsonNet.Add(Timer.ElapsedMilliseconds);
        }

        private static void JsonNetWriterHelper(bool output)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            var writer = new JsonTextWriter(sw);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("CPU");
            writer.WriteValue("Intel");
            writer.WritePropertyName("PSU");
            writer.WriteValue("500W");
            writer.WritePropertyName("Drives");
            writer.WriteStartArray();
            writer.WriteValue("DVD read/writer");
            writer.WriteValue("500 gigabyte hard drive");
            writer.WriteValue("200 gigabype hard drive");
            writer.WriteEnd();
            writer.WriteEndObject();
            if (output) Console.WriteLine(sw.ToString());
        }

        private static void RunWriterTestJson()
        {
            Timer.Restart();
            for (var i = 0; i < NumberOfIterations*NumberOfIterations; i++)
            {
                JsonWriterHelper(OutputJsonData);
            }
            TimingResultsJsonReader.Add(Timer.ElapsedMilliseconds);
        }

        private static void JsonWriterHelper(bool output)
        {
            var buffer = new byte[1024];
            var stream = new MemoryStream(buffer);
            var writer = new JsonWriter(stream, FormattingData.Encoding.Utf8, prettyPrint: true);
            writer.WriteObjectStart();
            writer.WriteAttribute("CPU", "Intel");
            writer.WriteAttribute("PSU", "500W");
            writer.WriteMember("Drives");
            writer.WriteArrayStart();
            writer.WriteString("DVD read/writer");
            writer.WriteString("500 gigabyte hard drive");
            writer.WriteString("200 gigabype hard drive");
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
            if (output) Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, (int)stream.Position));
        }

        private static void RunReaderTest()
        {
            Output("====== TEST Read ProjectLockJson ======");
            for (var i = 0; i < NumberOfSamples; i++)
            {
                JsonNetReaderHelper(new StringReader(TestJson.ProjectLockJson), OutputJsonData);
                // Do not test first iteration
                RunTestJsonNet(TestJson.ProjectLockJson, OutputJsonData);
                JsonReaderHelper(new Utf8String(TestJson.ProjectLockJson), OutputJsonData); // Do not test first iteration
                RunTestJsonReader(TestJson.ProjectLockJson, OutputJsonData);
            }

            Output("Json.NET Timing Results");
            foreach (var res in TimingResultsJsonNet)
            {
                Output(res.ToString());
            }

            Output("System.Text.Json Timing Results");
            foreach (var res in TimingResultsJsonReader)
            {
                Output(res.ToString());
            }
            
            TimingResultsJsonNet.Clear();
            TimingResultsJsonReader.Clear();
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
