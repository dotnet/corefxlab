// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace System.IO
{
    public struct FileChange
    {
        internal FileChange(string directory, string path, WatcherChangeTypes type)
        {
            Debug.Assert(path != null);
            Directory = directory;
            Name = path;
            ChangeType = type;
        }

        public string Directory { get; }
        public string Name { get; }
        public WatcherChangeTypes ChangeType { get; }
    }
}    
