// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.IO
{
    public class PollingFileSystemEventArgs : EventArgs
    {
        public PollingFileSystemEventArgs(FileChange[] changes)
        {
            Changes = changes;
        }

        public FileChange[] Changes { get; }
    }
}
