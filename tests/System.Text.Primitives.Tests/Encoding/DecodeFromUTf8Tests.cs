﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Primitives.Tests.Encoding
{
    public class DecodeFromUtf8Tests : ITextEncoderTest
    {
        private static TextEncoder utf8 = TextEncoder.Utf8;
        private static Text.Encoding testEncoder = Text.Encoding.UTF8;

        // Open issue: https://github.com/dotnet/corefxlab/issues/1512
        //[Fact]
        public void InputBufferEmpty(TextEncoderTestHelper.SupportedEncoding from)
        {
            throw new NotImplementedException();
        }

        //[Fact]
        public void OutputBufferEmpty(TextEncoderTestHelper.SupportedEncoding from)
        {
            throw new NotImplementedException();
        }

        //[Fact]
        public void InputBufferLargerThanOutputBuffer(TextEncoderTestHelper.SupportedEncoding from)
        {
            throw new NotImplementedException();
        }

        //[Fact]
        public void OutputBufferLargerThanInputBuffer(TextEncoderTestHelper.SupportedEncoding from)
        {
            throw new NotImplementedException();
        }

        //[Fact]
        public void InputBufferContainsOnlyInvalidData(TextEncoderTestHelper.SupportedEncoding from)
        {
            throw new NotImplementedException();
        }

        //[Fact]
        public void InputBufferContainsSomeInvalidData(TextEncoderTestHelper.SupportedEncoding from)
        {
            throw new NotImplementedException();
        }

        //[Fact]
        public void InputBufferEndsTooEarlyAndRestart(TextEncoderTestHelper.SupportedEncoding from)
        {
            throw new NotImplementedException();
        }

        //[Fact]
        public void InputBufferContainsOnlyASCII(TextEncoderTestHelper.SupportedEncoding from)
        {
            throw new NotImplementedException();
        }

        //[Fact]
        public void InputBufferContainsNonASCII(TextEncoderTestHelper.SupportedEncoding from)
        {
            throw new NotImplementedException();
        }

        //[Fact]
        public void InputBufferContainsAllCodePoints(TextEncoderTestHelper.SupportedEncoding from)
        {
            throw new NotImplementedException();
        }
    }
}
