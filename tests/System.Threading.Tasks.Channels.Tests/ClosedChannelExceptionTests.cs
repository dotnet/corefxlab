// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace System.Threading.Tasks.Channels.Tests
{
    public class ClosedChannelExceptionTests
    {
        [Fact]
        public void Ctors()
        {
            var e = new ClosedChannelException();
            Assert.NotEmpty(e.Message);
            Assert.Null(e.InnerException);

            e = new ClosedChannelException("hello");
            Assert.Equal("hello", e.Message);
            Assert.Null(e.InnerException);

            var inner = new FormatException();
            e = new ClosedChannelException("hello", inner);
            Assert.Equal("hello", e.Message);
            Assert.Same(inner, e.InnerException);
        }
    }
}
