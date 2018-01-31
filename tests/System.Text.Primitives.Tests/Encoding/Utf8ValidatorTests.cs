// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Linq;
using Xunit;

namespace System.Text.Primitives.Tests.Encoding
{
    public class Utf8ValidatorTests
    {
        [Fact]
        public void TryConsume_EmptySequence_ReturnsSuccess()
        {
            // Arrange

            Utf8Validator validator = new Utf8Validator();

            // Act & assert

            Assert.True(validator.TryConsume(ReadOnlySpan<byte>.Empty, isFinalChunk: false));
            Assert.True(validator.TryConsume(ReadOnlySpan<byte>.Empty, isFinalChunk: true));
        }

        [Fact]
        public void TryConsume_WithPartialSequence_FollowedByEmptySequence_AsFinalChunk_ReturnsFailure()
        {
            // Arrange

            Utf8Validator validator = new Utf8Validator();

            // Act & assert

            Assert.True(validator.TryConsume(new byte[] { 0xC2 }, isFinalChunk: false)); // [ C2 ] is valid start of incomplete multi-byte sequence
            Assert.True(validator.TryConsume(ReadOnlySpan<byte>.Empty, isFinalChunk: false));
            Assert.False(validator.TryConsume(ReadOnlySpan<byte>.Empty, isFinalChunk: true));
        }

        [Fact]
        public void TryConsume_OnceInvalid_AlwaysInvalid()
        {
            // Arrange

            Utf8Validator validator = new Utf8Validator();

            // Act & assert

            Assert.False(validator.TryConsume(new byte[] { 0xFF }, isFinalChunk: true)); // [ FF ] is always an invalid byte
            Assert.False(validator.TryConsume(new byte[] { 0x20 }, isFinalChunk: false));
            Assert.False(validator.TryConsume(new byte[] { 0x20 }, isFinalChunk: true));
        }

        [Fact]
        public void TryConsume_WithNoExistingPartialSequence_AndValidCompleteInputs_ReturnsSuccess()
        {
            // Arrange

            Utf8Validator validator = new Utf8Validator();

            // Act & assert

            Assert.True(validator.TryConsume(new byte[] { 0x10, 0x11, 0x12, 0x13 }, isFinalChunk: true));
            Assert.True(validator.TryConsume(new byte[] { 0x20, 0x21, 0x22, 0x23 }, isFinalChunk: true));
        }

        [Theory]
        [InlineData(new object[] { new[] { "20", "21", "22", "23", "24" } })]
        [InlineData(new object[] { new[] { "C2", "80", "C2", "80C2", "80C280C280" } })]
        [InlineData(new object[] { new[] { "E0", "BF", "BF", "E0BF", "BFE0", "BFBFE0BFBF", "E0BF", "BFE0BFBFE0BFBF" } })]
        [InlineData(new object[] { new[] { "F1", "80", "80", "80", "F180", "8080", "F180", "8080F1", "808080F1", "808080F1808080" } })]
        public void TryConsume_SuccessCases(string[] chunks)
        {
            // Arrange

            Utf8Validator validator = new Utf8Validator();

            // Act & assert - loop

            foreach (var chunk in chunks)
            {
                Assert.True(validator.TryConsume(TestHelper.DecodeHex(chunk), isFinalChunk: false));
            }

            // Act & assert - final

            Assert.True(validator.TryConsume(ReadOnlySpan<byte>.Empty, isFinalChunk: true));
        }


        [Theory]
        [InlineData(new object[] { new[] { "80" } })] // standalone continuation byte
        [InlineData(new object[] { new[] { "C2", "80C2", "80C280C2", "C2" } })] // C2 C2 is not a valid multi-byte sequence
        [InlineData(new object[] { new[] { "C2", "80C2", "80C280C2", "80C220" } })] // C2 20 is not a valid multi-byte sequence
        [InlineData(new object[] { new[] { "C2", "80C2", "80C280C2", "80C180" } })] // C1 is never a valid code unit
        [InlineData(new object[] { new[] { "F4", "808080F4", "8080", "80F490" } })] // out-of-range sequence
        [InlineData(new object[] { new[] { "F0", "908080F0", "80" } })] // overlong sequence
        public void TryConsume_FailureCases(string[] chunks)
        {
            // Arrange

            Utf8Validator validator = new Utf8Validator();

            // Act & assert - all but last chunk

            for (int i = 0; i < chunks.Length - 1; i++)
            {
                Assert.True(validator.TryConsume(TestHelper.DecodeHex(chunks[i]), isFinalChunk: false));
            }

            // Act & assert - final chunk (which should be bad)

            Assert.False(validator.TryConsume(TestHelper.DecodeHex(chunks.Last()), isFinalChunk: false));
        }
    }
}
