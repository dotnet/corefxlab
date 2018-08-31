// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Buffers.Text
{
    /// <summary>
    /// Allows processing discontiguous buffers of UTF-8 data, letting the caller know whether
    /// any data seen so far is invalid UTF-8. The caller should invoke <see cref="TryConsume(ReadOnlySpan{byte, bool})"/>
    /// in a loop until all data has been consumed.
    /// </summary>
    /// <remarks>
    /// This type is a mutable struct.
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public struct Utf8Validator
    {
        /// <summary>
        /// A sequence that represents a fresh no-partial-buffer validity checker.
        /// </summary>
        private const uint DefaultSequence = 0;

        /// <summary>
        /// A sequence that represents a validity checker which has seen invalid data.
        /// </summary>
        private const uint InvalidSequence = unchecked((uint)~0);

        // Packed data that contains both the partial sequence seen and the expected number of bytes remaining.
        // PS1B: partial sequence first byte, etc.
        // LEN: BYTE stating how many bytes (0 .. 3) have been read so far into the partial sequence.
        // If the high bit of LEN is set, an invalid sequence was seen earlier.
        // Big-endian machine: [ PS1B, PS2B, PS3B, LEN ]
        // Little-endian machine: [ PS3B, PS2B, PS1B, LEN ]
        private uint _partialSequence;

        private string DebuggerDisplay
        {
            get
            {
                if (_partialSequence == 0)
                {
                    return "Data VALID so far; no partial sequence consumed.";
                }
                else if (IsInvalid)
                {
                    return "Data INVALID.";
                }
                else
                {
                    switch ((byte)_partialSequence)
                    {
                        case 1:
                            return (BitConverter.IsLittleEndian)
                                ? $"Data VALID so far; partial sequence [ {(byte)(_partialSequence >> 8):X2} ] consumed."
                                : $"Data VALID so far; partial sequence [ {(_partialSequence >> 24):X2} ] consumed.";
                        case 2:
                            return (BitConverter.IsLittleEndian)
                                ? $"Data VALID so far; partial sequence [ {(byte)(_partialSequence >> 8):X2} {(byte)(_partialSequence >> 16):X2} ] consumed."
                                : $"Data VALID so far; partial sequence [ {(_partialSequence >> 24):X2} {(_partialSequence >> 16):X2} ] consumed.";

                        case 3:
                            return (BitConverter.IsLittleEndian)
                                ? $"Data VALID so far; partial sequence [ {(byte)(_partialSequence >> 8):X2} {(byte)(_partialSequence >> 16):X2} {(_partialSequence >> 24):X2} ] consumed."
                                : $"Data VALID so far; partial sequence [ {(_partialSequence >> 24):X2} {(_partialSequence >> 16):X2} {(byte)(_partialSequence >> 8):X2} ] consumed.";

                        default:
                            return "** INTERNAL ERROR **";
                    }
                }
            }
        }

        private bool IsInvalid => IsInvalidPartialSequence(_partialSequence);

        private static uint ConsumeDataWithExistingPartialSequence(uint partialSequence, ReadOnlySpan<byte> utf8Bytes)
        {
            Debug.Assert(partialSequence != 0 && !IsInvalidPartialSequence(partialSequence));

            int partialSequenceOriginalByteCount = (byte)partialSequence;

            if (BitConverter.IsLittleEndian)
            {
                // When we turn this into a Span<byte>, want MSB to be the first byte of the partial sequence
                partialSequence >>= 8;
            }

            // Copy as much data as we can from the input buffer to our partial sequence.

            Span<byte> partialSequenceAsBytes = stackalloc byte[sizeof(uint)];
            MemoryMarshal.Write(partialSequenceAsBytes, ref partialSequence);

            int numBytesToCopyFromInputToPartialSequence = Math.Min(4 - partialSequenceOriginalByteCount, utf8Bytes.Length);
            utf8Bytes.Slice(0, numBytesToCopyFromInputToPartialSequence).CopyTo(partialSequenceAsBytes.Slice(partialSequenceOriginalByteCount));
            int partialSequenceNewByteCount = partialSequenceOriginalByteCount + numBytesToCopyFromInputToPartialSequence;

            // And check for validity of the new (hopefully complete) partial sequence.

            var validity = Utf8Utility.PeekFirstSequence(partialSequenceAsBytes.Slice(0, partialSequenceNewByteCount), out int numBytesConsumed, out _);
            Debug.Assert(1 <= numBytesConsumed && numBytesConsumed <= 4);

            if (validity == SequenceValidity.WellFormed)
            {
                // This is the happy path; we've consumed some set of bytes from the input
                // buffer and it has caused the partial sequence to validate. Let's calculate
                // how many bytes from the input buffer were required to complete the sequence,
                // then strip them off the incoming data.

                // n.b. This might not be the same as numBytesToCopyFromInputToPartialSequence.
                int numBytesRequiredFromInputBufferToFinishPartialSequence = numBytesConsumed - partialSequenceOriginalByteCount;
                return ConsumeDataWithoutExistingPartialSequence(utf8Bytes.Slice(numBytesRequiredFromInputBufferToFinishPartialSequence));
            }
            else if (validity == SequenceValidity.Incomplete)
            {
                // We've consumed all data available to us and we still have an incomplete sequence.
                // It's still valid (until we see invalid bytes), so squirrel away what we've seen
                // and report success to our caller.

                Debug.Assert(numBytesConsumed < 4);
                Debug.Assert(numBytesConsumed == partialSequenceNewByteCount);

                // Put all partial data into the high 3 bytes, making room for us to
                // write the count of partial bytes in the buffer as the low byte.

                partialSequence = MemoryMarshal.Read<uint>(partialSequenceAsBytes);
                if (BitConverter.IsLittleEndian)
                {
                    return (partialSequence << 8) | (uint)numBytesConsumed;
                }
                else
                {
                    return (partialSequence & unchecked((uint)~0xFF)) | (uint)numBytesConsumed;
                }
            }
            else
            {
                // Truly invalid data.
                // (Shouldn't have gotten 'Empty' or 'WellFormed'.)

                Debug.Assert(validity == SequenceValidity.Invalid);

                return InvalidSequence;
            }
        }

        private static uint ConsumeDataWithoutExistingPartialSequence(ReadOnlySpan<byte> utf8Bytes)
        {
            var indexOfFirstInvalidSequence = Utf8Utility.GetIndexOfFirstInvalidUtf8Sequence(utf8Bytes);
            if (indexOfFirstInvalidSequence < 0)
            {
                // Successfully consumed entire buffer without error.

                return DefaultSequence;
            }
            else
            {
                // Couldn't consume entire buffer; is this due to a partial buffer or truly invalid data?

                utf8Bytes = utf8Bytes.Slice(indexOfFirstInvalidSequence);
                var validity = Utf8Utility.PeekFirstSequence(utf8Bytes, out int numBytesConsumed, out _);
                if (validity == SequenceValidity.Incomplete)
                {
                    // Saw a partial (not invalid) sequence, remember it for next time.

                    Debug.Assert(1 <= numBytesConsumed && numBytesConsumed <= 3);

                    // Put all partial data into the high 3 bytes, making room for us to
                    // write the count of partial bytes in the buffer as the low byte.

                    Span<byte> partialSequenceAsBytes = stackalloc byte[sizeof(uint)];
                    MemoryMarshal.Write(partialSequenceAsBytes, ref numBytesConsumed);

                    if (BitConverter.IsLittleEndian)
                    {
                        utf8Bytes.Slice(0, numBytesConsumed).CopyTo(partialSequenceAsBytes.Slice(1));
                    }
                    else
                    {
                        utf8Bytes.Slice(0, numBytesConsumed).CopyTo(partialSequenceAsBytes);
                    }

                    return MemoryMarshal.Read<uint>(partialSequenceAsBytes);
                }
                else
                {
                    // Truly invalid data.
                    // (Shouldn't have gotten 'Empty' or 'WellFormed'.)

                    Debug.Assert(validity == SequenceValidity.Invalid);

                    return InvalidSequence;
                }
            }
        }

        private static bool IsInvalidPartialSequence(uint partialSequence) => ((byte)partialSequence & 0x80) != 0;

        /// <summary>
        /// Consumes UTF-8 data from the provided buffer. If <paramref name="utf8Bytes"/> ends
        /// with an incomplete (but not invalid) sequence, this instance will save the
        /// incomplete sequence, this method will return <see langword="true"/>, and the
        /// next call to this method will automatically prepend the saved sequence before
        /// processing. This allows the caller to invoke this method in a loop passing in
        /// discontiguous buffers, even if UTF-8 sequences are split across those buffers.
        /// If this method returns <see langword="false"/>, the entire instance is marked
        /// as invalid, and all subsequent calls to this method will also return <see langword="false"/>.
        /// </summary>
        /// <param name="utf8Bytes">A buffer containing UTF-8 data to validate.</param>
        /// <param name="isFinalChunk">
        /// <see langword="true"/> if the caller will pass no more data to the validator,
        /// <see langword="false"/> if the caller will pass more data to the validator.
        /// If <see langword="true"/>, the validator will treat an incomplete sequence at
        /// the end of <paramref name="utf8Bytes"/> as an error instead of saving it for
        /// the next call.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if no invalid data has yet been seen,
        /// <see langword="false"/> if invalid data has ever been seen.
        /// </returns>
        /// <remarks>
        /// It is important that the caller invoke this method with
        /// <paramref name="isFinalChunk"/>=<see langword="true"/> when there is no more
        /// data, otherwise the caller may inadvertently believe that the entire input data
        /// is valid even if there's an unfinished UTF-8 sequence at the very end of the data.
        /// It's also ok for the caller to pass an empty span for <paramref name="utf8Bytes"/>
        /// when calling with <paramref name="isFinalChunk"/>=<see langword="true"/> to signal
        /// "there is no more data; fail if there were unfinished sequences in the previous call."
        /// </remarks>
        public bool TryConsume(ReadOnlySpan<byte> utf8Bytes, bool isFinalChunk)
        {
            if (utf8Bytes.Length > 0 && !IsInvalid)
            {
                if (_partialSequence == 0)
                {
                    _partialSequence = ConsumeDataWithoutExistingPartialSequence(utf8Bytes);
                }
                else
                {
                    _partialSequence = ConsumeDataWithExistingPartialSequence(_partialSequence, utf8Bytes);
                }
            }

            // If the caller signaled the end of the data stream but there's still an
            // unfinished sequence awaiting processing, this is failure.

            if (isFinalChunk && _partialSequence != 0)
            {
                _partialSequence = InvalidSequence;
            }

            return !IsInvalid;
        }
    }
}
