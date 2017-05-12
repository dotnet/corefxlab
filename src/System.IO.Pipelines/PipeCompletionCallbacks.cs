// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;

namespace System.IO.Pipelines
{
    internal struct PipeCompletionCallbacks
    {
        private readonly ArrayPool<PipeCompletionCallback> _pool;
        private readonly int _count;
        private readonly Exception _exception;
        private readonly PipeCompletionCallback[] _callbacks;

        public PipeCompletionCallbacks(ArrayPool<PipeCompletionCallback> pool, int count, Exception exception, PipeCompletionCallback[] callbacks)
        {
            _pool = pool;
            _count = count;
            _exception = exception;
            _callbacks = callbacks;
        }

        public void Execute()
        {
            if (_callbacks == null || _count == 0)
            {
                return;
            }

            try
            {
                for (int i = 0; i < _count; i++)
                {
                    var callback = _callbacks[i];
                    try
                    {
                        callback.Callback(_exception, callback.State);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
            finally
            {
                _pool.Return(_callbacks);
            }
        }
    }
}