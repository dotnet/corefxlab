// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text
{
    public struct EncodingData : IEquatable<EncodingData>
    {
        private static EncodingData s_invariantUtf16;
        private static EncodingData s_invariantUtf8;
        private byte[][] _digitsAndSymbols; // this could be flattened into a single array
        private TrieNode[] _parsingTrie;
        private Encoding _encoding;

        public enum Encoding : byte
        {
            Utf16 = 0,
            Utf8 = 1,
        }

        // The parsing trie is structured as an array, which means that there are two types of
        // "nodes" for representational purposes
        //
        // The first node type (the parent node) uses the valueOrNumChildren to represent the number of children
        // underneath it. The index is unused for this type of node, except when it's used for
        // sequential node mapping (see below). If valueOrNumChildren is zero for this type of node, the index
        // is used and represents an index into _digitsAndSymbols.
        //
        // The second node types immediately follow the first (the childe nodes). They are composed of a value
        // (valueOrNumChildren), which is walked via binary search, and an index, which points to another
        // node contained in the array.
        //
        // We use the int index here to encode max-min info for sequential leaves
        // It's very common for digits to be encoded sequentially, so we save time by mapping here
        // The index is formatted as such: 0xAABBCCDD, where AA = the min value,
        // BB = the index of the min value relative to the current node (1-indexed),
        // CC = the max value, and DD = the max value's index in the same coord-system as BB.
        public struct TrieNode
        {
            public byte valueOrNumChildren;
            public int index;
        }

        // TODO: make these private once bin file generator is used
        public EncodingData(byte[][] digitsAndSymbols, TrieNode[] parsingTrie, Encoding encoding)
        {
            _digitsAndSymbols = digitsAndSymbols;
            _encoding = encoding;
            _parsingTrie = parsingTrie;
        }

        public EncodingData(byte[][] digitsAndSymbols, Encoding encoding)
        {
            _digitsAndSymbols = digitsAndSymbols;
            _encoding = encoding;
            _parsingTrie = null;
        }

        // This binary search implementation returns an int representing either:
        //      - the index of the item searched for (if the value is positive)
        //      - the index of the location where the item should be placed to maintain a sorted list (if the value is negative)
        private int BinarySearch(int nodeIndex, int level, byte value)
        {
            int maxMinLimits = _parsingTrie[nodeIndex].index;
            if (maxMinLimits > 0 && value > maxMinLimits >> 24 && value < (maxMinLimits << 16) >> 24)
            {
                // See the comments on the struct above for more information about this format
                return nodeIndex + ((maxMinLimits << 8) >> 24) + value - (maxMinLimits >> 24);
            }

            int leftBound = nodeIndex + 1, rightBound = nodeIndex + _parsingTrie[nodeIndex].valueOrNumChildren;
            int midIndex = 0;
            while (true)
            {
                if (leftBound > rightBound)  // if the search failed
                {
                    // this loop is necessary because binary search takes the floor
                    // of the middle, which means it can give incorrect indices for insertion.
                    // we should never iterate up more than two indices.
                    while (midIndex < nodeIndex + _parsingTrie[nodeIndex].valueOrNumChildren
                        && _parsingTrie[midIndex].valueOrNumChildren < value)
                    {
                        midIndex++;
                    }
                    return -midIndex;
                }

                midIndex = (leftBound + rightBound) / 2; // find the middle value

                byte mValue = _parsingTrie[midIndex].valueOrNumChildren;

                if (mValue < value)
                    leftBound = midIndex + 1;
                else if (mValue > value)
                    rightBound = midIndex - 1;
                else
                    return midIndex;
            }
        }

        // it might be worth compacting the data into a single byte array.
        // Also, it would be great if we could freeze it.
        static EncodingData()
        {
            var utf16digitsAndSymbols = new byte[][] {
                new byte[] { 48, 0, }, // digit 0
                new byte[] { 49, 0, },
                new byte[] { 50, 0, },
                new byte[] { 51, 0, },
                new byte[] { 52, 0, },
                new byte[] { 53, 0, },
                new byte[] { 54, 0, },
                new byte[] { 55, 0, },
                new byte[] { 56, 0, },
                new byte[] { 57, 0, }, // digit 9
                new byte[] { 46, 0, }, // decimal separator
                new byte[] { 44, 0, }, // group separator
                new byte[] { 73, 0, 110, 0, 102, 0, 105, 0, 110, 0, 105, 0, 116, 0, 121, 0, }, // Infinity
                new byte[] { 45, 0, }, // minus sign 
                new byte[] { 43, 0, }, // plus sign 
                new byte[] { 78, 0, 97, 0, 78, 0, }, // NaN
                new byte[] { 69, 0, }, // E
                new byte[] { 101, 0, }, // e
            };

            s_invariantUtf16 = new EncodingData(utf16digitsAndSymbols, Encoding.Utf16);

            var utf8digitsAndSymbols = new byte[][] {
                new byte[] { 48, },
                new byte[] { 49, },
                new byte[] { 50, },
                new byte[] { 51, },
                new byte[] { 52, },
                new byte[] { 53, },
                new byte[] { 54, },
                new byte[] { 55, },
                new byte[] { 56, },
                new byte[] { 57, }, // digit 9
                new byte[] { 46, }, // decimal separator
                new byte[] { 44, }, // group separator
                new byte[] { 73, 110, 102, 105, 110, 105, 116, 121, },
                new byte[] { 45, }, // minus sign
                new byte[] { 43, }, // plus sign
                new byte[] { 78, 97, 78, }, // NaN
                new byte[] { 69, }, // E
                new byte[] { 101, }, // e
            };

            s_invariantUtf8 = new EncodingData(utf8digitsAndSymbols, Encoding.Utf8);
        }

        public static EncodingData InvariantUtf16
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return s_invariantUtf16;
            }
        }
        public static EncodingData InvariantUtf8
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return s_invariantUtf8;
            }
        }

        /// <summary>
        /// Parse the next byte in a byte array. Will return either a DigitOrSymbol value, an InvalidCharacter, or a Continue
        /// </summary>
        /// <param name="nextByte">The next byte to be parsed</param>
        /// <param name="bytesParsed">The total number of bytes parsed (will be zero until a code unit is deciphered)</param>
        /// <returns></returns>
        internal bool TryParseNextCodingUnit(ReadOnlySpan<byte> buffer, out uint value, out int consumed)
        {
            int trieIndex = 0;
            int codeUnitIndex = 0;
            consumed = 0;
            while (true) {
                if (_parsingTrie[trieIndex].valueOrNumChildren == 0)    // if numChildren == 0, we're on a leaf & we've found our value and completed the code unit
                {
                    if (VerifyCodeUnit(buffer, codeUnitIndex, _parsingTrie[trieIndex].index)) {
                        consumed = _digitsAndSymbols[_parsingTrie[trieIndex].index].Length - codeUnitIndex;
                        value = (uint)_parsingTrie[trieIndex].index;  // return the parsed value
                        return true;
                    }
                    else {
                        value = 0;
                        consumed = 0;
                        return false;
                    }
                }
                else {
                    int search = BinarySearch(trieIndex, codeUnitIndex, buffer[0]);    // we search the _parsingTrie for the nextByte

                    if (search > 0)   // if we found a node
                    {
                        trieIndex = _parsingTrie[search].index;
                        consumed++;
                        codeUnitIndex++;
                    }
                    else {
                        value = 0;
                        consumed = 0;
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Parse the next byte in a byte array. Will return either a DigitOrSymbol value, an InvalidCharacter, or a Continue
        /// </summary>
        /// <param name="nextByte">The next byte to be parsed</param>
        /// <param name="bytesParsed">The total number of bytes parsed (will be zero until a code unit is deciphered)</param>
        /// <returns></returns>
        internal bool TryParseNextCodingUnit(ref byte[] buffer, ref int bufferIndex, out uint value)
        {
            int trieIndex = 0;
            int codeUnitIndex = 0;

            while (true)
            {
                if (_parsingTrie[trieIndex].valueOrNumChildren == 0)    // if numChildren == 0, we're on a leaf & we've found our value and completed the code unit
                {
                    if (VerifyCodeUnit(ref buffer, bufferIndex, codeUnitIndex, _parsingTrie[trieIndex].index))
                    {
                        bufferIndex += _digitsAndSymbols[_parsingTrie[trieIndex].index].Length - codeUnitIndex;
                        value = (uint)_parsingTrie[trieIndex].index;  // return the parsed value
                        return true;
                    }
                    else
                    {
                        value = 0;
                        return false;
                    }
                }
                else
                {
                    int search = BinarySearch(trieIndex, codeUnitIndex, buffer[bufferIndex]);    // we search the _parsingTrie for the nextByte

                    if (search > 0)   // if we found a node
                    {
                        trieIndex = _parsingTrie[search].index;
                        bufferIndex++;
                        codeUnitIndex++;
                    }
                    else
                    {
                        value = 0;
                        return false;
                    }
                }
            }
        }
        private bool VerifyCodeUnit(ref byte[] buffer, int index, int codeUnitIndex, int digitOrSymbol)
        {
            int codeUnitLength = _digitsAndSymbols[digitOrSymbol].Length;
            if (codeUnitIndex == codeUnitLength - 1)
                return true;
            
            for (int i = 0; i < codeUnitLength - codeUnitIndex; i++)
            {
                if (buffer[i + index] != _digitsAndSymbols[digitOrSymbol][i + codeUnitIndex])
                    return false;
            }

            return true;
        }

        private bool VerifyCodeUnit(ReadOnlySpan<byte> buffer, int codeUnitIndex, int digitOrSymbol)
        {
            int codeUnitLength = _digitsAndSymbols[digitOrSymbol].Length;
            if (codeUnitIndex == codeUnitLength - 1)
                return true;

            for (int i = 0; i < codeUnitLength - codeUnitIndex; i++) {
                if (buffer[i] != _digitsAndSymbols[digitOrSymbol][i + codeUnitIndex])
                    return false;
            }

            return true;
        }

        public bool TryWriteDigit(ulong digit, Span<byte> buffer, out int bytesWritten)
        {
            Precondition.Require(digit < 10);
            return TryWriteDigitOrSymbol(digit, buffer, out bytesWritten);
        }

        public bool TryWriteSymbol(Symbol symbol, Span<byte> buffer, out int bytesWritten)
        {
            var symbolIndex = (ushort)symbol;
            return TryWriteDigitOrSymbol(symbolIndex, buffer, out bytesWritten);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryWriteDigitOrSymbol(ulong digitOrSymbolIndex, Span<byte> buffer, out int bytesWritten)
        {
            byte[] bytes = _digitsAndSymbols[digitOrSymbolIndex];
            bytesWritten = bytes.Length;
            if (bytesWritten > buffer.Length)
            {
                bytesWritten = 0;
                return false;
            }

            if (bytesWritten == 2)
            {
                buffer[0] = bytes[0];
                buffer[1] = bytes[1];
                return true;
            }

            if (bytesWritten == 1)
            {
                buffer[0] = bytes[0];
                return true;
            }

            buffer.Set(bytes);
            return true;
        }

        public enum Symbol : ushort
        {
            DecimalSeparator = 10,
            GroupSeparator = 11,
            InfinitySign = 12,
            MinusSign = 13,
            PlusSign = 14,          
            NaN = 15,
            Exponent = 16,
        }

        public bool IsInvariantUtf16
        {
            get { return _digitsAndSymbols == s_invariantUtf16._digitsAndSymbols; }
        }
        public bool IsInvariantUtf8
        {
            get { return _digitsAndSymbols == s_invariantUtf8._digitsAndSymbols; }
        }

        public bool IsUtf16
        {
            get { return _encoding == Encoding.Utf16; }
        }
        public bool IsUtf8
        {
            get { return _encoding == Encoding.Utf8; }
        }

        public static bool operator==(EncodingData left, EncodingData right)
        {
            return left._digitsAndSymbols == right._digitsAndSymbols;
        }
        public static bool operator!=(EncodingData left, EncodingData right)
        {
            return left._digitsAndSymbols == right._digitsAndSymbols;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            if (obj is EncodingData) return Equals((EncodingData)obj);
            return false;
        }

        public bool Equals(EncodingData other)
        {
            return this == other;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return _digitsAndSymbols.GetHashCode();
        }
    }
}
