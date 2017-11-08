// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System {
    public struct Range
    {
        public Range(long index, long length)
        {
            Index = index;
            Length = length;
        }
        public readonly long Index;
        public readonly long Length;

        public void Deconstruct(out long index, out long length)
        {
            index = Index;
            length = Length;
        }
    }
}
