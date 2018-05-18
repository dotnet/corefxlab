// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace System.Text
{
    public sealed partial class Utf8String
    {
        private static bool EqualsOrdinal(Utf8String utf8, ReadOnlySpan<char> utf16)
        {
            // Optimize ASCII strings

            if (utf8.ContainsOnlyAsciiData)
            {
                return AsciiInvariantHelpers.EqualsCaseSensitive(utf8.Bytes, utf16);
            }

            // Non-ASCII string goes down slow code path where we enumerate each code point.
            // n.b. Invalid sequences always cause an equality check failure since there's no
            // possible way to transcode an invalid sequence to a different UTF representation.

            ReadOnlySpan<byte> utf8Data = utf8.Bytes;

            while (!utf8Data.IsEmpty)
            {
                var (validity, utf8Scalar, utf8BytesConsumed) = UnicodeReader.PeekFirstScalarUtf8(utf8Data);
                if (validity == SequenceValidity.ValidSequence)
                {
                    var (utf16Scalar, utf16CharsConsumed) = UnicodeEnumerator.GetNextScalarUtf16(utf16); // returns negative number on error
                    if (utf8Scalar.Value == (uint)utf16Scalar)
                    {
                        utf8Data = utf8Data.Slice(utf8BytesConsumed);
                        utf16 = utf16.Slice(utf16CharsConsumed);
                        continue;
                    }
                }

                return false; // found a sequence that was invalid or ran out of data unexpectedly
            }

            // Ran through both the UTF-8 and the UTF-16 buffers in their entirety and saw no mismatched scalars.

            Debug.Assert(utf8Data.IsEmpty);
            Debug.Assert(utf16.IsEmpty);
            return true;
        }
    }
}
