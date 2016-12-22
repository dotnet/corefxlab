using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Samples
{
    public class FakeListener
    {
        private readonly List<PipelineConnection> _connections = new List<PipelineConnection>();
        private Task[] _connectionTasks;

        public FakeListener(PipelineFactory factory, int concurrentConnections)
        {
            for (int i = 0; i < concurrentConnections; i++)
            {
                _connections.Add(new PipelineConnection(factory));
            }
        }

        public void OnConnection(Func<IPipelineConnection, Task> callback)
        {
            _connectionTasks = new Task[_connections.Count];
            for (int i = 0; i < _connections.Count; i++)
            {
                _connectionTasks[i] = callback(_connections[i]);
            }
        }

        public Task ExecuteRequestAsync(byte[] request)
        {
            var tasks = new Task[_connections.Count];
            for (int i = 0; i < _connections.Count; i++)
            {
                var connection = _connections[i];
                tasks[i] = connection.Input.WriteAsync(request);
            }

            return Task.WhenAll(tasks);
        }

        public void Dispose()
        {
            foreach (var c in _connections)
            {
                c.Input.CompleteWriter();
            }

            Task.WaitAll(_connectionTasks);
        }
    }
}