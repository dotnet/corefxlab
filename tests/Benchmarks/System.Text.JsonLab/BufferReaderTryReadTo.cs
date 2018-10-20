// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System.Buffers;
using System.Buffers.Reader;

namespace System.Text.JsonLab.Benchmarks
{
    [DisassemblyDiagnoser(printAsm: true, printPrologAndEpilog: true, recursiveDepth: 2)]
    [MemoryDiagnoser]
    public class BufferReaderTryReadTo
    {
        private ReadOnlySequence<byte> _sequenceSingle;

        const int NumberOfStrings = 1_000;

        [GlobalSetup]
        public void Setup()
        {
            var builder = new StringBuilder();
            for (int j = 0; j < NumberOfStrings; j++)
            {
                builder.Append('\"');
                for (int i = 0; i < 10; i++)
                {
                    builder.Append('a');
                }
                builder.Append('\"');
            }
            string jsonString = builder.ToString();   // "aaaaaaaaaa""aaaaaaaaaa"...."aaaaaaaaaa""aaaaaaaaaa"

            char[] whitespace = { ' ', '\t', '\r', '\n' };
            builder = new StringBuilder();
            var random = new Random(42);
            for (int i = 0; i < 1_000; i++)
            {
                int index = random.Next(0, 4);
                builder.Append(whitespace[index]);
            }
            //jsonString = builder.ToString();

            _sequenceSingle = new ReadOnlySequence<byte>(Encoding.UTF8.GetBytes(jsonString));
        }

        [Benchmark]
        public void SingleSegmentBufferReader_original()
        {
            var reader = new BufferReader<byte>(_sequenceSingle);
            for (int i = 0; i < NumberOfStrings; i++)
                ConsumeString_original(ref reader);
        }

        [Benchmark]
        public void SingleSegmentBufferReader_original_with_opts()
        {
            var reader = new BufferReader<byte>(_sequenceSingle);
            for (int i = 0; i < NumberOfStrings; i++)
                ConsumeString_original_opt(ref reader);
        }

        [Benchmark]
        public void SingleSegmentBufferReaderSkipAdvance()
        {
            var reader = new BufferReader<byte>(_sequenceSingle);
            for (int i = 0; i < NumberOfStrings; i++)
                ConsumeStringSkipAdvance(ref reader);
        }

        [Benchmark]
        public void SingleSegmentBufferReaderNoOptionalArg()
        {
            var reader = new BufferReader<byte>(_sequenceSingle);
            for (int i = 0; i < NumberOfStrings; i++)
                ConsumeStringNoOptionalArg(ref reader);
        }

        [Benchmark]
        public void SingleSegmentBufferReaderUnsafeAdvance()
        {
            var reader = new BufferReader<byte>(_sequenceSingle);
            for (int i = 0; i < NumberOfStrings; i++)
                ConsumeStringUnsafeAdvance(ref reader);
        }

        [Benchmark(Baseline = true)]
        public void SingleSegmentSpan()
        {
            ReadOnlySpan<byte> span = _sequenceSingle.First.Span;
            for (int i = 0; i < NumberOfStrings; i++)
                ConsumeString(span);
        }

        private bool ConsumeStringSkipAdvance(ref BufferReader<byte> reader)
        {
            if (reader.TryReadToAndAdvanceSkipN(out ReadOnlySpan<byte> value, (byte)'\"', (byte)'\\'))
            {
                return true;
            }
            return false;
        }

        private bool ConsumeStringNoOptionalArg(ref BufferReader<byte> reader)
        {
            reader.Advance(1);
            if (reader.TryReadToAndAdvance(out ReadOnlySpan<byte> value, (byte)'\"', (byte)'\\'))
            {
                return true;
            }
            return false;
        }

        private bool ConsumeStringUnsafeAdvance(ref BufferReader<byte> reader)
        {
            reader.UnsafeAdvance(1);
            if (reader.TryReadToAndAdvance(out ReadOnlySpan<byte> value, (byte)'\"', (byte)'\\'))
            {
                return true;
            }
            return false;
        }

        private bool ConsumeString_original(ref BufferReader<byte> reader)
        {
            reader.Advance(1);
            if (reader.TryReadTo(out ReadOnlySpan<byte> value, (byte)'\"', (byte)'\\', advancePastDelimiter: true))
            {
                return true;
            }
            return false;
        }

        private bool ConsumeString_original_opt(ref BufferReader<byte> reader)
        {
            if (reader.TryReadTo_opt(out ReadOnlySpan<byte> value, (byte)'\"', (byte)'\\', startIndex: 1, advancePastDelimiter: true))
            {
                return true;
            }
            return false;
        }

        private bool ConsumeString(ReadOnlySpan<byte> _buffer)
        {
            //Create local copy to avoid bounds checks.
            ReadOnlySpan<byte> localCopy = _buffer;

            int idx = localCopy.Slice(1).IndexOf((byte)'\"');
            if (idx < 0)
            {
                return false;
            }

            if (localCopy[idx] != '\\')
            {
                localCopy = localCopy.Slice(1, idx);
                return true;
            }
            else
            {
                return ConsumeStringWithNestedQuotes(_buffer);
            }
        }

        private bool ConsumeStringWithNestedQuotes(ReadOnlySpan<byte> _buffer)
        {
            //TODO: Optimize looking for nested quotes
            //TODO: Avoid redoing first IndexOf search
            long i = 0 + 1;
            while (true)
            {
                int counter = 0;
                int foundIdx = _buffer.Slice((int)i).IndexOf((byte)'\"');
                if (foundIdx == -1)
                {
                    return false;
                }
                if (foundIdx == 0)
                    break;
                for (long j = i + foundIdx - 1; j >= i; j--)
                {
                    if (_buffer[(int)j] != '\\')
                    {
                        if (counter % 2 == 0)
                        {
                            i += foundIdx;
                            goto FoundEndOfString;
                        }
                        break;
                    }
                    else
                        counter++;
                }
                i += foundIdx + 1;
            }

        FoundEndOfString:
            long startIndex = 1;
            ReadOnlySpan<byte> localCopy = _buffer.Slice((int)startIndex, (int)(i - startIndex));
            return true;
        }

        private int SkipWhiteSpace_original(ref BufferReader<byte> reader)
        {
            int _position = 0;
            while (reader.TryPeek(out byte val))
            {
                if (val != ' ' && val != '\r' && val != '\n' && val != '\t')
                    break;

                if (val == '\n')
                    _position = 0;
                else
                    _position++;

                reader.Advance(1);
            }
            return _position;
        }

        private int SkipWhiteSpace(ReadOnlySpan<byte> _buffer)
        {
            int _position = 0;
            //Create local copy to avoid bounds checks.
            ReadOnlySpan<byte> localCopy = _buffer;
            for (int i = 0; i < localCopy.Length; i++)
            {
                byte val = localCopy[i];
                if (val != ' ' && val != '\r' && val != '\n' && val != '\t')
                    break;

                if (val == '\n')
                    _position = 0;
                else
                    _position++;
            }
            return _position;
        }

        [Benchmark]
        public void SingleSegmentBufferReader_TryRead_original()
        {
            var reader = new BufferReader<byte>(_sequenceSingle);
            for (int i = 0; i < NumberOfStrings; i++)
                reader.TryReadOriginal(out int value);
        }

        [Benchmark]
        public void SingleSegmentBufferReader_TryRead_nomethodcall()
        {
            var reader = new BufferReader<byte>(_sequenceSingle);
            for (int i = 0; i < NumberOfStrings; i++)
                reader.TryRead_NoSlowMethodCall(out int value);
        }

        [Benchmark]
        public void SingleSegmentBufferReader_TryRead_optimized()
        {
            var reader = new BufferReader<byte>(_sequenceSingle);
            for (int i = 0; i < NumberOfStrings; i++)
                reader.TryReadOriginal_optimized(out int value);
        }
    }
}
