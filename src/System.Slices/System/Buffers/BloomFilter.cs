// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;


namespace System.Runtime {
    
    // Naive Bloom filter
    // Should really adapt, and alter mapping so false conflicts go away.
    // This should be sufficient for initial prototyping.
    public class BloomFilter {

        private byte [] bits;

        public BloomFilter() {
            bits = new byte[0x100 >> 3];
        }

        public void Clear() {
            for (int i = 0; i < bits.Length; i++) {
                bits[i] = 0;
            }
        } 

        public void Add(Object o) {
            var i = o.GetHashCode();
            for (int k = 0; k < 4; k++) {
                int ibit  = i & 0x7;
                int ibyte = (i & 0xff) >> 3;
                bits[ibyte] |= (byte)(1 << ibit);
                i >>= 8; 
            }
        }

        public bool DoesNotContain(Object o) {
            var i = o.GetHashCode();
            for (int k = 0; k < 4; k++) {
                int ibit  = i & 0x7;
                int ibyte = (i & 0xff) >> 3;
                if((bits[ibyte] & (byte)(1 << ibit)) == 0) return true;
                i >>= 8; 
            }
            return false;
        }
    }
}