using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

    internal static class Interop
    {
        internal static partial class Brotli
        {
            static bool x86 = IntPtr.Size == 4;
            [DllImport("brotli_dll.x86.dll", EntryPoint = "CompressStream")]
            private static extern int Compress86(uint available_int, IntPtr next_in, ref UIntPtr available_out, ref IntPtr next_out, IntPtr state, int operation);
            [DllImport("brotli_dll.x64.dll", EntryPoint = "CompressStream")]
            private static extern int Compress64(uint available_int, IntPtr next_in, ref UIntPtr available_out, ref IntPtr next_out, IntPtr state, int operation);

            [DllImport("brotli_dll.x86.dll", EntryPoint = "BrotilEncoderStateCreate")]
            private static extern int CreateEncoderState86(out IntPtr state, int quality, int lgwin);
            [DllImport("brotli_dll.x64.dll", EntryPoint = "BrotilEncoderStateCreate")]
            private static extern int CreateEncoderState64(out IntPtr state, int quality, int lgwin);
            [DllImport("brotli_dll.x86.dll", EntryPoint = "BrotilEncoderStateDestroy")]
            private static extern int DestroyEncoderState86(IntPtr state);
            [DllImport("brotli_dll.x64.dll", EntryPoint = "BrotilEncoderStateDestroy")]
            private static extern int DestroyEncoderState64(IntPtr state);

        }
    }

