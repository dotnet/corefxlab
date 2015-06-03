// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace System.IO.FileSystem
{
    public struct FileChange
    {
        string _path;
        ChangeType _chageType;

        internal FileChange(string path, ChangeType type)
        {
            Debug.Assert(path != null);

            _path = path;
            _chageType = type;
        }

        public string Path
        {
            get
            {
                return _path;
            }
        }

        public ChangeType ChageType
        {
            get
            {
                return _chageType;
            }
        }
    }
}    
