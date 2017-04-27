// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.IO.Pipelines.Networking.Windows.RIO.Internal
{
    public static class CpuInfo
    {
        const string Kernel_32 = "Kernel32";
        const int ErrorInsufficientBuffer = 0x7A;
        private static readonly int SLPISize = Marshal.SizeOf<SystemLogicalProcessorInformation>();

        private static int _numaNodeCount;
        private static ulong _physicalCoreMask;
        private static ulong _secondaryCoreMask;
        private static int _physicalCoreCount;
        private static int _logicalProcessorCount;
        private static int _processorL1CacheCount;
        private static int _processorL2CacheCount;
        private static int _processorL3CacheCount;
        private static int _processorPackageCount;

        public static int PhysicalCoreCount => _physicalCoreCount != 0 ? _physicalCoreCount : GetPhysicalProcessorCount();
        public static int LogicalProcessorCount => _logicalProcessorCount != 0 ? _logicalProcessorCount : GetLogicalProcessorCount();
        public static ulong PhysicalCoreMask => _physicalCoreMask != 0 ? _physicalCoreMask : GetPhysicalProcessorMask();
        public static ulong SecondaryCoreMask => _secondaryCoreMask != 0 ? _secondaryCoreMask : GetSecondaryProcessorMask();

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int GetPhysicalProcessorCount()
        {
            QueryCpuInfo();
            return _physicalCoreCount;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int GetLogicalProcessorCount()
        {
            QueryCpuInfo();
            return _logicalProcessorCount;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ulong GetPhysicalProcessorMask()
        {
            QueryCpuInfo();
            return _physicalCoreMask;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ulong GetSecondaryProcessorMask()
        {
            QueryCpuInfo();
            return _secondaryCoreMask;
        }

        private static int CountSetBits(ulong bitMask)
        {
            const int lshift = sizeof(ulong) * 8 - 1;
            var bitSetCount = 0;

            for (var i = 0; i <= lshift; i++)
            {
                var bitTest = 1UL << i;
                bitSetCount += ((bitMask & bitTest) == bitTest ? 1 : 0);
            }

            return bitSetCount;
        }

        private static ulong FirstSetBit(ulong bitMask)
        {
            const int lshift = sizeof(ulong) * 8 - 1;

            for (var i = 0; i <= lshift; i++)
            {
                var bitTest = 1UL << i;

                if ((bitMask & bitTest) == bitTest)
                {
                    return bitTest;
                }
            }

            return 0;
        }

        private static ulong SecondSetBit(ulong bitMask)
        {
            const int lshift = sizeof(ulong) * 8 - 1;

            var isFirst = true;
            for (var i = 0; i <= lshift; i++)
            {
                var bitTest = 1UL << i;

                if ((bitMask & bitTest) == bitTest)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                        continue;
                    }

                    return bitTest;
                }
            }

            return 0;
        }

        private static unsafe void QueryCpuInfo()
        {
            uint returnLength = 1;

            while (true)
            {
                var buffer = stackalloc byte[(int)returnLength];
                if (!GetLogicalProcessorInformation(new IntPtr(buffer), ref returnLength))
                {
                    var error = GetLastError();
                    if (error == 0 || error == ErrorInsufficientBuffer)
                    {
                        continue;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }

                var byteOffset = 0;
                while (byteOffset + SLPISize <= returnLength)
                {
                    var slpi = Unsafe.Read<SystemLogicalProcessorInformation>(buffer + byteOffset);

                    switch (slpi.Relationship)
                    {
                        case LogicalProcessorRelationship.RelationNumaNode:
                            // Non-NUMA systems report a single record of this type.
                            _numaNodeCount++;
                            break;

                        case LogicalProcessorRelationship.RelationProcessorCore:
                            _physicalCoreCount++;

                            // A hyperthreaded core supplies more than one logical processor.
                            var mask = slpi.ProcessorMask;
                            var bits = CountSetBits(mask);
                            _logicalProcessorCount += bits;

                            if (bits == 1)
                            {
                                _physicalCoreMask |= mask;
                                _secondaryCoreMask |= mask;
                            }
                            else
                            {
                                _physicalCoreMask |= FirstSetBit(mask);
                                _secondaryCoreMask |= SecondSetBit(mask);
                            }

                            break;

                        case LogicalProcessorRelationship.RelationCache:
                            // Cache data is in ptr->Cache, one CACHE_DESCRIPTOR structure for each cache. 
                            var cache = slpi.Info.Cache;
                            if (cache.Level == 1)
                            {
                                _processorL1CacheCount++;
                            }
                            else if (cache.Level == 2)
                            {
                                _processorL2CacheCount++;
                            }
                            else if (cache.Level == 3)
                            {
                                _processorL3CacheCount++;
                            }
                            break;

                        case LogicalProcessorRelationship.RelationProcessorPackage:
                            // Logical processors share a physical package.
                            _processorPackageCount++;
                            break;

                        default:
                            throw new Exception("Error: Unsupported LOGICAL_PROCESSOR_RELATIONSHIP value.");
                    }
                    byteOffset += SLPISize;
                }

                break;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SystemLogicalProcessorInformation
        {
            public ulong ProcessorMask;
            public LogicalProcessorRelationship Relationship;
            public ProcessorInfo Info;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct ProcessorInfo
        {
            [FieldOffset(0)]
            public ProcessorCore ProcessorCore;
            [FieldOffset(0)]
            public Node NumaNode;
            [FieldOffset(0)]
            public CacheDescriptor Cache;
            [FieldOffset(0)]
            public Reserved Reserved;
        }

        public enum LogicalProcessorRelationship
        {
            RelationProcessorCore,
            RelationNumaNode,
            RelationCache,
            RelationProcessorPackage
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ProcessorCore
        {
            public byte Flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Reserved
        {
            public ulong Reserved0;
            public ulong Reserved1;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Node
        {
            public uint NodeNumber;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CacheDescriptor
        {
            public byte Level;
            public byte Associativity;
            public ushort LineSize;
            public uint Size;
            public ProcessorCacheType Type;
        }

        public enum ProcessorCacheType
        {
            CacheUnified,
            CacheInstruction,
            CacheData,
            CacheTrace
        }

        [DllImport(Kernel_32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetLogicalProcessorInformation(IntPtr Buffer, ref uint returnedLength);

        [DllImport(Kernel_32, SetLastError = true)]
        private static extern long GetLastError();
    }
}
