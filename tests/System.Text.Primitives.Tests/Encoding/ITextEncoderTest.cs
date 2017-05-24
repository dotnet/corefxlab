// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Primitives.Tests.Encoding
{
    public interface ITextEncoderTest
    {
        void InputBufferEmpty(TextEncoderTestHelper.SupportedEncoding from);
        void OutputBufferEmpty(TextEncoderTestHelper.SupportedEncoding from);
        void InputOutputBufferSizeCombinations(TextEncoderTestHelper.SupportedEncoding from);
        void InputBufferLargerThanOutputBuffer(TextEncoderTestHelper.SupportedEncoding from);
        void OutputBufferLargerThanInputBuffer(TextEncoderTestHelper.SupportedEncoding from);
        void InputBufferContainsOnlyInvalidData(TextEncoderTestHelper.SupportedEncoding from);
        void InputBufferContainsSomeInvalidData(TextEncoderTestHelper.SupportedEncoding from);
        void InputBufferEndsTooEarlyAndRestart(TextEncoderTestHelper.SupportedEncoding from);
        void InputBufferContainsOnlyASCII(TextEncoderTestHelper.SupportedEncoding from);
        void InputBufferContainsNonASCII(TextEncoderTestHelper.SupportedEncoding from);
        void InputBufferContainsAllCodePoints(TextEncoderTestHelper.SupportedEncoding from);
    }
}
