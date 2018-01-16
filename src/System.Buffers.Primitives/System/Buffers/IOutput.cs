// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers
{
    public interface IOutput
    {
        void Advance(int bytes);
        /// <summary>desiredBufferLength == 0 means "i don't care"</summary>
        Memory<byte> GetMemory(int minimumLength = 0);
        /// <summary>desiredBufferLength == 0 means "i don't care"</summary>
        Span<byte> GetSpan(int minimumLength = 0);
    }
}
