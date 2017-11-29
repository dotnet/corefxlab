// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Native;
using Xunit;

namespace System.Buffers.Tests
{
    public class NativeBufferPoolTests
    {
        [Fact]
        public void BasicsWork() {
            var pool = NativeMemoryPool.Shared;
            var buffer = pool.Rent(10);
            buffer.Dispose();
            buffer = pool.Rent(10);
            buffer.Dispose();
        }
    }
}
