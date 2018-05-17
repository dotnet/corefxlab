// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text
{
    public sealed partial class Utf8String : IConvertible
    {
        TypeCode IConvertible.GetTypeCode() => Type.GetTypeCode(typeof(Utf8String)); // simply bubble up what the runtime believes we are

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            const uint TRUE_LITTLE_ENDIAN = 0x65757274U; // "uert"
            const uint TRUE_BIG_ENDIAN = 0x74727565U; // "true"
            const uint FALS_LITTLE_ENDIAN = 0x736C6166U; // "slaf"
            const uint FALS_BIG_ENDIAN = 0x66616C73U; // "fals"

            // This mimics Convert.ToBoolean(string, IFormatProvider).
            // The data must equal "true" or "false" exactly, using an ordinal ignore case comparison.
            // We also trim whitespace *and null* characters.

            var data = Bytes;

            // First, optimistically assume the data doesn't need to be trimmed, and try parsing directly.

            // TODO: Use BinaryPrimitives.TryReadMachineEndian<T> when it's exposed via ref assemblies.

            if (data.Length == 4)
            {
                uint dataAsDWord = Unsafe.ReadUnaligned<uint>(ref MemoryMarshal.GetReference(data)) | 0x20202020U; // normalize to lowercase
                if (dataAsDWord == (BitConverter.IsLittleEndian ? TRUE_LITTLE_ENDIAN : TRUE_BIG_ENDIAN))
                {
                    return true;
                }
            }
            else if (data.Length == 5)
            {
                uint dataAsDWord = Unsafe.ReadUnaligned<uint>(ref MemoryMarshal.GetReference(data)) | 0x20202020U; // normalize to lowercase
                if (dataAsDWord == (BitConverter.IsLittleEndian ? FALS_LITTLE_ENDIAN : FALS_BIG_ENDIAN) && ((data[4] | 0x20U) == 0x65U /* "e" */))
                {
                    return false;
                }
            }

            // Fast cases didn't match. Try trimming whitespace and nulls.
            // Need to trim in a loop since whitespace and nulls could be nested.

            if (data.Length <= 4)
            {
                goto Failure; // don't bother; nothing will match after whitespace removal
            }

            while (true)
            {
                int originalDataLength = data.Length;
                data = Utf8TrimHelpers.TrimSingleScalar(Utf8TrimHelpers.TrimWhiteSpace(data, TrimType.Both), UnicodeScalar.DangerousCreateWithoutValidation(0) /* null */, TrimType.Both);
                if (data.Length == originalDataLength)
                {
                    break; // no data was trimmed
                }
            }

            // Try the checks again with the trimmed data.

            if (data.Length == 4)
            {
                uint dataAsDWord = Unsafe.ReadUnaligned<uint>(ref MemoryMarshal.GetReference(data)) | 0x20202020U; // normalize to lowercase
                if (dataAsDWord == (BitConverter.IsLittleEndian ? TRUE_LITTLE_ENDIAN : TRUE_BIG_ENDIAN))
                {
                    return true;
                }
            }
            else if (data.Length == 5)
            {
                uint dataAsDWord = Unsafe.ReadUnaligned<uint>(ref MemoryMarshal.GetReference(data)) | 0x20202020U; // normalize to lowercase
                if (dataAsDWord == (BitConverter.IsLittleEndian ? FALS_LITTLE_ENDIAN : FALS_BIG_ENDIAN) && ((data[4] | 0x20U) == 0x65U /* "e" */))
                {
                    return false;
                }
            }

        Failure:

            // No match

            // TODO: What's an appropriate exception message?
            throw new InvalidOperationException("This Utf8String does not represent a valid System.Boolean.");
        }

        byte IConvertible.ToByte(IFormatProvider provider) => throw null;

        char IConvertible.ToChar(IFormatProvider provider)
        {
            var (validity, scalar, numBytesConsumed) = UnicodeReader.PeekFirstScalar(MemoryMarshal.Cast<byte, Utf8Char>(Bytes));
            if (validity != SequenceValidity.ValidSequence || numBytesConsumed != _length || !scalar.IsBmp)
            {
                // TODO: What's an appropriate exception message?
                throw new InvalidOperationException("This Utf8String does not represent a single System.Char.");
            }

            return (char)scalar.Value;
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw null;

        decimal IConvertible.ToDecimal(IFormatProvider provider) => throw null;

        double IConvertible.ToDouble(IFormatProvider provider) => throw null;

        short IConvertible.ToInt16(IFormatProvider provider) => throw null;

        int IConvertible.ToInt32(IFormatProvider provider) => throw null;

        long IConvertible.ToInt64(IFormatProvider provider) => throw null;

        sbyte IConvertible.ToSByte(IFormatProvider provider) => throw null;

        float IConvertible.ToSingle(IFormatProvider provider) => throw null;

        // IConvertible.ToString(IFormatProvider) is implicitly implemented.

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            switch (Type.GetTypeCode(conversionType))
            {
                case TypeCode.Boolean:
                    return ((IConvertible)this).ToBoolean(provider);

                case TypeCode.Char:
                    return ((IConvertible)this).ToChar(provider);

                case TypeCode.SByte:
                    return ((IConvertible)this).ToSByte(provider);

                case TypeCode.Byte:
                    return ((IConvertible)this).ToByte(provider);

                case TypeCode.Int16:
                    return ((IConvertible)this).ToInt16(provider);

                case TypeCode.UInt16:
                    return ((IConvertible)this).ToUInt16(provider);

                case TypeCode.Int32:
                    return ((IConvertible)this).ToInt32(provider);

                case TypeCode.UInt32:
                    return ((IConvertible)this).ToUInt32(provider);

                case TypeCode.Int64:
                    return ((IConvertible)this).ToInt64(provider);

                case TypeCode.UInt64:
                    return ((IConvertible)this).ToUInt64(provider);

                case TypeCode.Single:
                    return ((IConvertible)this).ToSingle(provider);

                case TypeCode.Double:
                    return ((IConvertible)this).ToDouble(provider);

                case TypeCode.Decimal:
                    return ((IConvertible)this).ToDecimal(provider);

                case TypeCode.DateTime:
                    return ((IConvertible)this).ToDateTime(provider);

                case TypeCode.String:
                    return ((IConvertible)this).ToString(provider);

                default:
                    if (conversionType == typeof(object) || conversionType == typeof(Utf8String))
                    {
                        return this;
                    }

                    // TODO: What's an appropriate exception message?
                    throw new InvalidOperationException("Unsupported conversion type.");
            }
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider) => throw null;

        uint IConvertible.ToUInt32(IFormatProvider provider) => throw null;

        ulong IConvertible.ToUInt64(IFormatProvider provider) => throw null;
    }
}
