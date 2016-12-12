using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Networking.Tls.Internal.Sspi
{
    [Flags]
    internal enum ContextFlags
    {
        Zero = 0,
        // The communicating parties must authenticate
        // their identities to each other. Without MutualAuth,
        // the client authenticates its identity to the server.
        // With MutualAuth, the server also must authenticate
        // its identity to the client.
        MutualAuth = 0x00000002,
        // The security package detects replayed packets and
        // notifies the caller if a packet has been replayed.
        // The use of this flag implies all of the conditions
        // specified by the Integrity flag.
        ReplayDetect = 0x00000004,
        // The context must be allowed to detect out-of-order
        // delivery of packets later through the message support
        // functions. Use of this flag implies all of the
        // conditions specified by the Integrity flag.
        SequenceDetect = 0x00000008,
        // The context must protect data while in transit.
        // Confidentiality is supported for NTLM with Microsoft
        // Windows NT version 4.0, SP4 and later and with the
        // Kerberos protocol in Microsoft Windows 2000 and later.
        Confidentiality = 0x00000010,
        UseSessionKey = 0x00000020,
        AllocateMemory = 0x00000100,

        // Connection semantics must be used.
        Connection = 0x00000800,

        // Client applications requiring extended error messages specify the
        // ISC_REQ_EXTENDED_ERROR flag when calling the InitializeSecurityContext
        // Server applications requiring extended error messages set
        // the ASC_REQ_EXTENDED_ERROR flag when calling AcceptSecurityContext.
        InitExtendedError = 0x00004000,
        AcceptExtendedError = 0x00008000,
        // A transport application requests stream semantics
        // by setting the ISC_REQ_STREAM and ASC_REQ_STREAM
        // flags in the calls to the InitializeSecurityContext
        // and AcceptSecurityContext functions
        InitStream = 0x00008000,
        AcceptStream = 0x00010000,
        // Buffer integrity can be verified; however, replayed
        // and out-of-sequence messages will not be detected
        InitIntegrity = 0x00010000,       // ISC_REQ_INTEGRITY
        AcceptIntegrity = 0x00020000,       // ASC_REQ_INTEGRITY

        InitManualCredValidation = 0x00080000,   // ISC_REQ_MANUAL_CRED_VALIDATION
        InitUseSuppliedCreds = 0x00000080,   // ISC_REQ_USE_SUPPLIED_CREDS
        InitIdentify = 0x00020000,   // ISC_REQ_IDENTIFY
        AcceptIdentify = 0x00080000,   // ASC_REQ_IDENTIFY

        ProxyBindings = 0x04000000,   // ASC_REQ_PROXY_BINDINGS
        AllowMissingBindings = 0x10000000,   // ASC_REQ_ALLOW_MISSING_BINDINGS

        UnverifiedTargetName = 0x20000000,   // ISC_REQ_UNVERIFIED_TARGET_NAME
    }

    internal enum CredentialUse
    {
        Inbound = 0x1,
        Outbound = 0x2,
        Both = 0x3,
    }

    [Flags]
    internal enum CredentialFlags
    {
        Zero = 0,
        NoSystemMapper = 0x02,
        NoNameCheck = 0x04,
        ValidateManual = 0x08,
        NoDefaultCred = 0x10,
        ValidateAuto = 0x20,
        SendAuxRecord = 0x00200000,
        UseStrongCrypto = 0x00400000,
    }

    internal enum SecurityBufferType
    {
        Empty = 0x00,
        Data = 0x01,
        Token = 0x02,
        Parameters = 0x03,
        Missing = 0x04,
        Extra = 0x05,
        Trailer = 0x06,
        Header = 0x07,
        Padding = 0x09,    // non-data padding
        Stream = 0x0A,
        ChannelBindings = 0x0E,
        Alert = 0x11,
        TargetHost = 0x10,
        ApplicationProtocols = 18,
        ReadOnlyFlag = unchecked((int)0x80000000),
        ReadOnlyWithChecksum = 0x10000000,
    }

    internal enum Endianness
    {
        Network = 0x00,
        Native = 0x10,
    }

    internal enum AlgId : uint
    {
        CALG_3DES = 0x00006603,//Triple DES encryption algorithm.
        CALG_3DES_112 = 0x00006609,//Two-key triple DES encryption with effective key length equal to 112 bits.
        CALG_AES = 0x00006611,//Advanced Encryption Standard (AES). This algorithm is supported by the Microsoft AES Cryptographic Provider.
        CALG_AES_128 = 0x0000660e,//128 bit AES. This algorithm is supported by the Microsoft AES Cryptographic Provider.
        CALG_AES_192 = 0x0000660f,//192 bit AES. This algorithm is supported by the Microsoft AES Cryptographic Provider.
        CALG_AES_256 = 0x00006610,//256 bit AES. This algorithm is supported by the Microsoft AES Cryptographic Provider.
        CALG_AGREEDKEY_ANY = 0x0000aa03,//Temporary algorithm identifier for handles of Diffie-Hellman–agreed keys.
        CALG_CYLINK_MEK = 0x0000660c,//An algorithm to create a 40-bit DES key that has parity bits and zeroed key bits to make its key length 64 bits. This algorithm is supported by the Microsoft Base Cryptographic Provider.
        CALG_DES = 0x00006601,//DES encryption algorithm.
        CALG_DESX = 0x00006604,//DESX encryption algorithm.
        CALG_DH_EPHEM = 0x0000aa02, //	Diffie-Hellman ephemeral key exchange algorithm.
        CALG_DH_SF = 0x0000aa01, //	Diffie-Hellman store and forward key exchange algorithm.
        CALG_DSS_SIGN = 0x00002200, //DSA public key signature algorithm.
        CALG_ECDH = 0x0000aa05, //Elliptic curve Diffie-Hellman key exchange algorithm.Note  This algorithm is supported only through Cryptography API: Next Generation. Windows Server 2003 and Windows XP:  This algorithm is not supported.
        CALG_ECDH_EPHEM = 0x0000ae06,//Ephemeral elliptic curve Diffie-Hellman key exchange algorithm.Note This algorithm is supported only through Cryptography API: Next Generation. Windows Server 2003 and Windows XP:  This algorithm is not supported.
        CALG_ECDSA = 0x00002203,//Elliptic curve digital signature algorithm.Note This algorithm is supported only through Cryptography API: Next Generation. Windows Server 2003 and Windows XP:  This algorithm is not supported.
        CALG_ECMQV = 0x0000a001,//Elliptic curve Menezes, Qu, and Vanstone (MQV) key exchange algorithm.This algorithm is not supported.
        CALG_HASH_REPLACE_OWF = 0x0000800b,//One way function hashing algorithm.
        CALG_HUGHES_MD5 = 0x0000a003,//Hughes MD5 hashing algorithm.
        CALG_HMAC = 0x00008009,//HMAC keyed hash algorithm. This algorithm is supported by the Microsoft Base Cryptographic Provider.
        CALG_KEA_KEYX = 0x0000aa04,//KEA key exchange algorithm (FORTEZZA). This algorithm is not supported.
        CALG_MAC = 0x00008005,//MAC keyed hash algorithm. This algorithm is supported by the Microsoft Base Cryptographic Provider.
        CALG_MD2 = 0x00008001,//MD2 hashing algorithm.This algorithm is supported by the Microsoft Base Cryptographic Provider.
        CALG_MD4 = 0x00008002,//MD4 hashing algorithm.
        CALG_MD5 = 0x00008003,//MD5 hashing algorithm.This algorithm is supported by the Microsoft Base Cryptographic Provider.
        CALG_NO_SIGN = 0x00002000,//No signature algorithm.
        CALG_OID_INFO_CNG_ONLY = 0xffffffff,//The algorithm is only implemented in CNG.The macro, IS_SPECIAL_OID_INFO_ALGID, can be used to determine whether a cryptography algorithm is only supported by using the CNG functions.
        CALG_OID_INFO_PARAMETERS = 0xfffffffe,//The algorithm is defined in the encoded parameters.The algorithm is only supported by using CNG. The macro, IS_SPECIAL_OID_INFO_ALGID, can be used to determine whether a cryptography algorithm is only supported by using the CNG functions.
        CALG_PCT1_MASTER = 0x00004c04,//Used by the Schannel.dll operations system.This ALG_ID should not be used by applications.
        CALG_RC2 = 0x00006602,//RC2 block encryption algorithm. This algorithm is supported by the Microsoft Base Cryptographic Provider.
        CALG_RC4 = 0x00006801,//RC4 stream encryption algorithm. This algorithm is supported by the Microsoft Base Cryptographic Provider.
        CALG_RC5 = 0x0000660d,//RC5 block encryption algorithm.
        CALG_RSA_KEYX = 0x0000a400,//RSA public key exchange algorithm.This algorithm is supported by the Microsoft Base Cryptographic Provider.
        CALG_RSA_SIGN = 0x00002400,//RSA public key signature algorithm.This algorithm is supported by the Microsoft Base Cryptographic Provider.
        CALG_SCHANNEL_ENC_KEY = 0x00004c07,//Used by the Schannel.dll operations system. This ALG_ID should not be used by applications.
        CALG_SCHANNEL_MAC_KEY = 0x00004c03,//Used by the Schannel.dll operations system.This ALG_ID should not be used by applications.
        CALG_SCHANNEL_MASTER_HASH = 0x00004c02,//Used by the Schannel.dll operations system.This ALG_ID should not be used by applications.
        CALG_SEAL = 0x00006802,//SEAL encryption algorithm.This algorithm is not supported.
        CALG_SHA = 0x00008004,//SHA hashing algorithm.This algorithm is supported by the Microsoft Base Cryptographic Provider.
        CALG_SHA1 = 0x00008004,//Same as CALG_SHA.This algorithm is supported by the Microsoft Base Cryptographic Provider.
        CALG_SHA_256 = 0x0000800c,//256 bit SHA hashing algorithm. This algorithm is supported by Microsoft Enhanced RSA and AES Cryptographic Provider..Windows XP with SP3:  This algorithm is supported by the Microsoft Enhanced RSA and AES Cryptographic Provider (Prototype).Windows XP with SP2, Windows XP with SP1 and Windows XP:  This algorithm is not supported.
        CALG_SHA_384 = 0x0000800d,//384 bit SHA hashing algorithm. This algorithm is supported by Microsoft Enhanced RSA and AES Cryptographic Provider.Windows XP with SP3:  This algorithm is supported by the Microsoft Enhanced RSA and AES Cryptographic Provider (Prototype).Windows XP with SP2, Windows XP with SP1 and Windows XP:  This algorithm is not supported.
        CALG_SHA_512 = 0x0000800e,//512 bit SHA hashing algorithm. This algorithm is supported by Microsoft Enhanced RSA and AES Cryptographic Provider.Windows XP with SP3:  This algorithm is supported by the Microsoft Enhanced RSA and AES Cryptographic Provider (Prototype).Windows XP with SP2, Windows XP with SP1 and Windows XP:  This algorithm is not supported.
        CALG_SKIPJACK = 0x0000660a,//Skipjack block encryption algorithm (FORTEZZA). This algorithm is not supported.
        CALG_SSL2_MASTER = 0x00004c05,//Used by the Schannel.dll operations system.This ALG_ID should not be used by applications.
        CALG_SSL3_MASTER = 0x00004c01,//Used by the Schannel.dll operations system.This ALG_ID should not be used by applications.
        CALG_SSL3_SHAMD5 = 0x00008008,//Used by the Schannel.dll operations system.This ALG_ID should not be used by applications.
        CALG_TEK = 0x0000660b,//TEK (FORTEZZA). This algorithm is not supported.
        CALG_TLS1_MASTER = 0x00004c06,//Used by the Schannel.dll operations system.This ALG_ID should not be used by applications.
        CALG_TLS1PRF = 0x0000800a,//Used by the Schannel.dll operations system.This ALG_ID should not be used by applications.
    }

    internal enum ContextAttribute
    {
        Sizes = 0x00,
        Names = 0x01,
        Lifespan = 0x02,
        DceInfo = 0x03,
        StreamSizes = 0x04,
        KeyInfo = 0x05,
        Authority = 0x06,
        PackageInfo = 0x0A,
        Session_Key = 0x09,
        UniqueBindings = 0x19,
        EndpointBindings = 0x1A,
        ClientSpecifiedSpn = 0x1B,
        RemoteCertificate = 0x53,
        LocalCertificate = 0x54,
        RootStore = 0x55,
        IssuerListInfoEx = 0x59,
        ConnectionInfo = 0x5A,
        ApplicationProtocol = 0x23,
    }
}