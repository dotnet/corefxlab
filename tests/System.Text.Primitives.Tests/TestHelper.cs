// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Text.Primitives.Tests
{
    public static class TestHelper
    {
        public static string SpanToString(Span<byte> span, TextEncoder encoder = null)
        {
            // Assume no encoder means the buffer is UTF-8
            encoder = encoder ?? TextEncoder.Utf8;
            Assert.True(encoder.TryDecode(span, out string text, out int consumed));
            return text;
        }

        // Borrowed from https://github.com/dotnet/corefx/blob/master/src/System.Memory/tests/AllocationHelper.cs

        private static readonly Mutex MemoryLock = new Mutex();
        private static readonly TimeSpan WaitTimeout = TimeSpan.FromSeconds(120);

        public static bool TryAllocNative(IntPtr size, out IntPtr memory)
        {
            memory = IntPtr.Zero;

            if (!MemoryLock.WaitOne(WaitTimeout))
                return false;

            try
            {
                memory = Marshal.AllocHGlobal(size);
            }
            catch (OutOfMemoryException)
            {
                memory = IntPtr.Zero;
                MemoryLock.ReleaseMutex();
            }

            return memory != IntPtr.Zero;
        }

        public static void ReleaseNative(ref IntPtr memory)
        {
            try
            {
                Marshal.FreeHGlobal(memory);
                memory = IntPtr.Zero;
            }
            finally
            {
                MemoryLock.ReleaseMutex();
            }
        }
    }
}
