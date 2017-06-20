// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text
{
    public abstract partial class SymbolTable
    {
        #region Private data

        private readonly byte[][] _symbols;                     // this could be flattened into a single array
        private readonly ParsingTrie.Node[] _parsingTrie;       // prefix tree used for parsing

        #endregion Private data

        #region Constructors

        protected SymbolTable(byte[][] symbols)
        {
            _symbols = symbols;
            _parsingTrie = ParsingTrie.Create(symbols);
        }

        #endregion Constructors

        #region Static instances

        public readonly static SymbolTable InvariantUtf8 = new Utf8InvariantSymbolTable();

        public readonly static SymbolTable InvariantUtf16 = new Utf16InvariantSymbolTable();

        #endregion Static instances

        public bool TryEncode(Symbol symbol, Span<byte> destination, out int bytesWritten)
        {
            byte[] bytes = _symbols[(int)symbol];
            bytesWritten = bytes.Length;
            if (bytesWritten > destination.Length)
            {
                bytesWritten = 0;
                return false;
            }

            if (bytesWritten == 2)
            {
                destination[0] = bytes[0];
                destination[1] = bytes[1];
                return true;
            }

            if (bytesWritten == 1)
            {
                destination[0] = bytes[0];
                return true;
            }

            new Span<byte>(bytes).CopyTo(destination);
            return true;
        }

        public abstract bool TryEncode(byte utf8, Span<byte> destination, out int bytesWritten);

        public abstract bool TryEncode(ReadOnlySpan<byte> utf8, Span<byte> destination, out int bytesConsumed, out int bytesWritten);

        public bool TryParse(ReadOnlySpan<byte> source, out Symbol symbol, out int bytesConsumed)
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
                    if (VerifySuffix(source, bufferIndex, symbol))
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
                    int search = BinarySearch(trieIndex, bufferIndex, source[0]);    // we search the _parsingTrie for the nextByte

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

        public abstract bool TryParse(ReadOnlySpan<byte> source, out byte utf8, out int bytesConsumed);

        public abstract bool TryParse(ReadOnlySpan<byte> source, Span<byte> utf8, out int bytesConsumed, out int bytesWritten);

        #region Private helpers

        // This binary search implementation returns an int representing either:
        //      - the index of the item searched for (if the value is positive)
        //      - the index of the location where the item should be placed to maintain a sorted list (if the value is negative)
        private int BinarySearch(int nodeIndex, int level, byte value)
        {
            int maxMinLimits = _parsingTrie[nodeIndex].IndexOrSymbol;
            if (maxMinLimits != 0 && value > (uint)maxMinLimits >> 24 && value < (uint)(maxMinLimits << 16) >> 24)
            {
                // See the comments on the struct above for more information about this format
                return (int)(nodeIndex + ((uint)(maxMinLimits << 8) >> 24) + value - ((uint)maxMinLimits >> 24));
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

        #endregion Private helpers
    }
}
