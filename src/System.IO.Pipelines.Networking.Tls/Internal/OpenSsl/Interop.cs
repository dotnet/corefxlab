using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Networking.Tls.Internal.OpenSsl
{
    internal unsafe static class Interop
    {
        public static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public static readonly bool IsOsx = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        private static class OsxLib
        {
            public const string SslDll = "libssl.1.0.0.dylib";
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static int SSL_CTX_use_PrivateKey(IntPtr ctx, IntPtr pkey);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static int SSL_CTX_use_certificate(IntPtr ctx, IntPtr cert);

            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public static extern int SSL_CTX_set_alpn_protos(IntPtr ctx, IntPtr protocolList, uint protocolListLength);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public static extern void SSL_get0_alpn_selected(IntPtr ssl, out byte* data, out int len);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public static extern void SSL_CTX_set_alpn_select_cb(IntPtr ctx, alpn_cb alpnCb, IntPtr arg);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void SSL_CTX_set_verify(IntPtr context, VerifyMode mode, IntPtr callback);

            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static int SSL_write(IntPtr ssl, void* buf, int len);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static int SSL_read(IntPtr ssl, void* buf, int len);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void SSL_set_connect_state(IntPtr ssl);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void SSL_set_accept_state(IntPtr sll);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static int SSL_do_handshake(IntPtr ssl);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr SSL_CTX_new(IntPtr sslMethod);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void SSL_free(IntPtr ssl);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void SSL_set_bio(IntPtr ssl, IntPtr readBio, IntPtr writeBio);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static SslErrorCodes SSL_get_error(IntPtr ssl, int errorIndetity);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr SSL_CIPHER_get_name(IntPtr ssl_cipher);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr SSL_get_current_cipher(IntPtr ssl);

            public static CipherInfo GetCipherInfo(IntPtr ssl)
            {
                var returnValue = new CipherInfo();
                var cPtr = SSL_get_current_cipher(ssl);
                returnValue.Name = Marshal.PtrToStringAnsi(SSL_CIPHER_get_name(cPtr));

                return returnValue;
            }

            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr SSL_new(IntPtr sslContext);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void SSL_CTX_free(IntPtr sslCtx);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr SSLv23_client_method();
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr SSLv23_server_method();
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            private extern static int SSL_CTX_ctrl(IntPtr ctx, int ctrlType, long options, IntPtr other);
            public static int SSL_CTX_set_options(IntPtr ctx, ContextOptions options) => SSL_CTX_ctrl(ctx, SSL_CTRL_OPTIONS, (long)options, IntPtr.Zero);

            private static readonly IntPtr ServerMethod = SSLv23_server_method();
            private static readonly IntPtr ClientMethod = SSLv23_client_method();
            public static IntPtr NewServerContext(VerifyMode mode)
            {
                var returnValue = SSL_CTX_new(ServerMethod);
                SSL_CTX_set_options(returnValue, Default_Context_Options);
                SSL_CTX_set_verify(returnValue, mode, IntPtr.Zero);
                return returnValue;
            }
            public static IntPtr NewClientContext(VerifyMode mode)
            {
                var returnValue = SSL_CTX_new(ClientMethod);
                SSL_CTX_set_options(returnValue, Default_Context_Options);
                SSL_CTX_set_verify(returnValue, mode, IntPtr.Zero);
                return returnValue;
            }

            public static int SetKeys(IntPtr ctx, IntPtr certificates, IntPtr privateKey)
            {
                if (certificates != IntPtr.Zero)
                {
                    InteropCrypto.CheckForErrorOrThrow(SSL_CTX_use_certificate(ctx, certificates));
                }
                if (privateKey != IntPtr.Zero)
                {
                    InteropCrypto.CheckForErrorOrThrow(SSL_CTX_use_PrivateKey(ctx, privateKey));
                }
                return 0;
            }
        }

        private static class WindowsLib
        {
            public const string SslDll = "ssleay32.dll";
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static int SSL_CTX_use_PrivateKey(IntPtr ctx, IntPtr pkey);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static int SSL_CTX_use_certificate(IntPtr ctx, IntPtr cert);

            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public static extern int SSL_CTX_set_alpn_protos(IntPtr ctx, IntPtr protocolList, uint protocolListLength);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public static extern void SSL_get0_alpn_selected(IntPtr ssl, out byte* data, out int len);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public static extern void SSL_CTX_set_alpn_select_cb(IntPtr ctx, alpn_cb alpnCb, IntPtr arg);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void SSL_CTX_set_verify(IntPtr context, VerifyMode mode, IntPtr callback);

            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static int SSL_write(IntPtr ssl, void* buf, int len);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static int SSL_read(IntPtr ssl, void* buf, int len);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void SSL_set_connect_state(IntPtr ssl);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void SSL_set_accept_state(IntPtr sll);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static int SSL_do_handshake(IntPtr ssl);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr SSL_CTX_new(IntPtr sslMethod);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void SSL_free(IntPtr ssl);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void SSL_set_bio(IntPtr ssl, IntPtr readBio, IntPtr writeBio);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static SslErrorCodes SSL_get_error(IntPtr ssl, int errorIndetity);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr SSL_CIPHER_get_name(IntPtr ssl_cipher);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr SSL_get_current_cipher(IntPtr ssl);

            public static CipherInfo GetCipherInfo(IntPtr ssl)
            {
                var returnValue = new CipherInfo();
                var cPtr = SSL_get_current_cipher(ssl);
                returnValue.Name = Marshal.PtrToStringAnsi(SSL_CIPHER_get_name(cPtr));

                return returnValue;
            }

            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr SSL_new(IntPtr sslContext);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void SSL_CTX_free(IntPtr sslCtx);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr SSLv23_client_method();
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr SSLv23_server_method();
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            private extern static int SSL_CTX_ctrl(IntPtr ctx, int ctrlType, long options, IntPtr other);
            public static int SSL_CTX_set_options(IntPtr ctx, ContextOptions options) => SSL_CTX_ctrl(ctx, SSL_CTRL_OPTIONS, (long)options, IntPtr.Zero);

            private static readonly IntPtr ServerMethod = SSLv23_server_method();
            private static readonly IntPtr ClientMethod = SSLv23_client_method();
            public static IntPtr NewServerContext(VerifyMode mode)
            {
                var returnValue = SSL_CTX_new(ServerMethod);
                SSL_CTX_set_options(returnValue, Default_Context_Options);
                SSL_CTX_set_verify(returnValue, mode, IntPtr.Zero);
                return returnValue;
            }
            public static IntPtr NewClientContext(VerifyMode mode)
            {
                var returnValue = SSL_CTX_new(ClientMethod);
                SSL_CTX_set_options(returnValue, Default_Context_Options);
                SSL_CTX_set_verify(returnValue, mode, IntPtr.Zero);
                return returnValue;
            }

            public static int SetKeys(IntPtr ctx, IntPtr certificates, IntPtr privateKey)
            {
                if (certificates != IntPtr.Zero)
                {
                    InteropCrypto.CheckForErrorOrThrow(SSL_CTX_use_certificate(ctx, certificates));
                }
                if (privateKey != IntPtr.Zero)
                {
                    InteropCrypto.CheckForErrorOrThrow(SSL_CTX_use_PrivateKey(ctx, privateKey));
                }
                return 0;
            }
        }

        private static class UnixLib
        {
            public const string SslDll = "libssl.so";
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static int SSL_CTX_use_PrivateKey(IntPtr ctx, IntPtr pkey);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static int SSL_CTX_use_certificate(IntPtr ctx, IntPtr cert);

            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public static extern int SSL_CTX_set_alpn_protos(IntPtr ctx, IntPtr protocolList, uint protocolListLength);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public static extern void SSL_get0_alpn_selected(IntPtr ssl, out byte* data, out int len);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public static extern void SSL_CTX_set_alpn_select_cb(IntPtr ctx, alpn_cb alpnCb, IntPtr arg);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void SSL_CTX_set_verify(IntPtr context, VerifyMode mode, IntPtr callback);

            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static int SSL_write(IntPtr ssl, void* buf, int len);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static int SSL_read(IntPtr ssl, void* buf, int len);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static int SSL_do_handshake(IntPtr ssl);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr SSL_CTX_new(IntPtr sslMethod);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void SSL_free(IntPtr ssl);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void SSL_set_connect_state(IntPtr ssl);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void SSL_set_accept_state(IntPtr sll);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void SSL_set_bio(IntPtr ssl, IntPtr readBio, IntPtr writeBio);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static SslErrorCodes SSL_get_error(IntPtr ssl, int errorIndetity);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr SSL_CIPHER_get_name(IntPtr ssl_cipher);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr SSL_get_current_cipher(IntPtr ssl);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static int SSL_CIPHER_get_bits(IntPtr ssl_cipher, out int alg_bits);

            public static CipherInfo GetCipherInfo(IntPtr ssl)
            {
                var returnValue = new CipherInfo();
                var cPtr = SSL_get_current_cipher(ssl);
                returnValue.Name = Marshal.PtrToStringAnsi(SSL_CIPHER_get_name(cPtr));
                SSL_CIPHER_get_bits(ssl, out returnValue.KeySizeInBits);
                return returnValue;
            }

            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr SSL_new(IntPtr sslContext);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static void SSL_CTX_free(IntPtr sslCtx);
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr SSLv23_client_method();
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr SSLv23_server_method();
            [DllImport(SslDll, CallingConvention = CallingConvention.Cdecl)]
            private extern static int SSL_CTX_ctrl(IntPtr ctx, int ctrlType, long options, IntPtr other);
            public static int SSL_CTX_set_options(IntPtr ctx, ContextOptions options) => SSL_CTX_ctrl(ctx, SSL_CTRL_OPTIONS, (long)options, IntPtr.Zero);


            private static readonly IntPtr ServerMethod = SSLv23_server_method();
            private static readonly IntPtr ClientMethod = SSLv23_client_method();
            public static IntPtr NewServerContext(VerifyMode verifyMode)
            {
                var returnValue = SSL_CTX_new(ServerMethod);
                SSL_CTX_set_options(returnValue, Default_Context_Options);
                SSL_CTX_set_verify(returnValue, verifyMode, IntPtr.Zero);
                return returnValue;
            }
            public static IntPtr NewClientContext(VerifyMode verifyMode)
            {
                var returnValue = SSL_CTX_new(ClientMethod);
                SSL_CTX_set_options(returnValue, Default_Context_Options);
                SSL_CTX_set_verify(returnValue, verifyMode, IntPtr.Zero);
                return returnValue;
            }

            public static int SetKeys(IntPtr ctx, IntPtr certificates, IntPtr privateKey)
            {
                if (certificates != IntPtr.Zero)
                {
                    InteropCrypto.CheckForErrorOrThrow(SSL_CTX_use_certificate(ctx, certificates));
                }
                if (privateKey != IntPtr.Zero)
                {
                    InteropCrypto.CheckForErrorOrThrow(SSL_CTX_use_PrivateKey(ctx, privateKey));
                }
                return 0;
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate AlpnStatus alpn_cb(IntPtr ssl, out byte* selProto, out byte selProtoLen, byte* inProtos, int inProtosLen, IntPtr arg);

        public static IntPtr NewServerContext(VerifyMode verifyMode) => IsWindows ? WindowsLib.NewServerContext(verifyMode) : IsOsx ? OsxLib.NewServerContext(verifyMode) : UnixLib.NewServerContext(verifyMode);
        public static IntPtr NewClientContext(VerifyMode verifyMode) => IsWindows ? WindowsLib.NewClientContext(verifyMode) : IsOsx ? OsxLib.NewClientContext(verifyMode) : UnixLib.NewClientContext(verifyMode);

        public static void SSL_set_bio(IntPtr ssl, InteropBio.BioHandle readBio, InteropBio.BioHandle writeBio)
        {
            if (IsWindows)
            {
                WindowsLib.SSL_set_bio(ssl, readBio.Handle, writeBio.Handle);
            }
            else if (IsOsx)
            {
                OsxLib.SSL_set_bio(ssl, readBio.Handle, writeBio.Handle);
            }
            else
            {
                UnixLib.SSL_set_bio(ssl, readBio.Handle, writeBio.Handle);
            }
        }
        public static void SSL_set_connect_state(IntPtr ssl)
        {
            if (IsWindows)
            {
                WindowsLib.SSL_set_connect_state(ssl);
            }
            else if (IsOsx)
            {
                OsxLib.SSL_set_connect_state(ssl);
            }
            else
            {
                UnixLib.SSL_set_connect_state(ssl);
            }
        }
        public static void SSL_set_accept_state(IntPtr ssl)
        {
            if (IsWindows)
            {
                WindowsLib.SSL_set_accept_state(ssl);
            }
            else if (IsOsx)
            {
                OsxLib.SSL_set_accept_state(ssl);
            }
            else
            {
                UnixLib.SSL_set_accept_state(ssl);
            }
        }
        public static int SSL_write(IntPtr ssl, void* buf, int len) => IsWindows ? WindowsLib.SSL_write(ssl, buf, len) : IsOsx ? OsxLib.SSL_write(ssl, buf, len) : UnixLib.SSL_write(ssl, buf, len);
        public static int SSL_read(IntPtr ssl, void* buf, int len) => IsWindows ? WindowsLib.SSL_read(ssl, buf, len) : IsOsx ? OsxLib.SSL_read(ssl, buf, len) : UnixLib.SSL_read(ssl, buf, len);
        public static int SSL_do_handshake(IntPtr ssl) => IsWindows ? WindowsLib.SSL_do_handshake(ssl) : IsOsx ? OsxLib.SSL_do_handshake(ssl) : UnixLib.SSL_do_handshake(ssl);
        public static SslErrorCodes SSL_get_error(IntPtr ssl, int errorIndetity) => IsWindows ? WindowsLib.SSL_get_error(ssl, errorIndetity) : IsOsx ? OsxLib.SSL_get_error(ssl, errorIndetity) : UnixLib.SSL_get_error(ssl, errorIndetity);
        public static int SetKeys(IntPtr ctx, IntPtr certificates, IntPtr privateKey) => IsWindows ? WindowsLib.SetKeys(ctx, certificates, privateKey) : IsOsx ? OsxLib.SetKeys(ctx, certificates, privateKey) : UnixLib.SetKeys(ctx, certificates, privateKey);
        public static void SSL_free(IntPtr ssl)
        {
            if (IsWindows)
            {
                WindowsLib.SSL_free(ssl);
            }
            else if (IsOsx)
            {
                OsxLib.SSL_free(ssl);
            }
            else
            {
                UnixLib.SSL_free(ssl);
            }
        }
        public static IntPtr SSL_new(IntPtr sslContext) => IsWindows ? WindowsLib.SSL_new(sslContext) : IsOsx ? OsxLib.SSL_new(sslContext) : UnixLib.SSL_new(sslContext);
        public static void SSL_CTX_free(IntPtr sslCtx)
        {
            if (IsWindows)
            {
                WindowsLib.SSL_CTX_free(sslCtx);
            }
            else if (IsOsx)
            {
                OsxLib.SSL_CTX_free(sslCtx);
            }
            else
            {
                UnixLib.SSL_CTX_free(sslCtx);
            }
        }
        public static void SSL_get0_alpn_selected(IntPtr ssl, out byte* data, out int len)
        {
            if (IsWindows)
            {
                WindowsLib.SSL_get0_alpn_selected(ssl, out data, out len);
            }
            else if (IsOsx)
            {
                OsxLib.SSL_get0_alpn_selected(ssl, out data, out len);
            }
            else
            {
                UnixLib.SSL_get0_alpn_selected(ssl, out data, out len);
            }
        }
        public static void SSL_CTX_set_alpn_select_cb(OpenSslSecurityContext context)
        {
            if (IsWindows)
            {
                WindowsLib.SSL_CTX_set_alpn_select_cb(context.SslContext, context.AlpnCallback, IntPtr.Zero);
            }
            else if (IsOsx)
            {
                OsxLib.SSL_CTX_set_alpn_select_cb(context.SslContext, context.AlpnCallback, IntPtr.Zero);
            }
            else
            {
                UnixLib.SSL_CTX_set_alpn_select_cb(context.SslContext, context.AlpnCallback, IntPtr.Zero);
            }
        }
        public static int SSL_CTX_set_alpn_protos(IntPtr ctx, IntPtr protocolList, uint protocolListLength) => IsWindows ? WindowsLib.SSL_CTX_set_alpn_protos(ctx, protocolList, protocolListLength) : IsOsx ? OsxLib.SSL_CTX_set_alpn_protos(ctx, protocolList, protocolListLength) : UnixLib.SSL_CTX_set_alpn_protos(ctx, protocolList, protocolListLength);
        public static CipherInfo GetCipherInfo(IntPtr ssl) => IsWindows ? WindowsLib.GetCipherInfo(ssl) : IsOsx ? OsxLib.GetCipherInfo(ssl) : UnixLib.GetCipherInfo(ssl);

        const int SSL_CTRL_OPTIONS = 32;
        const ContextOptions Default_Context_Options = ContextOptions.SSL_OP_NO_SSLv2 | ContextOptions.SSL_OP_NO_SSLv3;

        [Flags]
        public enum ContextOptions : long
        {
            SSL_OP_TLS_ROLLBACK_BUG = 0x00800000,
            SSL_OP_NO_SSLv2 = 0x01000000,
            SSL_OP_NO_SSLv3 = 0x02000000,
            SSL_OP_NO_TLSv1 = 0x04000000,
            SSL_OP_NO_TLSv1_2 = 0x08000000,
            SSL_OP_NO_TLSv1_1 = 0x10000000,
        }

        public enum AlpnStatus : uint
        {
            SSL_TLSEXT_ERR_OK = 0,
            SSL_TLSEXT_ERR_ALERT_WARNING = 1,
            SSL_TLSEXT_ERR_ALERT_FATAL = 2,
            SSL_TLSEXT_ERR_NOACK = 3,
        }

        [Flags]
        public enum VerifyMode : int
        {
            SSL_VERIFY_NONE = 0x00,
            SSL_VERIFY_PEER = 0x01,
            SSL_VERIFY_FAIL_IF_NO_PEER_CERT = 0x02,
            SSL_VERIFY_CLIENT_ONCE = 0x04,
        }

        public enum SslErrorCodes
        {
            SSL_NOTHING = 1,
            SSL_WRITING = 2,
            SSL_READING = 3,
        }
    }
}
