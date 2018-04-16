// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text
{
    /// <summary>
    /// Provides low-level methods for reading data directly from Unicode strings.
    /// </summary>
    public static class UnicodeReader
    {
        /// <summary>
        /// Given a UTF-8 input string, returns the first scalar value in the string.
        /// </summary>
        /// <param name="utf8Data">The UTF-8 input string to process.</param>
        /// <returns>
        /// If <paramref name="utf8Data"/> is empty, returns <see cref="SequenceValidity.Empty"/>, and the caller should
        /// not attempt to use the returned <see cref="UnicodeScalar"/> value.
        /// If <paramref name="utf8Data"/> begins with a valid UTF-8 representation of a scalar value, returns
        /// <see cref="SequenceValidity.ValidSequence"/>, the <see cref="UnicodeScalar"/> which appears at the
        /// beginning of the string, and the number of UTF-8 code units required to encode the scalar.
        /// If <paramref name="utf8Data"/> begins with an invalid or incomplete UTF-8 representation of a scalar
        /// value, returns <see cref="SequenceValidity.InvalidSequence"/>, <see cref="UnicodeScalar.ReplacementChar"/>,
        /// and the number of UTF-8 code units that the caller should skip before attempting to read the next scalar.
        /// </returns>
        public static (SequenceValidity status, UnicodeScalar scalar, int charsConsumed) PeekFirstScalar(ReadOnlySpan<Utf8Char> utf8Data) => throw null;

        /// <summary>
        /// Given a UTF-16 input string, returns the first scalar value in the string.
        /// </summary>
        /// <param name="utf16Data">The UTF-16 input string to process.</param>
        /// <returns>
        /// If <paramref name="utf16Data"/> is empty, returns <see cref="SequenceValidity.Empty"/>, and the caller should
        /// not attempt to use the returned <see cref="UnicodeScalar"/> value.
        /// If <paramref name="utf16Data"/> begins with a valid UTF-16 representation of a scalar value, returns
        /// <see cref="SequenceValidity.ValidSequence"/>, the <see cref="UnicodeScalar"/> which appears at the
        /// beginning of the string, and the number of UTF-16 code units required to encode the scalar.
        /// If <paramref name="utf16Data"/> begins with an invalid or incomplete UTF-16 representation of a scalar
        /// value, returns <see cref="SequenceValidity.InvalidSequence"/>, <see cref="UnicodeScalar.ReplacementChar"/>,
        /// and the number of UTF-16 code units that the caller should skip before attempting to read the next scalar.
        /// </returns>
        public static (SequenceValidity status, UnicodeScalar scalar, int charsConsumed) PeekFirstScalar(ReadOnlySpan<char> utf16Data) => throw null;
    }
}
