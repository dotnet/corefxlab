using System.Collections.Generic;
using System.Globalization;

namespace System
{
    public struct Range<T> : IEquatable<Range<T>>, IFormattable
    {
        public readonly T From;
        public readonly T To;

        public Range(T from, T to)
        {
            From = from;
            To = to;
        }

        public void Deconstruct(out T from, out T to)
        {
            from = From;
            to = To;
        }

        public override bool Equals(object obj) =>
            obj is Range<T> && Equals((Range<T>)obj);

        public bool Equals(Range<T> other)
        {
            var comparer = EqualityComparer<T>.Default;
            return comparer.Equals(From, other.From) && comparer.Equals(To, other.To);
        }

        public override int GetHashCode()
        {
            var comparer = EqualityComparer<T>.Default;
            var fh = comparer.GetHashCode(From);
            var th = comparer.GetHashCode(To);

            // From System.Numerics.Hashing.HashHelpers
            uint rol5 = ((uint)fh << 5) | ((uint)fh >> 27);
            return ((int)rol5 + fh) ^ th;
        }

        public override string ToString()
        {
            var separator = NumberFormatInfo.GetInstance(CultureInfo.CurrentCulture).NumberGroupSeparator;
            return ToStringConcat(From.ToString(), To.ToString(), separator);
        }

        public string ToString(string format) =>
            ToString(format, CultureInfo.CurrentCulture);

        public string ToString(string format, IFormatProvider formatProvider)
        {
            var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            return ToStringConcat(ToStringValue(From, format, formatProvider), ToStringValue(To, format, formatProvider), separator);
        }

        private static string ToStringConcat(string from, string to, string separator)
        {
            return "[" + from + separator + to + ")";
        }

        private static string ToStringValue(T value, string format, IFormatProvider formatProvider)
        {
            if (value is IFormattable formattable)
                return formattable.ToString(format, formatProvider);
            else
                return value.ToString();
        }
    }
}
