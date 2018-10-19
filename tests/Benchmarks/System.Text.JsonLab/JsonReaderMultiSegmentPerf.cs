// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using Benchmarks;
using System.Buffers;
using System.Buffers.Tests;
using System.Collections.Generic;

namespace System.Text.JsonLab.Benchmarks
{
    // Since there are 15 tests here (5 * 3), setting low values for the warmupCount and targetCount
    [SimpleJob(warmupCount: 3, targetCount: 5)]
    [MemoryDiagnoser]
    public class JsonReaderMultiSegmentPerf
    {
        // Keep the JsonStrings resource names in sync with TestCaseType enum values.
        public enum TestCaseType
        {
            Json400KB
        }

        private string _jsonString;
        private byte[] _dataUtf8;
        private Dictionary<int, ReadOnlySequence<byte>> _sequences;
        private ReadOnlySequence<byte> _sequenceSingle;

        [ParamsSource(nameof(TestCaseValues))]
        public TestCaseType TestCase;

        public static IEnumerable<TestCaseType> TestCaseValues() => (IEnumerable<TestCaseType>)Enum.GetValues(typeof(TestCaseType));

        [GlobalSetup]
        public void Setup()
        {
            _jsonString = JsonStrings.ResourceManager.GetString(TestCase.ToString());

            _dataUtf8 = Encoding.UTF8.GetBytes(_jsonString);

            _sequenceSingle = new ReadOnlySequence<byte>(_dataUtf8);

            int[] segmentSizes = { 1_000, 2_000, 4_000, 8_000 };

            _sequences = new Dictionary<int, ReadOnlySequence<byte>>();

            for (int i = 0; i < segmentSizes.Length; i++)
            {
                int segmentSize = segmentSizes[i];
                _sequences.Add(segmentSize, GetSequence(_dataUtf8, segmentSize));
            }
        }

        private static ReadOnlySequence<byte> GetSequence(byte[] _dataUtf8, int segmentSize)
        {
            int numberOfSegments = _dataUtf8.Length / segmentSize + 1;
            byte[][] buffers = new byte[numberOfSegments][];

            for (int j = 0; j < numberOfSegments - 1; j++)
            {
                buffers[j] = new byte[segmentSize];
                Array.Copy(_dataUtf8, j * segmentSize, buffers[j], 0, segmentSize);
            }

            int remaining = _dataUtf8.Length % segmentSize;
            buffers[numberOfSegments - 1] = new byte[remaining];
            Array.Copy(_dataUtf8, _dataUtf8.Length - remaining, buffers[numberOfSegments - 1], 0, remaining);

            return BufferFactory.Create(buffers);
        }

        [Benchmark]
        public void SingleSegmentSequence()
        {
            var json = new Utf8JsonReader(_sequenceSingle);
            while (json.Read()) ;
        }

        [Benchmark]
        [Arguments(4_000)]
        public void MultiSegmentSequence(int segmentSize)
        {
            var json = new Utf8JsonReader(_sequences[segmentSize]);
            while (json.Read()) ;
            json.Dispose();
        }

        [Benchmark]
        [Arguments(4_000)]
        public void MultiSegmentSequenceUsingSpan(int segmentSize)
        {
            ReadOnlySequence<byte> sequenceMultiple = _sequences[segmentSize];

            byte[] buffer = ArrayPool<byte>.Shared.Rent(segmentSize * 2);
            JsonReaderState state = default;
            int previous = 0;
            foreach (ReadOnlyMemory<byte> memory in sequenceMultiple)
            {
                ReadOnlySpan<byte> span = memory.Span;
                Span<byte> bufferSpan = buffer;
                span.CopyTo(bufferSpan.Slice(previous));
                bufferSpan = bufferSpan.Slice(0, span.Length + previous);

                bool isFinalBlock = bufferSpan.Length == 0;

                var json = new Utf8JsonReader(bufferSpan, isFinalBlock, state);
                while (json.Read()) ;

                if (isFinalBlock)
                    break;

                state = json.State;

                if (json.Consumed != bufferSpan.Length)
                {
                    ReadOnlySpan<byte> leftover = bufferSpan.Slice((int)json.Consumed);
                    previous = leftover.Length;
                    leftover.CopyTo(buffer);
                }
                else
                {
                    previous = 0;
                }
            }
            ArrayPool<byte>.Shared.Return(buffer);
        }

        [Benchmark]
        [Arguments(4_000)]
        public void MultiSegmentSequenceUsingReaderSequence(int segmentSize)
        {
            ReadOnlySequence<byte> sequenceMultiple = _sequences[segmentSize];

            var json = new Utf8JsonReaderSequence(sequenceMultiple);
            while (json.Read()) ;
            json.Dispose();
        }
    }
}
