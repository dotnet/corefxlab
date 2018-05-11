// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Utf8.Resources;

namespace System.Text
{
    public sealed partial class Utf8String : IConvertible
    {
        TypeCode IConvertible.GetTypeCode() => Type.GetTypeCode(typeof(Utf8String)); // simply bubble up what the runtime believes we are

        bool IConvertible.ToBoolean(IFormatProvider provider) => throw null;

        byte IConvertible.ToByte(IFormatProvider provider) => throw null;

        char IConvertible.ToChar(IFormatProvider provider) => throw null;

        DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw null;

        decimal IConvertible.ToDecimal(IFormatProvider provider) => throw null;

        double IConvertible.ToDouble(IFormatProvider provider) => throw null;

        short IConvertible.ToInt16(IFormatProvider provider) => throw null;

        int IConvertible.ToInt32(IFormatProvider provider) => throw null;

        long IConvertible.ToInt64(IFormatProvider provider) => throw null;

        sbyte IConvertible.ToSByte(IFormatProvider provider) => throw null;

        float IConvertible.ToSingle(IFormatProvider provider) => throw null;

        // IConvertible.ToString(IFormatProvider) is implicitly implemented.

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => throw null;

        ushort IConvertible.ToUInt16(IFormatProvider provider) => throw null;

        uint IConvertible.ToUInt32(IFormatProvider provider) => throw null;

        ulong IConvertible.ToUInt64(IFormatProvider provider) => throw null;
    }
}
