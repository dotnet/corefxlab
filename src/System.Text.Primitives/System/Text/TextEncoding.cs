// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Utf8;
using System.Text.Utf16;

namespace System.Text
{
    public abstract class TextEncoder
    {
        #region EncodingName enum

        public enum EncodingName : byte
        {
            Utf16 = 0,
            Utf8 = 1,
            //Ascii = 2,
            //Utf16BE = 3,
            //ISO8859_1 = 4,
        }

        #endregion EncodingName enum

        #region Symbol enum

        public enum Symbol : ushort
        {
            D0 = 0,
            D1 = 1,
            D2 = 2,
            D3 = 3,
            D4 = 4,
            D5 = 5,
            D6 = 6,
            D7 = 7,
            D8 = 8,
            D9 = 9,
            DecimalSeparator = 10,
            GroupSeparator = 11,
            InfinitySign = 12,
            MinusSign = 13,
            PlusSign = 14,
            NaN = 15,
            Exponent = 16,
        }

        #endregion Symbol enum

        #region Static instances

        public readonly static TextEncoder InvariantUtf8 = new Utf8TextEncoding();
        public readonly static TextEncoder InvariantUtf16 = new Utf16TextEncodingLE();

        #endregion Static instances

        #region Private data

        private readonly EncodingName _encodingName;
        private readonly byte[][] _symbols;                     // this could be flattened into a single array
        private readonly ParsingTrie.Node[] _parsingTrie;       // prefix tree used for parsing

        #endregion Private data

        #region Properties

        public EncodingName Encoding => _encodingName;

        public bool IsInvariantUtf8 => this == InvariantUtf8;

        public bool IsInvariantUtf16 => this == InvariantUtf16;

        #endregion Properties

        #region Constructors

        protected TextEncoder(byte[][] symbols, EncodingName encodingName)
        {
            _encodingName = encodingName;
            _symbols = symbols;
            _parsingTrie = ParsingTrie.Create(symbols);
        }

        #endregion Constructors

        #region Decode Methods

        /// <summary>
        /// Decodes a span containing a sequence of bytes into a UTF-16 string value.
        /// </summary>
        /// <param name="encodedBytes">The data span to consume for decoding. The encoded data should be relative to the concrete instance of the class used.</param>
        /// <param name="text">An output parameter to capture the decoded string.</param>
        /// <param name="bytesConsumed">An output parameter to capture the number of bytes successfully consumed during decoding from <paramref name="encodedBytes"/></param>
        /// <returns>
        /// True if successfully able to consume the entire data span in the decoding process.
        /// False if decoding failed. <paramref name="text"/> will contain as much of the string as possible to decode with
        /// <paramref name="bytesConsumed"> set to the number of bytes used.
        /// 
        /// It is possible to continue decoding from the stop point by slicing off the processed part of <paramref name="encodedBytes"/>, fixing
        /// errors (reading more data, patching encoded bytes, etc.) and calling this method again.
        /// </returns>
        public abstract bool TryDecode(ReadOnlySpan<byte> encodedBytes, out string text, out int bytesConsumed);

        /// <summary>
        /// Decodes a span containing a sequence of bytes into UTF-8 bytes.
        /// </summary>
        /// <param name="encodedBytes">The data span to consume for decoding. The encoded data should be relative to the concrete instance of the class used.</param>
        /// <param name="utf8">A span buffer to capture the decoded UTF-8 bytes.</param>
        /// <param name="bytesConsumed">An output parameter to capture the number of bytes successfully consumed during decoding from <paramref name="encodedBytes"/></param>
        /// <param name="bytesWritten">A output parameter to capture the number of bytes written to <paramref name="utf8"/></param>
        /// <returns>
        /// True if successfully able to consume the entire data span in the decoding process.
        /// False if decoding failed. <paramref name="utf8"/> will contain as much of the data as possible to decode with
        /// <paramref name="bytesConsumed"> set to the number of bytes used.
        /// 
        /// It is possible to continue decoding from the stop point by slicing off the processed part of <paramref name="encodedBytes"/>, fixing
        /// errors (reading more data, patching encoded bytes, etc.) and calling this method again.
        /// </returns>
        public abstract bool TryDecode(ReadOnlySpan<byte> encodedBytes, Span<byte> utf8, out int bytesConsumed, out int bytesWritten);

        /// <summary>
        /// Decodes a span containing a sequence of bytes into UTF-16 characters.
        /// </summary>
        /// <param name="encodedBytes">The data span to consume for decoding. The encoded data should be relative to the concrete instance of the class used.</param>
        /// <param name="utf16">A span buffer to capture the decoded UTF-16 characters.</param>
        /// <param name="bytesConsumed">An output parameter to capture the number of bytes successfully consumed during decoding from <paramref name="encodedBytes"/></param>
        /// <param name="charactersWritten">A output parameter to capture the number of bytes written to <paramref name="utf16"/></param>
        /// <returns>
        /// True if successfully able to consume the entire data span in the decoding process.
        /// False if decoding failed. <paramref name="utf16"/> will contain as much of the data as possible to decode with
        /// <paramref name="bytesConsumed"> set to the number of bytes used.
        /// 
        /// It is possible to continue decoding from the stop point by slicing off the processed part of <paramref name="encodedBytes"/>, fixing
        /// errors (reading more data, patching encoded bytes, etc.) and calling this method again.
        /// </returns>
        public abstract bool TryDecode(ReadOnlySpan<byte> encodedBytes, Span<char> utf16, out int bytesConsumed, out int charactersWritten);

        /// <summary>
        /// Decodes a span containing a sequence of bytes into UTF-32 characters.
        /// </summary>
        /// <param name="encodedBytes">The data span to consume for decoding. The encoded data should be relative to the concrete instance of the class used.</param>
        /// <param name="utf32">A span buffer to capture the decoded UTF-32 characters.</param>
        /// <param name="bytesConsumed">An output parameter to capture the number of bytes successfully consumed during decoding from <paramref name="encodedBytes"/></param>
        /// <param name="charactersWritten">A output parameter to capture the number of bytes written to <paramref name="utf32"/></param>
        /// <returns>
        /// True if successfully able to consume the entire data span in the decoding process.
        /// False if decoding failed. <paramref name="utf32"/> will contain as much of the data as possible to decode with
        /// <paramref name="bytesConsumed"> set to the number of bytes used.
        /// 
        /// It is possible to continue decoding from the stop point by slicing off the processed part of <paramref name="encodedBytes"/>, fixing
        /// errors (reading more data, patching encoded bytes, etc.) and calling this method again.
        /// </returns>
        public abstract bool TryDecode(ReadOnlySpan<byte> encodedBytes, Span<uint> utf32, out int bytesConsumed, out int charactersWritten);

        #endregion Decode Methods

        #region Encode Methods

        /// <summary>
        /// Encodes a span containing a sequence of UTF-8 bytes. The target encoding is relative to the concrete class being executed.
        /// </summary>
        /// <param name="utf8">The data span to consume for encoding.</param>
        /// <param name="encodedBytes">A span buffer to write the encoded bytes.</param>
        /// <param name="bytesConsumed">An output parameter to capture the number of bytes successfully consumed during encoding from <paramref name="utf8"/></param>
        /// <param name="bytesWritten">A output parameter to capture the number of bytes written to <paramref name="encodedBytes"/></param>
        /// <returns>
        /// True if successfully able to consume the entire data span in the encoding process.
        /// False if encoding failed. <paramref name="encodedBytes"/> will contain as much of the data as possible to encode with
        /// <paramref name="bytesConsumed"> set to the number of bytes used.
        /// 
        /// It is possible to continue encoding from the stop point by slicing off the processed part of <paramref name="utf8"/>, fixing
        /// errors (reading more data, new output buffer, etc.) and calling this method again.
        /// </returns>
        public abstract bool TryEncode(ReadOnlySpan<byte> utf8, Span<byte> encodedBytes, out int bytesConsumed, out int bytesWritten);

        /// <summary>
        /// Encodes a span containing a sequence of UTF-16 characters. The target encoding is relative to the concrete class being executed.
        /// </summary>
        /// <param name="utf16">The data span to consume for encoding.</param>
        /// <param name="encodedBytes">A span buffer to write the encoded bytes.</param>
        /// <param name="charactersConsumed">An output parameter to capture the number of characters successfully consumed during encoding from <paramref name="utf8"/></param>
        /// <param name="bytesWritten">A output parameter to capture the number of bytes written to <paramref name="encodedBytes"/></param>
        /// <returns>
        /// True if successfully able to consume the entire data span in the encoding process.
        /// False if encoding failed. <paramref name="encodedBytes"/> will contain as much of the data as possible to encode with
        /// <paramref name="charactersConsumed"> set to the number of characters used.
        /// 
        /// It is possible to continue encoding from the stop point by slicing off the processed part of <paramref name="utf16"/>, fixing
        /// errors (reading more data, new output buffer, etc.) and calling this method again.
        /// </returns>
        public abstract bool TryEncode(ReadOnlySpan<char> utf16, Span<byte> encodedBytes, out int charactersConsumed, out int bytesWritten);

        /// <summary>
        /// Encodes a span containing a sequence of UTF-32 characters. The target encoding is relative to the concrete class being executed.
        /// </summary>
        /// <param name="utf32">The data span to consume for encoding.</param>
        /// <param name="encodedBytes">A span buffer to write the encoded bytes.</param>
        /// <param name="charactersConsumed">An output parameter to capture the number of characters successfully consumed during encoding from <paramref name="utf8"/></param>
        /// <param name="bytesWritten">A output parameter to capture the number of bytes written to <paramref name="encodedBytes"/></param>
        /// <returns>
        /// True if successfully able to consume the entire data span in the encoding process.
        /// False if encoding failed. <paramref name="encodedBytes"/> will contain as much of the data as possible to encode with
        /// <paramref name="charactersConsumed"> set to the number of characters used.
        /// 
        /// It is possible to continue encoding from the stop point by slicing off the processed part of <paramref name="utf32"/>, fixing
        /// errors (reading more data, new output buffer, etc.) and calling this method again.
        /// </returns>
        public abstract bool TryEncode(ReadOnlySpan<uint> utf32, Span<byte> encodedBytes, out int charactersConsumed, out int bytesWritten);

        /// <summary>
        /// Encodes a UTF-16 string. The target encoding is relative to the concrete class being executed.
        /// </summary>
        /// <param name="text">A string containing the characters to encode.</param>
        /// <param name="encodedBytes">A span buffer to write the encoded bytes.</param>
        /// <param name="bytesWritten">A output parameter to capture the number of bytes written to <paramref name="encodedBytes"/></param>
        /// <returns>
        /// True if successfully able to encode the entire string of characters, else false.
        ///
        /// A failure likely means the output buffer is too small.
        /// </returns>
        public abstract bool TryEncode(string text, Span<byte> encodedBytes, out int bytesWritten);

        #endregion Encode Methods

        #region Symbol Parsing / Formatting

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

        #endregion Symbol Parsing / Formatting

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

        #region Static factory methods

        public static TextEncoder CreateUtf8Encoder(byte[][] symbols)
        {
            return new Utf8TextEncoding(symbols);
        }

        public static TextEncoder CreateUtf16Encoder(byte[][] symbols)
        {
            return new Utf16TextEncodingLE(symbols);
        }

        #endregion Static factory methods
    }
}