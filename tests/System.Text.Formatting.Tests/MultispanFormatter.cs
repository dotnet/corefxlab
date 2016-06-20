// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Globalization;
using System.IO;
using System.Text.Formatting;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public partial class SystemTextFormattingTests
    {
        [Fact]
        public void MultispanFormatterBasics()
        {
            var data = new Multispan<byte>();
            var formatter = new MultispanFormatter(data, 10, FormattingData.InvariantUtf8);
            formatter.Append(new string('x', 10));
            formatter.Append(new string('x', 8));
            formatter.Append(new string('x', 8));
            formatter.Append(new string('x', 5));
            formatter.Append(new string('x', 5));

            data = formatter.Multispan;

            var bytesWritten = data.TotalItemCount();
            Assert.Equal(36, bytesWritten);

            var array = new byte[bytesWritten];
            data.CopyTo(array);
            foreach(byte b in array) {
                Assert.Equal((byte)'x', b);
            }
        }
    }
}
