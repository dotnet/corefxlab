// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.CsProj;
using BenchmarkDotNet.Toolchains.DotNetCli;
using System.Buffers.Text;
using System.IO;
using System.Text.Formatting;

namespace System.Text.JsonLab.Benchmarks
{
    //[SimpleJob(invocationCount: 2_097_152, warmupCount: 5, targetCount: 10)]
    //[SimpleJob(invocationCount: 16_384, warmupCount: 5, targetCount: 10)]
    //[MemoryDiagnoser]
    //[DisassemblyDiagnoser(printPrologAndEpilog: true, recursiveDepth: 3)]
    //[SimpleJob(warmupCount: 5, targetCount: 10)]
    [Config(typeof(ConfigWithCustomEnvVars))]
    public class JsonWriterPerf
    {
        private class ConfigWithCustomEnvVars : ManualConfig
        {
            private const string JitNoInline = "COMPlus_TieredCompilation";

            public ConfigWithCustomEnvVars()
            {
                Add(Job.Core
                    .With(new[] { new EnvironmentVariable(JitNoInline, "0") })
                    .WithWarmupCount(3)
                    .WithTargetCount(5)
                    .With(CsProjCoreToolchain.From(NetCoreAppSettings.NetCoreApp30)));
            }
        }

        private static readonly byte[] Message = Encoding.UTF8.GetBytes("message");
        private static readonly byte[] HelloWorld = Encoding.UTF8.GetBytes("Hello, World!");
        private static readonly byte[] ExtraArray = Encoding.UTF8.GetBytes("ExtraArray");

        private static readonly byte[] First = Encoding.UTF8.GetBytes("first");
        private static readonly byte[] John = Encoding.UTF8.GetBytes("John");
        private static readonly byte[] FirNst = Encoding.UTF8.GetBytes("fir\nst");
        private static readonly byte[] JohNn = Encoding.UTF8.GetBytes("Joh\nn");

        private const int ExtraArraySize = 100;
        private const int BufferSize = 1024 + (ExtraArraySize * 64);

        private ArrayFormatterWrapper _arrayFormatterWrapper;
        private ArrayFormatter _arrayFormatter;
        private MemoryStream _memoryStream;
        private StreamWriter _streamWriter;

        private int[] _data;
        private byte[] _output;
        private long[] _longs;
        private int[] dataArray;
        private DateTime MyDate;

        [Params(false)]
        public bool Formatted;

        [Params(true)]
        public bool SkipValidation;

        [GlobalSetup]
        public void Setup()
        {
            _data = new int[ExtraArraySize];
            Random rand = new Random(42);

            for (int i = 0; i < ExtraArraySize; i++)
            {
                _data[i] = rand.Next(-10000, 10000);
            }

            var buffer = new byte[BufferSize];
            _memoryStream = new MemoryStream(buffer);
            _streamWriter = new StreamWriter(_memoryStream, new UTF8Encoding(false), BufferSize, true);
            _arrayFormatterWrapper = new ArrayFormatterWrapper(10000, SymbolTable.InvariantUtf8);
            _arrayFormatter = new ArrayFormatter(BufferSize, SymbolTable.InvariantUtf8);

            // To pass an initialBuffer to Utf8Json:
            // _output = new byte[BufferSize];
            _output = null;

            var random = new Random(42);
            const int numberOfItems = 10;

            _longs = new long[numberOfItems];
            _longs[0] = 0;
            _longs[1] = long.MaxValue;
            _longs[2] = long.MinValue;
            _longs[3] = 12345678901;
            _longs[4] = -12345678901;
            for (int i = 5; i < numberOfItems; i++)
            {
                long value = random.Next(int.MinValue, int.MaxValue);
                value += value < 0 ? int.MinValue : int.MaxValue;
                _longs[i] = value;
            }

            dataArray = new int[100];
            for (int i = 0; i < 100; i++)
                dataArray[i] = 12345;

            MyDate = DateTime.Now;
        }

        //[Benchmark]
        public void WriterSystemTextJsonValues()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Indented = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            json.WriteStartArray(Encoding.UTF8.GetBytes("numbers"), suppressEscaping: true);
            for (int i = 0; i < 100; i++)
                json.WriteNumberValue(12345);
            json.WriteEndArray();
            json.WriteEndObject();
            json.Flush();

            //using (var json = new Newtonsoft.Json.JsonTextWriter(GetWriter()))
            //{
            //    json.Formatting = Formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

            //    json.WriteStartArray();
            //    for (int i = 0; i < 100; i++)
            //        json.WriteValue(12345);
            //    json.WriteEndArray();
            //}
        }

        //[Benchmark]
        public void WriterSystemTextJsonArrayValues()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Indented = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            json.WriteNumberArray(Encoding.UTF8.GetBytes("numbers"), dataArray, suppressEscaping: true);
            json.WriteEndObject();
            json.Flush();

            //using (var json = new Newtonsoft.Json.JsonTextWriter(GetWriter()))
            //{
            //    json.Formatting = Formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

            //    json.WriteStartArray();
            //    for (int i = 0; i < 100; i++)
            //        json.WriteValue(12345);
            //    json.WriteEndArray();
            //}
        }

        //[Benchmark]
        /*public void WriterSystemTextJsonValues_withMethod()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Indented = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2(_arrayFormatterWrapper, state);

            json.WriteStartArray();
            for (int i = 0; i < 100; i++)
                json.WriteNumberValue_withMethod(12345);
            json.WriteEndArray();
            json.Flush();

            //using (var json = new Newtonsoft.Json.JsonTextWriter(GetWriter()))
            //{
            //    json.Formatting = Formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

            //    json.WriteStartArray();
            //    for (int i = 0; i < 100; i++)
            //        json.WriteValue(12345);
            //    json.WriteEndArray();
            //}
        }*/

        //[Benchmark]
        public void WritePropertyValueSuppressEscaping()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Indented = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            for (int i = 0; i < 100; i++)
                json.WriteString("first", "John", suppressEscaping: true);
            json.WriteEndObject();
            json.Flush();
        }

        //[Benchmark]
        public void WritePropertyValueEscapeUnnecessarily()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Indented = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            for (int i = 0; i < 100; i++)
                json.WriteString("first", "John", suppressEscaping: false);
            json.WriteEndObject();
            json.Flush();
        }

        //[Benchmark]
        public void WritePropertyValueEscapingRequiredLarger()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Indented = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            for (int i = 0; i < 100; i++)
                json.WriteString("fir\nstaaaa\naaaa", "Joh\nnaaaa\naaaa");
            json.WriteEndObject();
            json.Flush();
        }

        //[Benchmark]
        public void WritePropertyValueEscapingRequired()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Indented = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            for (int i = 0; i < 100; i++)
                json.WriteString("fir\nst", "Joh\nn");
            json.WriteEndObject();
            json.Flush();



            //using (var json = new Newtonsoft.Json.JsonTextWriter(GetWriter()))
            //{
            //    json.Formatting = Formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

            //    json.WriteStartObject();
            //    for (int i = 0; i < 100; i++)
            //    {
            //        json.WritePropertyName("first");
            //        json.WriteValue("John");
            //    }
            //    json.WriteEndObject();
            //}


            //json.WriteStartObject();
            ////json.WriteNumber("age", 42, suppressEscaping: true);
            //json.WriteString("first", "John", suppressEscaping: true);
            //json.WriteString("last", "Smith", suppressEscaping: true);
            ////json.WriteStartArray("phoneNumbers", suppressEscaping: true);
            ////json.WriteStringValue("425-000-1212", suppressEscaping: true);
            ////json.WriteStringValue("425-000-1213", suppressEscaping: true);
            ////json.WriteEndArray();
            ////json.WriteStartObject("address", suppressEscaping: true);
            //json.WriteString("street", "1 Microsoft Way", suppressEscaping: true);
            //json.WriteString("city", "Redmond", suppressEscaping: true);
            ////json.WriteNumber("zip", 98052, suppressEscaping: true);
            ////json.WriteEndObject();
            ////json.WriteArray(ExtraArray, _data.AsSpan(0, 10), suppressEscaping: true);
            //json.WriteEndObject();
            //json.Flush();
        }

        //[Benchmark]
        public void NewtonsoftSuppressEscaping()
        {
            using (var json = new Newtonsoft.Json.JsonTextWriter(GetWriter()))
            {
                json.Formatting = Formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

                json.WriteStartObject();
                for (int i = 0; i < 100; i++)
                {
                    json.WritePropertyName("first", escape: false);
                    json.WriteValue("John");
                }
                json.WriteEndObject();
            }
        }

        //[Benchmark]
        public void NewtonsoftEscapeUnnecessarily()
        {
            using (var json = new Newtonsoft.Json.JsonTextWriter(GetWriter()))
            {
                json.Formatting = Formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

                json.WriteStartObject();
                for (int i = 0; i < 100; i++)
                {
                    json.WritePropertyName("first", escape: true);
                    json.WriteValue("John");
                }
                json.WriteEndObject();
            }
        }

        //[Benchmark]
        public void NewtonsoftEscapingRequired()
        {
            using (var json = new Newtonsoft.Json.JsonTextWriter(GetWriter()))
            {
                json.Formatting = Formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

                json.WriteStartObject();
                for (int i = 0; i < 100; i++)
                {
                    json.WritePropertyName("fir\nst");
                    json.WriteValue("Joh\nn");
                }
                json.WriteEndObject();
            }
        }

        //[Benchmark (Baseline = true)]
        public void WriterSystemTextJsonBasicNoDefault()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Indented = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            for (int i = 0; i < 100; i++)
                json.WriteString("first", "John");
            json.WriteEndObject();
            json.Flush();
        }

        //[Benchmark]
        public void Utf8JsonWriteString()
        {
            global::Utf8Json.JsonWriter json = new global::Utf8Json.JsonWriter();

            json.WriteBeginObject();
            for (int i = 0; i < 100; i++)
            {
                json.WritePropertyName("fir\nst");
                json.WriteString("Joh\nn");
            }
            json.WriteEndObject();
        }

        //[Benchmark]
        public void Utf8JsonWriteStringRaw()
        {
            global::Utf8Json.JsonWriter json = new global::Utf8Json.JsonWriter();

            byte[] first = global::Utf8Json.JsonWriter.GetEncodedPropertyName("fir\nst");
            byte[] john = global::Utf8Json.JsonWriter.GetEncodedPropertyName("Joh\nn");

            json.WriteBeginObject();
            for (int i = 0; i < 100; i++)
            {
                json.WriteRaw(first);
                json.WriteNameSeparator();
                json.WriteRaw(john);
            }
            json.WriteEndObject();
        }

        //[Benchmark]
        public void WriteDateTimeUnescaped()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Indented = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            for (int i = 0; i < 100; i++)
                json.WriteString(First, MyDate, suppressEscaping: true);
            json.WriteEndObject();
            json.Flush();
        }

        //[Benchmark]
        public void WriteDateTimeUnescapedOverhead()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Indented = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            for (int i = 0; i < 100; i++)
                json.WriteString(First, MyDate, suppressEscaping: false);
            json.WriteEndObject();
            json.Flush();
        }

        //[Benchmark]
        public void WriteNullUnescapedUtf16()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Indented = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            for (int i = 0; i < 100; i++)
                json.WriteNull("first", suppressEscaping: true);
            json.WriteEndObject();
            json.Flush();
        }

        //[Benchmark]
        public void WriteBoolUnescapedUtf16()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Indented = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            for (int i = 0; i < 100; i++)
                json.WriteBoolean("first", value: true, suppressEscaping: true);
            json.WriteEndObject();
            json.Flush();
        }

        //[Benchmark]
        public void NewtonsoftBoolUnescaped()
        {
            using (var json = new Newtonsoft.Json.JsonTextWriter(GetWriter()))
            {
                json.Formatting = Formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

                json.WriteStartObject();
                for (int i = 0; i < 100; i++)
                {
                    json.WritePropertyName("first", escape: false);
                    json.WriteValue(true);
                }
                json.WriteEndObject();
            }
        }

        [Benchmark]
        public void WriteNumberUnescapedUtf16()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Indented = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            for (int i = 0; i < 100; i++)
                json.WriteNumber("first", value: 123456, suppressEscaping: true);
            json.WriteEndObject();
            json.Flush();
        }

        [Benchmark]
        public void NewtonsoftNumberUnescaped()
        {
            using (var json = new Newtonsoft.Json.JsonTextWriter(GetWriter()))
            {
                json.Formatting = Formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

                json.WriteStartObject();
                for (int i = 0; i < 100; i++)
                {
                    json.WritePropertyName("first", escape: false);
                    json.WriteValue(123456);
                }
                json.WriteEndObject();
            }
        }

        //[Benchmark]
        public void WriteNullUnescapedOverheadUtf16()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Indented = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            for (int i = 0; i < 100; i++)
                json.WriteNull("first", suppressEscaping: false);
            json.WriteEndObject();
            json.Flush();
        }

        //[Benchmark]
        public void NewtonsoftNullUnescaped()
        {
            using (var json = new Newtonsoft.Json.JsonTextWriter(GetWriter()))
            {
                json.Formatting = Formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

                json.WriteStartObject();
                for (int i = 0; i < 100; i++)
                {
                    json.WritePropertyName("first", escape: false);
                    json.WriteValue((object)null);
                }
                json.WriteEndObject();
            }
        }

        //[Benchmark]
        public void NewtonsoftNullUnescapedOverhead()
        {
            using (var json = new Newtonsoft.Json.JsonTextWriter(GetWriter()))
            {
                json.Formatting = Formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

                json.WriteStartObject();
                for (int i = 0; i < 100; i++)
                {
                    json.WritePropertyName("first", escape: true);
                    json.WriteValue((object)null);
                }
                json.WriteEndObject();
            }
        }

        //[Benchmark]
        public void WriteDateTimeUnescapedUtf16()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Indented = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            for (int i = 0; i < 100; i++)
                json.WriteString("first", MyDate, suppressEscaping: true);
            json.WriteEndObject();
            json.Flush();
        }

        //[Benchmark]
        public void WriteDateTimeUnescapedOverheadUtf16()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Indented = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            for (int i = 0; i < 100; i++)
                json.WriteString("first", MyDate, suppressEscaping: false);
            json.WriteEndObject();
            json.Flush();
        }


        //[Benchmark]
        public void NewtonsoftDateTimeUnescaped()
        {
            using (var json = new Newtonsoft.Json.JsonTextWriter(GetWriter()))
            {
                json.Formatting = Formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

                json.WriteStartObject();
                for (int i = 0; i < 100; i++)
                {
                    json.WritePropertyName("first", escape: false);
                    json.WriteValue(MyDate);
                }
                json.WriteEndObject();
            }
        }

        //[Benchmark]
        public void NewtonsoftDateTimeUnescapedOverhead()
        {
            using (var json = new Newtonsoft.Json.JsonTextWriter(GetWriter()))
            {
                json.Formatting = Formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

                json.WriteStartObject();
                for (int i = 0; i < 100; i++)
                {
                    json.WritePropertyName("first", escape: true);
                    json.WriteValue(MyDate);
                }
                json.WriteEndObject();
            }
        }

        //[Benchmark]
        public void WriterSystemTextJsonBasicUtf8Unescaped()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Indented = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            for (int i = 0; i < 100; i++)
                json.WriteString(First, John, suppressEscaping: true);
            json.WriteEndObject();
            json.Flush();
        }

        //[Benchmark]
        public void WriterSystemTextJsonBasicUtf8UnescapedSkip()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Indented = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            for (int i = 0; i < 100; i++)
                json.WriteStringSkipEscape(First, John);
            json.WriteEndObject();
            json.Flush();
        }

        //[Benchmark]
        public void WriterSystemTextJsonBasicUtf8UnescapedOverhead()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Indented = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            for (int i = 0; i < 100; i++)
                json.WriteString(First, John, suppressEscaping: false);
            json.WriteEndObject();
            json.Flush();
        }

        //[Benchmark]
        public void WriterSystemTextJsonBasicUtf8()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Indented = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            for (int i = 0; i < 100; i++)
                json.WriteString(FirNst, JohNn);
            json.WriteEndObject();
            json.Flush();

            //json.WriteStartObject();
            ////json.WriteNumber("age", 42, suppressEscaping: true);
            //json.WriteString("first", "John", suppressEscaping: true);
            //json.WriteString("last", "Smith", suppressEscaping: true);
            ////json.WriteStartArray("phoneNumbers", suppressEscaping: true);
            ////json.WriteStringValue("425-000-1212", suppressEscaping: true);
            ////json.WriteStringValue("425-000-1213", suppressEscaping: true);
            ////json.WriteEndArray();
            ////json.WriteStartObject("address", suppressEscaping: true);
            //json.WriteString("street", "1 Microsoft Way", suppressEscaping: true);
            //json.WriteString("city", "Redmond", suppressEscaping: true);
            ////json.WriteNumber("zip", 98052, suppressEscaping: true);
            ////json.WriteEndObject();
            ////json.WriteArray(ExtraArray, _data.AsSpan(0, 10), suppressEscaping: true);
            //json.WriteEndObject();
            //json.Flush();
        }

        //[Benchmark]
        public void WriterSystemTextJsonBasicUtf8NoDefault()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Indented = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            for (int i = 0; i < 100; i++)
                json.WriteString(First, John);
            json.WriteEndObject();
            json.Flush();
        }

        //[Benchmark(Baseline = true)]
        public void WriterNewtonsoftBasic()
        {
            WriterNewtonsoftBasic(Formatted, GetWriter(), _data.AsSpan(0, 10));
        }

        //[Benchmark]
        public void WriterUtf8JsonBasic()
        {
            WriterUtf8JsonBasic(_data.AsSpan(0, 10));
        }

        //[Benchmark]
        public void WriterSystemTextJsonHelloWorld()
        {
            _arrayFormatterWrapper.Clear();

            WriterSystemTextJsonHelloWorldUtf8(Formatted, _arrayFormatterWrapper);
        }

        //[Benchmark]
        public void WriteArray()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Indented = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            json.WriteNumberArray(Message, _longs, suppressEscaping: true);
            json.WriteEndObject();
            json.Flush();
        }

        //[Benchmark]
        public void WriteArrayLoop()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Indented = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            json.WriteStartArray(Message, suppressEscaping: true);
            for (int i = 0; i < _longs.Length; i++)
                json.WriteNumberValue(_longs[i]);
            json.WriteEndArray();
            json.WriteEndObject();
            json.Flush();
        }

        //[Benchmark]
        public void WriteArrayUtf8Json()
        {
            global::Utf8Json.JsonWriter json = new global::Utf8Json.JsonWriter(_output);

            json.WriteBeginObject();
            json.WritePropertyName("message");
            json.WriteBeginArray();
            for (int i = 0; i < _longs.Length; i++)
                json.WriteInt64(_longs[i]);
            json.WriteEndArray();
            json.WriteEndObject();
        }

        //[Benchmark]
        public void WriteArrayNewtonsoft()
        {
            using (var json = new Newtonsoft.Json.JsonTextWriter(GetWriter()))
            {
                json.Formatting = Formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

                json.WriteStartObject();
                json.WritePropertyName("message");
                json.WriteStartArray();
                for (var i = 0; i < _longs.Length; i++)
                {
                    json.WriteValue(_longs[i]);
                }
                json.WriteEnd();
                json.WriteEnd();
            }
        }

        //[Benchmark]
        public void WriterNewtonsoftHelloWorld()
        {
            WriterNewtonsoftHelloWorld(Formatted, GetWriter());
        }

        //[Benchmark]
        public void WriterUtf8JsonHelloWorld()
        {
            WriterUtf8JsonHelloWorldHelper(_output);
        }

        //[Benchmark]
        [Arguments(1)]
        [Arguments(2)]
        [Arguments(5)]
        [Arguments(10)]
        [Arguments(100)]
        [Arguments(1000)]
        [Arguments(10000)]
        [Arguments(100000)]
        [Arguments(1000000)]
        [Arguments(10000000)]
        public void WriterSystemTextJsonArrayOnly(int size)
        {
            _arrayFormatterWrapper.Clear();
            WriterSystemTextJsonArrayOnlyUtf8(Formatted, _arrayFormatterWrapper, _data.AsSpan(0, size));
        }

        //[Benchmark]
        [Arguments(1)]
        [Arguments(2)]
        [Arguments(5)]
        [Arguments(10)]
        [Arguments(100)]
        [Arguments(1000)]
        [Arguments(10000)]
        [Arguments(100000)]
        [Arguments(1000000)]
        [Arguments(10000000)]
        public void WriterUtf8JsonArrayOnly(int size)
        {
            WriterUtf8JsonArrayOnly(_data.AsSpan(0, size), _output);
        }

        private TextWriter GetWriter()
        {
            _memoryStream.Seek(0, SeekOrigin.Begin);
            return _streamWriter;
        }

        private static void WriterNewtonsoftBasic(bool formatted, TextWriter writer, ReadOnlySpan<int> data)
        {
            using (var json = new Newtonsoft.Json.JsonTextWriter(writer))
            {
                json.Formatting = formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

                json.WriteStartObject();
                json.WritePropertyName("age");
                json.WriteValue(42);
                json.WritePropertyName("first");
                json.WriteValue("John");
                json.WritePropertyName("last");
                json.WriteValue("Smith");
                json.WritePropertyName("phoneNumbers");
                json.WriteStartArray();
                json.WriteValue("425-000-1212");
                json.WriteValue("425-000-1213");
                json.WriteEnd();
                json.WritePropertyName("address");
                json.WriteStartObject();
                json.WritePropertyName("street");
                json.WriteValue("1 Microsoft Way");
                json.WritePropertyName("city");
                json.WriteValue("Redmond");
                json.WritePropertyName("zip");
                json.WriteValue(98052);
                json.WriteEnd();

                json.WritePropertyName("ExtraArray");
                json.WriteStartArray();
                for (var i = 0; i < data.Length; i++)
                {
                    json.WriteValue(data[i]);
                }
                json.WriteEnd();

                json.WriteEnd();
            }
        }

        private static void WriterUtf8JsonBasic(ReadOnlySpan<int> data)
        {
            global::Utf8Json.JsonWriter json = new global::Utf8Json.JsonWriter();

            json.WriteBeginObject();
            json.WritePropertyName("age");
            json.WriteInt32(42);
            json.WriteValueSeparator();
            json.WritePropertyName("first");
            json.WriteString("John");
            json.WriteValueSeparator();
            json.WritePropertyName("last");
            json.WriteString("Smith");
            json.WriteValueSeparator();
            json.WritePropertyName("phoneNumbers");
            json.WriteBeginArray();
            json.WriteString("425-000-1212");
            json.WriteValueSeparator();
            json.WriteString("425-000-1213");
            json.WriteEndArray();
            json.WriteValueSeparator();
            json.WritePropertyName("address");
            json.WriteBeginObject();
            json.WritePropertyName("street");
            json.WriteString("1 Microsoft Way");
            json.WriteValueSeparator();
            json.WritePropertyName("city");
            json.WriteString("Redmond");
            json.WriteValueSeparator();
            json.WritePropertyName("zip");
            json.WriteInt32(98052);
            json.WriteEndObject();
            json.WriteValueSeparator();

            json.WritePropertyName("ExtraArray");
            json.WriteBeginArray();
            for (var i = 0; i < data.Length - 1; i++)
            {
                json.WriteInt32(data[i]);
                json.WriteValueSeparator();
            }
            if (data.Length > 0)
                json.WriteInt32(data[data.Length - 1]);
            json.WriteEndArray();

            json.WriteEndObject();
        }

        private static void WriterSystemTextJsonHelloWorldUtf8(bool formatted, ArrayFormatterWrapper output)
        {
            var json = new Utf8JsonWriter<ArrayFormatterWrapper>(output, formatted);

            json.WriteObjectStart();
            json.WriteAttributeUtf8(Message, HelloWorld);
            json.WriteObjectEnd();
            json.Flush();
        }

        private static void WriterNewtonsoftHelloWorld(bool formatted, TextWriter writer)
        {
            using (var json = new Newtonsoft.Json.JsonTextWriter(writer))
            {
                json.Formatting = formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

                json.WriteStartObject();
                json.WritePropertyName("message");
                json.WriteValue("Hello, World!");
                json.WriteEnd();
            }
        }

        private static void WriterUtf8JsonHelloWorldHelper(byte[] output)
        {
            global::Utf8Json.JsonWriter json = new global::Utf8Json.JsonWriter(output);

            json.WriteBeginObject();
            json.WritePropertyName("message");
            json.WriteString("Hello, World!");
            json.WriteEndObject();
        }

        private static void WriterSystemTextJsonArrayOnlyUtf8(bool formatted, ArrayFormatterWrapper output, ReadOnlySpan<int> data)
        {
            var json = new Utf8JsonWriter<ArrayFormatterWrapper>(output, formatted);

            json.WriteArrayUtf8(ExtraArray, data);
            json.Flush();
        }

        private static void WriterUtf8JsonArrayOnly(ReadOnlySpan<int> data, byte[] output)
        {
            global::Utf8Json.JsonWriter json = new global::Utf8Json.JsonWriter(output);

            json.WriteBeginArray();
            json.WritePropertyName("ExtraArray");
            for (var i = 0; i < data.Length - 1; i++)
            {
                json.WriteInt32(data[i]);
                json.WriteValueSeparator();
            }
            if (data.Length > 0)
                json.WriteInt32(data[data.Length - 1]);
            json.WriteEndArray();
        }
    }
}
