// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Buffers
{
    // These sources are taken from corclr repo (src\mscorlib\src\System\Buffer.cs with x64 path removed)
    // The reason for this duplication is that System.Runtime.dll 4.0.0 did not expose Buffer.MemoryCopy,
    // but we need to make this component work with System.Runtime.dll 4.0.0
    // The methods AreOverlapping and SlowCopyBackwards are not from Buffer.cs. Buffer.cs does an internal CLR call for these.
    static class BufferInternal
    {
        private unsafe static void Memmove(byte* dest, byte* src, uint len)
        {
            if (AreOverlapping(dest, src, len))
            {
                SlowCopyBackwards(dest, src, len);
                return;
            }

            // This is portable version of memcpy. It mirrors what the hand optimized assembly versions of memcpy typically do.
            switch (len)
            {
                case 0:
                    return;
                case 1:
                    *dest = *src;
                    return;
                case 2:
                    *(short*)dest = *(short*)src;
                    return;
                case 3:
                    *(short*)dest = *(short*)src;
                    *(dest + 2) = *(src + 2);
                    return;
                case 4:
                    *(int*)dest = *(int*)src;
                    return;
                case 5:
                    *(int*)dest = *(int*)src;
                    *(dest + 4) = *(src + 4);
                    return;
                case 6:
                    *(int*)dest = *(int*)src;
                    *(short*)(dest + 4) = *(short*)(src + 4);
                    return;
                case 7:
                    *(int*)dest = *(int*)src;
                    *(short*)(dest + 4) = *(short*)(src + 4);
                    *(dest + 6) = *(src + 6);
                    return;
                case 8:
                    *(int*)dest = *(int*)src;
                    *(int*)(dest + 4) = *(int*)(src + 4);
                    return;
                case 9:
                    *(int*)dest = *(int*)src;
                    *(int*)(dest + 4) = *(int*)(src + 4);
                    *(dest + 8) = *(src + 8);
                    return;
                case 10:
                    *(int*)dest = *(int*)src;
                    *(int*)(dest + 4) = *(int*)(src + 4);
                    *(short*)(dest + 8) = *(short*)(src + 8);
                    return;
                case 11:
                    *(int*)dest = *(int*)src;
                    *(int*)(dest + 4) = *(int*)(src + 4);
                    *(short*)(dest + 8) = *(short*)(src + 8);
                    *(dest + 10) = *(src + 10);
                    return;
                case 12:
                    *(int*)dest = *(int*)src;
                    *(int*)(dest + 4) = *(int*)(src + 4);
                    *(int*)(dest + 8) = *(int*)(src + 8);
                    return;
                case 13:
                    *(int*)dest = *(int*)src;
                    *(int*)(dest + 4) = *(int*)(src + 4);
                    *(int*)(dest + 8) = *(int*)(src + 8);
                    *(dest + 12) = *(src + 12);
                    return;
                case 14:
                    *(int*)dest = *(int*)src;
                    *(int*)(dest + 4) = *(int*)(src + 4);
                    *(int*)(dest + 8) = *(int*)(src + 8);
                    *(short*)(dest + 12) = *(short*)(src + 12);
                    return;
                case 15:
                    *(int*)dest = *(int*)src;
                    *(int*)(dest + 4) = *(int*)(src + 4);
                    *(int*)(dest + 8) = *(int*)(src + 8);
                    *(short*)(dest + 12) = *(short*)(src + 12);
                    *(dest + 14) = *(src + 14);
                    return;
                case 16:
                    *(int*)dest = *(int*)src;
                    *(int*)(dest + 4) = *(int*)(src + 4);
                    *(int*)(dest + 8) = *(int*)(src + 8);
                    *(int*)(dest + 12) = *(int*)(src + 12);
                    return;
                default:
                    break;
            }

            if (((int)dest & 3) != 0)
            {
                if (((int)dest & 1) != 0)
                {
                    *dest = *src;
                    src++;
                    dest++;
                    len--;
                    if (((int)dest & 2) == 0)
                        goto Aligned;
                }
                *(short*)dest = *(short*)src;
                src += 2;
                dest += 2;
                len -= 2;
                Aligned:;
            }

            uint count = len / 16;
            while (count > 0)
            {
                ((int*)dest)[0] = ((int*)src)[0];
                ((int*)dest)[1] = ((int*)src)[1];
                ((int*)dest)[2] = ((int*)src)[2];
                ((int*)dest)[3] = ((int*)src)[3];
                dest += 16;
                src += 16;
                count--;
            }

            if ((len & 8) != 0)
            {
                ((int*)dest)[0] = ((int*)src)[0];
                ((int*)dest)[1] = ((int*)src)[1];
                dest += 8;
                src += 8;
            }
            if ((len & 4) != 0)
            {
                ((int*)dest)[0] = ((int*)src)[0];
                dest += 4;
                src += 4;
            }
            if ((len & 2) != 0)
            {
                ((short*)dest)[0] = ((short*)src)[0];
                dest += 2;
                src += 2;
            }
            if ((len & 1) != 0)
                *dest = *src;

            return;
        }

        private static unsafe void SlowCopyBackwards(byte* dest, byte* src, uint len)
        {
            Debug.Assert(len <= int.MaxValue);
            if (len == 0) return;

            for (int i = ((int)len) - 1; i >= 0; i--)
            {
                dest[i] = src[i];
            }
        }

        private static unsafe bool AreOverlapping(byte* dest, byte* src, uint len)
        {
            byte* srcEnd = src + len;
            byte* destEnd = dest + len;
            if (srcEnd >= dest && srcEnd <= destEnd)
            {
                return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void MemoryCopy(void* source, void* destination, int destinationSizeInBytes, int sourceBytesToCopy)
        {
            if (sourceBytesToCopy > destinationSizeInBytes)
            {
                throw new ArgumentOutOfRangeException("sourceBytesToCopy");
            }

            Memmove((byte*)destination, (byte*)source, checked((uint)sourceBytesToCopy));
        }
        
        // TODO: This is naive implementation. Do we have any better native implementation?
        // TODO: Should we start thinking about supporting long lengths?
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool MemoryEqual(void* lhs, int lhsLength, void* rhs, int rhsLength)
        {
            // 1. Fast paths
            if (lhsLength != rhsLength)
                return false;
            if (lhs == rhs)
                return true;

            switch (lhsLength)
            {
                case 0: return true;
                case 1: return *(byte*)lhs == *(byte*)rhs;
                case 2: return *(UInt16*)lhs == *(UInt16*)rhs;
                case 3: return *(UInt16*)lhs == *(UInt16*)rhs &&  *((byte*)lhs + 2) == *((byte*)rhs + 2);
                case 4: return *(UInt32*)lhs == *(UInt32*)rhs;
                case 8: return *(UInt64*)lhs == *(UInt64*)rhs;
            }
            
            // 2. We split bytes into two groups of: (n/4)*4 and n%4 bytes
            
            // TODO: should we play with alignment here?
            UInt32* a = (UInt32*)lhs;
            UInt32* b = (UInt32*)rhs;
            
            // I think those are one processor instruction, so keeping it together
            int count = lhsLength / sizeof(UInt32);
            int remainder = lhsLength % sizeof(UInt32);
            
            // 2.a. We split first group into another two groups to inline comparison by 8 comparisons each
            int count_div_8 = count / 8;
            int count_mod_8 = count % 8;
            
            // 2.b. Do the comparison of first's group first group
            while (count_div_8-- != 0)
            {
                if (*a++ != *b++) return false;
                if (*a++ != *b++) return false;
                if (*a++ != *b++) return false;
                if (*a++ != *b++) return false;
                
                if (*a++ != *b++) return false;
                if (*a++ != *b++) return false;
                if (*a++ != *b++) return false;
                if (*a++ != *b++) return false;
            }
            
            // 2.c. Do the comparison of first's group second group
            switch (count_mod_8)
            {
                case 0: break;
                case 1:
                    if (*a++ != *b++) return false;
                    break;
                case 2:
                    if (*a++ != *b++) return false;
                    if (*a++ != *b++) return false;
                    break;
                case 3:
                    if (*a++ != *b++) return false;
                    if (*a++ != *b++) return false;
                    if (*a++ != *b++) return false;
                    break;
                case 4:
                    if (*a++ != *b++) return false;
                    if (*a++ != *b++) return false;
                    if (*a++ != *b++) return false;
                    if (*a++ != *b++) return false;
                    break;
                case 5:
                    if (*a++ != *b++) return false;
                    if (*a++ != *b++) return false;
                    if (*a++ != *b++) return false;
                    if (*a++ != *b++) return false;
                    
                    if (*a++ != *b++) return false;
                    break;
                case 6:
                    if (*a++ != *b++) return false;
                    if (*a++ != *b++) return false;
                    if (*a++ != *b++) return false;
                    if (*a++ != *b++) return false;
                    
                    if (*a++ != *b++) return false;
                    if (*a++ != *b++) return false;
                    break;
                case 7:
                    if (*a++ != *b++) return false;
                    if (*a++ != *b++) return false;
                    if (*a++ != *b++) return false;
                    if (*a++ != *b++) return false;
                    
                    if (*a++ != *b++) return false;
                    if (*a++ != *b++) return false;
                    if (*a++ != *b++) return false;
                    break;
            }
            
            // 3. Compare bytes in second group
            switch (remainder)
            {
                case 0: return true;
                case 1: return *(byte*)a == *(byte*)b;
                case 2: return *(UInt16*)a == *(UInt16*)b;
                case 3: return *(UInt16*)a == *(UInt16*)b &&  *((byte*)a + 2) == *((byte*)b + 2);
            }
            
            // We actually should never get here
            return false;
        }
    }
}