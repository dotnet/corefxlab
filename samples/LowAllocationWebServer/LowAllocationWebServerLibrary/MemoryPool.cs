// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Net
{
    static class MemoryPoolHelper {

        public static BufferSequence Rent(int size = BufferSequence.DefaultBufferSize)
        {
            return new BufferSequence(size);
        }
    }
}
