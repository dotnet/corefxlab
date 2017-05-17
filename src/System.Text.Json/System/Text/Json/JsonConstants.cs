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

        public const char OpenBrace = '{';
        public const char CloseBrace = '}';
        public const char OpenBracket = '[';
        public const char CloseBracket = ']';
        public const char Space = ' ';
        public const char CarriageReturn = '\r';
        public const char LineFeed = '\n';
        public const char ListSeperator = ',';
        public const char KeyValueSeperator = ':';
        public const char Quote = '"';

        #endregion Control characters

        #region Common values

        public static readonly char[] TrueValue = { 't', 'r', 'u', 'e' };
        public static readonly char[] FalseValue = { 'f', 'a', 'l', 's', 'e' };
        public static readonly char[] NullValue = { 'n', 'u', 'l', 'l' };
        public static readonly char[] UndefinedValue = { 'u', 'n', 'd', 'e', 'f', 'i', 'n', 'e', 'd' };

        #endregion Common values
    }
}
