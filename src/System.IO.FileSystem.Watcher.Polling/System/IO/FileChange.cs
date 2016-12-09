// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace System.IO.FileSystem
{
    public struct FileChange
    {
        string _directory;
        string _path;
        ChangeType _chageType;

        internal FileChange(string directory, string path, ChangeType type)
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

        public ChangeType ChangeType
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
                return _directory.Substring(4, _directory.Length - 6);
            }
        }

        public string Path
        {
            get
            {
                return Directory + '\\' + Name;
            }
        }
    }
}    
