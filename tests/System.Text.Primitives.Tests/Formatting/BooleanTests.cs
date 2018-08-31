// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using Xunit;

namespace System.Text.Primitives.Tests
{
    public partial class FormattingTests
    {
        [Theory]
        [InlineData('G', true, "True")]
        [InlineData('G', false, "False")]
        [InlineData('l', true, "true")]
        [InlineData('l', false, "false")]
        [InlineData(' ', true, "True")]
        [InlineData(' ', false, "False")]
        public void BooleanUtf16(char format, bool value, string expected)
        {
            StandardFormat f = format == ' ' ? default : new StandardFormat(format);
            byte[] buffer = new byte[256];
            Assert.True(Utf16Formatter.TryFormat(value, buffer, out int bytesWritten, f));
            var actual = Text.Encoding.Unicode.GetString(buffer, 0, bytesWritten);
            Assert.Equal(expected, actual);
        }
    }
}
