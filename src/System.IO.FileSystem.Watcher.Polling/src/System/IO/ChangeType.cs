// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.IO.FileSystem
{
    public enum ChangeType : byte
    {
        // the order of these values matter.
        Deleted = 0,
        Created = 1,
        Changed = 2,
    }
}    
