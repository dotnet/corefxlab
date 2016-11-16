using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Networking.Tls.Internal.Sspi
{
    internal unsafe static class Interop
    {
        internal const string Dll = "sspicli.dll";

        [DllImport(Dll, ExactSpelling = true, SetLastError = true)]
        internal static extern int EnumerateSecurityPackagesW([Out] out int pkgnum, [Out] out SecPkgInfo* info);

        [DllImport(Dll, ExactSpelling = true, SetLastError = true)]
        internal static extern int FreeContextBuffer([In] IntPtr contextBuffer);

        [DllImport(Dll, ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        internal unsafe static extern SecurityStatus AcquireCredentialsHandleW([In] string principal, [In] string moduleName, [In] int usage, [In] void* logonID, [In] ref SecureCredential authdata, [In] void* keyCallback, [In] void* keyArgument, ref SSPIHandle handlePtr, [Out] out long timeStamp);

        [DllImport(Dll, ExactSpelling = true, SetLastError = true)]
        internal static extern int DeleteSecurityContext(ref SSPIHandle handlePtr);

        [DllImport(Dll, ExactSpelling = true, SetLastError = true)]
        internal static extern int FreeCredentialsHandle(ref SSPIHandle handlePtr);

        [DllImport(Dll, ExactSpelling = true, SetLastError = true)]
        internal unsafe static extern SecurityStatus AcceptSecurityContext(ref SSPIHandle credentialHandle, [In] void* inContextPtr, SecurityBufferDescriptor* inputBuffer, [In] ContextFlags inFlags, [In] Endianness endianness, [In, Out] void* newContextPtr, SecurityBufferDescriptor* outputBuffer, [In, Out] ref ContextFlags attributes, out long timeStamp);

        internal static CipherInfo GetCipherInfo(SSPIHandle _contextPointer)
        {
            _SecPkgContext_ConnectionInfo connInfo;
            QueryContextAttributesW(ref _contextPointer, ContextAttribute.ConnectionInfo, out connInfo);
            var returnValue = new CipherInfo()
            {
                Bits = connInfo.dwCipherStrength,
                Name = $"{connInfo.aiCipher}-{connInfo.dwHashStrength}"
            };
            FreeContextBuffer((IntPtr)Unsafe.AsPointer(ref connInfo));
            return returnValue;
        }

        [DllImport(Dll, ExactSpelling = true, SetLastError = true)]
        internal static unsafe extern SecurityStatus DecryptMessage([In] ref SSPIHandle contextHandle, [In, Out] SecurityBufferDescriptor inputOutput, [In] uint sequenceNumber, uint* qualityOfProtection);

        [DllImport(Dll, ExactSpelling = true, SetLastError = true)]
        internal static extern SecurityStatus EncryptMessage(ref SSPIHandle contextHandle, [In] uint qualityOfProtection, [In, Out] SecurityBufferDescriptor inputOutput, [In] uint sequenceNumber);

        [DllImport(Dll, ExactSpelling = true, SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int QueryContextAttributesW(ref SSPIHandle phContext, [In] ContextAttribute contextFlag, [Out] out ContextStreamSizes sizes);

        [DllImport(Dll, ExactSpelling = true, SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int QueryContextAttributesW(ref SSPIHandle phContext, [In] ContextAttribute contextFlag, [Out] out _SecPkgContext_KeyInfo sizes);

        [DllImport(Dll, ExactSpelling = true, SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int QueryContextAttributesW(ref SSPIHandle phContext, [In] ContextAttribute contextFlag, [Out] out ContextApplicationProtocol protocolInfo);

        [DllImport(Dll, ExactSpelling = true, SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int QueryContextAttributesW(ref SSPIHandle phContext, [In] ContextAttribute contextFlag, [Out] out SessionKey sessionKey);

        [DllImport(Dll, ExactSpelling = true, SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int QueryContextAttributesW(ref SSPIHandle phContext, [In] ContextAttribute contextFlag, [Out] out _SecPkgContext_ConnectionInfo connectionInfo);

        [DllImport(Dll, ExactSpelling = true, SetLastError = true)]
        internal unsafe static extern SecurityStatus InitializeSecurityContextW(ref SSPIHandle credentialHandle, [In] void* inContextPtr, [In] string targetName, [In] ContextFlags inFlags, [In] int reservedI, [In] Endianness endianness, SecurityBufferDescriptor* inputBuffer, [In] int reservedII, [In, Out] void* newContextPtr, SecurityBufferDescriptor* outputBuffer, [In, Out] ref ContextFlags attributes, out long timeStamp);

        internal const int SP_PROT_TLS1_0_SERVER = 0x00000040;
        internal const int SP_PROT_TLS1_0_CLIENT = 0x00000080;
        internal const int SP_PROT_TLS1_0 = (SP_PROT_TLS1_0_SERVER | SP_PROT_TLS1_0_CLIENT);

        internal const int SP_PROT_TLS1_1_SERVER = 0x00000100;
        internal const int SP_PROT_TLS1_1_CLIENT = 0x00000200;
        internal const int SP_PROT_TLS1_1 = (SP_PROT_TLS1_1_SERVER | SP_PROT_TLS1_1_CLIENT);

        internal const int SP_PROT_TLS1_2_SERVER = 0x00000400;
        internal const int SP_PROT_TLS1_2_CLIENT = 0x00000800;
        internal const int SP_PROT_TLS1_2 = (SP_PROT_TLS1_2_SERVER | SP_PROT_TLS1_2_CLIENT);

        internal const int SP_PROT_NONE = 0;

        // These two constants are not taken from schannel.h. 
        internal const int ClientProtocolMask = (SP_PROT_TLS1_0_CLIENT | SP_PROT_TLS1_1_CLIENT | SP_PROT_TLS1_2_CLIENT);
        internal const int ServerProtocolMask = (SP_PROT_TLS1_0_SERVER | SP_PROT_TLS1_1_SERVER | SP_PROT_TLS1_2_SERVER);
    }
}
