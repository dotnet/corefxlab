// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using Benchmarks;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;

namespace System.Text.JsonLab.Benchmarks
{
    // Keep the JsonStrings resource names in sync with TestCaseType enum values.
    public enum TestCaseType
    {
        HelloWorld,
        BasicJson,
        BasicLargeNum,
        SpecialNumForm,
        ProjectLockJson,
        FullSchema1,
        FullSchema2,
        DeepTree,
        BroadTree,
        LotsOfNumbers,
        LotsOfStrings,
        Json400B,
        Json4KB,
        Json40KB,
        Json400KB
    }

    // Since there are 120 tests here (4 * 2 * 15), setting low values for the warmupCount, targetCount, and invocationCount
    [SimpleJob(-1, 3, 5, 1024)]
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
            _jsonString = JsonStrings.ResourceManager.GetString(TestCase.ToString());
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
            TextReader reader;
            if (IsUTF8Encoded)
            {
                _stream.Seek(0, SeekOrigin.Begin);
                reader = _reader;
            }
            else
            {
                reader = new StringReader(_jsonString);
            }
            var json = new Newtonsoft.Json.JsonTextReader(reader);
            while (json.Read()) ;
        }

        [Benchmark]
        public string ReaderNewtonsoftReaderReturnString()
        {
            TextReader reader;
            if (IsUTF8Encoded)
            {
                _stream.Seek(0, SeekOrigin.Begin);
                reader = _reader;
            }
            else
            {
                reader = new StringReader(_jsonString);
            }

            return NewtonsoftReturnStringHelper(reader);
        }

        [Benchmark]
        public void ReaderSystemTextJsonLabEmptyLoop()
        {
            JsonReader json = IsUTF8Encoded ?
                new JsonReader(_dataUtf8, SymbolTable.InvariantUtf8) :
                new JsonReader(_dataUtf16, SymbolTable.InvariantUtf16);

            while (json.Read()) ;
        }

        [Benchmark]
        public byte[] ReaderSystemTextJsonLabReturnBytes()
        {
            return IsUTF8Encoded ?
                    JsonLabReturnBytesHelper(_dataUtf8, SymbolTable.InvariantUtf8) :
                    JsonLabReturnBytesHelper(_dataUtf16, SymbolTable.InvariantUtf16, 2);
        }

        private string NewtonsoftReturnStringHelper(TextReader reader)
        {
            StringBuilder sb = new StringBuilder();
            var json = new Newtonsoft.Json.JsonTextReader(reader);
            while (json.Read())
            {
                if (json.Value != null)
                {
                    sb.Append(json.Value).Append(", ");
                }
            }

            return sb.ToString();
        }

        private byte[] JsonLabReturnBytesHelper(byte[] data, SymbolTable symbolTable, int utf16Multiplier = 1)
        {
            byte[] outputArray = new byte[data.Length];

            Span<byte> destination = outputArray;
            var json = new JsonReader(data, symbolTable);
            while (json.Read())
            {
                JsonTokenType tokenType = json.TokenType;
                ReadOnlySpan<byte> valueSpan = json.Value;
                switch (tokenType)
                {
                    case JsonTokenType.PropertyName:
                        valueSpan.CopyTo(destination);
                        destination[valueSpan.Length] = (byte)',';
                        destination[valueSpan.Length + 1 * utf16Multiplier] = (byte)' ';
                        destination = destination.Slice(valueSpan.Length + 2 * utf16Multiplier);
                        break;
                    case JsonTokenType.Value:
                        var valueType = json.ValueType;

                        switch (valueType)
                        {
                            case JsonValueType.True:
                                destination[0] = (byte)'T';
                                destination[1 * utf16Multiplier] = (byte)'r';
                                destination[2 * utf16Multiplier] = (byte)'u';
                                destination[3 * utf16Multiplier] = (byte)'e';
                                destination = destination.Slice(4 * utf16Multiplier);
                                break;
                            case JsonValueType.False:
                                destination[0] = (byte)'F';
                                destination[1 * utf16Multiplier] = (byte)'a';
                                destination[2 * utf16Multiplier] = (byte)'l';
                                destination[3 * utf16Multiplier] = (byte)'s';
                                destination[4 * utf16Multiplier] = (byte)'e';
                                destination = destination.Slice(5 * utf16Multiplier);
                                break;
                        }

                        valueSpan.CopyTo(destination);
                        destination[valueSpan.Length] = (byte)',';
                        destination[valueSpan.Length + 1 * utf16Multiplier] = (byte)' ';
                        destination = destination.Slice(valueSpan.Length + 2 * utf16Multiplier);
                        break;
                    default:
                        break;
                }
            }
            return outputArray;
        }
    }
}
