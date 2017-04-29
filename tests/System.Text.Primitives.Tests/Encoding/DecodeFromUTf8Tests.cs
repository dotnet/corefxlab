// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Primitives.Tests.Encoding
{
    public class DecodeFromUtf8Tests : ITextEncoderTest
    {
        private static TextEncoder utf8 = TextEncoder.Utf8;
        private static Text.Encoding testEncoder = Text.Encoding.UTF8;

        //[Fact]
        public void InputBufferEmpty()
        {
            throw new NotImplementedException();
        }

        //[Fact]
        public void OutputBufferEmpty()
        {
            throw new NotImplementedException();
        }

        //[Fact]
        public void InputBufferLargerThanOutputBuffer()
        {
            throw new NotImplementedException();
        }

        //[Fact]
        public void OutputBufferLargerThanInputBuffer()
        {
            throw new NotImplementedException();
        }

        //[Fact]
        public void InputBufferContainsOnlyInvalidData()
        {
            throw new NotImplementedException();
        }

        //[Fact]
        public void InputBufferContainsSomeInvalidData()
        {
            throw new NotImplementedException();
        }

        //[Fact]
        public void InputBufferEndsOnHighSurrogateAndRestart()
        {
            throw new NotImplementedException();
        }

        //[Fact]
        public void InputBufferContainsOnlyASCII()
        {
            throw new NotImplementedException();
        }

        //[Fact]
        public void InputBufferContainsNonASCII()
        {
            throw new NotImplementedException();
        }

        //[Fact]
        public void InputBufferContainsAllCodePoints()
        {
            throw new NotImplementedException();
        }
    }
}
