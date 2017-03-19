// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace System.Buffers.Experimental.System.Buffers
{
    public class OwnedNativeMemory : OwnedMemory<byte>
    {
        public OwnedNativeMemory(int length) : this(length, Marshal.AllocHGlobal(length))
        { }

        public OwnedNativeMemory(int length, IntPtr address) : base(null, 0, length, address) { }

        public static implicit operator IntPtr(OwnedNativeMemory owner)
        {
            unsafe
            {
                return new IntPtr(owner.Pointer);
            }
        }

        ~OwnedNativeMemory()
        {
            Dispose(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (base.Pointer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(base.Pointer);
            }
            base.Dispose(disposing);
        }

        public new unsafe byte* Pointer => (byte*)base.Pointer.ToPointer();
    }
}
