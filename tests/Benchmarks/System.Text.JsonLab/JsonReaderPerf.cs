// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;
using Benchmarks;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;

namespace System.Text.JsonLab.Benchmarks
{
    public enum TestCaseType
    {
        HelloWorld,
        Basic,
        BasicLargeNum,
        SpecialNumForm,
        SpecialStrings,
        ProjectLockJson,
        FullSchema1,
        FullSchema2,
        DeepTree,
        BroadTree,
        LotsOfNumbers,
        LotsOfStrings,
        Json300B,
        Json3KB,
        Json30KB,
        Json300KB,
        Json3MB,
    }

    [MemoryDiagnoser]
    public class JsonReaderPerf
    {
        private string _jsonString;
        private byte[] _dataUtf8;
        private byte[] _dataUtf16;
        private MemoryStream _stream;
        private StreamReader _reader;

        [Params(true, false)]
        public bool IsUTF8Encoded;

        [ParamsSource(nameof(TestCaseValues))]
        public TestCaseType TestCase;

        public static IEnumerable<TestCaseType> TestCaseValues() => (IEnumerable<TestCaseType>)Enum.GetValues(typeof(TestCaseType));

        [GlobalSetup]
        public void Setup()
        {
            switch (TestCase)
            {
                case TestCaseType.HelloWorld:
                    _jsonString = JsonStrings.HelloWorld;
                    break;
                case TestCaseType.Basic:
                    _jsonString = JsonStrings.BasicJson;
                    break;
                case TestCaseType.BasicLargeNum:
                    _jsonString = JsonStrings.BasicJsonWithLargeNum;
                    break;
                case TestCaseType.SpecialNumForm:
                    _jsonString = JsonStrings.JsonWithSpecialNumFormat;
                    break;
                //case TestCaseType.SpecialStrings:
                //    _jsonString = JsonStrings.JsonWithSpecialStrings;
                //    break;
                case TestCaseType.ProjectLockJson:
                    _jsonString = JsonStrings.ProjectLockJson;
                    break;
                case TestCaseType.FullSchema1:
                    _jsonString = JsonStrings.FullJsonSchema1;
                    break;
                case TestCaseType.FullSchema2:
                    _jsonString = JsonStrings.FullJsonSchema2;
                    break;
                case TestCaseType.DeepTree:
                    _jsonString = JsonStrings.DeepTree;
                    break;
                case TestCaseType.BroadTree:
                    _jsonString = JsonStrings.BroadTree;
                    break;
                case TestCaseType.LotsOfNumbers:
                    _jsonString = JsonStrings.LotsOfNumbers;
                    break;
                case TestCaseType.LotsOfStrings:
                    _jsonString = JsonStrings.LotsOfStrings;
                    break;
                case TestCaseType.Json300B:
                    _jsonString = JsonStrings.Json300B;
                    break;
                case TestCaseType.Json3KB:
                    _jsonString = JsonStrings.Json3KB;
                    break;
                case TestCaseType.Json30KB:
                    _jsonString = JsonStrings.Json30KB;
                    break;
                case TestCaseType.Json300KB:
                    _jsonString = JsonStrings.Json300KB;
                    break;
                case TestCaseType.Json3MB:
                    _jsonString = JsonStrings.Json3MB;
                    break;
            }

            if (IsUTF8Encoded)
            {
                _dataUtf8 = Encoding.UTF8.GetBytes(_jsonString);
                _stream = new MemoryStream(_dataUtf8);
                _reader = new StreamReader(_stream, Encoding.UTF8, false, 1024, true);
            }
            else
            {
                _dataUtf16 = Encoding.Unicode.GetBytes(_jsonString);
            }
        }

        [Benchmark(Baseline = true)]
        public void ReaderNewtonsoftReaderEmptyLoop()
        {
            if (IsUTF8Encoded)
            {
                _stream.Seek(0, SeekOrigin.Begin);
                using (var json = new Newtonsoft.Json.JsonTextReader(_reader))
                    while (json.Read()) ;
            }
            else
            {
                var json = new Newtonsoft.Json.JsonTextReader(new StringReader(_jsonString));
                while (json.Read()) ;
            }
        }

        [Benchmark]
        public string ReaderNewtonsoftReaderReturnString()
        {
            StringBuilder sb = new StringBuilder();
            if (IsUTF8Encoded)
            {
                _stream.Seek(0, SeekOrigin.Begin);
                using (var json = new Newtonsoft.Json.JsonTextReader(_reader))
                    while (json.Read())
                    {
                        if (json.Value != null)
                        {
                            sb.Append(json.Value + ", ");
                        }
                    }
            }
            else
            {
                var json = new Newtonsoft.Json.JsonTextReader(new StringReader(_jsonString));
                while (json.Read())
                {
                    if (json.Value != null)
                    {
                        sb.Append(json.Value + ", ");
                    }
                }
            }
            return sb.ToString();
        }

        [Benchmark]
        public void ReaderSystemTextJsonLabEmptyLoop()
        {
            if (IsUTF8Encoded)
            {
                var json = new JsonReader(_dataUtf8, SymbolTable.InvariantUtf8);
                while (json.Read()) ;
            }
            else
            {
                var json = new JsonReader(_dataUtf16, SymbolTable.InvariantUtf16);
                while (json.Read()) ;
            }
        }

        [Benchmark]
        public byte[] ReaderSystemTextJsonLabReturnBytes()
        {
            if (IsUTF8Encoded)
            {
                byte[] outputArray = new byte[_dataUtf8.Length];
                Span<byte> destination = outputArray;
                var json = new JsonReader(_dataUtf8, SymbolTable.InvariantUtf8);
                while (json.Read())
                {
                    var tokenType = json.TokenType;
                    switch (tokenType)
                    {
                        case JsonTokenType.PropertyName:
                            json.Value.CopyTo(destination);
                            destination[json.Value.Length] = (byte)',';
                            destination[json.Value.Length + 1] = (byte)' ';
                            destination = destination.Slice(json.Value.Length + 2);
                            break;
                        case JsonTokenType.Value:
                            var valueType = json.ValueType;

                            switch (valueType)
                            {
                                case JsonValueType.True:
                                    destination[0] = (byte)'T';
                                    destination[1] = (byte)'r';
                                    destination[2] = (byte)'u';
                                    destination[3] = (byte)'e';
                                    destination = destination.Slice(4);
                                    break;
                                case JsonValueType.False:
                                    destination[0] = (byte)'F';
                                    destination[1] = (byte)'a';
                                    destination[2] = (byte)'l';
                                    destination[3] = (byte)'s';
                                    destination[4] = (byte)'e';
                                    destination = destination.Slice(5);
                                    break;
                            }

                            json.Value.CopyTo(destination);
                            destination[json.Value.Length] = (byte)',';
                            destination[json.Value.Length + 1] = (byte)' ';
                            destination = destination.Slice(json.Value.Length + 2);
                            break;
                        default:
                            break;
                    }
                }
                return outputArray;
            }
            else
            {
                byte[] outputArray = new byte[_dataUtf16.Length];
                Span<byte> destination = outputArray;
                var json = new JsonReader(_dataUtf16, SymbolTable.InvariantUtf16);
                while (json.Read())
                {
                    var tokenType = json.TokenType;
                    switch (tokenType)
                    {
                        case JsonTokenType.PropertyName:
                            json.Value.CopyTo(destination);
                            destination[json.Value.Length] = (byte)',';
                            destination[json.Value.Length + 2] = (byte)' ';
                            destination = destination.Slice(json.Value.Length + 4);
                            break;
                        case JsonTokenType.Value:
                            var valueType = json.ValueType;

                            switch (valueType)
                            {
                                case JsonValueType.True:
                                    destination[0] = (byte)'T';
                                    destination[2] = (byte)'r';
                                    destination[4] = (byte)'u';
                                    destination[6] = (byte)'e';
                                    destination = destination.Slice(8);
                                    break;
                                case JsonValueType.False:
                                    destination[0] = (byte)'F';
                                    destination[2] = (byte)'a';
                                    destination[4] = (byte)'l';
                                    destination[6] = (byte)'s';
                                    destination[8] = (byte)'e';
                                    destination = destination.Slice(10);
                                    break;
                            }

                            json.Value.CopyTo(destination);
                            destination[json.Value.Length] = (byte)',';
                            destination[json.Value.Length + 2] = (byte)' ';
                            destination = destination.Slice(json.Value.Length + 4);
                            break;
                        default:
                            break;
                    }
                }
                return outputArray;
            }
        }
    }
}
