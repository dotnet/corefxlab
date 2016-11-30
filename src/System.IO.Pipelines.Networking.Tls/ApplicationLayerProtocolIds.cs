using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Networking.Tls
{
    //Ordering of these protocol id's is not defined in any specification.
    //The ordering provided has been done by estimated order of importance/use
    //at the time of writing, this is because our search is in a for loop and coming
    //across the answer first will be quickest
    [Flags]
    public enum ApplicationLayerProtocolIds
    {
        None = 0,
        Http11 = 0x00001,
        Http2OverTls = 0x00002,
        Http2OverTcp = 0x00004,
        WebRtcMediaAndData = 0x0008,
        ConfidentialWebRtcMediaAndData = 0x0010,
        Ftp = 0x0020,
        TraversalUsingRelaysAroundNat = 0x0040,
        NatDiscoveryUsingSessionTraversalUtilitiesforNat = 0x0080,
        Spdy1 = 0x0100,
        Spdy2 = 0x0200,
        Spdy3 = 0x0400,
    }
}

