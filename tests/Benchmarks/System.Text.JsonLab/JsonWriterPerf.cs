// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using System.Buffers.Text;
using System.IO;
using System.Text.Formatting;

namespace System.Text.JsonLab.Benchmarks
{
    //[Config(typeof(JustDisassembly))]
    [SimpleJob(warmupCount: 3, targetCount: 5)]
    //[MemoryDiagnoser]
    [DisassemblyDiagnoser(printPrologAndEpilog: true, recursiveDepth: 3)]
    public class JsonWriterPerf
    {
        /*public class JustDisassembly : ManualConfig
        {
            public JustDisassembly()
            {
                Add(Job.Dry);
            }
        }*/

        private static readonly byte[] Message = Encoding.UTF8.GetBytes("message");
        private static readonly byte[] HelloWorld = Encoding.UTF8.GetBytes("Hello, World!");
        private static readonly byte[] ExtraArray = Encoding.UTF8.GetBytes("ExtraArray");

        private const int ExtraArraySize = 10000000;
        private const int BufferSize = 1024 + (ExtraArraySize * 64);

        private ArrayFormatterWrapper _arrayFormatterWrapper;
        private ArrayFormatter _arrayFormatter;
        private MemoryStream _memoryStream;
        private StreamWriter _streamWriter;

        private int[] _data;
        private byte[] _output;
        private long[] _longs;
        private string _filePathCore;
        private string _filePathCoreUtf16;
        private string _filePathNewtonsoft;

        private Memory<byte> _memory;

        //private byte[] _byteBuffer;
        //private int _idx;

        //[Params(10, 20)]
        //public int _indent;

        [Params(/*1, 10,*/ 100_000)]
        public int Size;

        [Params(true)]
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
            _arrayFormatterWrapper = new ArrayFormatterWrapper(BufferSize, SymbolTable.InvariantUtf8);
            _arrayFormatter = new ArrayFormatter(BufferSize, SymbolTable.InvariantUtf8);

            // To pass an initialBuffer to Utf8Json:
            // _output = new byte[BufferSize];
            _output = new byte[21 * 1_000 + 4];

            //_idx = 0;
            //_byteBuffer = new byte[_indent];

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

            _filePathCore = @"E:\GitHub\Fork\corefxlab\src\System.Text.JsonLab\bin\Release\netcoreapp2.1\outputCore";
            _filePathCoreUtf16 = @"E:\GitHub\Fork\corefxlab\src\System.Text.JsonLab\bin\Release\netcoreapp2.1\outputCoreUtf16";
            _filePathNewtonsoft = @"E:\GitHub\Fork\corefxlab\src\System.Text.JsonLab\bin\Release\netcoreapp2.1\outputNewtonsoft";
            if (Formatted)
            {
                _filePathNewtonsoft = @"E:\GitHub\Fork\corefxlab\src\System.Text.JsonLab\bin\Release\netcoreapp2.1\outputNewtonsoftFormatted";
                if (SkipValidation)
                {
                    _filePathCore = @"E:\GitHub\Fork\corefxlab\src\System.Text.JsonLab\bin\Release\netcoreapp2.1\outputCoreFormattedSkip";
                    _filePathCoreUtf16 = @"E:\GitHub\Fork\corefxlab\src\System.Text.JsonLab\bin\Release\netcoreapp2.1\outputCoreFormattedSkipUtf16";
                }
                else
                {
                    _filePathCore = @"E:\GitHub\Fork\corefxlab\src\System.Text.JsonLab\bin\Release\netcoreapp2.1\outputCoreFormatted";
                    _filePathCoreUtf16 = @"E:\GitHub\Fork\corefxlab\src\System.Text.JsonLab\bin\Release\netcoreapp2.1\outputCoreFormattedUtf16";
                }
            }
            else
            {
                if (SkipValidation)
                {
                    _filePathCore = @"E:\GitHub\Fork\corefxlab\src\System.Text.JsonLab\bin\Release\netcoreapp2.1\outputCoreSkip";
                    _filePathCoreUtf16 = @"E:\GitHub\Fork\corefxlab\src\System.Text.JsonLab\bin\Release\netcoreapp2.1\outputCoreSkipUtf16";
                }
            }

            _filePathCore += Size + ".json";
            _filePathCoreUtf16 += Size + ".json";
            _filePathNewtonsoft += Size + ".json";

            _memory = new byte[100_000_000];
        }

        //[Benchmark]
        public void WriterSystemTextJsonBasic()
        {
            _arrayFormatterWrapper.Clear();
            WriterSystemTextJsonBasicUtf8(Formatted, _arrayFormatterWrapper, _data.AsSpan(0, 10));
        }

        //[Benchmark(Baseline = true)]
        public void WriterNewtonsoftBasic()
        {
            Span<int> data = _data.AsSpan(0, 10);
            using (var json = new Newtonsoft.Json.JsonTextWriter(GetWriter()))
            {
                json.Formatting = Formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

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

            //WriterNewtonsoftBasic(Formatted, GetWriter(), _data.AsSpan(0, 10));
        }

        //[Benchmark]
        public void WriterUtf8JsonBasic()
        {
            WriterUtf8JsonBasic(_data.AsSpan(0, 10));
        }

        //[Benchmark(Baseline = true)]
        public void WriterSystemTextJsonHelloWorld()
        {
            _arrayFormatterWrapper.Clear();

            //WriterSystemTextJsonHelloWorldUtf8(Formatted, _arrayFormatterWrapper);
            var json = new Utf8JsonWriter<ArrayFormatterWrapper>(_arrayFormatterWrapper, Formatted);

            json.WriteObjectStart();
            json.WriteAttribute("message", "Hello, World!");
            json.WriteAttribute("message", "Hello, World!");
            json.WriteAttribute("message", "Hello, World!");
            json.WriteAttribute("message", "Hello, World!");
            json.WriteAttribute("message", "Hello, World!");
            json.WriteAttribute("message", "Hello, World!");
            json.WriteAttribute("message", "Hello, World!");
            json.WriteAttribute("message", "Hello, World!");
            json.WriteAttribute("message", "Hello, World!");
            json.WriteAttribute("message", "Hello, World!");
            json.WriteObjectEnd();
        }

        //[Benchmark(Baseline = true)]
        public void WriteNewtonsoft()
        {
            using (var json = new Newtonsoft.Json.JsonTextWriter(GetWriter()))
            {
                json.Formatting = Formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

                json.WriteStartObject();
                for (int i = 0; i < Size; i++)
                {
                    json.WritePropertyName("message");
                    json.WriteValue("Hello, World!");
                }
                json.WriteEnd();
            }
        }

        //[Benchmark]
        public void WriteNetCore()
        {
            _memoryStream.Seek(0, SeekOrigin.Begin);

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = Formatted, SkipValidation = SkipValidation });

            Utf8JsonWriter2<Buffers.IBufferWriter<byte>> json = Utf8JsonWriter2.CreateFromStream(_memoryStream, state);

            //_arrayFormatterWrapper.Clear();

            //var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = Formatted, SkipValidation = SkipValidation });

            //var json = new Utf8JsonWriter2<ArrayFormatterWrapper>(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            json.WriteNumber("age", 42);
            json.WriteString("first", "John");
            json.WriteString("last", "Smith");
            json.WriteStartArray("phoneNumbers");
            json.WriteValue("425-000-1212");
            json.WriteValue("425-000-1213");
            json.WriteEndArray();
            json.WriteStartObject("address");
            json.WriteString("street", "1 Microsoft Way");
            json.WriteString("city", "Redmond");
            json.WriteNumber("zip", 98052);
            json.WriteEndObject();
            json.WriteArray(ExtraArray, _data.AsSpan(0, 10));
            json.WriteEndObject();
            //json.Flush();

            json.Dispose();
        }

        //[Benchmark]
        public void CtorWithMemory()
        {
            _memoryStream.Seek(0, SeekOrigin.Begin);

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = Formatted, SkipValidation = SkipValidation });

            Utf8JsonWriter2<Buffers.IBufferWriter<byte>> json = Utf8JsonWriter2.CreateFromMemory(_memory, state);

            json.WriteStartObject();
            json.WriteNumber("age", 42);
            json.WriteString("first", "John");
            json.WriteString("last", "Smith");
            json.WriteStartArray("phoneNumbers");
            json.WriteValue("425-000-1212");
            json.WriteValue("425-000-1213");
            json.WriteEndArray();
            json.WriteStartObject("address");
            json.WriteString("street", "1 Microsoft Way");
            json.WriteString("city", "Redmond");
            json.WriteNumber("zip", 98052);
            json.WriteEndObject();
            json.WriteArray(ExtraArray, _data.AsSpan(0, 10));
            json.WriteEndObject();

            json.Dispose();
        }

        //[Benchmark]
        public void CtorWithStream()
        {
            _memoryStream.Seek(0, SeekOrigin.Begin);

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = Formatted, SkipValidation = SkipValidation });

            Utf8JsonWriter2<Buffers.IBufferWriter<byte>> json = Utf8JsonWriter2.CreateFromStream(_memoryStream, state);

            json.WriteStartObject();
            json.WriteNumber("age", 42);
            json.WriteString("first", "John");
            json.WriteString("last", "Smith");
            json.WriteStartArray("phoneNumbers");
            json.WriteValue("425-000-1212");
            json.WriteValue("425-000-1213");
            json.WriteEndArray();
            json.WriteStartObject("address");
            json.WriteString("street", "1 Microsoft Way");
            json.WriteString("city", "Redmond");
            json.WriteNumber("zip", 98052);
            json.WriteEndObject();
            json.WriteArray(ExtraArray, _data.AsSpan(0, 10));
            json.WriteEndObject();

            json.Dispose();
        }

        //[Benchmark (Baseline = true)]
        public void CtorWithIBufferWriterStruct()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2<ArrayFormatterWrapper>(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            json.WriteNumber("age", 42);
            json.WriteString("first", "John");
            json.WriteString("last", "Smith");
            json.WriteStartArray("phoneNumbers");
            json.WriteValue("425-000-1212");
            json.WriteValue("425-000-1213");
            json.WriteEndArray();
            json.WriteStartObject("address");
            json.WriteString("street", "1 Microsoft Way");
            json.WriteString("city", "Redmond");
            json.WriteNumber("zip", 98052);
            json.WriteEndObject();
            json.WriteArray(ExtraArray, _data.AsSpan(0, 10));
            json.WriteEndObject();

            json.Dispose();
        }

        [Benchmark]
        public void WriteStart()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2<ArrayFormatterWrapper>(_arrayFormatterWrapper, state);

            for (int i = 0; i < 1000; i++)
                json.WriteStartObject("message");
            /*json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();*/

            json.Dispose();
        }

        [Benchmark]
        public void WriteStartUtf8()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2<ArrayFormatterWrapper>(_arrayFormatterWrapper, state);

            for (int i = 0; i < 1000; i++)
                json.WriteStartObject(Message);
            /*json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();*/

            json.Dispose();
        }

        //[Benchmark]
        public void CtorWithIBufferWriterClass()
        {
            _arrayFormatter.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2<ArrayFormatter>(_arrayFormatter, state);

            json.WriteStartObject();
            json.WriteNumber("age", 42);
            json.WriteString("first", "John");
            json.WriteString("last", "Smith");
            json.WriteStartArray("phoneNumbers");
            json.WriteValue("425-000-1212");
            json.WriteValue("425-000-1213");
            json.WriteEndArray();
            json.WriteStartObject("address");
            json.WriteString("street", "1 Microsoft Way");
            json.WriteString("city", "Redmond");
            json.WriteNumber("zip", 98052);
            json.WriteEndObject();
            json.WriteArray(ExtraArray, _data.AsSpan(0, 10));
            json.WriteEndObject();

            json.Dispose();
        }

        //[Benchmark(Baseline = true)]
        public void CallOnStructIBW()
        {
            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2<ArrayFormatterWrapper>(_arrayFormatterWrapper, state);

            json.NoopApi();
            json.NoopApi();
            json.NoopApi();
            json.NoopApi();
            json.NoopApi();
            json.NoopApi();
            json.NoopApi();
            json.NoopApi();
            json.NoopApi();
            json.NoopApi();

            //_arrayFormatterWrapper.NoopApi(1);
            //_arrayFormatterWrapper.NoopApi(2);
            //_arrayFormatterWrapper.NoopApi(3);
            //_arrayFormatterWrapper.NoopApi(4);
            //_arrayFormatterWrapper.NoopApi(5);
            //_arrayFormatterWrapper.NoopApi(6);
            //_arrayFormatterWrapper.NoopApi(7);
            //_arrayFormatterWrapper.NoopApi(8);
            //_arrayFormatterWrapper.NoopApi(9);
            //_arrayFormatterWrapper.NoopApi(10);
        }

        //[Benchmark]
        public void CallOnClassIBW()
        {
            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2<ArrayFormatter>(_arrayFormatter, state);

            json.NoopApi();
            json.NoopApi();
            json.NoopApi();
            json.NoopApi();
            json.NoopApi();
            json.NoopApi();
            json.NoopApi();
            json.NoopApi();
            json.NoopApi();
            json.NoopApi();

            //_arrayFormatter.NoopApi(1);
            //_arrayFormatter.NoopApi(2);
            //_arrayFormatter.NoopApi(3);
            //_arrayFormatter.NoopApi(4);
            //_arrayFormatter.NoopApi(5);
            //_arrayFormatter.NoopApi(6);
            //_arrayFormatter.NoopApi(7);
            //_arrayFormatter.NoopApi(8);
            //_arrayFormatter.NoopApi(9);
            //_arrayFormatter.NoopApi(10);
        }

        //[Benchmark]
        public void WriteNetCoreUtf8()
        {
            _memoryStream.Seek(0, SeekOrigin.Begin);

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = Formatted, SkipValidation = SkipValidation });

            Utf8JsonWriter2<Buffers.IBufferWriter<byte>> json = Utf8JsonWriter2.CreateFromMemory(_memory, state);
            json.WriteStartObject();
            for (int i = 0; i < Size; i++)
                json.WriteString(Message, HelloWorld);
            json.WriteEndObject();
            json.Flush();

            //_memoryStream.Write(_memory.Span.Slice(0, (int)json.BytesCommitted));

            /*Utf8JsonWriter2<Buffers.IBufferWriter<byte>> json = Utf8JsonWriter2.CreateFromStream(_memoryStream, state);
            json.WriteStartObject();
            for (int i = 0; i < Size; i++)
                json.WriteString(Message, HelloWorld);
            json.WriteEndObject();

            json.Dispose();*/
        }

        //[Benchmark]
        public void WriteNetCoreUtf16()
        {
            _memoryStream.Seek(0, SeekOrigin.Begin);
            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = Formatted, SkipValidation = SkipValidation });

            Utf8JsonWriter2<Buffers.IBufferWriter<byte>> json = Utf8JsonWriter2.CreateFromStream(_memoryStream, state);
            json.WriteStartObject();
            for (int i = 0; i < Size; i++)
                json.WriteString("message", "Hello, World!");
            json.WriteEndObject();

            json.Dispose();
        }

        //[Benchmark(Baseline = true)]
        public void WriteArray()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2<ArrayFormatterWrapper>(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            json.WriteArray(Message, _longs);
            json.WriteEndObject();
            json.Flush();
        }

        //[Benchmark]
        public void WriteArrayLoop()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2<ArrayFormatterWrapper>(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            json.WriteStartArray(Message);
            for (int i = 0; i < _longs.Length; i++)
                json.WriteValue(_longs[i]);
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
            for (var i = 0; i < _longs.Length - 1; i++)
            {
                json.WriteInt64(_longs[i]);
                json.WriteValueSeparator();
            }
            if (_longs.Length > 0)
                json.WriteInt64(_longs[_longs.Length - 1]);
            json.WriteEndArray();
            json.WriteEndObject();
        }

        //[Benchmark]
        public void WriteArrayUtf8Json_2()
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
        public void WriteBoolStrings()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2<ArrayFormatterWrapper>(_arrayFormatterWrapper, state);

            json.WriteStartObject();

            json.WriteBoolean("message", true);
            json.WriteBoolean("message", false);
            json.WriteBoolean("message", true);
            json.WriteBoolean("message", false);
            json.WriteBoolean("message", true);
            json.WriteBoolean("message", false);
            json.WriteBoolean("message", true);
            json.WriteBoolean("message", false);
            json.WriteBoolean("message", true);
            json.WriteBoolean("message", false);

            json.WriteEndObject();
            json.Flush();
        }

        //[Benchmark]
        public void WriteBoolSpanChars()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2<ArrayFormatterWrapper>(_arrayFormatterWrapper, state);

            ReadOnlySpan<char> spanChar = "message";

            json.WriteStartObject();

            json.WriteBoolean(spanChar, true);
            json.WriteBoolean(spanChar, false);
            json.WriteBoolean(spanChar, true);
            json.WriteBoolean(spanChar, false);
            json.WriteBoolean(spanChar, true);
            json.WriteBoolean(spanChar, false);
            json.WriteBoolean(spanChar, true);
            json.WriteBoolean(spanChar, false);
            json.WriteBoolean(spanChar, true);
            json.WriteBoolean(spanChar, false);

            json.WriteEndObject();
            json.Flush();
        }

        //[Benchmark]
        public void WriteBoolSpanBytes()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2<ArrayFormatterWrapper>(_arrayFormatterWrapper, state);

            json.WriteStartObject();

            json.WriteBoolean(Message, true);
            json.WriteBoolean(Message, false);
            json.WriteBoolean(Message, true);
            json.WriteBoolean(Message, false);
            json.WriteBoolean(Message, true);
            json.WriteBoolean(Message, false);
            json.WriteBoolean(Message, true);
            json.WriteBoolean(Message, false);
            json.WriteBoolean(Message, true);
            json.WriteBoolean(Message, false);

            json.WriteEndObject();
            json.Flush();
        }

        //[Benchmark]
        public void WriterCoreNumbers()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2<ArrayFormatterWrapper>(_arrayFormatterWrapper, state);

            json.WriteStartObject();
            json.WriteNumber("message", 1234567);
            json.WriteNumber("message", 123456);
            json.WriteNumber("message", 12345);
            json.WriteNumber("message", 12345678);
            json.WriteNumber("message", 1234);
            json.WriteNumber("message", 123);
            json.WriteNumber("message", 123456789);
            json.WriteNumber("message", 12);
            json.WriteNumber("message", 1);
            json.WriteNumber("message", 1234567890);
            json.WriteEndObject();

            json.Flush();

            //WriterSystemTextJsonHelloWorldUtf82(option, _arrayFormatterWrapper);
        }

        ////[Benchmark(Baseline = true)]
        //public void Loop()
        //{
        //    var indent = _indent;
        //    var idx = _idx;
        //    while (indent > 0)
        //    {
        //        _byteBuffer[idx++] = (byte)' ';
        //        _byteBuffer[idx++] = (byte)' ';
        //        indent -= 2;
        //    }
        //}

        ////[Benchmark]
        //public void Fill()
        //{
        //    var indent = _indent;
        //    var idx = _idx;
        //    _byteBuffer.AsSpan(idx, indent).Fill((byte)' ');
        //    idx += indent;
        //}

        //[Benchmark(Baseline = true)]
        public void Compromise()
        {
            //Helper2(_indent, 0, _byteBuffer);

            /*var indent = _indent;
            var idx = _idx;
            if (indent < 8)
            {
                while (indent > 0)
                {
                    _byteBuffer[idx++] = (byte)' ';
                    _byteBuffer[idx++] = (byte)' ';
                    indent -= 2;
                }
            }
            else
            {
                _byteBuffer.AsSpan(idx, indent).Fill((byte)' ');
                idx += indent;
            }*/
        }

        private static void Helper2(int indent, int i, Span<byte> byteBuffer)
        {
            if (indent < 8)
            {
                while (indent > 0)
                {
                    byteBuffer[i++] = (byte)' ';
                    byteBuffer[i++] = (byte)' ';
                    indent -= 2;
                }
            }
            else
            {
                byteBuffer.Slice(i, indent).Fill((byte)' ');
                i += indent;
            }
        }

        //[Benchmark]
        public void Compromise_new()
        {
            //Helper(_byteBuffer.AsSpan(0, _indent));
        }

        private static void Helper(Span<byte> byteBuffer)
        {
            int i = 0;
            if (byteBuffer.Length < 8)
            {
                while (i < byteBuffer.Length)
                {
                    byteBuffer[i++] = (byte)' ';
                    byteBuffer[i++] = (byte)' ';
                }
            }
            else
            {
                byteBuffer.Fill((byte)' ');
                i += byteBuffer.Length;
            }
        }

        //[Benchmark]
        public void WriterCoreNumbersUtf8()
        {
            _arrayFormatterWrapper.Clear();

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = Formatted, SkipValidation = SkipValidation });

            var json = new Utf8JsonWriter2<ArrayFormatterWrapper>(_arrayFormatterWrapper, state);

            ReadOnlySpan<byte> key = new byte[] { (byte)'a' };

            json.WriteStartObject();
            for (int i = 0; i < 1_000; i++)
                json.WriteNumber(key, 1);
            json.WriteEndObject();

            json.Flush();

            //WriterSystemTextJsonHelloWorldUtf82(option, _arrayFormatterWrapper);
        }

        //[Benchmark]
        public void WriterNewtonsoftHelloWorld()
        {
            WriterNewtonsoftHelloWorld(Formatted, GetWriter());
        }

        //[Benchmark(Baseline = true)]
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

        private static void WriterSystemTextJsonBasicUtf8(bool formatted, ArrayFormatterWrapper output, ReadOnlySpan<int> data)
        {
            var json = new Utf8JsonWriter<ArrayFormatterWrapper>(output, formatted);

            json.WriteObjectStart();
            json.WriteAttribute("age", 42);
            json.WriteAttribute("first", "John");
            json.WriteAttribute("last", "Smith");
            json.WriteArrayStart("phoneNumbers");
            json.WriteValue("425-000-1212");
            json.WriteValue("425-000-1213");
            json.WriteArrayEnd();
            json.WriteObjectStart("address");
            json.WriteAttribute("street", "1 Microsoft Way");
            json.WriteAttribute("city", "Redmond");
            json.WriteAttribute("zip", 98052);
            json.WriteObjectEnd();
            json.WriteArrayUtf8(ExtraArray, data);
            json.WriteObjectEnd();
            json.Flush();
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
            //json.WriteAttributeUtf8(Message, HelloWorld);
            //json.WriteObjectEnd();
            json.Flush();
        }

        private static void WriterSystemTextJsonHelloWorldUtf82(JsonWriterState state, ArrayFormatterWrapper output)
        {
            var json = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);

            json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();
            json.WriteStartObject();
            //json.WriteString(Message, HelloWorld);
            //json.WriteEndObject();
            //json.Flush();
        }

        private static void WriterNewtonsoftHelloWorld(bool formatted, TextWriter writer)
        {
            using (var json = new Newtonsoft.Json.JsonTextWriter(writer))
            {
                json.Formatting = formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

                json.WriteStartObject();
                json.WritePropertyName("message");
                json.WriteValue(1234567);
                json.WritePropertyName("message");
                json.WriteValue(123456);
                json.WritePropertyName("message");
                json.WriteValue(12345);
                json.WritePropertyName("message");
                json.WriteValue(12345678);
                json.WritePropertyName("message");
                json.WriteValue(1234);
                json.WritePropertyName("message");
                json.WriteValue(123);
                json.WritePropertyName("message");
                json.WriteValue(123456789);
                json.WritePropertyName("message");
                json.WriteValue(12);
                json.WritePropertyName("message");
                json.WriteValue(1);
                json.WritePropertyName("message");
                json.WriteValue(1234567890);
                json.WriteEndObject();
            }
        }

        private static void WriterUtf8JsonHelloWorldHelper(byte[] output)
        {
            global::Utf8Json.JsonWriter json = new global::Utf8Json.JsonWriter(output);

            /*byte[] message = global::Utf8Json.JsonWriter.GetEncodedPropertyName("message");

            byte[] rawBytes = new byte[(message.Length + 1) * 10 + 2 + 55 + 9];

            rawBytes[0] = (byte)'{';
            Array.Copy(message, 0, rawBytes, 1, message.Length);
            rawBytes[10] = (byte)':';
            Array.Copy(message, 0, rawBytes, 11, message.Length);
            rawBytes[20] = (byte)':';
            Array.Copy(message, 0, rawBytes, 21, message.Length);
            rawBytes[30] = (byte)':';
            Array.Copy(message, 0, rawBytes, 31, message.Length);
            rawBytes[40] = (byte)':';
            Array.Copy(message, 0, rawBytes, 41, message.Length);
            rawBytes[50] = (byte)':';
            Array.Copy(message, 0, rawBytes, 51, message.Length);
            rawBytes[60] = (byte)':';
            Array.Copy(message, 0, rawBytes, 61, message.Length);
            rawBytes[70] = (byte)':';
            Array.Copy(message, 0, rawBytes, 71, message.Length);
            rawBytes[80] = (byte)':';
            Array.Copy(message, 0, rawBytes, 81, message.Length);
            rawBytes[90] = (byte)':';
            Array.Copy(message, 0, rawBytes, 91, message.Length);
            rawBytes[100] = (byte)':';
            rawBytes[rawBytes.Length - 1] = (byte)'}';

            json.WriteRaw(rawBytes);*/

            /*json.WriteBeginObject();
            json.WriteRaw(message);
            json.WriteNameSeparator();
            json.WriteInt32(1234567);
            json.WriteRaw(message);
            json.WriteNameSeparator();
            json.WriteInt32(123456);
            json.WriteRaw(message);
            json.WriteNameSeparator();
            json.WriteInt32(12345);
            json.WriteRaw(message);
            json.WriteNameSeparator();
            json.WriteInt32(12345678);
            json.WriteRaw(message);
            json.WriteNameSeparator();
            json.WriteInt32(1234);
            json.WriteRaw(message);
            json.WriteNameSeparator();
            json.WriteInt32(123);
            json.WriteRaw(message);
            json.WriteNameSeparator();
            json.WriteInt32(123456789);
            json.WriteRaw(message);
            json.WriteNameSeparator();
            json.WriteInt32(12);
            json.WriteRaw(message);
            json.WriteNameSeparator();
            json.WriteInt32(1);
            json.WriteRaw(message);
            json.WriteNameSeparator();
            json.WriteInt32(1234567890);
            json.WriteEndObject();*/

            /*json.WriteBeginArray();
            json.WriteRaw(message);
            json.WriteNameSeparator();
            json.WriteInt32(1234567);
            json.WriteRaw(message);
            json.WriteNameSeparator();
            json.WriteInt32(123456);
            json.WriteRaw(message);
            json.WriteNameSeparator();
            json.WriteInt32(12345);
            json.WriteRaw(message);
            json.WriteNameSeparator();
            json.WriteInt32(12345678);
            json.WriteRaw(message);
            json.WriteNameSeparator();
            json.WriteInt32(1234);
            json.WriteRaw(message);
            json.WriteNameSeparator();
            json.WriteInt32(123);
            json.WriteRaw(message);
            json.WriteNameSeparator();
            json.WriteInt32(123456789);
            json.WriteRaw(message);
            json.WriteNameSeparator();
            json.WriteInt32(12);
            json.WriteRaw(message);
            json.WriteNameSeparator();
            json.WriteInt32(1);
            json.WriteRaw(message);
            json.WriteNameSeparator();
            json.WriteInt32(1234567890);
            json.WriteEndObject();*/

            /*json.WriteBeginArray();
            json.WritePropertyName("message");
            json.WriteInt32(1234567);
            json.WritePropertyName("message");
            json.WriteInt32(123456);
            json.WritePropertyName("message");
            json.WriteInt32(12345);
            json.WritePropertyName("message");
            json.WriteInt32(12345678);
            json.WritePropertyName("message");
            json.WriteInt32(1234);
            json.WritePropertyName("message");
            json.WriteInt32(123);
            json.WritePropertyName("message");
            json.WriteInt32(123456789);
            json.WritePropertyName("message");
            json.WriteInt32(12);
            json.WritePropertyName("message");
            json.WriteInt32(1);
            json.WritePropertyName("message");
            json.WriteInt32(1234567890);
            json.WriteEndObject();*/
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
