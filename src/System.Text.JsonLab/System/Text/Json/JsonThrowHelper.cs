// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

namespace System.Text.JsonLab
{
    internal static class JsonThrowHelper
    {
        public static void ThrowArgumentException(string message)
        {
            throw GetArgumentException(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentException GetArgumentException(string message)
        {
            return new ArgumentException(message);
        }

        public static void ThrowFormatException()
        {
            throw GetFormatException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static FormatException GetFormatException()
        {
            return new FormatException();
        }

        public static void ThrowOutOfMemoryException()
        {
            throw GetOutOfMemoryException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static OutOfMemoryException GetOutOfMemoryException()
        {
            return new OutOfMemoryException();
        }

        public static void ThrowNotImplementedException()
        {
            throw GetNotImplementedException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static NotImplementedException GetNotImplementedException()
        {
            return new NotImplementedException();
        }

        public static void ThrowJsonReaderException()
        {
            throw GetJsonReaderException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static JsonReaderException GetJsonReaderException()
        {
            return new JsonReaderException();
        }
    }
}
