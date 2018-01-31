// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

namespace System.Text.Primitives.Tests.MemoryProtection
{
    /// <summary>
    /// Contains factory methods to create <see cref="INativeMemory"/> instances.
    /// </summary>
    public static partial class NativeMemory
    {
        private const int THREADSTATIC_BUFFER_SIZE = 64 * 1024;

        [ThreadStatic]
        private static INativeMemory _readonlyThreadStaticBuffer;

        [ThreadStatic]
        private static INativeMemory _writeableThreadStaticBuffer;

        /// <summary>
        /// Allocates a new <see cref="NativeMemory"/> region which is immediately preceded by
        /// or immediately followed by a poison (MEM_NOACCESS) page. If <paramref name="placement"/>
        /// is <see cref="PoisonPagePlacement.BeforeSpan"/>, then attempting to read the memory
        /// immediately before the returned <see cref="NativeMemory"/> will result in an AV.
        /// If <paramref name="placement"/> is <see cref="PoisonPagePlacement.AfterSpan"/>, then
        /// attempting to read the memory immediately after the returned <see cref="NativeMemory"/>
        /// will result in AV.
        /// </summary>
        /// <remarks>
        /// The newly-allocated memory will be populated with random data.
        /// </remarks>
        public static INativeMemory Allocate(int cb, PoisonPagePlacement placement)
        {
            var retVal = AllocateWithoutDataPopulation(cb, placement);
            new Random().NextBytes(retVal.Span); // doesn't need to be cryptographically strong
            return retVal;
        }

        /// <summary>
        /// Similar to <see cref="Allocate(int, PoisonPagePlacement)"/>, but populates the allocated
        /// native memory block from existing data rather than using random data.
        /// </summary>
        public static INativeMemory AllocateFromExistingData(ReadOnlySpan<byte> data, PoisonPagePlacement placement)
        {
            var retVal = AllocateWithoutDataPopulation(data.Length, placement);
            data.CopyTo(retVal.Span);
            return retVal;
        }

        private static INativeMemory AllocateWithoutDataPopulation(int cb, PoisonPagePlacement placement)
        {
            //
            // PRECONDITION CHECKS
            //

            if (cb < 0)
            {
                throw new ArgumentOutOfRangeException(
                    message: "Number of bytes to allocate must be non-negative.",
                    paramName: nameof(cb));
            }

            if (placement != PoisonPagePlacement.BeforeSpan && placement != PoisonPagePlacement.AfterSpan)
            {
                throw new ArgumentOutOfRangeException(
                    message: "Invalid enum value.",
                    paramName: nameof(placement));
            }

            //
            // PROCESSING
            //

            return (Environment.OSVersion.Platform == PlatformID.Win32NT)
                ? AllocateWithoutDataPopulationWindows(cb, placement) /* Windows-specific code */
                : AllocateWithoutDataPopulationDefault(cb) /* non-Windows-specific code */;
        }

        private static INativeMemory EnsureThreadStaticBuffer(ref INativeMemory threadStaticBuffer)
        {
            // No interlocked needed since [ThreadStatic], multiple threads can't access

            var retVal = threadStaticBuffer;
            if (retVal == null)
            {
                retVal = Allocate(THREADSTATIC_BUFFER_SIZE, PoisonPagePlacement.AfterSpan);
                threadStaticBuffer = retVal;
            }
            return retVal;
        }

        /// <summary>
        /// Copies <paramref name="data"/> to a read-only [ThreadStatic] buffer immediately followed by a poison
        /// page, then returns a span to the data in the buffer. If this method is called multiple times from the
        /// same thread, the most recent call "wins" and overwrites the data in the buffer.
        /// </summary>
        public static ReadOnlySpan<byte> GetProtectedReadonlyBuffer(Span<byte> data)
        {
            var buffer = EnsureThreadStaticBuffer(ref _readonlyThreadStaticBuffer);
            var bufferSpan = buffer.Span;
            bufferSpan = bufferSpan.Slice(bufferSpan.Length - data.Length);

            buffer.MakeWriteable();
            data.CopyTo(bufferSpan);
            buffer.MakeReadonly();
            return bufferSpan;
        }

        /// <summary>
        /// Returns a writeable buffer of length <paramref name="cb"/> which is immediately followed by a
        /// poison page. If this method is called multiple times from the same thread, it will return
        /// overlapping buffers. The buffer will be filled with random data on each call.
        /// </summary>
        public static Span<byte> GetProtectedWriteableBuffer(int cb)
        {
            var buffer = EnsureThreadStaticBuffer(ref _writeableThreadStaticBuffer);
            var bufferSpan = buffer.Span;
            bufferSpan = bufferSpan.Slice(checked(bufferSpan.Length - cb));

            new Random().NextBytes(bufferSpan); // fill with random data, doesn't need to be cryptographically secure
            return bufferSpan;
        }

        /// <summary>
        /// Similar to <see cref="GetProtectedWriteableBuffer(int)"/>, but returns a <see cref="Span{char}"/> instead of
        /// a <see cref="Span{byte}"/>. The <paramref name="cch"/> parameter is the size of the span in elements.
        /// </summary>
        public static Span<char> GetProtectedWriteableCharBuffer(int cch)
        {
            var buffer = EnsureThreadStaticBuffer(ref _writeableThreadStaticBuffer);
            var bufferSpan = buffer.Span;
            bufferSpan = bufferSpan.Slice(checked(bufferSpan.Length - 2 * cch));

            new Random().NextBytes(bufferSpan); // fill with random data, doesn't need to be cryptographically secure
            return MemoryMarshal.Cast<byte, char>(bufferSpan);
        }
    }
}
