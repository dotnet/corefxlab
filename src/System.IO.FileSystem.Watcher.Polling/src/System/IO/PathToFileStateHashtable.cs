// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.IO.FileSystem
{
    // this is a quick an dirty hashtable optimized for the PollingWatcher
    // It allows mutating struct values (FileState) contained in the hashtable
    // It allows both string and char* (filenames from WIN32_FIND_DATA) lookups
    // It has optimized Equals and GetHasCode
    // It implements removals by marking values as "removed" (Path==null) and then garbage collecting them when table is resized
    class PathToFileStateHashtable
    {
        TraceSource _trace;
        int _count;
        int _nextValuesIndex = 1; // the first Values slot is reserved so that default(Bucket) knows that it is not pointing to any value.
        public FileState[] Values { get; private set; }
        private Bucket[] Buckets;

        public PathToFileStateHashtable(TraceSource trace, int capacity = 4)
        {
            _trace = trace;
            Values = new FileState[capacity];

            // +1 is needed so that there are always more buckets than values.
            // this is so that unsuccesful search always terminates (as it terminates at an empty bucket)
            // note that today the "+1" is not strictly required, as one Values slot is reserved, but I am future proofing here
            Buckets = new Bucket[GetPrime(capacity + 1)];
        }

        public int Count
        {
            get
            {
                return _count;
            }
        }

        public void Add(string directory, string file, FileState value)
        {
            if(_nextValuesIndex >= Values.Length) // Resize
            {
                Resize();
            }

            Values[_nextValuesIndex] = value;
            var bucket = ComputeBucket(file);

            while (true)
            {
                if (Buckets[bucket].IsEmpty)
                {
                    Buckets[bucket] = new Bucket(directory, file, _nextValuesIndex);
                    _count++;
                    _nextValuesIndex++;
                    return;
                }
                bucket = NextCandidateBucket(bucket);
            }
        }

        public void Remove(string directory, string file)
        {
            var index = IndexOf(directory, file);
            Debug.Assert(index != -1, "this should never happen");

            Values[index].Path = null;
            Values[index].Directory = null;
            _count--;
        }

        public int IndexOf(string directory, string file)
        {
            int bucket = ComputeBucket(file);
            while (true)
            {
                var valueIndex = Buckets[bucket].ValuesIndex;
                if (valueIndex == 0)
                {
                    return -1; // not found
                }

                if (Equal(Buckets[bucket].Key, directory, file))
                {
                    if (Values[valueIndex].Path != null) return valueIndex;
                }
                bucket = NextCandidateBucket(bucket);
            }
        }

        public unsafe int IndexOf(string directory, char* file)
        {
            int bucket = ComputeBucket(file);
            while (true)
            {
                var valueIndex = Buckets[bucket].ValuesIndex;
                if (valueIndex == 0)
                {
                    return -1; // not found
                }

                if (Equal(Buckets[bucket].Key, directory, file))
                {
                    if (Values[valueIndex].Path != null) return valueIndex;
                }
                bucket = NextCandidateBucket(bucket);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int NextCandidateBucket(int bucket)
        {
            bucket++;
            if (bucket >= Buckets.Length)
            {
                bucket = 0;
            }
            return bucket;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe bool Equal(FullPath fullPath, string directory, char* file)
        {

            if (!String.Equals(fullPath.Directory, directory, StringComparison.Ordinal))
            {
                return false;
            }

            int index;
            for (index = 0; index < fullPath.File.Length; index++)
            {
                if (file[index] == 0) return false;
                if (file[index] != fullPath.File[index]) return false;
            }
            if (file[index] != 0) return false;
            return true;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe bool Equal(FullPath fullPath, string directory, string file)
        {
            if (!String.Equals(fullPath.Directory, directory, StringComparison.Ordinal))
            {
                return false;
            }

            if (!String.Equals(fullPath.File, file, StringComparison.Ordinal))
            {
                return false;
            }
            return true;
        }

        private unsafe int GetHashCode(string path)
        {
            int code = 0;
            for (int index = 0; index < path.Length; index++)
            {
                char next = path[index];
                code |= next;
                code <<= 8;
                if (index > 8) break;
            }
            return code;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe int GetHashCode(char* nullTerminatedString)
        {
            int code = 0;
            int index = 0;
            while (true)
            {
                char next = *nullTerminatedString;
                if (next == 0)
                {
                    break;
                }
                nullTerminatedString++;
                code |= next;
                code <<= 8;
                if (index > 8) break;
                index++;
            }
            return code;
        }

        private int ComputeBucket(string file)
        {
            var hash = GetHashCode(file);
            if (hash == Int32.MinValue) hash = Int32.MaxValue;

            var bucket = Math.Abs(hash) % Buckets.Length;
            return bucket;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe int ComputeBucket(char* file)
        {
            var hash = GetHashCode(file);
            if (hash == Int32.MinValue) hash = Int32.MaxValue;

            var bucket = Math.Abs(hash) % Buckets.Length;
            return bucket;
        }

        private void Resize()
        {
            // this is because sometimes we just need to garbade collect instead of increase size
            var newSize = Math.Max(_count * 2, 4);

            if (_trace.Switch.ShouldTrace(TraceEventType.Verbose))
            {
                _trace.TraceEvent(TraceEventType.Verbose, 5, "Resizing hashtable from {0} to {1}", Values.Length, newSize);
            }

            var bigger = new PathToFileStateHashtable(_trace, newSize);

            foreach (var existingValue in this)
            {
                bigger.Add(existingValue.Directory, existingValue.Path, existingValue);
            }
            Values = bigger.Values;
            Buckets = bigger.Buckets;
            this._nextValuesIndex = bigger._nextValuesIndex;
            this._count = bigger._count;
        }

        private static readonly int[] primes = {
            3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
            1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
            17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
            187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
            1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369};

        private static bool IsPrime(int candidate)
        {
            if ((candidate & 1) != 0)
            {
                int limit = (int)Math.Sqrt(candidate);
                for (int divisor = 3; divisor <= limit; divisor += 2)
                {
                    if ((candidate % divisor) == 0)
                        return false;
                }
                return true;
            }
            return (candidate == 2);
        }

        private static int GetPrime(int min)
        {
            for (int i = 0; i < primes.Length; i++)
            {
                int prime = primes[i];
                if (prime >= min) return prime;
            }

            //outside of our predefined table. 
            //compute the hard way. 
            for (int i = (min | 1); i < Int32.MaxValue; i += 2)
            {
                if (IsPrime(i))
                    return i;
            }
            return min;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public struct Enumerator
        {
            PathToFileStateHashtable _table;
            int _index;

            public Enumerator(PathToFileStateHashtable table)
            {
                _table = table;
                _index = 0;
            }

            public bool MoveNext()
            {
                do
                {
                    _index++;
                    if (_index > _table._nextValuesIndex || _index >= _table.Values.Length) { return false; }
                }
                while (_table.Values[_index].Path == null);

                return true;
            }

            public FileState Current
            {
                get { return _table.Values[_index]; }
            }
        }

        public override string ToString()
        {
            return _count.ToString();
        }

        struct Bucket
        {
            public FullPath Key;
            public int ValuesIndex;

            public Bucket(string directory, string file, int valueIndex)
            {
                Key.Directory = directory;
                Key.File = file;
                ValuesIndex = valueIndex;
            }
            public bool IsEmpty { get { return ValuesIndex == 0; } }

            public override string ToString()
            {
                if (IsEmpty) return "empty";
                return Key.ToString();
            }
        }

        struct FullPath
        {
            public string Directory;
            public string File;

            public override string ToString()
            {
                return File;
            }
        }
    }
}
