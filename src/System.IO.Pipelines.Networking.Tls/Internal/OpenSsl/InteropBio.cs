using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Networking.Tls.Internal.OpenSsl
{
    internal unsafe static class InteropBio
    {
        private static class UnixLib
        {
            public const string CryptoDll = "libcrypto.so";
            [DllImport(CryptoDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr BIO_new_file(string filename, string mode);
            [DllImport(CryptoDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr BIO_new(IntPtr type);
            [DllImport(CryptoDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void BIO_free(IntPtr bio);
        }

        private static class OsxLib
        {
            public const string CryptoDll = "libcrypto.1.0.0.dylib";
            [DllImport(CryptoDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr BIO_new_file(string filename, string mode);
            [DllImport(CryptoDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr BIO_new(IntPtr type);
            [DllImport(CryptoDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void BIO_free(IntPtr bio);
        }

        private static class WindowsLib
        {
            public const string CryptoDll = "libeay32.dll";
            [DllImport(CryptoDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr BIO_new_file(string filename, string mode);
            [DllImport(CryptoDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr BIO_new(IntPtr type);
            [DllImport(CryptoDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void BIO_free(IntPtr bio);
        }

        private static BioHandle BIO_new_file(string fileName, string fileAccessType)
        {
            BioHandle handle = new BioHandle();
            if (Interop.IsWindows)
            {
                handle.Handle = WindowsLib.BIO_new_file(fileName, fileAccessType);
            }
            else if (Interop.IsOsx)
            {
                handle.Handle = OsxLib.BIO_new_file(fileName, fileAccessType);
            }
            else
            {
                handle.Handle = UnixLib.BIO_new_file(fileName, fileAccessType);
            }
            return handle;
        }
        public static BioHandle BIO_new_file_read(string fileName) => BIO_new_file(fileName, "r");
        public static BioHandle BIO_new_file_write(string filename) => BIO_new_file(filename, "w");
        public static BioHandle BIO_new(IntPtr type)
        {
            BioHandle handle = new BioHandle();
            if (Interop.IsWindows)
            {
                handle.Handle = WindowsLib.BIO_new(type);
            }
            else if (Interop.IsOsx)
            {
                handle.Handle = OsxLib.BIO_new(type);
            }
            else
            {
                handle.Handle = UnixLib.BIO_new(type);
            }
            return handle;
        }
        public static void BIO_free(IntPtr bio)
        {
            if (Interop.IsWindows)
            {
                WindowsLib.BIO_free(bio);
            }
            else if (Interop.IsOsx)
            {
                OsxLib.BIO_free(bio);
            }
            else
            {
                UnixLib.BIO_free(bio);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BioHandle
        {
            public IntPtr Handle;
            public void FreeBio()
            {
                if (Handle != IntPtr.Zero)
                {
                    BIO_free(Handle);
                    Handle = IntPtr.Zero;
                }
            }
        }
    }
}
