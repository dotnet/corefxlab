// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Primitives.Tests.Encoding
{
    public class DecodeFromUtf16Tests : ITextEncoderTest
    {
        private static TextEncoder utf16 = TextEncoder.Utf16;

        public void InputBufferContainsAllCodePoints()
        {
            throw new NotImplementedException();
        }

        public void InputBufferContainsNonASCII()
        {
            throw new NotImplementedException();
        }

        public void InputBufferContainsOnlyASCII()
        {
            throw new NotImplementedException();
        }

        public void InputBufferContainsOnlyInvalidData()
        {
            throw new NotImplementedException();
        }

        public void InputBufferContainsSomeInvalidData()
        {
            throw new NotImplementedException();
        }

        public void InputBufferEmpty()
        {
            throw new NotImplementedException();
        }

        public void InputBufferLargerThanOutputBuffer()
        {
            throw new NotImplementedException();
        }

        public void OutputBufferEmpty()
        {
            throw new NotImplementedException();
        }

        public void OutputBufferLargerThanInputBuffer()
        {
            throw new NotImplementedException();
        }
    }
}
