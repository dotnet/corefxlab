// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Xunit;

namespace System.Buffers.Tests
{
    public class BufferHandleTests
    {
        [Fact]
        public void MemoryHandleFreeUninitialized()
        {
            var handle = default(MemoryHandle);
            handle.Dispose();
        }
    }
}
