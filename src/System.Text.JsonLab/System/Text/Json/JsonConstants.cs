﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using System.Buffers;

namespace System.Text.JsonLab
{
    internal static class JsonConstants
    {
        #region Formatting constants

        public static readonly StandardFormat NumberFormat = new StandardFormat('D');
        public static readonly StandardFormat DateTimeFormat = new StandardFormat('O');
        public static readonly StandardFormat GuidFormat = new StandardFormat('D');

        #endregion Formatting constants

        #region Control characters

        public const byte OpenBrace = (byte)'{';
        public const byte CloseBrace = (byte)'}';
        public const byte OpenBracket = (byte)'[';
        public const byte CloseBracket = (byte)']';
        public const byte Space = (byte)' ';
        public const byte CarriageReturn = (byte)'\r';
        public const byte LineFeed = (byte)'\n';
        public const byte Tab = (byte)'\t';
        public const byte ListSeperator = (byte)',';
        public const byte KeyValueSeperator = (byte)':';
        public const byte Quote = (byte)'"';
        public const byte ReverseSolidus = (byte)'\\';
        public const byte Solidus = (byte)'/';
        public const byte BackSpace = (byte)'\b';
        public const byte FormFeed = (byte)'\f';

        #endregion Control characters

        #region Common values

        public static ReadOnlySpan<byte> TrueValue => new byte[] { (byte)'t', (byte)'r', (byte)'u', (byte)'e' };
        public static ReadOnlySpan<byte> FalseValue => new byte[] { (byte)'f', (byte)'a', (byte)'l', (byte)'s', (byte)'e' };
        public static ReadOnlySpan<byte> NullValue => new byte[] { (byte)'n', (byte)'u', (byte)'l', (byte)'l' };

        public static ReadOnlySpan<byte> Delimiters => new byte[] { ListSeperator, CloseBrace, CloseBracket, Space, LineFeed, CarriageReturn, Tab, Solidus };

        public static ReadOnlySpan<byte> WhiteSpace => new byte[] { Space, LineFeed, CarriageReturn, Tab };

        public static ReadOnlySpan<byte> EndOfComment => new byte[] { (byte)'*', Solidus }; // TODO: Add a constant for '*'

        // Explicitly skipping ReverseSolidus since that is handled separately
        public static ReadOnlySpan<byte> EscapableChars => new byte[] { Quote, (byte)'n', (byte)'r', (byte)'t', Solidus, (byte)'u', (byte)'b', (byte)'f' };

        #endregion Common values

        public const int RemoveFlagsBitMask = 0x7FFFFFFF;
        public const int MaxDepth = (int.MaxValue - 2_000_001_000) / 2;  // 73_741_323 (to account for double space indentation), leaving 1_000 buffer for "JSONifying"
        public const int MaxTokenSize = 1_000_000_000; // 1 GB
        public const int MaxCharacterTokenSize = 1_000_000_000 / 3; // 333 million characters, i.e. 333 MB

        public const int MaximumInt64Length = 20;   // 19 + sign (i.e. -9223372036854775808)
        public const int MaximumUInt64Length = 20;  // i.e. 18446744073709551615
        public const int MaximumDoubleLength = 32;  // TODO: Should it be 22?
        public const int MaximumSingleLength = 32;  // TODO: Should it be 13?
        public const int MaximumDecimalLength = 32; // TODO: Should it be 31?
    }
}
