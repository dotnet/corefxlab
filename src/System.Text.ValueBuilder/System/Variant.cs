// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace System
{
    /// <summary>
    /// <see cref="Variant"/> is a wrapper that avoids boxing common value types.
    /// </summary>
    public readonly struct Variant
    {
        private readonly Union _union;
        public readonly VariantType Type;
        private readonly object _object;

        /// <summary>
        /// Get the value as an object if the value is stored as an object.
        /// </summary>
        /// <param name="value">The value, if an object, or null.</param>
        /// <returns>True if the value is actually an object.</returns>
        public bool TryGetValue(out object value)
        {
            bool isObject = Type == VariantType.Object;
            value = isObject ? _object : null;
            return isObject;
        }

        /// <summary>
        /// Get the value as the requested type <typeparamref name="T"/> if actually stored as that type.
        /// </summary>
        /// <param name="value">The value if stored as (T), or default.</param>
        /// <returns>True if the <see cref="Variant"/> is of the requested type.</returns>
        public unsafe bool TryGetValue<T>(out T value) where T : unmanaged
        {
            value = default;
            bool success = false;

            // Checking the type gets all of the non-relevant compares elided by the JIT
            if((typeof(T) == typeof(bool) && Type == VariantType.Boolean)
                || (typeof(T) == typeof(byte) && Type == VariantType.Byte)
                || (typeof(T) == typeof(char) && Type == VariantType.Char)
                || (typeof(T) == typeof(DateTime) && Type == VariantType.DateTime)
                || (typeof(T) == typeof(DateTimeOffset) && Type == VariantType.DateTimeOffset)
                || (typeof(T) == typeof(decimal) && Type == VariantType.Decimal)
                || (typeof(T) == typeof(double) && Type == VariantType.Double)
                || (typeof(T) == typeof(Guid) && Type == VariantType.Guid)
                || (typeof(T) == typeof(short) && Type == VariantType.Int16)
                || (typeof(T) == typeof(int) && Type == VariantType.Int32)
                || (typeof(T) == typeof(long) && Type == VariantType.Int64)
                || (typeof(T) == typeof(sbyte) && Type == VariantType.SByte)
                || (typeof(T) == typeof(float) && Type == VariantType.Single)
                || (typeof(T) == typeof(TimeSpan) && Type == VariantType.TimeSpan)
                || (typeof(T) == typeof(ushort) && Type == VariantType.UInt16)
                || (typeof(T) == typeof(uint) && Type == VariantType.UInt32)
                || (typeof(T) == typeof(ulong) && Type == VariantType.UInt64))
            {
                // The JIT is able to generate more efficient code when including the
                // code for CastTo<T>() directly.
                fixed (void* u = &_union)
                {
                    value = *(T*)u;
                }
                success = true;
            }

            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe T CastTo<T>() where T : unmanaged
        {
            fixed (void* u = &_union)
            {
                return *(T*)u;
            }
        }

        // We have explicit constructors for each of the supported types for performance
        // and to restrict Variant to "safe" types. Allowing any struct that would fit
        // into the Union would expose users to issues where bad struct state could cause
        // hard failures like buffer overruns etc.

        // Setting this = default is a bit simpler than setting _object and _union to
        // default and generates less assembly / faster construction.

        public Variant(bool value)
        {
            this = default;
            _union.Boolean = value;
            Type = VariantType.Boolean;
        }

        public Variant(byte value)
        {
            this = default;
            _union.Byte = value;
            Type = VariantType.Byte;
        }

        public Variant(sbyte value)
        {
            this = default;
            _union.SByte = value;
            Type = VariantType.SByte;
        }

        public Variant(short value)
        {
            this = default;
            _union.Int16 = value;
            Type = VariantType.Int16;
        }

        public Variant(ushort value)
        {
            this = default;
            _union.UInt16 = value;
            Type = VariantType.UInt16;
        }

        public Variant(int value)
        {
            this = default;
            _union.Int32 = value;
            Type = VariantType.Int32;
        }

        public Variant(uint value)
        {
            this = default;
            _union.UInt32 = value;
            Type = VariantType.UInt32;
        }

        public Variant(long value)
        {
            this = default;
            _union.Int64 = value;
            Type = VariantType.Int64;
        }

        public Variant(ulong value)
        {
            this = default;
            _union.UInt64 = value;
            Type = VariantType.UInt64;
        }

        public Variant(float value)
        {
            this = default;
            _union.Single = value;
            Type = VariantType.Single;
        }

        public Variant(double value)
        {
            this = default;
            _union.Double = value;
            Type = VariantType.Double;
        }

        public Variant(decimal value)
        {
            this = default;
            _union.Decimal = value;
            Type = VariantType.Decimal;
        }

        public Variant(DateTime value)
        {
            this = default;
            _union.DateTime = value;
            Type = VariantType.DateTime;
        }

        public Variant(DateTimeOffset value)
        {
            this = default;
            _union.DateTimeOffset = value;
            Type = VariantType.DateTimeOffset;
        }

        public Variant(Guid value)
        {
            this = default;
            _union.Guid = value;
            Type = VariantType.Guid;
        }

        public Variant(object value)
        {
            this = default;
            _object = value;
            Type = VariantType.Object;
        }

        // The Variant struct gets laid out as follows on x64:
        //
        // | 4 bytes | 4 bytes |               16 bytes                |      8 bytes      |
        // |---------|---------|---------------------------------------|-------------------|
        // |  Type   |  Unused |                Union                  |       Object      |
        //
        // Layout of the struct is automatic and cannot be modified via [StructLayout].
        // Alignment requirements force Variant to be a multiple of 8 bytes. We could
        // shrink from 32 to 24 bytes by either dropping the 16 byte types (DateTimeOffset,
        // Decimal, and Guid) or stashing the flags in the Union and leveraging flag objects
        // for the types that exceed 8 bytes. (DateTimeOffset might fit in 12, need to test.)
        //
        // We could theoretically do sneaky things with unused bits in the object pointer, much
        // like ATOMs in Window handles (lowest 64K values). Presumably that isn't doable
        // without runtime support though (putting "bad" values in an object pointer)?
        //
        // We could also allow storing arbitrary "unmanaged" values that would fit into 16 bytes.
        // In that case we could store the typeof(T) in the _object field. That probably is only
        // particularly useful for something like enums. I think we can avoid boxing, would need
        // to expose a static entry point for formatting on System.Enum. Something like:
        //
        //  public static string Format(Type enumType, ulong value)
        //  {
        //      RuntimeType rtType = enumType as RuntimeType;
        //      if (rtType == null)
        //          throw new ArgumentException(SR.Arg_MustBeType, nameof(enumType));
        //
        //      if (!enumType.IsEnum)
        //          throw new ArgumentException(SR.Arg_MustBeEnum, nameof(enumType));
        //
        //      return Enum.InternalFormat(rtType, ulong) ?? ulong.ToString();
        //  }
        //
        // That is the minbar- as the string values are cached it would be a positive. We can
        // obviously do even better if we expose a TryFormat that takes an input span. There
        // is a little bit more to that, but nothing serious.

        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
        private struct Union
        {
            [FieldOffset(0)] public byte Byte;
            [FieldOffset(0)] public sbyte SByte;
            [FieldOffset(0)] public char Char;
            [FieldOffset(0)] public bool Boolean;
            [FieldOffset(0)] public short Int16;
            [FieldOffset(0)] public ushort UInt16;
            [FieldOffset(0)] public int Int32;
            [FieldOffset(0)] public uint UInt32;
            [FieldOffset(0)] public long Int64;
            [FieldOffset(0)] public ulong UInt64;
            [FieldOffset(0)] public DateTime DateTime;              // 8 bytes  (ulong)
            [FieldOffset(0)] public DateTimeOffset DateTimeOffset;  // 16 bytes (DateTime & short)
            [FieldOffset(0)] public float Single;                   // 4 bytes
            [FieldOffset(0)] public double Double;                  // 8 bytes
            [FieldOffset(0)] public decimal Decimal;                // 16 bytes (4 ints)
            [FieldOffset(0)] public Guid Guid;                      // 16 bytes (int, 2 shorts, 8 bytes)
        }

        /// <summary>
        /// Get the value as an object, boxing if necessary.
        /// </summary>
        public object Box()
        {
            switch (Type)
            {
                case VariantType.Boolean:
                    return CastTo<bool>();
                case VariantType.Byte:
                    return CastTo<byte>();
                case VariantType.Char:
                    return CastTo<char>();
                case VariantType.DateTime:
                    return CastTo<DateTime>();
                case VariantType.DateTimeOffset:
                    return CastTo<DateTimeOffset>();
                case VariantType.Decimal:
                    return CastTo<decimal>();
                case VariantType.Double:
                    return CastTo<double>();
                case VariantType.Guid:
                    return CastTo<Guid>();
                case VariantType.Int16:
                    return CastTo<short>();
                case VariantType.Int32:
                    return CastTo<int>();
                case VariantType.Int64:
                    return CastTo<long>();
                case VariantType.Object:
                    return _object;
                case VariantType.SByte:
                    return CastTo<sbyte>();
                case VariantType.Single:
                    return CastTo<float>();
                case VariantType.TimeSpan:
                    return CastTo<TimeSpan>();
                case VariantType.UInt16:
                    return CastTo<ushort>();
                case VariantType.UInt32:
                    return CastTo<uint>();
                case VariantType.UInt64:
                    return CastTo<ulong>();
                default:
                    throw new InvalidOperationException();
            }
        }

        // Idea is that you can cast to whatever supported type you want if you're explicit.
        // Worst case is you get default or nonsense values.

        public static explicit operator bool(in Variant variant) => variant.CastTo<bool>();
        public static explicit operator byte(in Variant variant) => variant.CastTo<byte>();
        public static explicit operator char(in Variant variant) => variant.CastTo<char>();
        public static explicit operator DateTime(in Variant variant) => variant.CastTo<DateTime>();
        public static explicit operator DateTimeOffset(in Variant variant) => variant.CastTo<DateTimeOffset>();
        public static explicit operator decimal(in Variant variant) => variant.CastTo<decimal>();
        public static explicit operator double(in Variant variant) => variant.CastTo<double>();
        public static explicit operator Guid(in Variant variant) => variant.CastTo<Guid>();
        public static explicit operator short(in Variant variant) => variant.CastTo<short>();
        public static explicit operator int(in Variant variant) => variant.CastTo<int>();
        public static explicit operator long(in Variant variant) => variant.CastTo<long>();
        public static explicit operator sbyte(in Variant variant) => variant.CastTo<sbyte>();
        public static explicit operator float(in Variant variant) => variant.CastTo<float>();
        public static explicit operator TimeSpan(in Variant variant) => variant.CastTo<TimeSpan>();
        public static explicit operator ushort(in Variant variant) => variant.CastTo<ushort>();
        public static explicit operator uint(in Variant variant) => variant.CastTo<uint>();
        public static explicit operator ulong(in Variant variant) => variant.CastTo<ulong>();

        public static implicit operator Variant(bool value) => new Variant(value);
        public static implicit operator Variant(byte value) => new Variant(value);
        public static implicit operator Variant(char value) => new Variant(value);
        public static implicit operator Variant(DateTime value) => new Variant(value);
        public static implicit operator Variant(DateTimeOffset value) => new Variant(value);
        public static implicit operator Variant(decimal value) => new Variant(value);
        public static implicit operator Variant(double value) => new Variant(value);
        public static implicit operator Variant(Guid value) => new Variant(value);
        public static implicit operator Variant(short value) => new Variant(value);
        public static implicit operator Variant(int value) => new Variant(value);
        public static implicit operator Variant(long value) => new Variant(value);
        public static implicit operator Variant(sbyte value) => new Variant(value);
        public static implicit operator Variant(float value) => new Variant(value);
        public static implicit operator Variant(TimeSpan value) => new Variant(value);
        public static implicit operator Variant(ushort value) => new Variant(value);
        public static implicit operator Variant(uint value) => new Variant(value);
        public static implicit operator Variant(ulong value) => new Variant(value);

        // Common object types
        public static implicit operator Variant(string value) => new Variant(value);

        public static Variant Create(in Variant variant) => variant;
        public static Variant2 Create(in Variant first, in Variant second) => new Variant2(in first, in second);
        public static Variant3 Create(in Variant first, in Variant second, in Variant third) => new Variant3(in first, in second, in third);

        /// <summary>
        /// Try to format the variant into the given span.
        /// </summary>
        /// <remarks>
        /// TODO: If we can make ISpanFormattable public (which this signature matches)
        /// we could format objects if they implemented said interface.
        /// </remarks>
        public bool TryFormat(ref ValueStringBuilder destination, ReadOnlySpan<char> format = default, IFormatProvider provider = null)
        {
            // TODO: This generates a a lot of assembly instructions (575). Is there a way to make this faster/smaller?
            bool success = false;
            int charsWritten = 0;

            switch (Type)
            {
                case VariantType.Boolean:
                    success = ((bool)this).TryFormat(destination.RawChars, out charsWritten);
                    break;
                case VariantType.Byte:
                    success = ((byte)this).TryFormat(destination.RawChars, out charsWritten, format, provider);
                    break;
                case VariantType.Char:
                    success = true;
                    destination.RawChars[0] = (char)this;
                    charsWritten = 1;
                    break;
                case VariantType.DateTime:
                    success = ((DateTime)this).TryFormat(destination.RawChars, out charsWritten, format, provider);
                    break;
                case VariantType.DateTimeOffset:
                    success = ((DateTimeOffset)this).TryFormat(destination.RawChars, out charsWritten, format, provider);
                    break;
                case VariantType.Decimal:
                    success = ((decimal)this).TryFormat(destination.RawChars, out charsWritten, format, provider);
                    break;
                case VariantType.Double:
                    success = ((double)this).TryFormat(destination.RawChars, out charsWritten, format, provider);
                    break;
                case VariantType.Guid:
                    success = ((Guid)this).TryFormat(destination.RawChars, out charsWritten, format);
                    break;
                case VariantType.Int16:
                    success = ((short)this).TryFormat(destination.RawChars, out charsWritten, format, provider);
                    break;
                case VariantType.Int32:
                    success = ((int)this).TryFormat(destination.RawChars, out charsWritten, format, provider);
                    break;
                case VariantType.Int64:
                    success = ((long)this).TryFormat(destination.RawChars, out charsWritten, format, provider);
                    break;
                case VariantType.SByte:
                    success = ((sbyte)this).TryFormat(destination.RawChars, out charsWritten, format, provider);
                    break;
                case VariantType.Single:
                    success = ((float)this).TryFormat(destination.RawChars, out charsWritten, format, provider);
                    break;
                case VariantType.TimeSpan:
                    success = ((TimeSpan)this).TryFormat(destination.RawChars, out charsWritten, format, provider);
                    break;
                case VariantType.UInt16:
                    success = ((ushort)this).TryFormat(destination.RawChars, out charsWritten, format, provider);
                    break;
                case VariantType.UInt32:
                    success = ((uint)this).TryFormat(destination.RawChars, out charsWritten, format, provider);
                    break;
                case VariantType.UInt64:
                    success = ((ulong)this).TryFormat(destination.RawChars, out charsWritten, format, provider);
                    break;
                case VariantType.Object:
                    // ISpanFormattable isn't public- if accessible this should check that *first*
                    string s = null;
                    if (_object is IFormattable formattable)
                    {
                        s = formattable.ToString(new string(format), provider);
                    }
                    else if (_object != null)
                    {
                        s = _object.ToString();
                    }

                    destination.Append(s);
                    break;
            }

            if (charsWritten != 0)
                destination.Length = charsWritten;

            return success;
        }
    }
}
