// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

namespace System.Text.JsonLab
{
    internal static class JsonThrowHelper
    {
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
