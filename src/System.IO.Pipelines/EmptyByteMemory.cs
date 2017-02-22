// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Runtime.CompilerServices;

namespace System.IO.Pipelines
{

    internal class EmptyByteMemory : OwnedMemory<byte>
    {
        private static OwnedMemory<byte> s_ownedMemory;

        public static void EnsureInitalized()
        {
            if (s_ownedMemory == null)
            {
                Initalize();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Initalize()
        {
            s_ownedMemory = new EmptyByteMemory();
            ReadOnlyEmpty = s_ownedMemory.Memory;
        }

        /// <summary>
        /// Should never be assigned to. Cannot be made readonly directly otherwise functions Slice etc will make a defensive copy.
        /// In future this may be able to be changed to a "readonly ref"
        /// </summary>
        public static Memory<byte> ReadOnlyEmpty;

        private EmptyByteMemory() : base(new byte[0], 0, 0) { }

        protected override void Dispose(bool disposing)
        { }
    }
}
