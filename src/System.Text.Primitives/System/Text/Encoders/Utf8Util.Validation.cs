// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

//
// This file contains workhorse methods for performing validation of UTF-8 byte sequences.
//

namespace System.Buffers.Text
{
    internal static partial class Utf8Util
    {
        // This method will consume as many ASCII bytes as it can using fast vectorized processing, returning the number of
        // consumed (ASCII) bytes. It's possible that the method exits early, perhaps because there is some non-ASCII byte
        // later in the sequence or because we're running out of input to search. The intent is that the caller *skips over*
        // the number of bytes returned by this method, then it continues data processing from the next byte.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static unsafe int ConsumeAsciiBytesVectorized(ref byte buffer, int length)
        {
            // Only allow vectorization if vector operations are hardware-accelerated
            // and vectors are at least 128 bits in length.
            if (Vector.IsHardwareAccelerated && (Vector<byte>.Count >= 2 * sizeof(ulong)))
            {
                // Need to pin buffer so the GC doesn't cause it to be misaligned when we're
                // reading vectors from it.
                fixed (byte* pbBuffer = &buffer)
                {
                    int offsetAtWhichVectorIsAligned;
                    {
                        int referenceMisalignment = (int)pbBuffer % Vector<byte>.Count;
                        offsetAtWhichVectorIsAligned = (referenceMisalignment == 0) ? 0 : (Vector<byte>.Count - referenceMisalignment);
                    }

                    if (length - offsetAtWhichVectorIsAligned < (Vector<byte>.Count * 2))
                    {
                        // After alignment there's not enough data left over to warrant setting
                        // up the vectorization code path.
                        return 0;
                    }

                    int numBytesConsumed = 0;

                    // Consume QWORDs or DWORDs until we're aligned with where we can start vectorization.
                    // It's ok if we duplicate a little bit of work by having a single QWORD or DWORD read
                    // overlap with the vector read area. It's not worth the overhead of checking for this
                    // condition and avoiding the duplicate work.

                    if (IntPtr.Size >= sizeof(ulong))
                    {
                        while (numBytesConsumed < offsetAtWhichVectorIsAligned)
                        {
                            if (!QWordAllBytesAreAscii(Unsafe.ReadUnaligned<ulong>(&pbBuffer[numBytesConsumed])))
                            {
                                return numBytesConsumed; // found a high bit set somewhere
                            }
                            numBytesConsumed += sizeof(ulong);
                        }
                    }
                    else
                    {
                        while (numBytesConsumed < offsetAtWhichVectorIsAligned)
                        {
                            if (!DWordAllBytesAreAscii(Unsafe.ReadUnaligned<uint>(&pbBuffer[numBytesConsumed])))
                            {
                                return numBytesConsumed; // found a high bit set somewhere
                            }
                            numBytesConsumed += sizeof(uint);
                        }
                    }

                    // At this point, we're potentially past where we know we can begin a fast
                    // vectorized search, so let's start from the proper aligned offset.

                    numBytesConsumed = offsetAtWhichVectorIsAligned;

                    var highBitMask = new Vector<byte>(0x80);
                    do
                    {
                        // Read two vector lines at a time.

                        Debug.Assert(length - numBytesConsumed >= 2 * Vector<byte>.Count); // Invariant should've been checked earlier, we need enough room to read data

                        if (((Unsafe.Read<Vector<byte>>(&pbBuffer[numBytesConsumed]) | Unsafe.Read<Vector<byte>>(&pbBuffer[numBytesConsumed + Vector<byte>.Count])) & highBitMask) != Vector<byte>.Zero)
                        {
                            break; // found a non-ascii character somewhere in this vector
                        }

                        numBytesConsumed += 2 * Vector<byte>.Count;
                    } while (length - numBytesConsumed >= 2 * Vector<byte>.Count);

                    return numBytesConsumed;
                }
            }
            else
            {
                return 0; // can't vectorize the search
            }
        }

        /// <summary>
        /// Returns the offset in <paramref name="inputBuffer"/> of where the first invalid UTF-8 sequence appears,
        /// or -1 if the input is valid UTF-8 text. (Empty inputs are considered valid.) On method return the
        /// <paramref name="runeCount"/> parameter will contain the total number of Unicode scalar values seen
        /// up to (but not including) the first invalid sequence, and <paramref name="surrogatePairCount"/> will
        /// contain the number of surrogate pairs present if this text up to (but not including) the first
        /// invalid sequence were represented as UTF-16. To get the total UTF-16 code unit count, add
        /// <paramref name="surrogatePairCount"/> to <paramref name="runeCount"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetIndexOfFirstInvalidUtf8Sequence(ReadOnlySpan<byte> input, out int runeCount, out int surrogatePairCount)
            => GetIndexOfFirstInvalidUtf8Sequence(ref input.DangerousGetPinnableReference(), input.Length, out runeCount, out surrogatePairCount);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int GetIndexOfFirstInvalidUtf8Sequence(ref byte inputBuffer, int inputLength, out int runeCount, out int surrogatePairCount)
        {
            // The fields below control where we read from the buffer.

            IntPtr inputBufferCurrentOffset = IntPtr.Zero;
            int tempRuneCount = inputLength;
            int tempSurrogatePairCount = 0;

            // If the sequence is long enough, try running vectorized "is this sequence ASCII?"
            // logic. We perform a small test of the first few bytes to make sure they're all
            // ASCII before we incur the cost of invoking the vectorized code path.

            if (Vector.IsHardwareAccelerated)
            {
                if (IntPtr.Size >= 8)
                {
                    // Test first 16 bytes and check for all-ASCII.
                    if ((inputLength >= 2 * sizeof(ulong) + 2 * Vector<byte>.Count) && QWordAllBytesAreAscii(ReadAndFoldTwoQWordsUnaligned(ref inputBuffer)))
                    {
                        inputBufferCurrentOffset = (IntPtr)(2 * sizeof(ulong) + ConsumeAsciiBytesVectorized(ref Unsafe.Add(ref inputBuffer, 2 * sizeof(ulong)), inputLength - 2 * sizeof(ulong)));
                    }
                }
                else
                {
                    // Test first 8 bytes and check for all-ASCII.
                    if ((inputLength >= 2 * sizeof(uint) + 2 * Vector<byte>.Count) && DWordAllBytesAreAscii(ReadAndFoldTwoDWordsUnaligned(ref inputBuffer)))
                    {
                        inputBufferCurrentOffset = (IntPtr)(2 * sizeof(uint) + ConsumeAsciiBytesVectorized(ref Unsafe.Add(ref inputBuffer, 2 * sizeof(uint)), inputLength - 2 * sizeof(uint)));
                    }
                }
            }

            IntPtr inputBufferOffsetAtWhichToAllowUnrolling = IntPtr.Zero;
            int inputBufferRemainingBytes = inputLength - ConvertIntPtrToInt32WithoutOverflowCheck(inputBufferCurrentOffset);

            // Begin the main loop.

#if DEBUG
            long lastOffsetProcessed = -1; // used for invariant checking in debug builds
#endif

            while (inputBufferRemainingBytes >= sizeof(uint))
            {
                BeforeReadDWord:

                // Read 32 bits at a time. This is enough to hold any possible UTF8-encoded scalar.

                Debug.Assert(inputLength - (int)inputBufferCurrentOffset >= sizeof(uint));
                uint thisDWord = Unsafe.ReadUnaligned<uint>(ref Unsafe.Add(ref inputBuffer, inputBufferCurrentOffset));

                AfterReadDWord:

#if DEBUG
                Debug.Assert(lastOffsetProcessed < (long)inputBufferCurrentOffset, "Algorithm should've made forward progress since last read.");
                lastOffsetProcessed = (long)inputBufferCurrentOffset;
#endif

                // First, check for the common case of all-ASCII bytes.

                if (DWordAllBytesAreAscii(thisDWord))
                {
                    // We read an all-ASCII sequence.

                    inputBufferCurrentOffset += 4;
                    inputBufferRemainingBytes -= 4;

                    // If we saw a sequence of all ASCII, there's a good chance a significant amount of following data is also ASCII.
                    // Below is basically unrolled loops with poor man's vectorization.

                    if (IntPtrIsLessThan(inputBufferCurrentOffset, inputBufferOffsetAtWhichToAllowUnrolling))
                    {
                        // We saw non-ASCII data last time we tried loop unrolling, so don't bother going
                        // down the unrolling path again until we've bypassed that data. No need to perform
                        // a bounds check here since we already checked the bounds as part of the loop unrolling path.
                        goto BeforeReadDWord;
                    }
                    else
                    {
                        if (IntPtr.Size >= 8)
                        {
                            while (inputBufferRemainingBytes >= 2 * sizeof(ulong))
                            {
                                // Don't use the "read and fold" utility method here since the JITter produces sub-optimal assembly,
                                // even with forced inlining.

                                ulong thisQWord = Unsafe.ReadUnaligned<ulong>(ref Unsafe.Add(ref inputBuffer, inputBufferCurrentOffset))
                                    | Unsafe.ReadUnaligned<ulong>(ref Unsafe.Add(ref inputBuffer, inputBufferCurrentOffset + sizeof(ulong)));

                                if (!QWordAllBytesAreAscii(thisQWord))
                                {
                                    // Non-ASCII data incoming, set flag which tells us not to go down this code path.
                                    // Fudge the value slightly if we're approaching the end of the buffer so we don't overrun it.
                                    // We know the code below won't overflow because the loop invariant requires that there be
                                    // enough remaining bytes to fit into an Int32.
                                    inputBufferOffsetAtWhichToAllowUnrolling = (IntPtr)Math.Min(ConvertIntPtrToInt32WithoutOverflowCheck(inputBufferCurrentOffset) + 2 * sizeof(ulong), inputLength - sizeof(uint));
                                    goto BeforeReadDWord;
                                }

                                inputBufferCurrentOffset += 2 * sizeof(ulong);
                                inputBufferRemainingBytes -= 2 * sizeof(ulong);
                            }
                        }
                        else
                        {
                            while (inputBufferRemainingBytes >= 4 * sizeof(uint))
                            {
                                // Don't use the "read and fold" utility method here since the JITter produces sub-optimal assembly,
                                // even with forced inlining.

                                thisDWord = Unsafe.ReadUnaligned<uint>(ref Unsafe.Add(ref inputBuffer, inputBufferCurrentOffset))
                                    | Unsafe.ReadUnaligned<uint>(ref Unsafe.Add(ref inputBuffer, inputBufferCurrentOffset + sizeof(uint)))
                                    | Unsafe.ReadUnaligned<uint>(ref Unsafe.Add(ref inputBuffer, inputBufferCurrentOffset + 2 * sizeof(uint)))
                                    | Unsafe.ReadUnaligned<uint>(ref Unsafe.Add(ref inputBuffer, inputBufferCurrentOffset + 3 * sizeof(uint)));

                                if (!DWordAllBytesAreAscii(thisDWord))
                                {
                                    // Non-ASCII data incoming, set flag which tells us not to go down this code path.
                                    // Fudge the value slightly if we're approaching the end of the buffer so we don't overrun it.
                                    // We know the code below won't overflow because the loop invariant requires that there be
                                    // enough remaining bytes to fit into an Int32.
                                    inputBufferOffsetAtWhichToAllowUnrolling = (IntPtr)Math.Min(ConvertIntPtrToInt32WithoutOverflowCheck(inputBufferCurrentOffset) + 4 * sizeof(uint), inputLength - sizeof(uint));
                                    goto BeforeReadDWord;
                                }

                                inputBufferCurrentOffset += 4 * sizeof(uint);
                                inputBufferRemainingBytes -= 4 * sizeof(uint);
                            }
                        }
                    }

                    continue;
                }

                // Next, try stripping off ASCII bytes one at a time.
                // We only handle up to three ASCII bytes here since we handled the four ASCII byte case above.

                if (DWordFirstByteIsAscii(thisDWord))
                {
                    if (DWordSecondByteIsAscii(thisDWord))
                    {
                        if (DWordThirdByteIsAscii(thisDWord))
                        {
                            inputBufferCurrentOffset += 3;
                            inputBufferRemainingBytes -= 3;
                        }
                        else
                        {
                            inputBufferCurrentOffset += 2;
                            inputBufferRemainingBytes -= 2;
                        }
                    }
                    else
                    {
                        inputBufferCurrentOffset += 1;
                        inputBufferRemainingBytes--;
                    }

                    if (inputBufferRemainingBytes < sizeof(uint))
                    {
                        goto ProcessRemainingBytesSlow; // Input buffer doesn't contain enough data to read a DWORD
                    }
                    else
                    {
                        // The input buffer at the current offset contains a non-ASCII byte.
                        // Read an entire DWORD and fall through to multi-byte consumption logic.
                        thisDWord = Unsafe.ReadUnaligned<uint>(ref Unsafe.Add(ref inputBuffer, inputBufferCurrentOffset));
                    }
                }

                // At this point, we know we're working with a multi-byte code unit,
                // but we haven't yet validated it.

                // The masks and comparands are derived from the Unicode Standard, Table 3-6.
                // Additionally, we need to check for valid byte sequences per Table 3-7.

                // Check the 2-byte case.

                BeforeProcessTwoByteSequence:

                if (DWordBeginsWithUtf8TwoByteMask(thisDWord))
                {
                    // Per Table 3-7, valid sequences are:
                    // [ C2..DF ] [ 80..BF ]

                    if (DWordBeginsWithOverlongUtf8TwoByteSequence(thisDWord)) { goto Error; }

                    ProcessTwoByteSequenceSkipOverlongFormCheck:

                    // Optimization: If this is a two-byte-per-character language like Cyrillic or Hebrew,
                    // there's a good chance that if we see one two-byte run then there's another two-byte
                    // run immediately after. Let's check that now.

                    // On little-endian platforms, we can check for the two-byte UTF8 mask *and* validate that
                    // the value isn't overlong using a single comparison. On big-endian platforms, we'll need
                    // to validate the mask and validate that the sequence isn't overlong as two separate comparisons.

                    if ((BitConverter.IsLittleEndian && DWordEndsWithValidUtf8TwoByteSequenceLittleEndian(thisDWord))
                      || (!BitConverter.IsLittleEndian && (DWordEndsWithUtf8TwoByteMask(thisDWord) && !DWordEndsWithOverlongUtf8TwoByteSequence(thisDWord))))
                    {
                        ConsumeTwoAdjacentKnownGoodTwoByteSequences:

                        // We have two runs of two bytes each.
                        inputBufferCurrentOffset += 4;
                        inputBufferRemainingBytes -= 4;
                        tempRuneCount -= 2; // 4 bytes -> 2 runes

                        if (inputBufferRemainingBytes >= sizeof(uint))
                        {
                            // Optimization: If we read a long run of two-byte sequences, the next sequence is probably
                            // also two bytes. Check for that first before going back to the beginning of the loop.

                            thisDWord = Unsafe.ReadUnaligned<uint>(ref Unsafe.Add(ref inputBuffer, inputBufferCurrentOffset));

                            if (BitConverter.IsLittleEndian)
                            {
                                if (DWordBeginsWithValidUtf8TwoByteSequenceLittleEndian(thisDWord))
                                {
                                    // The next sequence is a valid two-byte sequence.
                                    goto ProcessTwoByteSequenceSkipOverlongFormCheck;
                                }
                            }
                            else
                            {
                                if (DWordBeginsAndEndsWithUtf8TwoByteMask(thisDWord))
                                {
                                    if (DWordBeginsWithOverlongUtf8TwoByteSequence(thisDWord) || DWordEndsWithOverlongUtf8TwoByteSequence(thisDWord))
                                    {
                                        // Mask said it was 2x 2-byte sequences but validation failed, go to beginning of loop for error handling
                                        goto AfterReadDWord;
                                    }
                                    else
                                    {
                                        // Validated next bytes are 2x 2-byte sequences
                                        goto ConsumeTwoAdjacentKnownGoodTwoByteSequences;
                                    }
                                }
                                else if (DWordBeginsWithUtf8TwoByteMask(thisDWord))
                                {
                                    if (DWordBeginsWithOverlongUtf8TwoByteSequence(thisDWord))
                                    {
                                        // Mask said it was a 2-byte sequence but validation failed
                                        goto Error;
                                    }
                                    else
                                    {
                                        // Validated next bytes are a single 2-byte sequence with no valid 2-byte sequence following
                                        goto ConsumeSingleKnownGoodTwoByteSequence;
                                    }
                                }
                            }

                            // If we reached this point, the next sequence is something other than a valid
                            // two-byte sequence, so go back to the beginning of the loop.
                            goto AfterReadDWord;
                        }
                        else
                        {
                            goto ProcessRemainingBytesSlow; // Running out of data - go down slow path
                        }
                    }

                    ConsumeSingleKnownGoodTwoByteSequence:

                    // The buffer contains a 2-byte sequence followed by 2 bytes that aren't a 2-byte sequence.
                    // Unlikely that a 3-byte sequence would follow a 2-byte sequence, so perhaps remaining
                    // bytes are ASCII?

                    if (DWordThirdByteIsAscii(thisDWord))
                    {
                        if (DWordFourthByteIsAscii(thisDWord))
                        {
                            inputBufferCurrentOffset += 4; // a 2-byte sequence + 2 ASCII bytes
                            inputBufferRemainingBytes -= 4; // a 2-byte sequence + 2 ASCII bytes
                            tempRuneCount--; // 2-byte sequence + 2 ASCII bytes -> 3 runes
                        }
                        else
                        {
                            inputBufferCurrentOffset += 3; // a 2-byte sequence + 1 ASCII byte
                            inputBufferRemainingBytes -= 3; // a 2-byte sequence + 1 ASCII byte
                            tempRuneCount--; // 2-byte sequence + 1 ASCII bytes -> 2 runes

                            // A two-byte sequence followed by an ASCII byte followed by a non-ASCII byte.
                            // Read in the next DWORD and jump directly to the start of the multi-byte processing block.

                            if (inputBufferRemainingBytes >= sizeof(uint))
                            {
                                thisDWord = Unsafe.ReadUnaligned<uint>(ref Unsafe.Add(ref inputBuffer, inputBufferCurrentOffset));
                                goto BeforeProcessTwoByteSequence;
                            }
                        }
                    }
                    else
                    {
                        inputBufferCurrentOffset += 2;
                        inputBufferRemainingBytes -= 2;
                        tempRuneCount--; // 2-byte sequence -> 1 rune1
                    }

                    continue;
                }

                // Check the 3-byte case.

                if (DWordBeginsWithUtf8ThreeByteMask(thisDWord))
                {
                    ProcessThreeByteSequenceWithCheck:

                    // We need to check for overlong or surrogate three-byte sequences.
                    //
                    // Per Table 3-7, valid sequences are:
                    // [   E0   ] [ A0..BF ] [ 80..BF ]
                    // [ E1..EC ] [ 80..BF ] [ 80..BF ]
                    // [   ED   ] [ 80..9F ] [ 80..BF ]
                    // [ EE..EF ] [ 80..BF ] [ 80..BF ]
                    //
                    // Big-endian examples of using the above validation table:
                    // E0A0 = 1110 0000 1010 0000 => invalid (overlong ) patterns are 1110 0000 100# ####
                    // ED9F = 1110 1101 1001 1111 => invalid (surrogate) patterns are 1110 1101 101# ####
                    // If using the bitmask ......................................... 0000 1111 0010 0000 (=0F20),
                    // Then invalid (overlong) patterns match the comparand ......... 0000 0000 0000 0000 (=0000),
                    // And invalid (surrogate) patterns match the comparand ......... 0000 1101 0010 0000 (=0D20).

                    if (BitConverter.IsLittleEndian)
                    {
                        // The "overlong or surrogate" check can be implemented using a single jump, but there's
                        // some overhead to moving the bits into the correct locations in order to perform the
                        // correct comparison, and in practice the processor's branch prediction capability is
                        // good enough that we shouldn't bother. So we'll use two jumps instead.

                        // Can't extract this check into its own helper method because JITter produces suboptimal
                        // assembly, even with aggressive inlining.

                        uint comparand = thisDWord & 0x0000200FU;
                        if ((comparand == 0U) || (comparand == 0x0000200DU)) { goto Error; }
                    }
                    else
                    {
                        uint comparand = thisDWord & 0x0F200000U;
                        if ((comparand == 0U) || (comparand == 0x0D200000U)) { goto Error; }
                    }

                    ProcessSingleThreeByteSequenceSkipOverlongAndSurrogateChecks:

                    inputBufferCurrentOffset += 3;
                    inputBufferRemainingBytes -= 3;
                    tempRuneCount -= 2; // 3 bytes -> 1 rune

                    // Occasionally one-off ASCII characters like spaces, periods, or newlines will make their way
                    // in to the text. If this happens strip it off now before seeing if the next character
                    // consists of three code units.

                    if (DWordFourthByteIsAscii(thisDWord))
                    {
                        inputBufferCurrentOffset += 1;
                        inputBufferRemainingBytes--;
                    }

                    SuccessfullyProcessedThreeByteSequence:

                    // Optimization: A three-byte character could indicate CJK text, which makes it likely
                    // that the character following this one is also CJK. We'll try to process several
                    // three-byte sequences at a time.

                    if (IntPtr.Size >= 8 && BitConverter.IsLittleEndian && inputBufferRemainingBytes >= (sizeof(ulong) + 1))
                    {
                        ulong thisQWord = Unsafe.ReadUnaligned<ulong>(ref Unsafe.Add(ref inputBuffer, inputBufferCurrentOffset));

                        // Is this three 3-byte sequences in a row?
                        // thisQWord = [ 10yyyyyy 1110zzzz | 10xxxxxx 10yyyyyy 1110zzzz | 10xxxxxx 10yyyyyy 1110zzzz ] [ 10xxxxxx ]
                        //               ---- CHAR 3  ----   --------- CHAR 2 ---------   --------- CHAR 1 ---------     -CHAR 3-
                        if ((thisQWord & 0xC0F0C0C0F0C0C0F0UL) == 0x80E08080E08080E0UL && IsUtf8ContinuationByte(Unsafe.Add(ref inputBuffer, inputBufferCurrentOffset + sizeof(ulong))))
                        {
                            // Saw a proper bitmask for three incoming 3-byte sequences, perform the
                            // overlong and surrogate sequence checking now.

                            // Check the first character.
                            // If the first character is overlong or a surrogate, fail immediately.

                            uint comparand = (uint)thisQWord & 0x200FU;
                            if ((comparand == 0UL) || (comparand == 0x200DU))
                            {
                                goto Error;
                            }

                            // Check the second character.
                            // If this character is overlong or a surrogate, process the first character (which we
                            // know to be good because the first check passed) before reporting an error.

                            comparand = (uint)(thisQWord >> 24) & 0x200FU;
                            if ((comparand == 0U) || (comparand == 0x200DU))
                            {
                                thisDWord = (uint)thisQWord;
                                goto ProcessSingleThreeByteSequenceSkipOverlongAndSurrogateChecks;
                            }

                            // Check the third character (and that the next unread byte is a continuation byte).
                            // If this character is overlong or a surrogate, process the first character (which we
                            // know to be good because the first check passed) before reporting an error.

                            comparand = (uint)(thisQWord >> 48) & 0x200FU;
                            if ((comparand == 0U) || (comparand == 0x200DU))
                            {
                                thisDWord = (uint)thisQWord;
                                goto ProcessSingleThreeByteSequenceSkipOverlongAndSurrogateChecks;
                            }

                            inputBufferCurrentOffset += 9;
                            inputBufferRemainingBytes -= 9;
                            tempRuneCount -= 6; // 9 bytes -> 3 runes
                            goto SuccessfullyProcessedThreeByteSequence;
                        }

                        // Is this two 3-byte sequences in a row?
                        // thisQWord = [ ######## ######## | 10xxxxxx 10yyyyyy 1110zzzz | 10xxxxxx 10yyyyyy 1110zzzz ]
                        //                                   --------- CHAR 2 ---------   --------- CHAR 1 ---------
                        if ((thisQWord & 0xC0C0F0C0C0F0UL) == 0x8080E08080E0UL)
                        {
                            // Saw a proper bitmask for two incoming 3-byte sequences, perform the
                            // overlong and surrogate sequence checking now.

                            // Check the first character.
                            // If the first character is overlong or a surrogate, fail immediately.

                            uint comparand = (uint)thisQWord & 0x200FU;
                            if ((comparand == 0UL) || (comparand == 0x200DU))
                            {
                                goto Error;
                            }

                            // Check the second character.
                            // If this character is overlong or a surrogate, process the first character (which we
                            // know to be good because the first check passed) before reporting an error.

                            comparand = (uint)(thisQWord >> 24) & 0x200FU;
                            if ((comparand == 0U) || (comparand == 0x200DU))
                            {
                                thisDWord = (uint)thisQWord;
                                goto ProcessSingleThreeByteSequenceSkipOverlongAndSurrogateChecks;
                            }

                            inputBufferCurrentOffset += 6;
                            inputBufferRemainingBytes -= 6;
                            tempRuneCount -= 4; // 6 bytes -> 2 runes

                            // The next char in the sequence didn't have a 3-byte marker, so it's probably
                            // an ASCII character. Jump back to the beginning of loop processing.
                            continue;
                        }

                        thisDWord = (uint)thisQWord;
                        if (DWordBeginsWithUtf8ThreeByteMask(thisDWord))
                        {
                            // A single three-byte sequence.
                            goto ProcessThreeByteSequenceWithCheck;
                        }
                        else
                        {
                            // Not a three-byte sequence; perhaps ASCII?
                            goto AfterReadDWord;
                        }
                    }

                    if (inputBufferRemainingBytes >= sizeof(uint))
                    {
                        thisDWord = Unsafe.ReadUnaligned<uint>(ref Unsafe.Add(ref inputBuffer, inputBufferCurrentOffset));

                        // Optimization: A three-byte character could indicate CJK text, which makes it likely
                        // that the character following this one is also CJK. We'll check for a three-byte sequence
                        // marker now and jump directly to three-byte sequence processing if we see one, skipping
                        // all of the logic at the beginning of the loop.

                        if (DWordBeginsWithUtf8ThreeByteMask(thisDWord))
                        {
                            goto ProcessThreeByteSequenceWithCheck; // Found another [not yet validated] three-byte sequence; process
                        }
                        else
                        {
                            goto AfterReadDWord; // Probably ASCII punctuation or whitespace; go back to start of loop
                        }
                    }
                    else
                    {
                        goto ProcessRemainingBytesSlow; // Running out of data
                    }
                }

                // Assume the 4-byte case, but we need to validate.

                {
                    // We need to check for overlong or invalid (over U+10FFFF) four-byte sequences.
                    //
                    // Per Table 3-7, valid sequences are:
                    // [   F0   ] [ 90..BF ] [ 80..BF ] [ 80..BF ]
                    // [ F1..F3 ] [ 80..BF ] [ 80..BF ] [ 80..BF ]
                    // [   F4   ] [ 80..8F ] [ 80..BF ] [ 80..BF ]

                    if (!DWordBeginsWithUtf8FourByteMask(thisDWord)) { goto Error; }

                    // Now check for overlong / out-of-range sequences.

                    if (BitConverter.IsLittleEndian)
                    {
                        // The DWORD we read is [ 10xxxxxx 10yyyyyy 10zzzzzz 11110www ].
                        // We want to get the 'w' byte in front of the 'z' byte so that we can perform
                        // a single range comparison. We'll take advantage of the fact that the JITter
                        // can detect a ROR / ROL operation, then we'll just zero out the bytes that
                        // aren't involved in the range check.

                        uint toCheck = (ushort)thisDWord;

                        // At this point, toCheck = [ 00000000 00000000 10zzzzzz 11110www ].

                        toCheck = (toCheck << 24) | (toCheck >> 8); // ROR 8 / ROL 24

                        // At this point, toCheck = [ 11110www 00000000 00000000 10zzzzzz ].

                        if (!IsInRangeInclusive(toCheck, 0xF0000090U, 0xF400008FU)) { goto Error; }
                    }
                    else
                    {
                        if (!IsInRangeInclusive(thisDWord, 0xF0900000U, 0xF48FFFFFU)) { goto Error; }
                    }

                    // Validation complete.

                    inputBufferCurrentOffset += 4;
                    inputBufferRemainingBytes -= 4;
                    tempRuneCount -= 3; // 4 bytes -> 1 rune
                    tempSurrogatePairCount++; // 4 bytes implies UTF16 surrogate pair

                    continue; // go back to beginning of loop for processing
                }
            }

            ProcessRemainingBytesSlow:

            Debug.Assert(inputBufferRemainingBytes < 4);
            while (inputBufferRemainingBytes > 0)
            {
                uint firstByte = Unsafe.Add(ref inputBuffer, inputBufferCurrentOffset);

                if (firstByte < 0x80U)
                {
                    // 1-byte (ASCII) case
                    inputBufferCurrentOffset += 1;
                    inputBufferRemainingBytes -= 1;
                    continue;
                }
                else if (inputBufferRemainingBytes >= 2)
                {
                    uint secondByte = Unsafe.Add(ref inputBuffer, inputBufferCurrentOffset + 1);
                    if (firstByte < 0xE0U)
                    {
                        // 2-byte case
                        if (firstByte >= 0xC2U && IsUtf8ContinuationByte(secondByte))
                        {
                            inputBufferCurrentOffset += 2;
                            inputBufferRemainingBytes -= 2;
                            tempRuneCount--; // 2 bytes -> 1 rune
                            continue;
                        }
                    }
                    else if (inputBufferRemainingBytes >= 3)
                    {
                        uint thirdByte = Unsafe.Add(ref inputBuffer, inputBufferCurrentOffset + 2);
                        if (firstByte <= 0xF0U)
                        {
                            if (firstByte == 0xE0U)
                            {
                                if (!IsInRangeInclusive(secondByte, 0xA0U, 0xBFU)) { goto Error; }
                            }
                            else if (firstByte == 0xEDU)
                            {
                                if (!IsInRangeInclusive(secondByte, 0x80U, 0x9FU)) { goto Error; }
                            }
                            else
                            {
                                if (!IsUtf8ContinuationByte(secondByte)) { goto Error; }
                            }

                            if (IsUtf8ContinuationByte(thirdByte))
                            {
                                inputBufferCurrentOffset += 3;
                                inputBufferRemainingBytes -= 3;
                                tempRuneCount -= 2; // 3 bytes -> 1 rune
                                continue;
                            }
                        }
                    }
                }

                // Error - no match.

                goto Error;
            }

            // If we reached this point, we're out of data, and we saw no bad UTF8 sequence.

            runeCount = tempRuneCount;
            surrogatePairCount = tempSurrogatePairCount;
            return -1;

            // Error handling logic.

            Error:

            runeCount = tempRuneCount - inputBufferRemainingBytes; // we assumed earlier each byte corresponded to a single rune, perform fixup now to account for unread bytes
            surrogatePairCount = tempSurrogatePairCount;
            return ConvertIntPtrToInt32WithoutOverflowCheck(inputBufferCurrentOffset);
        }
    }
}
