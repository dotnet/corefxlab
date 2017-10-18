// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System
{
    public static class RangeExtensions
    {
        #region IsEmpty Methods

        public static bool IsEmpty<T>(this Range<T> range)
        {
            return EqualityComparer<T>.Default.Equals(range.From, range.To);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(this Range<Byte> range)
        {
            return range.From == range.To;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(this Range<SByte> range)
        {
            return range.From == range.To;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(this Range<UInt16> range)
        {
            return range.From == range.To;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(this Range<Int16> range)
        {
            return range.From == range.To;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(this Range<UInt32> range)
        {
            return range.From == range.To;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(this Range<Int32> range)
        {
            return range.From == range.To;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(this Range<UInt64> range)
        {
            return range.From == range.To;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(this Range<Int64> range)
        {
            return range.From == range.To;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(this Range<Single> range)
        {
            return range.From == range.To;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(this Range<Double> range)
        {
            return range.From == range.To;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(this Range<Decimal> range)
        {
            return range.From == range.To;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(this Range<DateTime> range)
        {
            return range.From == range.To;
        }

        #endregion

        #region Length Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Byte Length(this Range<Byte> range)
        {
            return (Byte)(range.To - range.From);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SByte Length(this Range<SByte> range)
        {
            return (SByte)(range.To - range.From);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt16 Length(this Range<UInt16> range)
        {
            return (UInt16)(range.To - range.From);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int16 Length(this Range<Int16> range)
        {
            return (Int16)(range.To - range.From);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 Length(this Range<UInt32> range)
        {
            return (UInt32)(range.To - range.From);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 Length(this Range<Int32> range)
        {
            return (Int32)(range.To - range.From);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 Length(this Range<UInt64> range)
        {
            return (UInt64)(range.To - range.From);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 Length(this Range<Int64> range)
        {
            return (Int64)(range.To - range.From);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single Length(this Range<Single> range)
        {
            return (Single)(range.To - range.From);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Double Length(this Range<Double> range)
        {
            return (Double)(range.To - range.From);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Decimal Length(this Range<Decimal> range)
        {
            return (Decimal)(range.To - range.From);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TimeSpan Length(this Range<DateTime> range)
        {
            return (TimeSpan)(range.To - range.From);
        }
        #endregion

        #region Normalization Methods

        public static bool IsNormalized<T>(this Range<T> range)
        {
            return Comparer<T>.Default.Compare(range.To, range.From) >= 0;
        }

        public static Range<T> Normalize<T>(this Range<T> range)
        {
            return (Comparer<T>.Default.Compare(range.To, range.From) >= 0) ? range : new Range<T>(range.To, range.From);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNormalized(this Range<Byte> range)
        {
            return range.To >= range.From;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Range<Byte> Normalize(this Range<Byte> range)
        {
            return (range.To >= range.From) ? range : new Range<Byte>(range.To, range.From);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNormalized(this Range<SByte> range)
        {
            return range.To >= range.From;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Range<SByte> Normalize(this Range<SByte> range)
        {
            return (range.To >= range.From) ? range : new Range<SByte>(range.To, range.From);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNormalized(this Range<UInt16> range)
        {
            return range.To >= range.From;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Range<UInt16> Normalize(this Range<UInt16> range)
        {
            return (range.To >= range.From) ? range : new Range<UInt16>(range.To, range.From);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNormalized(this Range<Int16> range)
        {
            return range.To >= range.From;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Range<Int16> Normalize(this Range<Int16> range)
        {
            return (range.To >= range.From) ? range : new Range<Int16>(range.To, range.From);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNormalized(this Range<UInt32> range)
        {
            return range.To >= range.From;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Range<UInt32> Normalize(this Range<UInt32> range)
        {
            return (range.To >= range.From) ? range : new Range<UInt32>(range.To, range.From);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNormalized(this Range<Int32> range)
        {
            return range.To >= range.From;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Range<Int32> Normalize(this Range<Int32> range)
        {
            return (range.To >= range.From) ? range : new Range<Int32>(range.To, range.From);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNormalized(this Range<UInt64> range)
        {
            return range.To >= range.From;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Range<UInt64> Normalize(this Range<UInt64> range)
        {
            return (range.To >= range.From) ? range : new Range<UInt64>(range.To, range.From);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNormalized(this Range<Int64> range)
        {
            return range.To >= range.From;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Range<Int64> Normalize(this Range<Int64> range)
        {
            return (range.To >= range.From) ? range : new Range<Int64>(range.To, range.From);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNormalized(this Range<Single> range)
        {
            return range.To >= range.From;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Range<Single> Normalize(this Range<Single> range)
        {
            return (range.To >= range.From) ? range : new Range<Single>(range.To, range.From);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNormalized(this Range<Double> range)
        {
            return range.To >= range.From;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Range<Double> Normalize(this Range<Double> range)
        {
            return (range.To >= range.From) ? range : new Range<Double>(range.To, range.From);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNormalized(this Range<Decimal> range)
        {
            return range.To >= range.From;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Range<Decimal> Normalize(this Range<Decimal> range)
        {
            return (range.To >= range.From) ? range : new Range<Decimal>(range.To, range.From);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNormalized(this Range<DateTime> range)
        {
            return range.To >= range.From;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Range<DateTime> Normalize(this Range<DateTime> range)
        {
            return (range.To >= range.From) ? range : new Range<DateTime>(range.To, range.From);
        }

        #endregion

        #region Contains Methods

        public static bool Contains<T>(this Range<T> range, T value)
        {
            var comparer = Comparer<T>.Default;
            return comparer.Compare(range.From, value) <= 0 && comparer.Compare(range.To, value) > 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Range<Byte> range, Byte value)
        {
            return range.From <= value && range.To > value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Range<SByte> range, SByte value)
        {
            return range.From <= value && range.To > value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Range<UInt16> range, UInt16 value)
        {
            return range.From <= value && range.To > value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Range<Int16> range, Int16 value)
        {
            return range.From <= value && range.To > value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Range<UInt32> range, UInt32 value)
        {
            return range.From <= value && range.To > value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Range<Int32> range, Int32 value)
        {
            return range.From <= value && range.To > value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Range<UInt64> range, UInt64 value)
        {
            return range.From <= value && range.To > value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Range<Int64> range, Int64 value)
        {
            return range.From <= value && range.To > value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Range<Single> range, Single value)
        {
            return range.From <= value && range.To > value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Range<Double> range, Double value)
        {
            return range.From <= value && range.To > value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Range<Decimal> range, Decimal value)
        {
            return range.From <= value && range.To > value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Range<DateTime> range, DateTime value)
        {
            return range.From <= value && range.To > value;
        }

        #endregion

        #region Intersection Methods

        public static bool Intersects<T>(this Range<T> range, Range<T> other)
        {
            var comparer = Comparer<T>.Default;
            return (comparer.Compare(range.From, other.From) <= 0)
                ? comparer.Compare(other.From, other.To) != 0 && comparer.Compare(range.To, other.From) > 0
                : comparer.Compare(range.From, range.To) != 0 && comparer.Compare(range.From, other.To) < 0;
        }

        public static Range<T> Intersect<T>(this Range<T> range, Range<T> other)
        {
            var comparer = Comparer<T>.Default;
            if (comparer.Compare(range.From, other.To) >= 0 || comparer.Compare(range.To, other.From) <= 0)
                return new Range<T>();

            return new Range<T>(
                (comparer.Compare(range.From, other.From) > 0) ? range.From : other.From,
                (comparer.Compare(range.To, other.To) < 0) ? range.To : other.To);
        }

        public static bool Intersects(this Range<Byte> range, Range<Byte> other)
        {
            return (range.From <= other.From)
                ? other.From != other.To && range.To > other.From
                : range.From != range.To && range.From < other.To;
        }

        public static Range<Byte> Intersect(this Range<Byte> range, Range<Byte> other)
        {
            if (range.From >= other.To || range.To <= other.From)
                return new Range<Byte>();

            return new Range<Byte>(
                (range.From > other.From) ? range.From : other.From,
                (range.To < other.To) ? range.To : other.To);
        }

        public static bool Intersects(this Range<SByte> range, Range<SByte> other)
        {
            return (range.From <= other.From)
                ? other.From != other.To && range.To > other.From
                : range.From != range.To && range.From < other.To;
        }

        public static Range<SByte> Intersect(this Range<SByte> range, Range<SByte> other)
        {
            if (range.From >= other.To || range.To <= other.From)
                return new Range<SByte>();

            return new Range<SByte>(
                (range.From > other.From) ? range.From : other.From,
                (range.To < other.To) ? range.To : other.To);
        }

        public static bool Intersects(this Range<UInt16> range, Range<UInt16> other)
        {
            return (range.From <= other.From)
                ? other.From != other.To && range.To > other.From
                : range.From != range.To && range.From < other.To;
        }

        public static Range<UInt16> Intersect(this Range<UInt16> range, Range<UInt16> other)
        {
            if (range.From >= other.To || range.To <= other.From)
                return new Range<UInt16>();

            return new Range<UInt16>(
                (range.From > other.From) ? range.From : other.From,
                (range.To < other.To) ? range.To : other.To);
        }

        public static bool Intersects(this Range<Int16> range, Range<Int16> other)
        {
            return (range.From <= other.From)
                ? other.From != other.To && range.To > other.From
                : range.From != range.To && range.From < other.To;
        }

        public static Range<Int16> Intersect(this Range<Int16> range, Range<Int16> other)
        {
            if (range.From >= other.To || range.To <= other.From)
                return new Range<Int16>();

            return new Range<Int16>(
                (range.From > other.From) ? range.From : other.From,
                (range.To < other.To) ? range.To : other.To);
        }

        public static bool Intersects(this Range<UInt32> range, Range<UInt32> other)
        {
            return (range.From <= other.From)
                ? other.From != other.To && range.To > other.From
                : range.From != range.To && range.From < other.To;
        }

        public static Range<UInt32> Intersect(this Range<UInt32> range, Range<UInt32> other)
        {
            if (range.From >= other.To || range.To <= other.From)
                return new Range<UInt32>();

            return new Range<UInt32>(
                (range.From > other.From) ? range.From : other.From,
                (range.To < other.To) ? range.To : other.To);
        }

        public static bool Intersects(this Range<Int32> range, Range<Int32> other)
        {
            return (range.From <= other.From)
                ? other.From != other.To && range.To > other.From
                : range.From != range.To && range.From < other.To;
        }

        public static Range<Int32> Intersect(this Range<Int32> range, Range<Int32> other)
        {
            if (range.From >= other.To || range.To <= other.From)
                return new Range<Int32>();

            return new Range<Int32>(
                (range.From > other.From) ? range.From : other.From,
                (range.To < other.To) ? range.To : other.To);
        }

        public static bool Intersects(this Range<UInt64> range, Range<UInt64> other)
        {
            return (range.From <= other.From)
                ? other.From != other.To && range.To > other.From
                : range.From != range.To && range.From < other.To;
        }

        public static Range<UInt64> Intersect(this Range<UInt64> range, Range<UInt64> other)
        {
            if (range.From >= other.To || range.To <= other.From)
                return new Range<UInt64>();

            return new Range<UInt64>(
                (range.From > other.From) ? range.From : other.From,
                (range.To < other.To) ? range.To : other.To);
        }

        public static bool Intersects(this Range<Int64> range, Range<Int64> other)
        {
            return (range.From <= other.From)
                ? other.From != other.To && range.To > other.From
                : range.From != range.To && range.From < other.To;
        }

        public static Range<Int64> Intersect(this Range<Int64> range, Range<Int64> other)
        {
            if (range.From >= other.To || range.To <= other.From)
                return new Range<Int64>();

            return new Range<Int64>(
                (range.From > other.From) ? range.From : other.From,
                (range.To < other.To) ? range.To : other.To);
        }

        public static bool Intersects(this Range<Single> range, Range<Single> other)
        {
            return (range.From <= other.From)
                ? other.From != other.To && range.To > other.From
                : range.From != range.To && range.From < other.To;
        }

        public static Range<Single> Intersect(this Range<Single> range, Range<Single> other)
        {
            if (range.From >= other.To || range.To <= other.From)
                return new Range<Single>();

            return new Range<Single>(
                (range.From > other.From) ? range.From : other.From,
                (range.To < other.To) ? range.To : other.To);
        }

        public static bool Intersects(this Range<Double> range, Range<Double> other)
        {
            return (range.From <= other.From)
                ? other.From != other.To && range.To > other.From
                : range.From != range.To && range.From < other.To;
        }

        public static Range<Double> Intersect(this Range<Double> range, Range<Double> other)
        {
            if (range.From >= other.To || range.To <= other.From)
                return new Range<Double>();

            return new Range<Double>(
                (range.From > other.From) ? range.From : other.From,
                (range.To < other.To) ? range.To : other.To);
        }

        public static bool Intersects(this Range<Decimal> range, Range<Decimal> other)
        {
            return (range.From <= other.From)
                ? other.From != other.To && range.To > other.From
                : range.From != range.To && range.From < other.To;
        }

        public static Range<Decimal> Intersect(this Range<Decimal> range, Range<Decimal> other)
        {
            if (range.From >= other.To || range.To <= other.From)
                return new Range<Decimal>();

            return new Range<Decimal>(
                (range.From > other.From) ? range.From : other.From,
                (range.To < other.To) ? range.To : other.To);
        }

        public static bool Intersects(this Range<DateTime> range, Range<DateTime> other)
        {
            return (range.From <= other.From)
                ? other.From != other.To && range.To > other.From
                : range.From != range.To && range.From < other.To;
        }

        public static Range<DateTime> Intersect(this Range<DateTime> range, Range<DateTime> other)
        {
            if (range.From >= other.To || range.To <= other.From)
                return new Range<DateTime>();

            return new Range<DateTime>(
                (range.From > other.From) ? range.From : other.From,
                (range.To < other.To) ? range.To : other.To);
        }

        #endregion
    }
}
