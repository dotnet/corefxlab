// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Binary.Base64Experimental.Tests;
using System.Buffers;
using BenchmarkDotNet.Attributes;
using Base64Encoder = System.Binary.Base64Experimental.Base64Experimental; // This name problematic, since Base64Experimental is part of namespace.

namespace System.Binary.Base64.Benchmarks
{
    public class Base64Encode
    {
        [Params(10, 100, 1_000, 1_000_000)]
        public int NumberOfBytes;
        private static readonly StandardFormat format = new StandardFormat('M');

        private static byte[] _source;
        private static byte[] _bytesDestination;
        private static char[] _charsDestination;

        [GlobalSetup]
        public void Setup()
        {
            _source = new byte[NumberOfBytes];
            Base64TestHelper.InitalizeBytes(_source);
            _bytesDestination = new byte[Base64Encoder.GetMaxEncodedToUtf8Length(NumberOfBytes, format)];
            _charsDestination = new char[Base64Encoder.GetMaxEncodedToUtf8Length(NumberOfBytes, format)];
        }

        [Benchmark]
        public OperationStatus Base64ExperimentalWithLineBreaks()
        {
            return Base64Encoder.EncodeToUtf8(_source, _bytesDestination, out _, out _, format);
        }

        [Benchmark(Baseline = true)]
        public int Base64ConvertWithLineBreaks()
        {
            return Convert.ToBase64CharArray(_source, 0, _source.Length, _charsDestination, 0, Base64FormattingOptions.InsertLineBreaks);
        }
    }
}
