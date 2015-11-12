// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Xunit;
using Microsoft.Xunit.Performance;

namespace System.Collections.Tests
{
    public class Perf_MVD
    {
        public static MultiValueDictionary<int, int> CreateMVD(int size)
        {
            MultiValueDictionary<int, int> mvd = new MultiValueDictionary<int, int>();
            Random rand = new Random(11231992);

            while (mvd.Count < size)
                mvd.Add(rand.Next(), rand.Next());

            return mvd;
        }

        [Benchmark]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(100000)]
        public void Add(int size)
        {
            MultiValueDictionary<int, int> dict = CreateMVD(size);
            foreach (var iteration in Benchmark.Iterations)
            {
                MultiValueDictionary<int, int> copyDict = new MultiValueDictionary<int, int>(dict);
                using (iteration.StartMeasurement())
                    for (int i = 0; i <= 20000; i++)
                    {
                        copyDict.Add(i * 10 + 1, 0); copyDict.Add(i * 10 + 2, 0); copyDict.Add(i * 10 + 3, 0);
                        copyDict.Add(i * 10 + 4, 0); copyDict.Add(i * 10 + 5, 0); copyDict.Add(i * 10 + 6, 0);
                        copyDict.Add(i * 10 + 7, 0); copyDict.Add(i * 10 + 8, 0); copyDict.Add(i * 10 + 9, 0);
                    }
            }
        }

        [Benchmark]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(100000)]
        public void AddRange(int size)
        {
            List<int> values = new List<int>();
            for (int i = 0; i < size; i++)
                values.Add(i);

            foreach (var iteration in Benchmark.Iterations)
            {
                MultiValueDictionary<int, int> empty = new MultiValueDictionary<int, int>();
                using (iteration.StartMeasurement())
                    for (int i = 0; i <= 20000; i++)
                        empty.AddRange(i, values);
            }
        }

        [Benchmark]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(100000)]
        public void Remove(int size)
        {
            MultiValueDictionary<int, int> dict = new MultiValueDictionary<int, int>();
            for (int i = 0; i < size; i++)
                for (int j = 0; j < 100; j++)
                    dict.Add(i, j);

            foreach (var iteration in Benchmark.Iterations)
            {
                MultiValueDictionary<int, int> copyDict = new MultiValueDictionary<int, int>(dict);
                using (iteration.StartMeasurement())
                    for (int i = 0; i <= size; i++)
                    {
                        copyDict.Remove(i);
                    }
            }
        }

        [Benchmark]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(100000)]
        public void RemoveItem(int size)
        {
            MultiValueDictionary<int, int> dict = new MultiValueDictionary<int, int>();
            for (int i = 0; i < size; i++)
                for (int j = 0; j < 100; j++)
                    dict.Add(i, j);

            foreach (var iteration in Benchmark.Iterations)
            {
                MultiValueDictionary<int, int> copyDict = new MultiValueDictionary<int, int>(dict);
                using (iteration.StartMeasurement())
                    for (int i = 0; i <= size; i++)
                        for (int j = 0; j <= 100; j++)
                            copyDict.RemoveItem(i, j);
            }
        }

        [Benchmark]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(100000)]
        public void Clear(int size)
        {
            MultiValueDictionary<int, int> dict = CreateMVD(size);
            foreach (var iteration in Benchmark.Iterations)
            {
                MultiValueDictionary<int, int> copyDict = new MultiValueDictionary<int, int>(dict);
                using (iteration.StartMeasurement())
                    copyDict.Clear();
            }
        }

        [Benchmark]
        public void ctor()
        {
            foreach (var iteration in Benchmark.Iterations)
                using (iteration.StartMeasurement())
                    for (int i = 0; i <= 20000; i++)
                    {
                        new MultiValueDictionary<int, string>(); new MultiValueDictionary<int, string>(); new MultiValueDictionary<int, string>();
                        new MultiValueDictionary<int, string>(); new MultiValueDictionary<int, string>(); new MultiValueDictionary<int, string>();
                        new MultiValueDictionary<int, string>(); new MultiValueDictionary<int, string>(); new MultiValueDictionary<int, string>();
                    }
        }

        [Benchmark]
        [InlineData(0)]
        [InlineData(1024)]
        [InlineData(4096)]
        [InlineData(16384)]
        public void ctor_int(int size)
        {
            foreach (var iteration in Benchmark.Iterations)
                using (iteration.StartMeasurement())
                    for (int i = 0; i <= 500; i++)
                    {
                        new MultiValueDictionary<int, string>(size); new MultiValueDictionary<int, string>(size); new MultiValueDictionary<int, string>(size);
                        new MultiValueDictionary<int, string>(size); new MultiValueDictionary<int, string>(size); new MultiValueDictionary<int, string>(size);
                        new MultiValueDictionary<int, string>(size); new MultiValueDictionary<int, string>(size); new MultiValueDictionary<int, string>(size);
                    }
        }

        [Benchmark]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(100000)]
        public void GetItem(int size)
        {
            MultiValueDictionary<int, int> dict = CreateMVD(size);

            // Setup
            IReadOnlyCollection<int> retrieved;
            for (int i = 1; i <= 9; i++)
                dict.Add(i, 0);

            // Actual perf testing
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    for (int i = 0; i <= 10000; i++)
                    {
                        retrieved = dict[1]; retrieved = dict[2]; retrieved = dict[3];
                        retrieved = dict[4]; retrieved = dict[5]; retrieved = dict[6];
                        retrieved = dict[7]; retrieved = dict[8]; retrieved = dict[9];
                    }
            }
        }

        [Benchmark]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(100000)]
        public void GetKeys(int size)
        {
            MultiValueDictionary<int, int> dict = CreateMVD(size);
            IEnumerable<int> result;
            foreach (var iteration in Benchmark.Iterations)
                using (iteration.StartMeasurement())
                    for (int i = 0; i <= 20000; i++)
                    {
                        result = dict.Keys; result = dict.Keys; result = dict.Keys;
                        result = dict.Keys; result = dict.Keys; result = dict.Keys;
                        result = dict.Keys; result = dict.Keys; result = dict.Keys;
                    }
        }

        [Benchmark]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(100000)]
        public void TryGetValue(int size)
        {
            MultiValueDictionary<int, int> dict = CreateMVD(size);
            // Setup
            IReadOnlyCollection<int> retrieved;
            Random rand = new Random(837322);
            int key = rand.Next(0, 400000);
            dict.Add(key, 12);

            // Actual perf testing
            foreach (var iteration in Benchmark.Iterations)
                using (iteration.StartMeasurement())
                    for (int i = 0; i <= 1000; i++)
                    {
                        dict.TryGetValue(key, out retrieved); dict.TryGetValue(key, out retrieved);
                        dict.TryGetValue(key, out retrieved); dict.TryGetValue(key, out retrieved);
                        dict.TryGetValue(key, out retrieved); dict.TryGetValue(key, out retrieved);
                        dict.TryGetValue(key, out retrieved); dict.TryGetValue(key, out retrieved);
                    }
        }

        [Benchmark]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(100000)]
        public void ContainsKey(int size)
        {
            MultiValueDictionary<int, int> dict = CreateMVD(size);

            // Setup
            Random rand = new Random(837322);
            int key = rand.Next(0, 400000);
            dict.Add(key, 12);

            // Actual perf testing
            foreach (var iteration in Benchmark.Iterations)
                using (iteration.StartMeasurement())
                    for (int i = 0; i <= 10000; i++)
                    {
                        dict.ContainsKey(key); dict.ContainsKey(key); dict.ContainsKey(key);
                        dict.ContainsKey(key); dict.ContainsKey(key); dict.ContainsKey(key);
                        dict.ContainsKey(key); dict.ContainsKey(key); dict.ContainsKey(key);
                        dict.ContainsKey(key); dict.ContainsKey(key); dict.ContainsKey(key);
                    }
        }

        [Benchmark]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(100000)]
        public void ContainsValue(int size)
        {
            MultiValueDictionary<int, int> dict = CreateMVD(size);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    for (int i = 0; i <= 20000; i++)
                    {
                        dict.ContainsValue(i); dict.ContainsValue(i); dict.ContainsValue(i);
                        dict.ContainsValue(i); dict.ContainsValue(i); dict.ContainsValue(i);
                        dict.ContainsValue(i); dict.ContainsValue(i); dict.ContainsValue(i);
                    }
            }
        }
    }
}
