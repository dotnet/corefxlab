// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace System.Text
{
    public struct EncodingData : IEquatable<EncodingData>
    {
        private static EncodingData s_invariantUtf16;
        private static EncodingData s_invariantUtf8;

        private byte[][] _symbols; // this could be flattened into a single array
        private ParsingTrieNode[] _parsingTrie; // prefix tree used for parsing
        private TextEncoder _encoder;

        public EncodingData(byte[][] symbols, TextEncoder encoder)
        {
            _symbols = symbols;
            _encoder = encoder;
            _parsingTrie = CreateParsingTrie(symbols);
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

        public bool TryParseSymbol(ReadOnlySpan<byte> buffer, out Symbol symbol, out int bytesConsumed)
        {
            int trieIndex = 0;
            int bufferIndex = 0;
            bytesConsumed = 0;
            while (true)
            {
                var node = _parsingTrie[trieIndex];
                if (node.ValueOrNumChildren == 0)    // if numChildren == 0, we're on a leaf & we've found our value
                {
                    symbol = (Symbol)node.IndexOrSymbol;
                    if (VerifySuffix(buffer, bufferIndex, symbol))
                    {
                        bytesConsumed = _symbols[node.IndexOrSymbol].Length - bufferIndex;
                        return true;
                    }
                    else
                    {
                        symbol = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                }
                else
                {
                    int search = BinarySearch(trieIndex, bufferIndex, buffer[0]);    // we search the _parsingTrie for the nextByte

                    if (search > 0)   // if we found a node
                    {
                        trieIndex = _parsingTrie[search].IndexOrSymbol;
                        bytesConsumed++;
                        bufferIndex++;
                    }
                    else
                    {
                        symbol = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                }
            }
        }

        public bool TryEncode(Symbol symbol, Span<byte> buffer, out int bytesWritten)
        {
            byte[] bytes = _symbols[(int)symbol];
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

            bytes.CopyTo(buffer);
            return true;
        }

        public bool IsInvariantUtf16
        {
            get { return _symbols == s_invariantUtf16._symbols; }
        }
        public bool IsInvariantUtf8
        {
            get { return _symbols == s_invariantUtf8._symbols; }
        }

        public TextEncoder TextEncoder => _encoder;

        public enum Symbol : ushort
        {
            D0                  = 0,
            D1                  = 1,
            D2                  = 2, 
            D3                  = 3,
            D4                  = 4,
            D5                  = 5, 
            D6                  = 6, 
            D7                  = 7,
            D8                  = 8,
            D9                  = 9,
            DecimalSeparator    = 10,
            GroupSeparator      = 11,
            InfinitySign        = 12,
            MinusSign           = 13,
            PlusSign            = 14,
            NaN                 = 15,
            Exponent            = 16,
        }

        public static bool operator==(EncodingData left, EncodingData right)
        {
            return left._symbols == right._symbols;
        }
        public static bool operator!=(EncodingData left, EncodingData right)
        {
            return left._symbols == right._symbols;
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
            return _symbols.GetHashCode();
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

            s_invariantUtf16 = new EncodingData(utf16digitsAndSymbols, TextEncoder.Utf16);

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

            s_invariantUtf8 = new EncodingData(utf8digitsAndSymbols, TextEncoder.Utf8);
        }

        // TODO: this should be removed
        /// <summary>
        /// Parse the next byte in a byte array. Will return either a DigitOrSymbol value, an InvalidCharacter, or a Continue
        /// </summary>
        /// <param name="nextByte">The next byte to be parsed</param>
        /// <param name="bytesParsed">The total number of bytes parsed (will be zero until a code unit is deciphered)</param>
        /// <returns></returns>
        internal bool TryParseSymbol(ReadOnlySpan<byte> buffer, out uint symbol, out int consumed)
        {
            int trieIndex = 0;
            int codeUnitIndex = 0;
            consumed = 0;
            while (true)
            {
                if (_parsingTrie[trieIndex].ValueOrNumChildren == 0)    // if numChildren == 0, we're on a leaf & we've found our value and completed the code unit
                {
                    symbol = (uint)_parsingTrie[trieIndex].IndexOrSymbol;  // return the parsed value
                    if (VerifySuffix(buffer, codeUnitIndex, (Symbol)symbol))
                    {                        
                        consumed = _symbols[(int)symbol].Length;
                        return true;
                    }
                    else
                    {
                        symbol = 0;
                        consumed = 0;
                        return false;
                    }
                }
                else
                {
                    int search = BinarySearch(trieIndex, codeUnitIndex, buffer[codeUnitIndex]);    // we search the _parsingTrie for the nextByte

                    if (search > 0)   // if we found a node
                    {
                        trieIndex = _parsingTrie[search].IndexOrSymbol;
                        consumed++;
                        codeUnitIndex++;
                    }
                    else
                    {
                        symbol = 0;
                        consumed = 0;
                        return false;
                    }
                }
            }
        }

        // This binary search implementation returns an int representing either:
        //      - the index of the item searched for (if the value is positive)
        //      - the index of the location where the item should be placed to maintain a sorted list (if the value is negative)
        private int BinarySearch(int nodeIndex, int level, byte value)
        {
            int maxMinLimits = _parsingTrie[nodeIndex].IndexOrSymbol;
            if (maxMinLimits > 0 && value > maxMinLimits >> 24 && value < (maxMinLimits << 16) >> 24)
            {
                // See the comments on the struct above for more information about this format
                return nodeIndex + ((maxMinLimits << 8) >> 24) + value - (maxMinLimits >> 24);
            }

            int leftBound = nodeIndex + 1, rightBound = nodeIndex + _parsingTrie[nodeIndex].ValueOrNumChildren;
            int midIndex = 0;
            while (true)
            {
                if (leftBound > rightBound)  // if the search failed
                {
                    // this loop is necessary because binary search takes the floor
                    // of the middle, which means it can give incorrect indices for insertion.
                    // we should never iterate up more than two indices.
                    while (midIndex < nodeIndex + _parsingTrie[nodeIndex].ValueOrNumChildren
                        && _parsingTrie[midIndex].ValueOrNumChildren < value)
                    {
                        midIndex++;
                    }
                    return -midIndex;
                }

                midIndex = (leftBound + rightBound) / 2; // find the middle value

                byte mValue = _parsingTrie[midIndex].ValueOrNumChildren;

                if (mValue < value)
                    leftBound = midIndex + 1;
                else if (mValue > value)
                    rightBound = midIndex - 1;
                else
                    return midIndex;
            }
        }

        private bool VerifySuffix(ReadOnlySpan<byte> buffer, int codeUnitIndex, Symbol symbol)
        {
            int codeUnitLength = _symbols[(int)symbol].Length;
            if (codeUnitIndex == codeUnitLength - 1)
                return true;

            for (int i = 0; i < codeUnitLength - codeUnitIndex; i++)
            {
                if (buffer[i + codeUnitIndex] != _symbols[(int)symbol][i + codeUnitIndex])
                    return false;
            }

            return true;
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
        struct ParsingTrieNode
        {
            public byte ValueOrNumChildren;
            public int IndexOrSymbol;
        }
        
        private static int CompareSuffix(Suffix x, Suffix y)
        {
            int index = 0;
            if (x.Bytes.Length == 0 || y.Bytes.Length == 0)
            {
                throw new ArgumentException("Symbol cannot be zero bytes long");
            }
            while (true)
            {
                if (index == x.Bytes.Length)
                {
                    if (index == y.Bytes.Length)
                    {
                        throw new ArgumentException(String.Format("Symbols cannot be identical"));
                    }
                    throw new ArgumentException("Symbols are ambiguous");
                }
                if (index == y.Bytes.Length)
                {
                    throw new ArgumentException("Symbols are ambiguous");
                }
                int compareResult = x.Bytes[index].CompareTo(y.Bytes[index]);
                if (compareResult != 0)
                {
                    return compareResult;
                }
                index++;
            }
        }

        private struct Suffix
        {
            public int SymbolIndex;
            public ReadOnlySpan<byte> Bytes;

            public Suffix(int symbolIndex, ReadOnlySpan<byte> bytes)
            {
                SymbolIndex = symbolIndex;
                Bytes = bytes;
            }
        }

        private struct SuffixClump
        {
            public byte BeginningByte;
            public List<Suffix> Suffixes;

            public SuffixClump(byte beginningByte)
            {
                BeginningByte = beginningByte;
                Suffixes = new List<Suffix>();
            }
        }

        // The return value here is the index in parsingTrieList at which the parent node was placed.
        private static int CreateParsingTrieNodeAndChildren(ref List<ParsingTrieNode> parsingTrieList, List<Suffix> sortedSuffixes)
        {
            // If there is only one suffix, create a leaf node
            if (sortedSuffixes.Count == 1)
            {
                ParsingTrieNode leafNode = new ParsingTrieNode();
                leafNode.ValueOrNumChildren = 0;
                leafNode.IndexOrSymbol = sortedSuffixes[0].SymbolIndex;
                int leafNodeIndex = parsingTrieList.Count;
                parsingTrieList.Add(leafNode);
                return leafNodeIndex;
            }

            // Group suffixes into clumps based on first byte
            List<SuffixClump> clumps = new List<SuffixClump>();
            byte beginningByte = sortedSuffixes[0].Bytes[0];
            SuffixClump currentClump = new SuffixClump(beginningByte);
            clumps.Add(currentClump);
            foreach (Suffix suffix in sortedSuffixes)
            {
                if (suffix.Bytes[0] == beginningByte)
                {
                    currentClump.Suffixes.Add(new Suffix(suffix.SymbolIndex, suffix.Bytes.Slice(1)));
                }
                else
                {
                    beginningByte = suffix.Bytes[0];
                    currentClump = new SuffixClump(beginningByte);
                    clumps.Add(currentClump);
                    currentClump.Suffixes.Add(new Suffix(suffix.SymbolIndex, suffix.Bytes.Slice(1)));
                }
            }

            // Now that we know how many children there are, create parent node and place in list
            ParsingTrieNode parentNode = new ParsingTrieNode();
            parentNode.ValueOrNumChildren = (byte)clumps.Count;
            parentNode.IndexOrSymbol = 0;
            int parentNodeIndex = parsingTrieList.Count;
            parsingTrieList.Add(parentNode);

            // Reserve space in list for child nodes. In this algorithm, all parent nodes are created first, leaving gaps for the child nodes
            // to be filled in once it is known where they point to.
            int childNodeStartIndex = parsingTrieList.Count;
            for (int i = 0; i < clumps.Count; i++)
            {
                parsingTrieList.Add(default(ParsingTrieNode));
            }

            // Process child nodes
            List<ParsingTrieNode> childNodes = new List<ParsingTrieNode>();
            foreach (SuffixClump clump in clumps)
            {
                ParsingTrieNode childNode = new ParsingTrieNode();
                childNode.ValueOrNumChildren = clump.BeginningByte;
                childNode.IndexOrSymbol = CreateParsingTrieNodeAndChildren(ref parsingTrieList, clump.Suffixes);
                childNodes.Add(childNode);
            }

            // Place child nodes in spots allocated for them
            int childNodeIndex = childNodeStartIndex;
            foreach (ParsingTrieNode childNode in childNodes)
            {
                parsingTrieList[childNodeIndex] = childNode;
                childNodeIndex++;
            }

            return parentNodeIndex;
        }

        private static ParsingTrieNode[] CreateParsingTrie(byte[][] symbols)
        {
            List<Suffix> symbolList = new List<Suffix>();
            for (int i = 0; i < symbols.Length; i++)
            {
                if (symbols[i] != null)
                {
                    symbolList.Add(new Suffix(i, symbols[i]));
                }
            }

            // Sort the symbol list. This is important for allowing binary search of the child nodes, as well as
            // counting the number of children a node has.
            symbolList.Sort(CompareSuffix);

            List<ParsingTrieNode> parsingTrieList = new List<ParsingTrieNode>();
            CreateParsingTrieNodeAndChildren(ref parsingTrieList, symbolList);

            return parsingTrieList.ToArray();
        }
    }
}
