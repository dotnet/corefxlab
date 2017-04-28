// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Primitives.Tests.Encoding
{
    public interface ITextEncoderTest
    {
        void InputBufferEmpty();
        void OutputBufferEmpty();
        void InputBufferLargerThanOutputBuffer();
        void OutputBufferLargerThanInputBuffer();
        void InputBufferContainsOnlyInvalidData();
        void InputBufferContainsSomeInvalidData();
        void InputBufferContainsOnlyASCII();
        void InputBufferContainsNonASCII();
        void InputBufferContainsAllCodePoints();
    }
}
