// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text.Json
{
    static class JsonConstants
    {
        #region Formatting constants

        public static readonly TextFormat NumberFormat = new TextFormat('D');
        public static readonly TextFormat DateTimeFormat = new TextFormat('O');
        public static readonly TextFormat GuidFormat = new TextFormat('D');

        #endregion Formatting constants

        #region Control characters

        public const byte OpenBrace = (byte)'{';
        public const byte CloseBrace = (byte)'}';
        public const byte OpenBracket = (byte)'[';
        public const byte CloseBracket = (byte)']';
        public const byte Space = (byte)' ';
        public const byte CarriageReturn = (byte)'\r';
        public const byte LineFeed = (byte)'\n';
        public const byte ListSeperator = (byte)',';
        public const byte KeyValueSeperator = (byte)':';
        public const byte Quote = (byte)'"';

        #endregion Control characters

        #region Common values

        public static readonly char[] TrueValue = { 't', 'r', 'u', 'e' };
        public static readonly char[] FalseValue = { 'f', 'a', 'l', 's', 'e' };
        public static readonly char[] NullValue = { 'n', 'u', 'l', 'l' };

        #endregion Common values
    }
}
