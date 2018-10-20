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
        private string _jsonString;
        private byte[] _dataUtf8;
        private ReadOnlySequence<byte> _sequenceSingle;

        [GlobalSetup]
        public void Setup()
        {
            var builder = new StringBuilder();
            for (int j = 0; j < 1_000; j++)
            {
                builder.Append('\"');
                for (int i = 0; i < 10; i++)
                {
                    builder.Append('a');
                }
                builder.Append('\"');
            }
            _jsonString = builder.ToString();   // "aaaaaaaaaa""aaaaaaaaaa"...."aaaaaaaaaa""aaaaaaaaaa"

            _dataUtf8 = Encoding.UTF8.GetBytes(_jsonString);

            _sequenceSingle = new ReadOnlySequence<byte>(_dataUtf8);
        }

        [Benchmark]
        public void SingleSegmentBufferReader_original()
        {
            var reader = new BufferReader<byte>(_sequenceSingle);
            for (int i = 0; i < 1_000; i++)
                ConsumeString_original(ref reader);
        }

        [Benchmark]
        public void SingleSegmentBufferReader1()
        {
            var reader = new BufferReader<byte>(_sequenceSingle);
            for (int i = 0; i < 1_000; i++)
                ConsumeString1(ref reader);
        }

        [Benchmark]
        public void SingleSegmentBufferReader2()
        {
            var reader = new BufferReader<byte>(_sequenceSingle);
            for (int i = 0; i < 1_000; i++)
                ConsumeString2(ref reader);
        }

        [Benchmark]
        public void SingleSegmentBufferReader3()
        {
            var reader = new BufferReader<byte>(_sequenceSingle);
            for (int i = 0; i < 1_000; i++)
                ConsumeString3(ref reader);
        }

        [Benchmark(Baseline = true)]
        public void SingleSegmentSpan()
        {
            ReadOnlySpan<byte> reader = _sequenceSingle.First.Span;
            for (int i = 0; i < 1_000; i++)
                ConsumeString(reader);
        }

        private bool ConsumeString1(ref BufferReader<byte> reader)
        {
            if (reader.TryReadToAndAdvanceSkipOne(out ReadOnlySpan<byte> value, (byte)'\"', (byte)'\\'))
            {
                return true;
            }
            return false;
        }

        private bool ConsumeString2(ref BufferReader<byte> reader)
        {
            reader.Advance(1);
            if (reader.TryReadToAndAdvance(out ReadOnlySpan<byte> value, (byte)'\"', (byte)'\\'))
            {
                return true;
            }
            return false;
        }

        private bool ConsumeString3(ref BufferReader<byte> reader)
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
    }
}
