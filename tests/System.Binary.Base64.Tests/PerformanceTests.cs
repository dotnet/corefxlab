// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Xunit;
using Microsoft.Xunit.Performance;
using System.Binary;

namespace System.Binary.Tests
{
    public class Base64PerformanceTests
    {
        private static int LOAD_ITERATIONS = 1;

        static void InitalizeBytes(Span<byte> bytes, int seed = 100)
        {
            var r = new Random(seed);
            for(int i=0; i<bytes.Length; i++) {
                bytes[i] = (byte)r.Next();
            }
        }

        [Benchmark]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(1000 * 1000)]
        [InlineData(1000 * 1000 * 50)]
        private static void Base64Encode(int numberOfBytes)
        {
            var source = new byte[numberOfBytes].Slice();
            InitalizeBytes(source);
            var destination = new byte[Base64.ComputeEncodedLength(numberOfBytes)].Slice();

            foreach (var iteration in Benchmark.Iterations) {
                using (iteration.StartMeasurement()) {
                    for (int i = 0; i < LOAD_ITERATIONS; i++) {
                        var encodedBytesCount = Base64.Encode(source, destination);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(1000 * 1000)]
        [InlineData(1000 * 1000 * 50)]
        private static void Base64EncodeBaseline(int numberOfBytes)
        {
            var source = new byte[numberOfBytes];
            InitalizeBytes(source.Slice());
            var destination = new char[Base64.ComputeEncodedLength(numberOfBytes)];

            foreach (var iteration in Benchmark.Iterations) {
                using (iteration.StartMeasurement()) {
                    for (int i = 0; i < LOAD_ITERATIONS; i++) {
                        var count = Convert.ToBase64CharArray(source, 0, source.Length, destination, 0);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(1000 * 1000)]
        [InlineData(1000 * 1000 * 50)]
        private static void Base64Decode(int numberOfBytes)
        {
            var source = new byte[numberOfBytes].Slice();
            InitalizeBytes(source);
            var encoded = new byte[Base64.ComputeEncodedLength(numberOfBytes)].Slice();
            var encodedBytesCount = Base64.Encode(source, encoded);

            foreach (var iteration in Benchmark.Iterations) {
                using (iteration.StartMeasurement()) {
                    for (int i = 0; i < LOAD_ITERATIONS; i++) {
                        Base64.Decode(encoded, source);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(1000 * 1000)]
        [InlineData(1000 * 1000 * 50)]
        private static void Base64DecodeBaseline(int numberOfBytes)
        {
            var source = new byte[numberOfBytes];
            InitalizeBytes(source.Slice());
            var encoded = Convert.ToBase64String(source).ToCharArray();

            foreach (var iteration in Benchmark.Iterations) {
                using (iteration.StartMeasurement()) {
                    for (int i = 0; i < LOAD_ITERATIONS; i++) {
                        Convert.FromBase64CharArray(encoded, 0, encoded.Length);
                    }
                }
            }
        }
    }
}