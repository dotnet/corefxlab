// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text
{
    /// <summary>
    /// A named, contiguous range of Unicode code points.
    /// </summary>
    public readonly struct UnicodeBlock : IEquatable<UnicodeBlock>
    {
        // Normally it would take 21 bits to represent the first code point (U+0000..U+10FFFF)
        // and 17 bits to represent the length (0 - 64K), but we can take advantage of the fact
        // that both values must be divisible by 16 to create a more compact representation.
        // Therefore, value's low 13 bits are (length >> 4), and the next 17 bits are (first code point >> 4).
        // We still have 2 bits left over for future use.
        private readonly uint _value;

        /// <summary>
        /// Constructs a <see cref="UnicodeBlock"/>.
        /// </summary>
        /// <param name="firstCodePoint">
        /// The first code point in this block. The value must be within the range U+0000..U+10FFFF, inclusive,
        /// and must be evenly divisible by 16.
        /// </param>
        /// <param name="length">
        /// The number of code points in this block. The length must be positive, must be evenly divisible
        /// by 16, and cannot cause the block to span multiple Unicode planes.
        /// </param>
        public UnicodeBlock(int firstCodePoint, int length)
        {
            // Check that firstCodePoint is in range [ U+0000..U+10FFFF ] and is evenly divisible by 16.
            // Rotating instead of performing a standard shift will move any low (MOD 16 != 0)
            // bits up high, allowing simplified range checks.
            uint firstCodePointDiv16 = (uint)firstCodePoint;
            firstCodePointDiv16 = (firstCodePointDiv16 >> 4) | (firstCodePointDiv16 << 28);
            if ((uint)firstCodePointDiv16 > 0x10FFFU)
            {
                // TODO: Real exception message.
                throw new ArgumentOutOfRangeException(paramName: nameof(firstCodePoint));
            }

            // Check that length is a positive multiple of 16 and that it doesn't cause this
            // block to span multiple planes. This check looks like:
            // if (!IsWithinRange(lengthDiv16, 1, 0x1000 - (firstCodePointDiv16 & 0xFFF))) { FAIL; }
            // => if (lengthDiv16 - 1 > 0x1000 - (firstCodePointDiv16 & 0xFFF) - 1) { FAIL; }
            // => if (lengthDiv16 - 1 > 0xFFF - (firstCodePointDiv16 & 0xFFF)) { FAIL; }
            // => if (lengthDiv16 - 1 > (0xFFF - firstCodePointDiv16) & 0xFFF)) { FAIL; }
            // => if (lengthDiv16 - 1 > (0x1000 - firstCodePointDiv16 - 1) & 0xFFF)) { FAIL; }
            // => if (lengthDiv16 - 1 > (-firstCodePointDiv16 - 1) & 0xFFF)) { FAIL; }
            // => if (lengthDiv16 - 1 > ~firstCodePointDiv16 & 0xFFF)) { FAIL; }
            uint lengthDiv16 = (uint)length;
            lengthDiv16 = (lengthDiv16 >> 4) | (lengthDiv16 << 28);
            if ((uint)(lengthDiv16 - 1) > (uint)(~firstCodePointDiv16 & 0xFFFU))
            {
                // TODO: Real exception message.
                throw new ArgumentOutOfRangeException(paramName: nameof(length));
            }

            _value = (firstCodePointDiv16 << 13) | lengthDiv16;
        }

        public static bool operator ==(UnicodeBlock a, UnicodeBlock b) => (a._value == b._value);

        public static bool operator !=(UnicodeBlock a, UnicodeBlock b) => (a._value != b._value);

        public int FirstCodePoint => (int)((_value >> 13) << 4);

        public int Length => (int)((_value & 0x1FFF) << 4);

        public int Plane => (int)(_value >> 25);

        public bool Contains(char value) => Contains((int)value);

        public bool Contains(int codePoint) => ((uint)(codePoint - FirstCodePoint) < (uint)Length);

        public bool Contains(UnicodeScalar value) => Contains((int)value.Value);

        public override bool Equals(object obj) => (obj is UnicodeBlock block) && Equals(block);

        public bool Equals(UnicodeBlock other) => (this._value == other._value);

        public override int GetHashCode() => _value.GetHashCode();

        /// <summary>
        /// Gets the defined name of this block, or <see langword="null"/> if no name is defined.
        /// </summary>
        /// <remarks>
        /// The name returned by this method is the name of the block to which <see cref="FirstCodePoint"/>
        /// belongs per the Unicode Standard version given by <see cref="UnicodeData.StandardVersion"/>.
        /// </remarks>
        public string GetName() => throw null;

        public static bool TryGetBlockForCharacter(char value, out UnicodeBlock block) => TryGetBlockForCodePoint((int)value, out block);

        public static bool TryGetBlockForCodePoint(int codePoint, out UnicodeBlock block) => throw null;

        public static bool TryGetBlockForScalar(UnicodeScalar value, out UnicodeBlock block) => TryGetBlockForCodePoint((int)value.Value, out block);
    }
}
