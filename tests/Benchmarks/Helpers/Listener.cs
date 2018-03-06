// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Samples
{
    public class FakeListener
    {
        private readonly List<DuplexPipe> _connections = new List<DuplexPipe>();
        private Task[] _connectionTasks;

        public FakeListener(MemoryPool<byte> pool, int concurrentConnections)
        {
            for (int i = 0; i < concurrentConnections; i++)
            {
                _connections.Add(new DuplexPipe(new PipeOptions(pool)));
            }
        }

        public void OnConnection(Func<IDuplexPipe, Task> callback)
        {
            _connectionTasks = new Task[_connections.Count];
            for (int i = 0; i < _connections.Count; i++)
            {
                _connectionTasks[i] = callback(_connections[i]);
            }
        }

        public Task ExecuteRequestAsync(byte[] request)
        {
            async Task Await(ValueTask<FlushResult> awaiter) => await awaiter;

            var tasks = new Task[_connections.Count];
            for (int i = 0; i < _connections.Count; i++)
            {
                var connection = _connections[i];
                tasks[i] = Await(connection.Input.Writer.WriteAsync(request));
            }

            return Task.WhenAll(tasks);
        }

        public void Dispose()
        {
            foreach (var c in _connections)
            {
                c.Input.Writer.Complete();
            }

            Task.WaitAll(_connectionTasks);
        }
    }
}
