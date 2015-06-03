// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.IO.FileSystem
{
    // this is a quick an dirty hashtable optimized for the PollingWatcher
    // It allows mutating struct values (FileState) contained in the hashtable
    // It allows both string and char* (filenames from WIN32_FIND_DATA) lookups
    // It implements removals by marking values as "removed" (Path==null) and then garbage collecting them when table is resized
    // TODO: the algorithm to compute the next bucket should be improved. It currently just increments the bucket index
    // TODO: hashcode generation should be improved too
    class PathToFileStateHashtable
    {
        int _index = 0;
        public FileState[] Values { get; private set; }
        private Bucket[] Buckets;

        public PathToFileStateHashtable(int capacity = 4)
        {
            Values = new FileState[capacity];
            Buckets = new Bucket[GetPrime(capacity)];
        }

        public static readonly int[] primes = {
            3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
            1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
            17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
            187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
            1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369};

        public static bool IsPrime(int candidate)
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

        public static int GetPrime(int min)
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

        public void Add(string directory, string file, FileState value)
        {
            _index++; 
            if(_index >= Values.Length) // Resize
            {
                var bigger = new PathToFileStateHashtable(Values.Length * 2);
                foreach(var existingValue in this)
                {
                    bigger.Add(existingValue.Directory, existingValue.Path, existingValue);
                }
                Values = bigger.Values;
                Buckets = bigger.Buckets;
            }

            Values[_index] = value;
            var bucket = ComputeBucket(file);

            while (true)
            {
                if (Buckets[bucket].IsEmpty)
                {
                    Buckets[bucket] = new Bucket(directory, file, _index);
                    return;
                }
                bucket = NextBucket(bucket);
            }
        }

        public int IndexOf(string directory, string file)
        {
            int bucket = ComputeBucket(file);
            while (true)
            {
                if (Buckets[bucket].ValuesIndex == 0)
                {
                    return -1; // not found
                }

                FullPath fullPath = Buckets[bucket].Key;

                if (string.Equals(fullPath.File, file, StringComparison.Ordinal) && string.Equals(fullPath.Directory, directory, StringComparison.Ordinal))
                {
                    return Buckets[bucket].ValuesIndex;
                }
                bucket = NextBucket(bucket);
            }
        }

        public unsafe int IndexOf(string directory, char* file)
        {
            int bucket = ComputeBucket(file);
            while (true)
            {
                if (Buckets[bucket].ValuesIndex == 0)
                {
                    return -1; // not found
                }

                if (Equal(Buckets[bucket].Key, directory, file))
                {
                    return Buckets[bucket].ValuesIndex;
                }
                bucket = NextBucket(bucket);
            }
        }

        public void Remove(string directory, string file)
        {
            var index = IndexOf(directory, file);
            Values[index].Path = null;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        internal struct Enumerator
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
                    if (_index >= _table._index) { return false; }
                }
                while (_table.Values[_index].Path == null);

                return true;
            }

            public FileState Current
            {
                get { return _table.Values[_index]; }
            }
        }

        private unsafe int GetHashCode(string path)
        {
            int code = 0;            
            for(int index=0; index<path.Length; index++) { 
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe bool Equal(FullPath fullPath, string directory, char* file)
        {
            if(!String.Equals(fullPath.Directory, directory, StringComparison.Ordinal)) {
                return false;
            }
            int index;
            for (index = 0; index < fullPath.File.Length; index++)
            {
                if (file[index] != fullPath.File[index]) return false; //TODO: this can read past the buffer. Needs to be fixed
            }
            if (file[index] != 0) return false;
            return true;
            
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int NextBucket(int bucket)
        {
            bucket++;
            if (bucket >= Buckets.Length)
            {
                bucket = 0;
            }
            return bucket;
        }

        private int ComputeBucket(string file)
        {
            var hash = GetHashCode(file);
            var bucket = Math.Abs(hash) % Buckets.Length;
            return bucket;
        }

        private unsafe int ComputeBucket(char* file)
        {
            var hash = GetHashCode(file);
            var bucket = Math.Abs(hash) % Buckets.Length;
            return bucket;
        }

        public override string ToString()
        {
            return _index.ToString();
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
        }

        struct FullPath
        {
            public string Directory;
            public string File;
        }
    }
}
