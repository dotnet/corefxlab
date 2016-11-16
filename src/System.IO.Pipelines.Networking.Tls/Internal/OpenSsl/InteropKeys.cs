using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Networking.Tls.Internal.OpenSsl
{
    internal static class InteropKeys
    {
        private static class WindowsLib
        {
            public const string CryptoDll = "libeay32.dll";
            [DllImport(CryptoDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static int PKCS12_parse(IntPtr p12, string password, out IntPtr privateKey, out IntPtr certificate, out IntPtr ca);
            [DllImport(CryptoDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void PKCS12_free(IntPtr p12);
            [DllImport(CryptoDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr d2i_PKCS12_bio(InteropBio.BioHandle inputBio, IntPtr p12);
        }

        private static class UnixLib
        {
            public const string CryptoDll = "libcrypto.so";
            [DllImport(CryptoDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static int PKCS12_parse(IntPtr p12, string password, out IntPtr privateKey, out IntPtr certificate, out IntPtr ca);
            [DllImport(CryptoDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void PKCS12_free(IntPtr p12);
            [DllImport(CryptoDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr d2i_PKCS12_bio(InteropBio.BioHandle inputBio, IntPtr p12);
        }

        private static class OsxLib
        {
            public const string CryptoDll = "libcrypto.1.0.0.dylib";
            [DllImport(CryptoDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static int PKCS12_parse(IntPtr p12, string password, out IntPtr privateKey, out IntPtr certificate, out IntPtr ca);
            [DllImport(CryptoDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void PKCS12_free(IntPtr p12);
            [DllImport(CryptoDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr d2i_PKCS12_bio(InteropBio.BioHandle inputBio, IntPtr p12);
        }

        private static int PKCS12_parse(IntPtr p12, string password, out IntPtr privateKey, out IntPtr certificate, out IntPtr ca) => Interop.IsWindows ? WindowsLib.PKCS12_parse(p12, password, out privateKey, out certificate, out ca) : Interop.IsOsx ? OsxLib.PKCS12_parse(p12, password, out privateKey, out certificate, out ca) : UnixLib.PKCS12_parse(p12, password, out privateKey, out certificate, out ca);
        private static IntPtr d2i_PKCS12_bio(InteropBio.BioHandle inputBio, IntPtr p12) => Interop.IsWindows ? WindowsLib.d2i_PKCS12_bio(inputBio, p12) : Interop.IsOsx ? OsxLib.d2i_PKCS12_bio(inputBio, p12) : UnixLib.d2i_PKCS12_bio(inputBio, p12);
        private static void PKCS12_free(IntPtr p12)
        {
            if (Interop.IsWindows)
            {
                WindowsLib.PKCS12_free(p12);
            }
            else if (Interop.IsOsx)
            {
                OsxLib.PKCS12_free(p12);
            }
            else
            {
                UnixLib.PKCS12_free(p12);
            }
        }

        public struct PK12Certifcate
        {
            public IntPtr Handle;
            public IntPtr CertificateHandle;
            public IntPtr AuthorityChainHandle;
            public IntPtr PrivateKeyHandle;

            public PK12Certifcate(InteropBio.BioHandle inputBio, string password)
            {
                Handle = d2i_PKCS12_bio(inputBio, IntPtr.Zero);
                PKCS12_parse(Handle, password, out PrivateKeyHandle, out CertificateHandle, out AuthorityChainHandle);
            }

            public void Free()
            {
                if (Handle != IntPtr.Zero)
                {
                    PKCS12_free(Handle);
                    Handle = IntPtr.Zero;
                }
            }
        }
    }
}
