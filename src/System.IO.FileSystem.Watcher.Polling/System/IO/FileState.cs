﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace System.IO
{
    [Serializable]
    internal struct FileState
    {
        internal long _version;  // removal notification are implemented something similar to "mark and sweep". This value is incremented in the mark phase
        public string Path;
        public string Directory;
        public DateTimeOffset LastWriteTimeUtc;
        public long Length;

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
    }
}
