using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Utf8;
using Newtonsoft.Json;
using JsonReader = System.Text.Json.JsonReader;
using JsonWriter = System.Text.Json.JsonWriter;
using JsonParser = System.Text.Json.JsonParser;
using System.Text.Formatting;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Json.Net.Tests
{
    internal class JsonNetComparison
    {
        private const int NumberOfIterations = 10;
        private const int NumberOfSamples = 10;

        private const bool OutputResults = true;
        private const bool OutputJsonData = false;

        private static readonly Stopwatch Timer = new Stopwatch();
        private static readonly List<long> TimingResultsJsonNet = new List<long>();
        private static readonly List<long> TimingResultsJsonReader = new List<long>();

        private static void Main()
        {
            RunParserTest();
            RunWriterTest();
            RunReaderTest();
            //Console.Read();
        }


        private static void RunReaderTest()
        {
            Output("====== TEST Read ======");

            // Do not test first iteration
            ReaderTestJsonNet(TestJson.Json3KB, false);
            ReaderTestJsonNet(TestJson.Json30KB, false);
            ReaderTestJsonNet(TestJson.Json300KB, false);
            ReaderTestJsonNet(TestJson.Json3MB, false);

            Output("Json.NET Timing Results");
            for (int i = 0; i < NumberOfSamples; i++)
            {
                ReaderTestJsonNet(TestJson.Json3KB, OutputResults);
                ReaderTestJsonNet(TestJson.Json30KB, OutputResults);
                ReaderTestJsonNet(TestJson.Json300KB, OutputResults);
                ReaderTestJsonNet(TestJson.Json3MB, OutputResults);
            }

            // Do not test first iteration
            ReaderTestSystemTextJson(TestJson.Json3KB, false);
            ReaderTestSystemTextJson(TestJson.Json30KB, false);
            ReaderTestSystemTextJson(TestJson.Json300KB, false);
            ReaderTestSystemTextJson(TestJson.Json3MB, false);

            Output("System.Text.Json Timing Results");
            for (int i = 0; i < NumberOfSamples; i++)
            {
                ReaderTestSystemTextJson(TestJson.Json3KB, OutputResults);
                ReaderTestSystemTextJson(TestJson.Json30KB, OutputResults);
                ReaderTestSystemTextJson(TestJson.Json300KB, OutputResults);
                ReaderTestSystemTextJson(TestJson.Json3MB, OutputResults);
            }
        }

        private static void ReaderTestJsonNet(string str, bool output)
        {
            Timer.Restart();
            for (var i = 0; i < NumberOfIterations; i++)
            {
                JsonNetReaderHelper(new StringReader(str), OutputJsonData);
            }
            if (output) Console.WriteLine(Timer.ElapsedTicks);
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

        private static void ReaderTestSystemTextJson(string str, bool output)
        {
            var utf8Str = new Utf8String(str);

            Timer.Restart();
            for (var i = 0; i < NumberOfIterations; i++)
            {
                JsonReaderHelper(utf8Str, OutputJsonData);
            }
            if (output) Console.WriteLine(Timer.ElapsedTicks);
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
                        var value = reader.GetValue();
                        if (output) Console.WriteLine(value);
                        break;
                    case JsonReader.JsonTokenType.Value:
                        value = reader.GetValue();
                        if (output) Console.WriteLine(value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static void RunParserTest()
        {
            Output("====== TEST Parse ======");

            // Do not test first iteration
            ParserTestJsonNet(TestJson.Json3KB, false);
            ParserTestJsonNet(TestJson.Json30KB, false);
            ParserTestJsonNet(TestJson.Json300KB, false);
            ParserTestJsonNet(TestJson.Json3MB, false);

            Output("Json.NET Timing Results");
            for (int i = 0; i < NumberOfSamples; i++)
            {
                ParserTestJsonNet(TestJson.Json3KB, OutputResults);
                ParserTestJsonNet(TestJson.Json30KB, OutputResults);
                ParserTestJsonNet(TestJson.Json300KB, OutputResults);
                ParserTestJsonNet(TestJson.Json3MB, OutputResults);
            }

            // Do not test first iteration
            ParserTestSystemTextJson(TestJson.Json3KB, 3, false);
            ParserTestSystemTextJson(TestJson.Json30KB, 30, false);
            ParserTestSystemTextJson(TestJson.Json300KB, 300, false);
            ParserTestSystemTextJson(TestJson.Json3MB, 3000, false);

            Output("System.Text.Json Timing Results");
            for (int i = 0; i < NumberOfSamples; i++)
            {
                ParserTestSystemTextJson(TestJson.Json3KB, 3, OutputResults);
                ParserTestSystemTextJson(TestJson.Json30KB, 30, OutputResults);
                ParserTestSystemTextJson(TestJson.Json300KB, 300, OutputResults);
                ParserTestSystemTextJson(TestJson.Json3MB, 3000, OutputResults);
            }
        }

        private static void ParserTestSystemTextJson(string str, int numElements, bool timeResults)
        {
            int strLength = str.Length;
            int byteLength = strLength * 2;
            var buffer = new byte[byteLength];
            for (var j = 0; j < strLength; j++)
            {
                buffer[j] = (byte)str[j];
            }

            SystemTextParserHelper(buffer, strLength, numElements, timeResults);
        }

        private static void SystemTextParserHelper(byte[] buffer, int strLength, int numElements, bool timeResults)
        {
            if (timeResults) Timer.Restart();
            var json = new JsonParser(buffer, strLength);
            var parseObject = json.Parse();
            if (timeResults) Console.WriteLine("Parse: " + Timer.ElapsedTicks);

            if (timeResults) Timer.Restart();
            for (int i = 0; i < numElements; i++)
            {
                var xElement = parseObject[i];
                var id = (Utf8String)xElement["_id"];
                var index = (int)xElement["index"];
                var guid = (Utf8String)xElement["guid"];
                var isActive = (bool)xElement["isActive"];
                var balance = (Utf8String)xElement["balance"];
                var picture = (Utf8String)xElement["picture"];
                var age = (int)xElement["age"];
                var eyeColor = (Utf8String)xElement["eyeColor"];
                var name = (Utf8String)xElement["name"];
                var gender = (Utf8String)xElement["gender"];
                var company = (Utf8String)xElement["company"];
                var email = (Utf8String)xElement["email"];
                var phone = (Utf8String)xElement["phone"];
                var address = (Utf8String)xElement["address"];
                var about = (Utf8String)xElement["about"];
                var registered = (Utf8String)xElement["registered"];
                var latitude = (double)xElement["latitude"];
                var longitude = (double)xElement["longitude"];
                var tags = xElement["tags"];
                var tags1 = (Utf8String)tags[0];
                var tags2 = (Utf8String)tags[1];
                var tags3 = (Utf8String)tags[2];
                var tags4 = (Utf8String)tags[3];
                var tags5 = (Utf8String)tags[4];
                var tags6 = (Utf8String)tags[5];
                var tags7 = (Utf8String)tags[6];
                var friends = xElement["friends"];
                var friend1 = friends[0];
                var friend2 = friends[1];
                var friend3 = friends[2];
                var id1 = (int)friend1["id"];
                var friendName1 = (Utf8String)friend1["name"];
                var id2 = (int)friend2["id"];
                var friendName2 = (Utf8String)friend2["name"];
                var id3 = (int)friend3["id"];
                var friendName3 = (Utf8String)friend3["name"];
                var greeting = (Utf8String)xElement["greeting"];
                var favoriteFruit = (Utf8String)xElement["favoriteFruit"];
            }
            if (timeResults) Console.WriteLine("Access: " + Timer.ElapsedTicks);
        }
        
        private static void ParserTestJsonNet(string str, bool timeResults)
        {
            if (timeResults) Timer.Restart();
            JArray parseObject = JArray.Parse(str);
            if (timeResults) Console.WriteLine("Parse: " + Timer.ElapsedTicks);

            if (timeResults) Timer.Restart();
            for (int i = 0; i < parseObject.Count; i++)
            {
                var xElement = parseObject[i];
                var id = (string)xElement["_id"];
                var index = (int)xElement["index"];
                var guid = (string)xElement["guid"];
                var isActive = (bool)xElement["isActive"];
                var balance = (string)xElement["balance"];
                var picture = (string)xElement["picture"];
                var age = (int)xElement["age"];
                var eyeColor = (string)xElement["eyeColor"];
                var name = (string)xElement["name"];
                var gender = (string)xElement["gender"];
                var company = (string)xElement["company"];
                var email = (string)xElement["email"];
                var phone = (string)xElement["phone"];
                var address = (string)xElement["address"];
                var about = (string)xElement["about"];
                var registered = (string)xElement["registered"];
                var latitude = (double)xElement["latitude"];
                var longitude = (double)xElement["longitude"];
                var tags = xElement["tags"];
                var tags1 = (string)tags[0];
                var tags2 = (string)tags[1];
                var tags3 = (string)tags[2];
                var tags4 = (string)tags[3];
                var tags5 = (string)tags[4];
                var tags6 = (string)tags[5];
                var tags7 = (string)tags[6];
                var friends = xElement["friends"];
                var friend1 = friends[0];
                var friend2 = friends[1];
                var friend3 = friends[2];
                var id1 = (int)friend1["id"];
                var friendName1 = (string)friend1["name"];
                var id2 = (int)friend2["id"];
                var friendName2 = (string)friend2["name"];
                var id3 = (int)friend3["id"];
                var friendName3 = (string)friend3["name"];
                var greeting = (string)xElement["greeting"];
                var favoriteFruit = (string)xElement["favoriteFruit"];
            }
            if (timeResults) Console.WriteLine("Access: " + Timer.ElapsedTicks);

        }

        private static void ParsingAccess3MBBreakdown(string str, bool timeResults)
        {
            int strLength = str.Length;
            int byteLength = strLength * 2;
            var buffer = new byte[byteLength];
            for (var j = 0; j < strLength; j++)
            {
                buffer[j] = (byte)str[j];
            }

            int iter = 10;

            var json = new JsonParser(buffer, strLength);
            var parseObject = json.Parse();

            var xElement = parseObject[1500];
            if (timeResults) Timer.Restart();
            for (int i = 0; i < iter; i++)
            {
                xElement = parseObject[1500];
            }
            if (timeResults) Console.WriteLine(Timer.ElapsedTicks);
            if (timeResults) Timer.Restart();

            var email = xElement["email"];
            if (timeResults) Timer.Restart();
            for (int i = 0; i < iter; i++)
            {
                email = xElement["email"];
            }
            if (timeResults) Console.WriteLine(Timer.ElapsedTicks);
            if (timeResults) Timer.Restart();

            var emailString = (Utf8String)email;
            if (timeResults) Timer.Restart();
            for (int i = 0; i < iter; i++)
            {
                emailString = (Utf8String)email;
            }
            if (timeResults) Console.WriteLine(Timer.ElapsedTicks);
            if (timeResults) Timer.Restart();

            var about = xElement["about"];
            if (timeResults) Timer.Restart();
            for (int i = 0; i < iter; i++)
            {
                about = xElement["about"];
            }
            if (timeResults) Console.WriteLine(Timer.ElapsedTicks);
            if (timeResults) Timer.Restart();

            var aboutString = (Utf8String)about;
            if (timeResults) Timer.Restart();
            for (int i = 0; i < iter; i++)
            {
                aboutString = (Utf8String)about;
            }
            if (timeResults) Console.WriteLine(Timer.ElapsedTicks);
            if (timeResults) Timer.Restart();

            var age = xElement["age"];
            if (timeResults) Timer.Restart();
            for (int i = 0; i < iter; i++)
            {
                age = xElement["age"];
            }
            if (timeResults) Console.WriteLine(Timer.ElapsedTicks);
            if (timeResults) Timer.Restart();

            var ageInt = (int)age;
            if (timeResults) Timer.Restart();
            for (int i = 0; i < iter; i++)
            {
                ageInt = (int)age;
            }
            if (timeResults) Console.WriteLine(Timer.ElapsedTicks);
            if (timeResults) Timer.Restart();

            var latitude = xElement["latitude"];
            if (timeResults) Timer.Restart();
            for (int i = 0; i < iter; i++)
            {
                latitude = xElement["latitude"];
            }
            if (timeResults) Console.WriteLine(Timer.ElapsedTicks);
            if (timeResults) Timer.Restart();

            var latitudeDouble = (double)latitude;
            if (timeResults) Timer.Restart();
            for (int i = 0; i < iter; i++)
            {
                latitudeDouble = (double)latitude;
            }
            if (timeResults) Console.WriteLine(Timer.ElapsedTicks);
            if (timeResults) Timer.Restart();

            var isActive = xElement["isActive"];
            if (timeResults) Timer.Restart();
            for (int i = 0; i < iter; i++)
            {
                isActive = xElement["isActive"];
            }
            if (timeResults) Console.WriteLine(Timer.ElapsedTicks);
            if (timeResults) Timer.Restart();

            var isActiveBool = (bool)isActive;
            if (timeResults) Timer.Restart();
            for (int i = 0; i < iter; i++)
            {
                isActiveBool = (bool)isActive;
            }
            if (timeResults) Console.WriteLine(Timer.ElapsedTicks);
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

        private static void Output(string str)
        {
            if (!OutputResults) return;
            Console.WriteLine(str);
        }
    }
}
