// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace System.IO.FileSystem
{
    struct FileChangeList
    {
        const int DefaultListSize = 4;

        FileChange[] _changes; // TODO: this should probably just use List<FileChange>. As of right now, FileChangeList must be passed by ref
        int _count;

        public bool IsEmpty { get { return _changes == null || _count == 0; } }

        internal void AddAdded(string path)
        {
            Debug.Assert(path != null);

            EnsureCapacity();
            _changes[_count++] = new FileChange(path, ChangeType.Created);
        }

        internal void AddChanged(string path)
        {
            Debug.Assert(path != null);

            EnsureCapacity();
            _changes[_count++] = new FileChange(path, ChangeType.Changed);
        }

        internal void AddRemoved(string path)
        {
            Debug.Assert(path != null);

            EnsureCapacity();
            _changes[_count++] = new FileChange(path, ChangeType.Deleted);
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
    }

    struct FileChange
    {
        public FileChange(string path, ChangeType type)
        {
            Debug.Assert(path != null);

            Path = path;
            Type = type;
        }
        string Path;
        ChangeType Type;
    }

    struct FileState
    {
        internal byte _version;  // removal notification are implemented something similar to "mark and sweep". This value is incremented in the mark phase
        public string Path;
        public ulong LastWrite;
        public ulong FileSize;

        public FileState(string path) : this()
        {
            Debug.Assert(path != null);

            Path = path;
        }

        public override string ToString()
        {
            return Path;
        }
    }
}    
