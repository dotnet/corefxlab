// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
            Buckets = new Bucket[capacity * 2];
        }

        public void Add(string key, FileState value)
        {
            _index++; 
            if(_index >= Values.Length) // Resize
            {
                var bigger = new PathToFileStateHashtable(Values.Length * 2);
                foreach(var existingValue in this)
                {
                    bigger.Add(existingValue.Path, existingValue);
                }
                Values = bigger.Values;
                Buckets = bigger.Buckets;
            }

            Values[_index] = value;
            var bucket = ComputeBucket(key);

            while (true)
            {
                if (Buckets[bucket].IsEmpty)
                {
                    Buckets[bucket] = new Bucket(key, _index);
                    return;
                }
                bucket = NextBucket(bucket);
            }
        }

        public int IndexOf(string key)
        {
            int bucket = ComputeBucket(key);
            while (true)
            {
                if (Buckets[bucket].ValuesIndex == 0)
                {
                    return -1; // not found
                }

                if (Buckets[bucket].Key.Equals(key))
                {
                    return Buckets[bucket].ValuesIndex;
                }
                bucket = NextBucket(bucket);
            }
        }

        public unsafe int IndexOf(ref WIN32_FIND_DATAW file)
        {
            int bucket = ComputeBucket(ref file);
            while (true)
            {
                if (Buckets[bucket].ValuesIndex == 0)
                {
                    return -1; // not found
                }

                if (Equal(Buckets[bucket].Key, ref file))
                {
                    return Buckets[bucket].ValuesIndex;
                }
                bucket = NextBucket(bucket);
            }
        }

        public void Remove(string key)
        {
            var index = IndexOf(key);
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

        private unsafe int GetHashCode(string str)
        {
            int code = 0;            
            for(int index=0; index<str.Length; index++) { 
                char next = str[index];
                code |= next;
                code <<= 8;
            }
            return code;
        }
        private unsafe int GetHashCode(ref WIN32_FIND_DATAW file)
        {
            int code = 0;
            fixed (char* fixedChars = file.cFileName)
            {
                char* pFixedChars = fixedChars;
                while (true)
                {
                    char next = *pFixedChars;
                    if (next == 0)
                    {
                        break;
                    }
                    pFixedChars++;
                    code |= next;
                    code <<= 8;
                }
            }
            return code;
        }
        private unsafe bool Equal(string left, ref WIN32_FIND_DATAW file)
        {
            fixed(char* fixedChars = file.cFileName)
            {
                int index;
                for (index = 0; index < left.Length; index++)
                {
                    if (fixedChars[index] != left[index]) return false;
                }
                if (fixedChars[index] != 0) return false;
                return true;
            }
        }

        private int NextBucket(int bucket)
        {
            bucket++;
            if (bucket >= Buckets.Length)
            {
                bucket = 0;
            }
            return bucket;
        }

        private int ComputeBucket(string key)
        {
            var hash = GetHashCode(key);
            var bucket = Math.Abs(hash) % Buckets.Length;
            return bucket;
        }

        private unsafe int ComputeBucket(ref WIN32_FIND_DATAW file)
        {
            var hash = GetHashCode(ref file);
            var bucket = Math.Abs(hash) % Buckets.Length;
            return bucket;
        }

        struct Bucket
        {
            public string Key;
            public int ValuesIndex;

            public Bucket(string key, int valueIndex)
            {
                Key = key;
                ValuesIndex = valueIndex;
            }
            public bool IsEmpty { get { return ValuesIndex == 0; } }
        }
    }
}
