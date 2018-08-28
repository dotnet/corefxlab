// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
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

        public static void ThrowArgumentExceptionInvalidUtf8String()
        {
            throw GetArgumentExceptionInvalidUtf8String();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentException GetArgumentExceptionInvalidUtf8String()
        {
            return new ArgumentException("Invalid or incomplete UTF-8 string");
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
            return new NotImplementedException("Reading JSON containing comments is not yet supported.");
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

        public static void ThrowInvalidCastException()
        {
            throw GetInvalidCastException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static InvalidCastException GetInvalidCastException()
        {
            return new InvalidCastException();
        }

        public static void ThrowKeyNotFoundException()
        {
            throw GetKeyNotFoundException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static KeyNotFoundException GetKeyNotFoundException()
        {
            return new KeyNotFoundException();
        }

        public static void ThrowInvalidOperationException(string message)
        {
            throw GetInvalidOperationException(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static InvalidOperationException GetInvalidOperationException(string message)
        {
            return new InvalidOperationException(message);
        }

        public static void ThrowInvalidOperationException()
        {
            throw GetInvalidOperationException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static InvalidOperationException GetInvalidOperationException()
        {
            return new InvalidOperationException();
        }

        public static void ThrowIndexOutOfRangeException()
        {
            throw GetIndexOutOfRangeException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static IndexOutOfRangeException GetIndexOutOfRangeException()
        {
            return new IndexOutOfRangeException();
        }
    }
}
