using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Networking.Tls.Internal.Sspi
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SecPkgInfo
    {
        public int fCapabilities;
        public ushort wVersion;
        public ushort wRPCID;
        public int cbMaxToken;
        public IntPtr Name;
        public IntPtr Comment;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SSPIHandle
    {
        public IntPtr handleHi;
        public IntPtr handleLo;

        public bool IsValid => handleHi != IntPtr.Zero && handleLo != IntPtr.Zero;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SecureCredential
    {
        public const int CurrentVersion = 0x4;
        public int version;
        public int cCreds;
        public IntPtr certContextArray;
        public IntPtr rootStore;
        public int cMappers;
        public IntPtr phMappers;
        public int cSupportedAlgs;
        public IntPtr palgSupportedAlgs;
        public int grbitEnabledProtocols;
        public int dwMinimumCipherStrength;
        public int dwMaximumCipherStrength;
        public int dwSessionLifespan;
        public CredentialFlags dwFlags;
        public int reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct _SecPkgContext_ConnectionInfo
    {
        public int dwProtocol;
        public AlgId aiCipher;
        public int dwCipherStrength;
        public AlgId aiHash;
        public int dwHashStrength;
        public AlgId aiExch;
        public int dwExchStrength;
    }

    internal unsafe struct _SecPkgContext_KeyInfo
    {
        public void* sSignatureAlgorithmName;
        public void* sEncryptAlgorithmName;
        public uint KeySize;
        public uint SignatureAlgorithm;
        public uint EncryptAlgorithm;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct SecurityBuffer
    {
        public SecurityBuffer(void* bufferPointer, int bufferSize, SecurityBufferType bufferType)
        {
            tokenPointer = bufferPointer;
            type = bufferType;
            size = bufferSize;
        }

        public int size;
        public SecurityBufferType type;
        public void* tokenPointer;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct SecurityBufferDescriptor
    {
        public int Version;
        public int Count;
        public void* UnmanagedPointer;

        public SecurityBufferDescriptor(int count)
        {
            Version = 0;
            Count = count;
            UnmanagedPointer = null;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ContextStreamSizes
    {
        public int header;
        public int trailer;
        public int maximumMessage;
        public int buffersCount;
        public int blockSize;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct ContextApplicationProtocol
    {
        public ApplicationProtocols.ApplicationProtocolNegotiationStatus ProtoNegoStatus;
        public ApplicationProtocols.ApplicaitonProtocolNegotiationExtension ProtoNegoExt;
        public byte ProtocolIdSize;
        public fixed byte ProtocolId[255];
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct SessionKey
    {
        uint SessionKeyLength;
        public fixed byte Key[2048];
    }
}
