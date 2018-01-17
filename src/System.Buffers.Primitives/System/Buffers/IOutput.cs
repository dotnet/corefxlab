// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers
{
    public interface IOutput
    {
        void Advance(int bytes);
        /// <summary>If minimumLength is equal to zero currently available memory would be returned</summary>
        Memory<byte> GetMemory(int minimumLength = 0);
        /// <summary>If minimumLength is equal to zero currently available memory would be returned/summary>
        Span<byte> GetSpan(int minimumLength = 0);
    }
}
