using System.IO.Pipelines.Networking.Tls.Internal.Sspi;

namespace System.IO.Pipelines.Networking.Tls
{
    internal static class ApplicationLayerProtocolExtension
    {
        //Source iana registry http://www.iana.org/assignments/tls-extensiontype-values/tls-extensiontype-values.xhtml
        private static readonly byte[] s_http11 = {0x68, 0x74, 0x74, 0x70, 0x2f, 0x31, 0x2e, 0x31}; //("http/1.1")
        private static readonly byte[] s_spdy1 = {0x73, 0x70, 0x64, 0x79, 0x2f, 0x31}; //("spdy/1")
        private static readonly byte[] s_spdy2 = {0x73, 0x70, 0x64, 0x79, 0x2f, 0x32}; //("spdy/2")
        private static readonly byte[] s_spdy3 = {0x73, 0x70, 0x64, 0x79, 0x2f, 0x33}; //("spdy/3")
        private static readonly byte[] s_traversalUsingRelaysAroundNat = {0x73, 0x74, 0x75, 0x6E, 0x2E, 0x74, 0x75, 0x72,0x6E}; //("stun.turn")
        private static readonly byte[] s_natDiscoveryUsingSessionTraversalUtilitiesForNat = {0x73, 0x74, 0x75, 0x6E, 0x2E,0x6e, 0x61, 0x74, 0x2d, 0x64, 0x69, 0x73, 0x63, 0x6f, 0x76, 0x65, 0x72, 0x79}; //("stun.nat-discovery")
        private static readonly byte[] s_http2overTls = {0x68, 0x32}; //("h2")
        private static readonly byte[] s_http2overTcp = {0x68, 0x32, 0x63}; //("h2c")
        private static readonly byte[] s_webRtcMediaAndData = {0x77, 0x65, 0x62, 0x72, 0x74, 0x63}; //("webrtc")
        private static readonly byte[] s_confidentialWebRtcMediaAndData = {0x63, 0x2d, 0x77, 0x65, 0x62, 0x72, 0x74, 0x63}; //("c-webrtc")
        private static readonly byte[] s_ftp = {0x66, 0x74, 0x70}; //("ftp")

        private static readonly byte[][] _allProtocols =
        {
            s_http11,
            s_http2overTls,
            s_http2overTcp,
            s_webRtcMediaAndData,
            s_confidentialWebRtcMediaAndData,
            s_ftp,
            s_traversalUsingRelaysAroundNat,
            s_natDiscoveryUsingSessionTraversalUtilitiesForNat,
            s_spdy1,
            s_spdy2,
            s_spdy3,
        };

        private static readonly ApplicationLayerProtocolIds[] s_listOfProtocolIds =(ApplicationLayerProtocolIds[]) Enum.GetValues(typeof(ApplicationLayerProtocolIds));

        //https://tools.ietf.org/html/rfc5246#section-8.1
        //As per the above RFC extensions have size first (2bytes) then the extension type (4bytes) then 2 more bytes
        //for the internal content length, then we have the length of the list in bytes (2 more bytes)
        //So the header before the list is 10bytes
        private const int AlpnHeaderLength = 10;
        //The "content offset" that is where we start counting the length of the content as per above is 4 bytes into
        //the Alpn buffer
        private const int AlpnContentOffset = 4;

        internal enum ApplicaitonProtocolNegotiationExtension
        {
            None = 0,
            Npn,
            Alpn,
        }

        internal enum ApplicationProtocolNegotiationStatus : uint
        {
            None = 0,
            Success,
            SelectedClientOnly,
        }

        internal unsafe static ApplicationLayerProtocolIds FindNegotiatedProtocolSspi(SSPIHandle context)
        {
            ContextApplicationProtocol protoInfo;
            Interop.QueryContextAttributesW(ref context, ContextAttribute.ApplicationProtocol, out protoInfo);

            if (protoInfo.ProtoNegoStatus != ApplicationProtocolNegotiationStatus.Success)
            {
                throw new InvalidOperationException("Could not negotiate a mutal application protocol");
            }
            return GetNegotiatedProtocol(new Span<byte>(protoInfo.ProtocolId, protoInfo.ProtocolIdSize));
        }

        internal static byte[] GetBufferForProtocolId(ApplicationLayerProtocolIds supportedProtocols, bool withAlpnHeader)
        {
            short listLength = 0;
            for (int i = 0; i < s_listOfProtocolIds.Length; i++)
            {
                if (((int) s_listOfProtocolIds.GetValue(i) & (int) supportedProtocols) > 0)
                {
                    listLength += (short) (_allProtocols[i].Length + 1);
                }
            }
            byte[] returnValue;
            if (withAlpnHeader)
            {
                //Now we know the total length of the list lets create our array of protocols and header
                returnValue = new byte[listLength + AlpnHeaderLength];
                var spa = new Span<byte>(returnValue);
                int length = returnValue.Length - AlpnContentOffset;
                spa.Write(length);
                spa = spa.Slice(4); //size of length
                spa.Write(ApplicaitonProtocolNegotiationExtension.Alpn);
                spa = spa.Slice(4); //size of Alpn type 
                spa.Write(listLength);
                spa = spa.Slice(2); //size of the list length 
                ProtocolList(supportedProtocols, spa);
            }
            else
            {
                //No header just make an array of the correct size
                returnValue = new byte[listLength];
                var spa = new Span<byte>(returnValue);
                ProtocolList(supportedProtocols, spa);
            }
            return returnValue;
        }

        private static void ProtocolList(ApplicationLayerProtocolIds supportedProtocols, Span<byte> spa)
        {
            for (int i = 0; i < s_listOfProtocolIds.Length; i++)
            {
                if (((int) s_listOfProtocolIds.GetValue(i) & (int) supportedProtocols) > 0)
                {
                    var value = _allProtocols[i];
                    spa.Write((byte) value.Length);
                    spa = spa.Slice(1);
                    var valueSpan = new Span<byte>(value);
                    valueSpan.CopyTo(spa);
                    spa = spa.Slice(value.Length);
                }
            }
        }

        internal static ApplicationLayerProtocolIds GetNegotiatedProtocol(Span<byte> matchedValue)
        {
            for (int i = 1; i < _allProtocols.Length; i++)
            {
                var proto = _allProtocols[i];
                if (matchedValue.SequenceEqual(proto))
                {
                    return (ApplicationLayerProtocolIds) (1 << (i - 1));
                }
            }
            throw new ArgumentOutOfRangeException($"Could not match the negotiated protocol to a valid protocol");
        }
    }
}