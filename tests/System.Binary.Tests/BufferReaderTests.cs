// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xunit;
using Microsoft.Xunit.Performance;
using System.Net;

namespace System.Buffers.Tests
{
    public class BufferReaderTests
    {

        [Fact]
        public void BufferReaderRead()
        {
            Assert.True(BitConverter.IsLittleEndian);

            ulong value = 0x8877665544332211; // [11 22 33 44 55 66 77 88]
            Span<byte> span;
            unsafe {
                span = new Span<byte>(&value, 8);
            }

            Assert.Equal<byte>(0x11, span.Read<byte>());
            Assert.True(span.TryRead(out byte byteValue));
            Assert.Equal(0x11, byteValue);

            Assert.Equal<sbyte>(0x11, span.Read<sbyte>());
            Assert.True(span.TryRead(out byte sbyteValue));
            Assert.Equal(0x11, byteValue);

            Assert.Equal<ushort>(0x1122, span.ReadUInt16BigEndian());
            Assert.True(span.TryReadUInt16BigEndian(out ushort ushortValue));
            Assert.Equal(0x1122, ushortValue);

            Assert.Equal<ushort>(0x2211, span.ReadUInt16LittleEndian());
            Assert.True(span.TryReadUInt16LittleEndian(out ushortValue));
            Assert.Equal(0x2211, ushortValue);

            Assert.Equal<short>(0x1122, span.ReadInt16BigEndian());
            Assert.True(span.TryReadInt16BigEndian(out short shortValue));
            Assert.Equal(0x1122, shortValue);

            Assert.Equal<short>(0x2211, span.ReadInt16LittleEndian());
            Assert.True(span.TryReadInt16LittleEndian(out shortValue));
            Assert.Equal(0x2211, ushortValue);

            Assert.Equal<uint>(0x11223344, span.ReadUInt32BigEndian());
            Assert.True(span.TryReadUInt32BigEndian(out uint uintValue));
            Assert.Equal<uint>(0x11223344, uintValue);

            Assert.Equal<uint>(0x44332211, span.ReadUInt32LittleEndian());
            Assert.True(span.TryReadUInt32LittleEndian(out uintValue));
            Assert.Equal<uint>(0x44332211, uintValue);

            Assert.Equal<int>(0x11223344, span.ReadInt32BigEndian());
            Assert.True(span.TryReadInt32BigEndian(out int intValue));
            Assert.Equal<int>(0x11223344, intValue);

            Assert.Equal<int>(0x44332211, span.ReadInt32LittleEndian());
            Assert.True(span.TryReadInt32LittleEndian(out intValue));
            Assert.Equal<int>(0x44332211, intValue);

            Assert.Equal<ulong>(0x1122334455667788, span.ReadUInt64BigEndian());
            Assert.True(span.TryReadUInt64BigEndian(out ulong ulongValue));
            Assert.Equal<ulong>(0x1122334455667788, ulongValue);

            Assert.Equal<ulong>(0x8877665544332211, span.ReadUInt64LittleEndian());
            Assert.True(span.TryReadUInt64LittleEndian(out ulongValue));
            Assert.Equal<ulong>(0x8877665544332211, ulongValue);

            Assert.Equal<long>(0x1122334455667788, span.ReadInt64BigEndian());
            Assert.True(span.TryReadInt64BigEndian(out long longValue));
            Assert.Equal<long>(0x1122334455667788, longValue);

            Assert.Equal<long>(unchecked((long)0x8877665544332211), span.ReadInt64LittleEndian());
            Assert.True(span.TryReadInt64LittleEndian(out longValue));
            Assert.Equal<long>(unchecked((long)0x8877665544332211), longValue);
        }

        [Fact]
        public void BufferWriteReadBigEndianHeterogeneousStruct()
        {
            Assert.True(BitConverter.IsLittleEndian);

            Span<byte> spanBE = new byte[Unsafe.SizeOf<TestStruct>()];

            spanBE.WriteInt16BigEndian(testStruct.S0);
            spanBE.Slice(2).WriteInt32BigEndian(testStruct.I0);
            spanBE.Slice(6).WriteInt64BigEndian(testStruct.L0);
            spanBE.Slice(14).WriteUInt16BigEndian(testStruct.US0);
            spanBE.Slice(16).WriteUInt32BigEndian(testStruct.UI0);
            spanBE.Slice(20).WriteUInt64BigEndian(testStruct.UL0);
            spanBE.Slice(28).WriteInt16BigEndian(testStruct.S1);
            spanBE.Slice(30).WriteInt32BigEndian(testStruct.I1);
            spanBE.Slice(34).WriteInt64BigEndian(testStruct.L1);
            spanBE.Slice(42).WriteUInt16BigEndian(testStruct.US1);
            spanBE.Slice(44).WriteUInt32BigEndian(testStruct.UI1);
            spanBE.Slice(48).WriteUInt64BigEndian(testStruct.UL1);

            var readStruct = new TestStruct
            {
                S0 = spanBE.ReadInt16BigEndian(),
                I0 = spanBE.Slice(2).ReadInt32BigEndian(),
                L0 = spanBE.Slice(6).ReadInt64BigEndian(),
                US0 = spanBE.Slice(14).ReadUInt16BigEndian(),
                UI0 = spanBE.Slice(16).ReadUInt32BigEndian(),
                UL0 = spanBE.Slice(20).ReadUInt64BigEndian(),
                S1 = spanBE.Slice(28).ReadInt16BigEndian(),
                I1 = spanBE.Slice(30).ReadInt32BigEndian(),
                L1 = spanBE.Slice(34).ReadInt64BigEndian(),
                US1 = spanBE.Slice(42).ReadUInt16BigEndian(),
                UI1 = spanBE.Slice(44).ReadUInt32BigEndian(),
                UL1 = spanBE.Slice(48).ReadUInt64BigEndian()
            };

            Assert.Equal(testStruct, readStruct);
        }

        [Fact]
        public void BufferWriteReadLittleEndianHeterogeneousStruct()
        {
            Assert.True(BitConverter.IsLittleEndian);

            Span<byte> spanLE = new byte[Unsafe.SizeOf<TestStruct>()];

            spanLE.WriteInt16LittleEndian(testStruct.S0);
            spanLE.Slice(2).WriteInt32LittleEndian(testStruct.I0);
            spanLE.Slice(6).WriteInt64LittleEndian(testStruct.L0);
            spanLE.Slice(14).WriteUInt16LittleEndian(testStruct.US0);
            spanLE.Slice(16).WriteUInt32LittleEndian(testStruct.UI0);
            spanLE.Slice(20).WriteUInt64LittleEndian(testStruct.UL0);
            spanLE.Slice(28).WriteInt16LittleEndian(testStruct.S1);
            spanLE.Slice(30).WriteInt32LittleEndian(testStruct.I1);
            spanLE.Slice(34).WriteInt64LittleEndian(testStruct.L1);
            spanLE.Slice(42).WriteUInt16LittleEndian(testStruct.US1);
            spanLE.Slice(44).WriteUInt32LittleEndian(testStruct.UI1);
            spanLE.Slice(48).WriteUInt64LittleEndian(testStruct.UL1);

            var readStruct = new TestStruct
            {
                S0 = spanLE.ReadInt16LittleEndian(),
                I0 = spanLE.Slice(2).ReadInt32LittleEndian(),
                L0 = spanLE.Slice(6).ReadInt64LittleEndian(),
                US0 = spanLE.Slice(14).ReadUInt16LittleEndian(),
                UI0 = spanLE.Slice(16).ReadUInt32LittleEndian(),
                UL0 = spanLE.Slice(20).ReadUInt64LittleEndian(),
                S1 = spanLE.Slice(28).ReadInt16LittleEndian(),
                I1 = spanLE.Slice(30).ReadInt32LittleEndian(),
                L1 = spanLE.Slice(34).ReadInt64LittleEndian(),
                US1 = spanLE.Slice(42).ReadUInt16LittleEndian(),
                UI1 = spanLE.Slice(44).ReadUInt32LittleEndian(),
                UL1 = spanLE.Slice(48).ReadUInt64LittleEndian()
            };

            Assert.Equal(testStruct, readStruct);
        }

        [Fact]
        public void ReadingStructByFieldOrReadAndReverse()
        {
            Assert.True(BitConverter.IsLittleEndian);
            Span<byte> spanBE = GetSpanBE();

            TestStructExplicit readStructAndReverse = spanBE.Read<TestStructExplicit>();
            if (BitConverter.IsLittleEndian)
            {
                readStructAndReverse.S0 = readStructAndReverse.S0.Reverse();
                readStructAndReverse.I0 = readStructAndReverse.I0.Reverse();
                readStructAndReverse.L0 = readStructAndReverse.L0.Reverse();
                readStructAndReverse.US0 = readStructAndReverse.US0.Reverse();
                readStructAndReverse.UI0 = readStructAndReverse.UI0.Reverse();
                readStructAndReverse.UL0 = readStructAndReverse.UL0.Reverse();
                readStructAndReverse.S1 = readStructAndReverse.S1.Reverse();
                readStructAndReverse.I1 = readStructAndReverse.I1.Reverse();
                readStructAndReverse.L1 = readStructAndReverse.L1.Reverse();
                readStructAndReverse.US1 = readStructAndReverse.US1.Reverse();
                readStructAndReverse.UI1 = readStructAndReverse.UI1.Reverse();
                readStructAndReverse.UL1 = readStructAndReverse.UL1.Reverse();
            }

            TestStructExplicit readStructByField = new TestStructExplicit
            {
                S0 = spanBE.ReadInt16BigEndian(),
                I0 = spanBE.Slice(2).ReadInt32BigEndian(),
                L0 = spanBE.Slice(6).ReadInt64BigEndian(),
                US0 = spanBE.Slice(14).ReadUInt16BigEndian(),
                UI0 = spanBE.Slice(16).ReadUInt32BigEndian(),
                UL0 = spanBE.Slice(20).ReadUInt64BigEndian(),
                S1 = spanBE.Slice(28).ReadInt16BigEndian(),
                I1 = spanBE.Slice(30).ReadInt32BigEndian(),
                L1 = spanBE.Slice(34).ReadInt64BigEndian(),
                US1 = spanBE.Slice(42).ReadUInt16BigEndian(),
                UI1 = spanBE.Slice(44).ReadUInt32BigEndian(),
                UL1 = spanBE.Slice(48).ReadUInt64BigEndian()
            };

            Assert.Equal(testExplicitStruct, readStructAndReverse);
            Assert.Equal(testExplicitStruct, readStructByField);
        }

        [Fact]
        private static void ReadStructFieldByFieldUsingBitConverter()
        {
            Assert.True(BitConverter.IsLittleEndian);

            Span<byte> spanLE = GetSpanLE();
            byte[] arrayLE = spanLE.ToArray();
            TestStructExplicit readStructLE = new TestStructExplicit
            {
                S0 = BitConverter.ToInt16(arrayLE, 0),
                I0 = BitConverter.ToInt32(arrayLE, 2),
                L0 = BitConverter.ToInt64(arrayLE, 6),
                US0 = BitConverter.ToUInt16(arrayLE, 14),
                UI0 = BitConverter.ToUInt32(arrayLE, 16),
                UL0 = BitConverter.ToUInt64(arrayLE, 20),
                S1 = BitConverter.ToInt16(arrayLE, 28),
                I1 = BitConverter.ToInt32(arrayLE, 30),
                L1 = BitConverter.ToInt64(arrayLE, 34),
                US1 = BitConverter.ToUInt16(arrayLE, 42),
                UI1 = BitConverter.ToUInt32(arrayLE, 44),
                UL1 = BitConverter.ToUInt64(arrayLE, 48),
            };

            Span<byte> spanBE = GetSpanBE();
            byte[] arrayBE = spanBE.ToArray();
            TestStructExplicit readStructBE = new TestStructExplicit
            {
                S0 = BitConverter.ToInt16(arrayBE, 0),
                I0 = BitConverter.ToInt32(arrayBE, 2),
                L0 = BitConverter.ToInt64(arrayBE, 6),
                US0 = BitConverter.ToUInt16(arrayBE, 14),
                UI0 = BitConverter.ToUInt32(arrayBE, 16),
                UL0 = BitConverter.ToUInt64(arrayBE, 20),
                S1 = BitConverter.ToInt16(arrayBE, 28),
                I1 = BitConverter.ToInt32(arrayBE, 30),
                L1 = BitConverter.ToInt64(arrayBE, 34),
                US1 = BitConverter.ToUInt16(arrayBE, 42),
                UI1 = BitConverter.ToUInt32(arrayBE, 44),
                UL1 = BitConverter.ToUInt64(arrayBE, 48),
            };
            if (BitConverter.IsLittleEndian)
            {
                readStructBE.S0 = IPAddress.NetworkToHostOrder(readStructBE.S0);
                readStructBE.I0 = IPAddress.NetworkToHostOrder(readStructBE.I0);
                readStructBE.L0 = IPAddress.NetworkToHostOrder(readStructBE.L0);
                readStructBE.US0 = readStructBE.US0.Reverse();
                readStructBE.UI0 = readStructBE.UI0.Reverse();
                readStructBE.UL0 = readStructBE.UL0.Reverse();
                readStructBE.S1 = IPAddress.NetworkToHostOrder(readStructBE.S1);
                readStructBE.I1 = IPAddress.NetworkToHostOrder(readStructBE.I1);
                readStructBE.L1 = IPAddress.NetworkToHostOrder(readStructBE.L1);
                readStructBE.US1 = readStructBE.US1.Reverse();
                readStructBE.UI1 = readStructBE.UI1.Reverse();
                readStructBE.UL1 = readStructBE.UL1.Reverse();
            }

            Assert.Equal(testExplicitStruct, readStructLE);
            Assert.Equal(testExplicitStruct, readStructBE);
        }

        private const int InnerCount = 100000;

        [Benchmark(InnerIterationCount = InnerCount)]
        private static void ReadStructAndReverseBE()
        {
            Span<byte> spanBE = GetSpanBE();

            TestStructExplicit readStruct = new TestStructExplicit();
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        readStruct = spanBE.Read<TestStructExplicit>();
                        if (BitConverter.IsLittleEndian)
                        {
                            readStruct.S0 = readStruct.S0.Reverse();
                            readStruct.I0 = readStruct.I0.Reverse();
                            readStruct.L0 = readStruct.L0.Reverse();
                            readStruct.US0 = readStruct.US0.Reverse();
                            readStruct.UI0 = readStruct.UI0.Reverse();
                            readStruct.UL0 = readStruct.UL0.Reverse();
                            readStruct.S1 = readStruct.S1.Reverse();
                            readStruct.I1 = readStruct.I1.Reverse();
                            readStruct.L1 = readStruct.L1.Reverse();
                            readStruct.US1 = readStruct.US1.Reverse();
                            readStruct.UI1 = readStruct.UI1.Reverse();
                            readStruct.UL1 = readStruct.UL1.Reverse();
                        }
                    }
                }
            }

            Assert.Equal(testExplicitStruct, readStruct);
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        private static void ReadStructAndReverseLE()
        {
            Span<byte> spanLE = GetSpanLE();

            TestStructExplicit readStruct = new TestStructExplicit();
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        readStruct = spanLE.Read<TestStructExplicit>();
                        if (!BitConverter.IsLittleEndian)
                        {
                            readStruct.S0 = readStruct.S0.Reverse();
                            readStruct.I0 = readStruct.I0.Reverse();
                            readStruct.L0 = readStruct.L0.Reverse();
                            readStruct.US0 = readStruct.US0.Reverse();
                            readStruct.UI0 = readStruct.UI0.Reverse();
                            readStruct.UL0 = readStruct.UL0.Reverse();
                            readStruct.S1 = readStruct.S1.Reverse();
                            readStruct.I1 = readStruct.I1.Reverse();
                            readStruct.L1 = readStruct.L1.Reverse();
                            readStruct.US1 = readStruct.US1.Reverse();
                            readStruct.UI1 = readStruct.UI1.Reverse();
                            readStruct.UL1 = readStruct.UL1.Reverse();
                        }
                    }
                }
            }

            Assert.Equal(testExplicitStruct, readStruct);
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        private static void ReadStructFieldByFieldBE()
        {
            Span<byte> spanBE = GetSpanBE();

            TestStructExplicit readStruct = new TestStructExplicit();
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        readStruct = new TestStructExplicit
                        {
                            S0 = spanBE.ReadInt16BigEndian(),
                            I0 = spanBE.Slice(2).ReadInt32BigEndian(),
                            L0 = spanBE.Slice(6).ReadInt64BigEndian(),
                            US0 = spanBE.Slice(14).ReadUInt16BigEndian(),
                            UI0 = spanBE.Slice(16).ReadUInt32BigEndian(),
                            UL0 = spanBE.Slice(20).ReadUInt64BigEndian(),
                            S1 = spanBE.Slice(28).ReadInt16BigEndian(),
                            I1 = spanBE.Slice(30).ReadInt32BigEndian(),
                            L1 = spanBE.Slice(34).ReadInt64BigEndian(),
                            US1 = spanBE.Slice(42).ReadUInt16BigEndian(),
                            UI1 = spanBE.Slice(44).ReadUInt32BigEndian(),
                            UL1 = spanBE.Slice(48).ReadUInt64BigEndian()
                        };
                    }
                }
            }

            Assert.Equal(testExplicitStruct, readStruct);
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        private static void ReadStructFieldByFieldLE()
        {
            Span<byte> spanLE = GetSpanLE();

            TestStructExplicit readStruct = new TestStructExplicit();
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        readStruct = new TestStructExplicit
                        {
                            S0 = spanLE.ReadInt16LittleEndian(),
                            I0 = spanLE.Slice(2).ReadInt32LittleEndian(),
                            L0 = spanLE.Slice(6).ReadInt64LittleEndian(),
                            US0 = spanLE.Slice(14).ReadUInt16LittleEndian(),
                            UI0 = spanLE.Slice(16).ReadUInt32LittleEndian(),
                            UL0 = spanLE.Slice(20).ReadUInt64LittleEndian(),
                            S1 = spanLE.Slice(28).ReadInt16LittleEndian(),
                            I1 = spanLE.Slice(30).ReadInt32LittleEndian(),
                            L1 = spanLE.Slice(34).ReadInt64LittleEndian(),
                            US1 = spanLE.Slice(42).ReadUInt16LittleEndian(),
                            UI1 = spanLE.Slice(44).ReadUInt32LittleEndian(),
                            UL1 = spanLE.Slice(48).ReadUInt64LittleEndian()
                        };
                    }
                }
            }

            Assert.Equal(testExplicitStruct, readStruct);
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        private static void ReadStructFieldByFieldUsingBitConverterLE()
        {
            Span<byte> spanLE = GetSpanLE();
            byte[] arrayLE = spanLE.ToArray();

            TestStructExplicit readStruct = new TestStructExplicit();
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        readStruct = new TestStructExplicit
                        {
                            S0 = BitConverter.ToInt16(arrayLE, 0),
                            I0 = BitConverter.ToInt32(arrayLE, 2),
                            L0 = BitConverter.ToInt64(arrayLE, 6),
                            US0 = BitConverter.ToUInt16(arrayLE, 14),
                            UI0 = BitConverter.ToUInt32(arrayLE, 16),
                            UL0 = BitConverter.ToUInt64(arrayLE, 20),
                            S1 = BitConverter.ToInt16(arrayLE, 28),
                            I1 = BitConverter.ToInt32(arrayLE, 30),
                            L1 = BitConverter.ToInt64(arrayLE, 34),
                            US1 = BitConverter.ToUInt16(arrayLE, 42),
                            UI1 = BitConverter.ToUInt32(arrayLE, 44),
                            UL1 = BitConverter.ToUInt64(arrayLE, 48),
                        };
                    }
                }
            }

            Assert.Equal(testExplicitStruct, readStruct);
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        private static void ReadStructFieldByFieldUsingBitConverterBE()
        {
            Span<byte> spanBE = GetSpanBE();
            byte[] arrayBE = spanBE.ToArray();

            TestStructExplicit readStruct = new TestStructExplicit();
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        readStruct = new TestStructExplicit
                        {
                            S0 = BitConverter.ToInt16(arrayBE, 0),
                            I0 = BitConverter.ToInt32(arrayBE, 2),
                            L0 = BitConverter.ToInt64(arrayBE, 6),
                            US0 = BitConverter.ToUInt16(arrayBE, 14),
                            UI0 = BitConverter.ToUInt32(arrayBE, 16),
                            UL0 = BitConverter.ToUInt64(arrayBE, 20),
                            S1 = BitConverter.ToInt16(arrayBE, 28),
                            I1 = BitConverter.ToInt32(arrayBE, 30),
                            L1 = BitConverter.ToInt64(arrayBE, 34),
                            US1 = BitConverter.ToUInt16(arrayBE, 42),
                            UI1 = BitConverter.ToUInt32(arrayBE, 44),
                            UL1 = BitConverter.ToUInt64(arrayBE, 48),
                        };
                        if (BitConverter.IsLittleEndian)
                        {
                            readStruct.S0 = readStruct.S0.Reverse();
                            readStruct.I0 = readStruct.I0.Reverse();
                            readStruct.L0 = readStruct.L0.Reverse();
                            readStruct.US0 = readStruct.US0.Reverse();
                            readStruct.UI0 = readStruct.UI0.Reverse();
                            readStruct.UL0 = readStruct.UL0.Reverse();
                            readStruct.S1 = readStruct.S1.Reverse();
                            readStruct.I1 = readStruct.I1.Reverse();
                            readStruct.L1 = readStruct.L1.Reverse();
                            readStruct.US1 = readStruct.US1.Reverse();
                            readStruct.UI1 = readStruct.UI1.Reverse();
                            readStruct.UL1 = readStruct.UL1.Reverse();
                        }
                    }
                }
            }

            Assert.Equal(testExplicitStruct, readStruct);
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        private static void MeasureReverse()
        {
            var myArray = new int[1000];
            
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        for (int j = 0; j < myArray.Length; j++)
                        {
                            myArray[j] = myArray[j].Reverse();
                        }
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        private static void MeasureReverseUsingNtoH()
        {
            var myArray = new int[1000];

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        for (int j = 0; j < myArray.Length; j++)
                        {
                            myArray[j] = IPAddress.NetworkToHostOrder(myArray[j]);
                        }
                    }
                }
            }
        }

        private static TestStructExplicit testExplicitStruct = new TestStructExplicit
        {
            S0 = short.MaxValue,
            I0 = int.MaxValue,
            L0 = long.MaxValue,
            US0 = ushort.MaxValue,
            UI0 = uint.MaxValue,
            UL0 = ulong.MaxValue,
            S1 = short.MinValue,
            I1 = int.MinValue,
            L1 = long.MinValue,
            US1 = ushort.MinValue,
            UI1 = uint.MinValue,
            UL1 = ulong.MinValue
        };

        private static TestStruct testStruct = new TestStruct
        {
            S0 = short.MaxValue,
            I0 = int.MaxValue,
            L0 = long.MaxValue,
            US0 = ushort.MaxValue,
            UI0 = uint.MaxValue,
            UL0 = ulong.MaxValue,
            S1 = short.MinValue,
            I1 = int.MinValue,
            L1 = long.MinValue,
            US1 = ushort.MinValue,
            UI1 = uint.MinValue,
            UL1 = ulong.MinValue
        };


        private static Span<byte> GetSpanBE()
        {
            Span<byte> spanBE = new byte[Unsafe.SizeOf<TestStructExplicit>()];

            spanBE.WriteInt16BigEndian(testExplicitStruct.S0);
            spanBE.Slice(2).WriteInt32BigEndian(testExplicitStruct.I0);
            spanBE.Slice(6).WriteInt64BigEndian(testExplicitStruct.L0);
            spanBE.Slice(14).WriteUInt16BigEndian(testExplicitStruct.US0);
            spanBE.Slice(16).WriteUInt32BigEndian(testExplicitStruct.UI0);
            spanBE.Slice(20).WriteUInt64BigEndian(testExplicitStruct.UL0);
            spanBE.Slice(28).WriteInt16BigEndian(testExplicitStruct.S1);
            spanBE.Slice(30).WriteInt32BigEndian(testExplicitStruct.I1);
            spanBE.Slice(34).WriteInt64BigEndian(testExplicitStruct.L1);
            spanBE.Slice(42).WriteUInt16BigEndian(testExplicitStruct.US1);
            spanBE.Slice(44).WriteUInt32BigEndian(testExplicitStruct.UI1);
            spanBE.Slice(48).WriteUInt64BigEndian(testExplicitStruct.UL1);

            Assert.Equal(56, spanBE.Length);
            return spanBE;
        }

        private static Span<byte> GetSpanLE()
        {
            Span<byte> spanLE = new byte[Unsafe.SizeOf<TestStructExplicit>()];

            spanLE.WriteInt16LittleEndian(testExplicitStruct.S0);
            spanLE.Slice(2).WriteInt32LittleEndian(testExplicitStruct.I0);
            spanLE.Slice(6).WriteInt64LittleEndian(testExplicitStruct.L0);
            spanLE.Slice(14).WriteUInt16LittleEndian(testExplicitStruct.US0);
            spanLE.Slice(16).WriteUInt32LittleEndian(testExplicitStruct.UI0);
            spanLE.Slice(20).WriteUInt64LittleEndian(testExplicitStruct.UL0);
            spanLE.Slice(28).WriteInt16LittleEndian(testExplicitStruct.S1);
            spanLE.Slice(30).WriteInt32LittleEndian(testExplicitStruct.I1);
            spanLE.Slice(34).WriteInt64LittleEndian(testExplicitStruct.L1);
            spanLE.Slice(42).WriteUInt16LittleEndian(testExplicitStruct.US1);
            spanLE.Slice(44).WriteUInt32LittleEndian(testExplicitStruct.UI1);
            spanLE.Slice(48).WriteUInt64LittleEndian(testExplicitStruct.UL1);

            Assert.Equal(56, spanLE.Length);
            return spanLE;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct TestStruct
        {
            public short S0;
            public int I0;
            public long L0;
            public ushort US0;
            public uint UI0;
            public ulong UL0;
            public short S1;
            public int I1;
            public long L1;
            public ushort US1;
            public uint UI1;
            public ulong UL1;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct TestStructExplicit
        {
            [FieldOffset(0)]
            public short S0;
            [FieldOffset(2)]
            public int I0;
            [FieldOffset(6)]
            public long L0;
            [FieldOffset(14)]
            public ushort US0;
            [FieldOffset(16)]
            public uint UI0;
            [FieldOffset(20)]
            public ulong UL0;
            [FieldOffset(28)]
            public short S1;
            [FieldOffset(30)]
            public int I1;
            [FieldOffset(34)]
            public long L1;
            [FieldOffset(42)]
            public ushort US1;
            [FieldOffset(44)]
            public uint UI1;
            [FieldOffset(48)]
            public ulong UL1;
        }
    }
}
