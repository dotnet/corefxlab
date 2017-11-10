﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Net
{
    static class MemoryPoolHelper {

        public static OwnedBuffer Rent(int size = OwnedBuffer.DefaultBufferSize)
        {
            return new OwnedBuffer(size);
        }
    }
}
