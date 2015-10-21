// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Text
{
    public struct UnicodeCodePoint : IEquatable<UnicodeCodePoint>
    {
        // TODO: make all methods CLSCompliant
        [CLSCompliant(false)]
        public UnicodeCodePoint(uint value) : this()
        {
            // TODO: Should we check if value is supported or just make it no-op and let people use IsSupportedCodePoint
            Value = value;
        }

        [CLSCompliant(false)]
        public uint Value { get; private set; }

        // TODO: Do we need to put these attributes everywhere or is compiler gonna do the right thing?
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSupportedCodePoint(UnicodeCodePoint codePoint)
        {
            return codePoint.Value < UnicodeConstants.FirstNotSupportedCodePoint;
        }

        // TODO: Should this be here or in Utf16LittleEndianEncoder?
        #region Surrogates
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSurrogate(UnicodeCodePoint codePoint)
        {
            return codePoint.Value >= UnicodeConstants.SurrogateRangeStart && codePoint.Value <= UnicodeConstants.SurrogateRangeEnd;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLowSurrogate(UnicodeCodePoint codePoint)
        {
            return codePoint.Value >= UnicodeConstants.LowSurrogateFirstCodePoint && codePoint.Value <= UnicodeConstants.LowSurrogateLastCodePoint;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsHighSurrogate(UnicodeCodePoint codePoint)
        {
            return codePoint.Value >= UnicodeConstants.HighSurrogateFirstCodePoint && codePoint.Value <= UnicodeConstants.HighSurrogateLastCodePoint;
        }
        #endregion

        [CLSCompliant(false)]
        public static explicit operator uint(UnicodeCodePoint codePoint) { return codePoint.Value; }
        [CLSCompliant(false)]
        public static explicit operator UnicodeCodePoint(uint value) { return new UnicodeCodePoint(value); }

        public bool Equals(UnicodeCodePoint other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is UnicodeCodePoint)
            {
                return Equals((UnicodeCodePoint)obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(UnicodeCodePoint left, UnicodeCodePoint right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(UnicodeCodePoint left, UnicodeCodePoint right)
        {
            return !left.Equals(right);
        }

        // TODO: Inline it and make it faster
        public static bool IsWhitespace(UnicodeCodePoint codePoint)
        {
            return Array.BinarySearch<uint>(UnicodeConstants.SortedWhitespaceCodePoints, codePoint.Value) >= 0;
        }
    }
}
