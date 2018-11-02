﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace Microsoft.Experimental.Collections.Benchmarks
{
    public class RefDictionaryKNucleotide
    {
        [Params(250_000, 2_500_000)]
        public int Size { get; set; }
        [Benchmark]
        public void Hack()
        {
            new KNucleotideHack().Main(Size);
        }
        [Benchmark(Baseline = true)]
        public void Dictionary()
        {
            new KNucleotideDictionary().Main(Size);
        }
        [Benchmark]
        public void RefDictionary()
        {
            new KNucleotideRefDictionary().Main(Size);
        }
    }

    public class KNucleotideHack
    {
        class Incrementor : IDisposable
        {
            static FieldInfo bucketsField = typeof(Dictionary<long, int>).GetField(
                "_buckets", BindingFlags.NonPublic | BindingFlags.Instance);
            static FieldInfo entriesField = typeof(Dictionary<long, int>).GetField(
                "_entries", BindingFlags.NonPublic | BindingFlags.Instance);
            static FieldInfo countField = typeof(Dictionary<long, int>).GetField(
                "_count", BindingFlags.NonPublic | BindingFlags.Instance);
            static MethodInfo resizeMethod = typeof(Dictionary<long, int>).GetMethod(
                "Resize", BindingFlags.NonPublic | BindingFlags.Instance,
                null, new Type[0], null);
            readonly Dictionary<long, int> dictionary;
            int[] buckets;
            IntPtr entries;
            GCHandle handle;
            int count;

            public Incrementor(Dictionary<long, int> d)
            {
                dictionary = d;
                Sync();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            void Sync()
            {
                buckets = (int[])bucketsField.GetValue(dictionary);
                handle = GCHandle.Alloc(entriesField.GetValue(dictionary),
                            GCHandleType.Pinned);
                entries = handle.AddrOfPinnedObject();
                count = (int)countField.GetValue(dictionary);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Increment(long key)
            {
                int hashCode = key.GetHashCode() & 0x7FFFFFFF;
                int targetBucket = hashCode % buckets.Length;
                for (int i = buckets[targetBucket] - 1; (uint)i < (uint)buckets.Length;
                    i = Marshal.ReadInt32(entries, i * 24 + 4))
                {
                    if (Marshal.ReadInt64(entries, i * 24 + 8) == key)
                    {
                        Marshal.WriteInt32(entries, i * 24 + 16,
                            Marshal.ReadInt32(entries, i * 24 + 16) + 1);
                        return;
                    }
                }
                if (count == buckets.Length)
                {
                    Dispose();
                    resizeMethod.Invoke(dictionary, null);
                    Sync();
                    targetBucket = hashCode % buckets.Length;
                }
                int index = count++;
                Marshal.WriteInt32(entries, index * 24, hashCode);
                Marshal.WriteInt32(entries, index * 24 + 4, buckets[targetBucket] - 1);
                Marshal.WriteInt64(entries, index * 24 + 8, key);
                Marshal.WriteInt32(entries, index * 24 + 16, 1);
                buckets[targetBucket] = index + 1;
            }

            public void Dispose()
            {
                countField.SetValue(dictionary, count);
                handle.Free();
            }
        }

        private const int BLOCK_SIZE = 1024 * 1024 * 8;
        private List<byte[]> threeBlocks = new List<byte[]>();
        private int threeStart, threeEnd;
        private byte[] tonum = new byte[256];
        private char[] tochar = new char[] { 'A', 'C', 'G', 'T' };

        private static int Read(Stream stream, byte[] buffer, int offset, int count)
        {
            var bytesRead = stream.Read(buffer, offset, count);
            return bytesRead == count ? offset + count
                 : bytesRead == 0 ? offset
                 : Read(stream, buffer, offset + bytesRead, count - bytesRead);
        }

        private static int Find(byte[] buffer, byte[] toFind, int i, ref int matchIndex)
        {
            if (matchIndex == 0)
            {
                i = Array.IndexOf(buffer, toFind[0], i);
                if (i == -1) return -1;
                matchIndex = 1;
                return Find(buffer, toFind, i + 1, ref matchIndex);
            }
            else
            {
                int bl = buffer.Length, fl = toFind.Length;
                while (i < bl && matchIndex < fl)
                {
                    if (buffer[i++] != toFind[matchIndex++])
                    {
                        matchIndex = 0;
                        return Find(buffer, toFind, i, ref matchIndex);
                    }
                }
                return matchIndex == fl ? i : -1;
            }
        }

        private void LoadThreeData(int size)
        {
            var file = "Benchmarks.Microsoft.Experimental.Collections.RefDictionaryKNucleotideFiles.input" + size + ".txt";
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(file))
            {

                // find three sequence
                int matchIndex = 0;
                var toFind = new[] { (byte)'>', (byte)'T', (byte)'H', (byte)'R', (byte)'E', (byte)'E' };
                var buffer = new byte[BLOCK_SIZE];
                do
                {
                    threeEnd = Read(stream, buffer, 0, BLOCK_SIZE);
                    threeStart = Find(buffer, toFind, 0, ref matchIndex);
                } while (threeStart == -1);

                // Skip to end of line
                matchIndex = 0;
                toFind = new[] { (byte)'\n' };
                threeStart = Find(buffer, toFind, threeStart, ref matchIndex);
                while (threeStart == -1)
                {
                    threeEnd = Read(stream, buffer, 0, BLOCK_SIZE);
                    threeStart = Find(buffer, toFind, 0, ref matchIndex);
                }
                threeBlocks.Add(buffer);

                if (threeEnd != BLOCK_SIZE) // Needs to be at least 2 blocks
                {
                    var bytes = threeBlocks[0];
                    for (int i = threeEnd; i < bytes.Length; i++)
                        bytes[i] = 255;
                    threeEnd = 0;
                    threeBlocks.Add(Array.Empty<byte>());
                    return;
                }

                // find next seq or end of input
                matchIndex = 0;
                toFind = new[] { (byte)'>' };
                threeEnd = Find(buffer, toFind, threeStart, ref matchIndex);
                while (threeEnd == -1)
                {
                    buffer = new byte[BLOCK_SIZE];
                    var bytesRead = Read(stream, buffer, 0, BLOCK_SIZE);
                    threeEnd = bytesRead == BLOCK_SIZE ? Find(buffer, toFind, 0, ref matchIndex)
                                : bytesRead;
                    threeBlocks.Add(buffer);
                }

                if (threeStart + 18 > BLOCK_SIZE) // Key needs to be in the first block
                {
                    byte[] block0 = threeBlocks[0], block1 = threeBlocks[1];
                    Buffer.BlockCopy(block0, threeStart, block0, threeStart - 18, BLOCK_SIZE - threeStart);
                    Buffer.BlockCopy(block1, 0, block0, BLOCK_SIZE - 18, 18);
                    for (int i = 0; i < 18; i++) block1[i] = 255;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Check(Incrementor inc, ref long rollingKey, byte nb, long mask)
        {
            if (nb == 255) return;
            rollingKey = ((rollingKey & mask) << 2) | nb;
            inc.Increment(rollingKey);
        }

        private Task<string> Count(int l, long mask, Func<Dictionary<long, int>, string> summary)
        {
            return Task.Run(() =>
            {
                long rollingKey = 0;
                var firstBlock = threeBlocks[0];
                var start = threeStart;
                while (--l > 0) rollingKey = (rollingKey << 2) | firstBlock[start++];
                var dict = new Dictionary<long, int>(1024);
                using (var incrementor = new Incrementor(dict))
                {
                    for (int i = start; i < firstBlock.Length; i++)
                        Check(incrementor, ref rollingKey, firstBlock[i], mask);

                    int lastBlockId = threeBlocks.Count - 1;
                    for (int bl = 1; bl < lastBlockId; bl++)
                    {
                        var bytes = threeBlocks[bl];
                        for (int i = 0; i < bytes.Length; i++)
                            Check(incrementor, ref rollingKey, bytes[i], mask);
                    }

                    var lastBlock = threeBlocks[lastBlockId];
                    for (int i = 0; i < threeEnd; i++)
                        Check(incrementor, ref rollingKey, lastBlock[i], mask);
                }
                return summary(dict);
            });
        }

        private string WriteFrequencies(Dictionary<long, int> freq, int fragmentLength)
        {
            var sb = new StringBuilder();
            double percent = 100.0 / freq.Values.Sum();
            foreach (var kv in freq.OrderByDescending(i => i.Value))
            {
                var keyChars = new char[fragmentLength];
                var key = kv.Key;
                for (int i = keyChars.Length - 1; i >= 0; --i)
                {
                    keyChars[i] = tochar[key & 0x3];
                    key >>= 2;
                }
                sb.Append(keyChars);
                sb.Append(" ");
                sb.AppendLine((kv.Value * percent).ToString("F3"));
            }
            return sb.ToString();
        }

        private string WriteCount(Dictionary<long, int> dictionary, string fragment)
        {
            long key = 0;
            for (int i = 0; i < fragment.Length; ++i)
                key = (key << 2) | tonum[fragment[i]];
            var n = dictionary.TryGetValue(key, out var v) ? v : 0;
            return string.Concat(n.ToString(), "\t", fragment);
        }

        public string[] Main(int size)
        {
            tonum['c'] = 1; tonum['C'] = 1;
            tonum['g'] = 2; tonum['G'] = 2;
            tonum['t'] = 3; tonum['T'] = 3;
            tonum['\n'] = 255; tonum['>'] = 255; tonum[255] = 255;

            LoadThreeData(size);

            Parallel.ForEach(threeBlocks, bytes =>
            {
                for (int i = 0; i < bytes.Length; i++)
                    bytes[i] = tonum[bytes[i]];
            });

            var task12 = Count(12, 8388607, d => WriteCount(d, "GGTATTTTAATT"));
            var task18 = Count(18, 34359738367, d => WriteCount(d, "GGTATTTTAATTTATAGT"));
            var task6 = Count(6, 0b1111111111, d => WriteCount(d, "GGTATT"));
            var task1 = Count(1, 0, d => WriteFrequencies(d, 1));
            var task2 = Count(2, 0b11, d => WriteFrequencies(d, 2));
            var task3 = Count(3, 0b1111, d => WriteCount(d, "GGT"));
            var task4 = Count(4, 0b111111, d => WriteCount(d, "GGTA"));

            return new[] {
                task1.Result,
                task2.Result,
                task3.Result,
                task4.Result,
                task6.Result,
                task12.Result,
                task18.Result,
            };
        }
    }

    public class KNucleotideDictionary
    {
        private const int BLOCK_SIZE = 1024 * 1024 * 8;
        private List<byte[]> threeBlocks = new List<byte[]>();
        private int threeStart, threeEnd;
        private byte[] tonum = new byte[256];
        private char[] tochar = new char[] { 'A', 'C', 'G', 'T' };

        private static int Read(Stream stream, byte[] buffer, int offset, int count)
        {
            var bytesRead = stream.Read(buffer, offset, count);
            return bytesRead == count ? offset + count
                 : bytesRead == 0 ? offset
                 : Read(stream, buffer, offset + bytesRead, count - bytesRead);
        }

        private static int Find(byte[] buffer, byte[] toFind, int i, ref int matchIndex)
        {
            if (matchIndex == 0)
            {
                i = Array.IndexOf(buffer, toFind[0], i);
                if (i == -1) return -1;
                matchIndex = 1;
                return Find(buffer, toFind, i + 1, ref matchIndex);
            }
            else
            {
                int bl = buffer.Length, fl = toFind.Length;
                while (i < bl && matchIndex < fl)
                {
                    if (buffer[i++] != toFind[matchIndex++])
                    {
                        matchIndex = 0;
                        return Find(buffer, toFind, i, ref matchIndex);
                    }
                }
                return matchIndex == fl ? i : -1;
            }
        }

        private void LoadThreeData(int size)
        {
            var file = "Benchmarks.Microsoft.Experimental.Collections.RefDictionaryKNucleotideFiles.input" + size + ".txt";
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(file))
            {

                // find three sequence
                int matchIndex = 0;
                var toFind = new[] { (byte)'>', (byte)'T', (byte)'H', (byte)'R', (byte)'E', (byte)'E' };
                var buffer = new byte[BLOCK_SIZE];
                do
                {
                    threeEnd = Read(stream, buffer, 0, BLOCK_SIZE);
                    threeStart = Find(buffer, toFind, 0, ref matchIndex);
                } while (threeStart == -1);

                // Skip to end of line
                matchIndex = 0;
                toFind = new[] { (byte)'\n' };
                threeStart = Find(buffer, toFind, threeStart, ref matchIndex);
                while (threeStart == -1)
                {
                    threeEnd = Read(stream, buffer, 0, BLOCK_SIZE);
                    threeStart = Find(buffer, toFind, 0, ref matchIndex);
                }
                threeBlocks.Add(buffer);

                if (threeEnd != BLOCK_SIZE) // Needs to be at least 2 blocks
                {
                    var bytes = threeBlocks[0];
                    for (int i = threeEnd; i < bytes.Length; i++)
                        bytes[i] = 255;
                    threeEnd = 0;
                    threeBlocks.Add(Array.Empty<byte>());
                    return;
                }

                // find next seq or end of input
                matchIndex = 0;
                toFind = new[] { (byte)'>' };
                threeEnd = Find(buffer, toFind, threeStart, ref matchIndex);
                while (threeEnd == -1)
                {
                    buffer = new byte[BLOCK_SIZE];
                    var bytesRead = Read(stream, buffer, 0, BLOCK_SIZE);
                    threeEnd = bytesRead == BLOCK_SIZE ? Find(buffer, toFind, 0, ref matchIndex)
                                : bytesRead;
                    threeBlocks.Add(buffer);
                }

                if (threeStart + 18 > BLOCK_SIZE) // Key needs to be in the first block
                {
                    byte[] block0 = threeBlocks[0], block1 = threeBlocks[1];
                    Buffer.BlockCopy(block0, threeStart, block0, threeStart - 18, BLOCK_SIZE - threeStart);
                    Buffer.BlockCopy(block1, 0, block0, BLOCK_SIZE - 18, 18);
                    for (int i = 0; i < 18; i++) block1[i] = 255;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Check(Dictionary<long, int> dict, ref long rollingKey, byte nb, long mask)
        {
            if (nb == 255) return;
            rollingKey = ((rollingKey & mask) << 2) | nb;

            if (dict.TryGetValue(rollingKey, out var v))
                dict[rollingKey] = v + 1;
            else
                dict[rollingKey] = 1;
        }

        private Task<string> Count(int l, long mask, Func<Dictionary<long, int>, string> summary)
        {
            return Task.Run(() =>
            {
                long rollingKey = 0;
                var firstBlock = threeBlocks[0];
                var start = threeStart;
                while (--l > 0) rollingKey = (rollingKey << 2) | firstBlock[start++];
                var dict = new Dictionary<long, int>(1024);
                for (int i = start; i < firstBlock.Length; i++)
                    Check(dict, ref rollingKey, firstBlock[i], mask);

                int lastBlockId = threeBlocks.Count - 1;
                for (int bl = 1; bl < lastBlockId; bl++)
                {
                    var bytes = threeBlocks[bl];
                    for (int i = 0; i < bytes.Length; i++)
                        Check(dict, ref rollingKey, bytes[i], mask);
                }

                var lastBlock = threeBlocks[lastBlockId];
                for (int i = 0; i < threeEnd; i++)
                    Check(dict, ref rollingKey, lastBlock[i], mask);
                return summary(dict);
            });
        }

        private string writeFrequencies(Dictionary<long, int> freq, int fragmentLength)
        {
            var sb = new StringBuilder();
            double percent = 100.0 / freq.Values.Sum();
            foreach (var kv in freq.OrderByDescending(i => i.Value))
            {
                var keyChars = new char[fragmentLength];
                var key = kv.Key;
                for (int i = keyChars.Length - 1; i >= 0; --i)
                {
                    keyChars[i] = tochar[key & 0x3];
                    key >>= 2;
                }
                sb.Append(keyChars);
                sb.Append(" ");
                sb.AppendLine((kv.Value * percent).ToString("F3"));
            }
            return sb.ToString();
        }

        private string WriteCount(Dictionary<long, int> dictionary, string fragment)
        {
            long key = 0;
            for (int i = 0; i < fragment.Length; ++i)
                key = (key << 2) | tonum[fragment[i]];
            var n = dictionary.TryGetValue(key, out var v) ? v : 0;
            return string.Concat(n.ToString(), "\t", fragment);
        }

        public string[] Main(int size)
        {
            tonum['c'] = 1; tonum['C'] = 1;
            tonum['g'] = 2; tonum['G'] = 2;
            tonum['t'] = 3; tonum['T'] = 3;
            tonum['\n'] = 255; tonum['>'] = 255; tonum[255] = 255;

            LoadThreeData(size);

            Parallel.ForEach(threeBlocks, bytes =>
            {
                for (int i = 0; i < bytes.Length; i++)
                    bytes[i] = tonum[bytes[i]];
            });

            var task12 = Count(12, 8388607, d => WriteCount(d, "GGTATTTTAATT"));
            var task18 = Count(18, 34359738367, d => WriteCount(d, "GGTATTTTAATTTATAGT"));
            var task6 = Count(6, 0b1111111111, d => WriteCount(d, "GGTATT"));
            var task1 = Count(1, 0, d => writeFrequencies(d, 1));
            var task2 = Count(2, 0b11, d => writeFrequencies(d, 2));
            var task3 = Count(3, 0b1111, d => WriteCount(d, "GGT"));
            var task4 = Count(4, 0b111111, d => WriteCount(d, "GGTA"));

            return new[] {
                task1.Result,
                task2.Result,
                task3.Result,
                task4.Result,
                task6.Result,
                task12.Result,
                task18.Result,
            };
        }
    }

    public class KNucleotideRefDictionary
    {
        private const int BLOCK_SIZE = 1024 * 1024 * 8;
        private List<byte[]> threeBlocks = new List<byte[]>();
        private int threeStart, threeEnd;
        private byte[] tonum = new byte[256];
        private char[] tochar = new char[] { 'A', 'C', 'G', 'T' };

        private static int Read(Stream stream, byte[] buffer, int offset, int count)
        {
            var bytesRead = stream.Read(buffer, offset, count);
            return bytesRead == count ? offset + count
                 : bytesRead == 0 ? offset
                 : Read(stream, buffer, offset + bytesRead, count - bytesRead);
        }

        private static int Find(byte[] buffer, byte[] toFind, int i, ref int matchIndex)
        {
            if (matchIndex == 0)
            {
                i = Array.IndexOf(buffer, toFind[0], i);
                if (i == -1) return -1;
                matchIndex = 1;
                return Find(buffer, toFind, i + 1, ref matchIndex);
            }
            else
            {
                int bl = buffer.Length, fl = toFind.Length;
                while (i < bl && matchIndex < fl)
                {
                    if (buffer[i++] != toFind[matchIndex++])
                    {
                        matchIndex = 0;
                        return Find(buffer, toFind, i, ref matchIndex);
                    }
                }
                return matchIndex == fl ? i : -1;
            }
        }

        private void LoadThreeData(int size)
        {
            var file = "Benchmarks.Microsoft.Experimental.Collections.RefDictionaryKNucleotideFiles.input" + size + ".txt";
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(file))
            {

                // find three sequence
                int matchIndex = 0;
                var toFind = new[] { (byte)'>', (byte)'T', (byte)'H', (byte)'R', (byte)'E', (byte)'E' };
                var buffer = new byte[BLOCK_SIZE];
                do
                {
                    threeEnd = Read(stream, buffer, 0, BLOCK_SIZE);
                    threeStart = Find(buffer, toFind, 0, ref matchIndex);
                } while (threeStart == -1);

                // Skip to end of line
                matchIndex = 0;
                toFind = new[] { (byte)'\n' };
                threeStart = Find(buffer, toFind, threeStart, ref matchIndex);
                while (threeStart == -1)
                {
                    threeEnd = Read(stream, buffer, 0, BLOCK_SIZE);
                    threeStart = Find(buffer, toFind, 0, ref matchIndex);
                }
                threeBlocks.Add(buffer);

                if (threeEnd != BLOCK_SIZE) // Needs to be at least 2 blocks
                {
                    var bytes = threeBlocks[0];
                    for (int i = threeEnd; i < bytes.Length; i++)
                        bytes[i] = 255;
                    threeEnd = 0;
                    threeBlocks.Add(Array.Empty<byte>());
                    return;
                }

                // find next seq or end of input
                matchIndex = 0;
                toFind = new[] { (byte)'>' };
                threeEnd = Find(buffer, toFind, threeStart, ref matchIndex);
                while (threeEnd == -1)
                {
                    buffer = new byte[BLOCK_SIZE];
                    var bytesRead = Read(stream, buffer, 0, BLOCK_SIZE);
                    threeEnd = bytesRead == BLOCK_SIZE ? Find(buffer, toFind, 0, ref matchIndex)
                                : bytesRead;
                    threeBlocks.Add(buffer);
                }

                if (threeStart + 18 > BLOCK_SIZE) // Key needs to be in the first block
                {
                    byte[] block0 = threeBlocks[0], block1 = threeBlocks[1];
                    Buffer.BlockCopy(block0, threeStart, block0, threeStart - 18, BLOCK_SIZE - threeStart);
                    Buffer.BlockCopy(block1, 0, block0, BLOCK_SIZE - 18, 18);
                    for (int i = 0; i < 18; i++) block1[i] = 255;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Check(RefDictionary<long, int> dict, ref long rollingKey, byte nb, long mask)
        {
            if (nb == 255) return;
            rollingKey = ((rollingKey & mask) << 2) | nb;
            dict[rollingKey]++;
        }

        private Task<string> Count(int l, long mask, Func<RefDictionary<long, int>, string> summary)
        {
            return Task.Run(() =>
            {
                long rollingKey = 0;
                var firstBlock = threeBlocks[0];
                var start = threeStart;
                while (--l > 0) rollingKey = (rollingKey << 2) | firstBlock[start++];
                var dict = new RefDictionary<long, int>(1024);
                
                for (int i = start; i < firstBlock.Length; i++)
                    Check(dict, ref rollingKey, firstBlock[i], mask);

                int lastBlockId = threeBlocks.Count - 1;
                for (int bl = 1; bl < lastBlockId; bl++)
                {
                    var bytes = threeBlocks[bl];
                    for (int i = 0; i < bytes.Length; i++)
                        Check(dict, ref rollingKey, bytes[i], mask);
                }

                var lastBlock = threeBlocks[lastBlockId];
                for (int i = 0; i < threeEnd; i++)
                    Check(dict, ref rollingKey, lastBlock[i], mask);
                
                return summary(dict);
            });
        }

        private string WriteFrequencies(RefDictionary<long, int> freq, int fragmentLength)
        {
            var sb = new StringBuilder();
            double percent = 100.0 / freq.Sum(i => i.Value);
            foreach (var kv in freq.OrderByDescending(i => i.Value))
            {
                var keyChars = new char[fragmentLength];
                var key = kv.Key;
                for (int i = keyChars.Length - 1; i >= 0; --i)
                {
                    keyChars[i] = tochar[key & 0x3];
                    key >>= 2;
                }
                sb.Append(keyChars);
                sb.Append(" ");
                sb.AppendLine((kv.Value * percent).ToString("F3"));
            }
            return sb.ToString();
        }

        private string WriteCount(RefDictionary<long, int> dictionary, string fragment)
        {
            long key = 0;
            for (int i = 0; i < fragment.Length; ++i)
                key = (key << 2) | tonum[fragment[i]];
            var v = dictionary.GetValueOrDefault(key);
            return string.Concat(v.ToString(), "\t", fragment);
        }

        public string[] Main(int size)
        {
            tonum['c'] = 1; tonum['C'] = 1;
            tonum['g'] = 2; tonum['G'] = 2;
            tonum['t'] = 3; tonum['T'] = 3;
            tonum['\n'] = 255; tonum['>'] = 255; tonum[255] = 255;

            LoadThreeData(size);

            Parallel.ForEach(threeBlocks, bytes =>
            {
                for (int i = 0; i < bytes.Length; i++)
                    bytes[i] = tonum[bytes[i]];
            });

            var task12 = Count(12, 8388607, d => WriteCount(d, "GGTATTTTAATT"));
            var task18 = Count(18, 34359738367, d => WriteCount(d, "GGTATTTTAATTTATAGT"));
            var task6 = Count(6, 0b1111111111, d => WriteCount(d, "GGTATT"));
            var task1 = Count(1, 0, d => WriteFrequencies(d, 1));
            var task2 = Count(2, 0b11, d => WriteFrequencies(d, 2));
            var task3 = Count(3, 0b1111, d => WriteCount(d, "GGT"));
            var task4 = Count(4, 0b111111, d => WriteCount(d, "GGTA"));

            return new[] {
                task1.Result,
                task2.Result,
                task3.Result,
                task4.Result,
                task6.Result,
                task12.Result,
                task18.Result,
            };
        }
    }
}
