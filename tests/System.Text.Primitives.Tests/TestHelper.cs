// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using Xunit;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Text.Primitives.Tests
{
    public static class TestHelper
    {
        public static string SpanToString(Span<byte> span, SymbolTable symbolTable = null)
        {
            if (symbolTable == null || symbolTable == SymbolTable.InvariantUtf8)
            {
                Assert.Equal(TransformationStatus.Done, Encoders.Utf16.ComputeEncodedBytesFromUtf8(span, out int needed));
                Span<byte> output = new byte[needed];
                Assert.Equal(TransformationStatus.Done, Encoders.Utf16.ConvertFromUtf8(span, output, out int consumed, out int written));
                return new string(output.NonPortableCast<byte, char>().ToArray());
            }
            else if (symbolTable == SymbolTable.InvariantUtf16)
            {
                return new string(span.NonPortableCast<byte, char>().ToArray());
            }

            throw new NotSupportedException();
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
