// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace System.IO.FileSystem
{
    struct FileChangeList
    {
        const int DefaultListSize = 4;

        FileChange[] _changes;
        int _count;

        public bool IsEmpty { get { return _changes == null || _count == 0; } }

        internal void AddAdded(string directory, string path)
        {
            Debug.Assert(path != null);

            EnsureCapacity();
            _changes[_count++] = new FileChange(directory, path, ChangeType.Created);
        }

        internal void AddChanged(string directory, string path)
        {
            Debug.Assert(path != null);

            EnsureCapacity();
            _changes[_count++] = new FileChange(directory, path, ChangeType.Changed);
        }

        internal void AddRemoved(string directory, string path)
        {
            Debug.Assert(path != null);

            EnsureCapacity();
            _changes[_count++] = new FileChange(directory, path, ChangeType.Deleted);
        }

        void EnsureCapacity()
        {
            if (_changes == null)
            {
                _changes = new FileChange[DefaultListSize];
            }
            if (_count >= _changes.Length)
            {
                var larger = new FileChange[_changes.Length * 2];
                _changes.CopyTo(larger, 0);
                _changes = larger;
            }
        }

        public override string ToString()
        {
            return _count.ToString();
        }

        public FileChange[] ToArray()
        {
            var result = new FileChange[_count];
            Array.Copy(_changes, result, _count);
            return result;
        }
    }

    struct FileState
    {
        internal byte _version;  // removal notification are implemented something similar to "mark and sweep". This value is incremented in the mark phase
        public string Path;
        public string Directory;
        public ulong LastWrite;
        public ulong FileSize;

        public FileState(string directory, string path) : this()
        {
            Debug.Assert(path != null);
            Directory = directory;
            Path = path;
        }

        public override string ToString()
        {
            return Path;
        }

        internal bool IsEmpty
        {
            get { return Path == null; }
        }
    }
}    
