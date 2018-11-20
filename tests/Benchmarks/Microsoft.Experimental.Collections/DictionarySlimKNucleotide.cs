// Licensed to the .NET Foundation under one or more agreements.
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

namespace Microsoft.Collections.Extensions.Benchmarks
{
    [GcConcurrent(true)]
    [GcServer(true)]
    public class DictionarySlimKNucleotide
    {
        [Params(250_000)] // , 2_500_000, 25_000_000)] // uncomment for slower benchmarks
        public int Size { get; set; }

        public string Filename => Path.Combine(Path.GetTempPath(),
            "corefxlab_dictionaryslim_input" + Size + ".txt");

        [GlobalSetup]
        public void CreateValuesList()
        {
            if (!File.Exists(Filename))
            {
                using (var fs = File.Create(Filename))
                {
                    DictionarySlimFasta.Create(Size, fs);
                }
            }
        }

        string[] _results;

        [GlobalCleanup]
        public void CheckResults()
        {
            var result = string.Join("j", _results.Select(i => i.Replace('\n', 'n').Replace("\r", "")));
            var expected = Size == 250_000 ? "A 30.298nT 30.157nC 19.793nG 19.752njAA 9.177nTA 9.137nAT 9.136nTT 9.094nAC 6.000nCA 5.999nGA 5.986nAG 5.985nTC 5.970nCT 5.970nGT 5.957nTG 5.956nCC 3.915nCG 3.910nGC 3.908nGG 3.902nj14717	GGTj4463	GGTAj472	GGTATTj9	GGTATTTTAATTj9	GGTATTTTAATTTATAGT"
                         : Size == 2_500_000 ? "A 30.297nT 30.151nC 19.798nG 19.755njAA 9.177nTA 9.133nAT 9.131nTT 9.091nCA 6.002nAC 6.001nAG 5.987nGA 5.984nCT 5.971nTC 5.971nGT 5.957nTG 5.956nCC 3.917nGC 3.910nCG 3.909nGG 3.903nj147166	GGTj44658	GGTAj4736	GGTATTj89	GGTATTTTAATTj89	GGTATTTTAATTTATAGT"
                         : Size == 25_000_000 ? "A 30.295nT 30.151nC 19.800nG 19.754njAA 9.177nTA 9.132nAT 9.131nTT 9.091nCA 6.002nAC 6.001nAG 5.987nGA 5.984nCT 5.971nTC 5.971nGT 5.957nTG 5.956nCC 3.917nGC 3.911nCG 3.909nGG 3.902nj1471758	GGTj446535	GGTAj47336	GGTATTj893	GGTATTTTAATTj893	GGTATTTTAATTTATAGT"
                         : "?";
            if (result != expected) throw new Exception("Incorrect result: " + result);
        }

        [Benchmark(Baseline = true)]
        public void Hack9() => _results = new KNucleotideHack9().Main(Size, Filename);
        [Benchmark]
        public void Dictionary() => _results = new KNucleotideDictionary().Main(Size, Filename);
        [Benchmark]
        public void DictionarySlim() => _results = new KNucleotideDictionarySlim().Main(Size, Filename);
    }

    public class KNucleotideHack9
    {
        // Incrementor uses reflection to access methods and data internal to
        // Dictionary to provide an Increment method.
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
            public unsafe void Increment(long key)
            {
                int hashCode = key.GetHashCode() & 0x7FFFFFFF;
                int * p;
                int i;
                int targetBucket = hashCode % buckets.Length;
                for (i = buckets[targetBucket] - 1; (uint)i < (uint)buckets.Length;
                        i = *(p+1))
                {
                    p = (int *)entries + i * 6;
                    if (*((long *)(p+2)) == key)
                    {
                        (*(p+4))++;
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
                i = count++;
                p = (int *)entries + i * 6;
                *p = hashCode;
                *(p+1) = buckets[targetBucket] - 1;
                *((long *)(p+2)) = key;
                *(p+4) = 1;
                buckets[targetBucket] = i + 1;
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

        private void LoadThreeData(int size, string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
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

        public string[] Main(int size, string filename)
        {
            tonum['c'] = 1; tonum['C'] = 1;
            tonum['g'] = 2; tonum['G'] = 2;
            tonum['t'] = 3; tonum['T'] = 3;
            tonum['\n'] = 255; tonum['>'] = 255; tonum[255] = 255;

            LoadThreeData(size, filename);

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

        private void LoadThreeData(int size, string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
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

        public string[] Main(int size, string filename)
        {
            tonum['c'] = 1; tonum['C'] = 1;
            tonum['g'] = 2; tonum['G'] = 2;
            tonum['t'] = 3; tonum['T'] = 3;
            tonum['\n'] = 255; tonum['>'] = 255; tonum[255] = 255;

            LoadThreeData(size, filename);

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

    public class KNucleotideDictionarySlim
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

        private void LoadThreeData(int size, string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
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
        private static void Check(DictionarySlim<long, int> dict, ref long rollingKey, byte nb, long mask)
        {
            if (nb == 255) return;
            rollingKey = ((rollingKey & mask) << 2) | nb;
            dict.GetOrAddValueRef(rollingKey)++;
        }

        private Task<string> Count(int l, long mask, Func<DictionarySlim<long, int>, string> summary)
        {
            return Task.Run(() =>
            {
                long rollingKey = 0;
                var firstBlock = threeBlocks[0];
                var start = threeStart;
                while (--l > 0) rollingKey = (rollingKey << 2) | firstBlock[start++];
                var dict = new DictionarySlim<long, int>(1024);

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

        private string WriteFrequencies(DictionarySlim<long, int> freq, int fragmentLength)
        {
            var sb = new StringBuilder();
            double percent = 100.0 / freq.Select(x => x.Value).Sum();
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

        private string WriteCount(DictionarySlim<long, int> dictionary, string fragment)
        {
            long key = 0;
            for (int i = 0; i < fragment.Length; ++i)
                key = (key << 2) | tonum[fragment[i]];
            dictionary.TryGetValue(key, out int v);
            return string.Concat(v.ToString(), "\t", fragment);
        }

        public string[] Main(int size, string filename)
        {
            tonum['c'] = 1; tonum['C'] = 1;
            tonum['g'] = 2; tonum['G'] = 2;
            tonum['t'] = 3; tonum['T'] = 3;
            tonum['\n'] = 255; tonum['>'] = 255; tonum[255] = 255;

            LoadThreeData(size, filename);

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
