// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers.Internal
{
    internal sealed partial class ManagedBufferPool : BufferPool
    {
        readonly static ManagedBufferPool s_shared = new ManagedBufferPool();

        public static ManagedBufferPool Shared
        {
            get
            {
                return s_shared;
            }
        }
    }
}
