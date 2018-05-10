// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace System.IO
{
    public struct FileChange
    {
        string _directory;
        string _path;
        WatcherChangeTypes _chageType;

        internal FileChange(string directory, string path, WatcherChangeTypes type)
        {
            Debug.Assert(path != null);
            _directory = directory;
            _path = path;
            _chageType = type;
        }

        public string Name
        {
            get
            {
                return _path;
            }
        }

        public WatcherChangeTypes ChangeType
        {
            get
            {
                return _chageType;
            }
        }

        public string Directory
        {
            get
            {
                return _directory;
            }
        }

        public string Path
        {
            get
            {
                return _path;
            }
        }
    }
}    
