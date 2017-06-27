// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using Xunit;

namespace System.Text.Primitives.Tests
{
    public static class TestHelper
    {
        public static string SpanToString(Span<byte> span, SymbolTable symbolTable = null)
        {
            if (symbolTable == null || symbolTable == SymbolTable.InvariantUtf8)
            {
                Assert.Equal(TransformationStatus.Done, Encoders.Utf16.ComputeEncodedBytesFromUtf8(span, out int needed));
                Span<byte> output = new byte[needed];
                Assert.Equal(TransformationStatus.Done, Encoders.Utf16.ConvertFromUtf8(span, output, out int consumed, out int written));
                return new string(output.NonPortableCast<byte, char>().ToArray());
            }
            else if (symbolTable == SymbolTable.InvariantUtf16)
            {
                return new string(span.NonPortableCast<byte, char>().ToArray());
            }

            throw new NotSupportedException();
        }
    }
}
