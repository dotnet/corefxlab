// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

internal static class SR
{
    internal static string Argument_EncodeDestinationTooSmall => Strings.Argument_EncodeDestinationTooSmall;

    internal static string Argument_InvalidOidValue => Strings.Argument_InvalidOidValue;

    internal static string Cryptography_Asn_EnumeratedValueRequiresNonFlagsEnum =>
        Strings.Cryptography_Asn_EnumeratedValueRequiresNonFlagsEnum;

    internal static string Cryptography_Asn_NamedBitListRequiresFlagsEnum =>
        Strings.Cryptography_Asn_NamedBitListRequiresFlagsEnum;

    internal static string Cryptography_Asn_NamedBitListValueTooBig => Strings.Cryptography_Asn_NamedBitListValueTooBig;

    internal static string Cryptography_Asn_UniversalValueIsFixed => Strings.Cryptography_Asn_UniversalValueIsFixed;

    internal static string Cryptography_Asn_UnusedBitCountRange => Strings.Cryptography_Asn_UnusedBitCountRange;

    internal static string Cryptography_AsnWriter_EncodeUnbalancedStack =>
        Strings.Cryptography_AsnWriter_EncodeUnbalancedStack;

    internal static string Cryptography_AsnWriter_PopWrongTag => Strings.Cryptography_AsnWriter_PopWrongTag;

    internal static string Cryptography_Der_Invalid_Encoding => Strings.Cryptography_Der_Invalid_Encoding;

    internal static string Cryptography_WriteEncodedValue_OneValueAtATime =>
        Strings.Cryptography_WriteEncodedValue_OneValueAtATime;

    internal static string Format(string resourceFormat, params object[] args)
    {
        return string.Format(resourceFormat, args);
    }
}
