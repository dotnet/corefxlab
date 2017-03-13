// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Pools;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Factory used to creaet instances of various pipelines.
    /// </summary>
    public class PipeFactory : IDisposable
    {
        private readonly BufferPool _pool;

        public PipeFactory() : this(new MemoryPool())
        {
        }

        public PipeFactory(BufferPool pool)
        {
            _pool = pool;
        }

        public IPipe Create()
        {
            return new Pipe(_pool);
        }

        public IPipe Create(PipeOptions options)
        {
            return new Pipe(_pool, options);
        }

        public void Dispose() => _pool.Dispose();
    }
}
