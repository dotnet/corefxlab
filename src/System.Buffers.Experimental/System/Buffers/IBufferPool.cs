﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers
{
    public interface IBufferPool<T> : IDisposable
    {
        OwnedMemory<T> Rent(int minimumBufferSize);

        void Return(OwnedMemory<T> buffer);
    }
}