using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class SchedulerFacts
    {
        [Fact]
        public async Task ReadAsyncCallbackRunsOnSchedulerThread()
        {
            var factory = new PipelineFactory();
            var scheduler = new ThreadScheduler();
            var pipe = factory.Create(new PipeOptions
            {
                Scheduler = scheduler
            });

            var reading = Task.Run(async () =>
            {
                await pipe.ReadAsync();

                Assert.Equal(Thread.CurrentThread.ManagedThreadId, scheduler.Thread.ManagedThreadId);
            });

            var buffer = pipe.Alloc();
            buffer.Write(Encoding.UTF8.GetBytes("Hello World"));
            await buffer.FlushAsync();

            await reading;
        }

        [Fact]
        public async Task DefaultSchedulerRunsInline()
        {
            var factory = new PipelineFactory();
            var pipe = factory.Create();

            var id = Thread.CurrentThread.ManagedThreadId;

            var reading = Task.Run(async () =>
            {
                await pipe.ReadAsync();

                Assert.Equal(Thread.CurrentThread.ManagedThreadId, id);
            });

            var buffer = pipe.Alloc();
            buffer.Write(Encoding.UTF8.GetBytes("Hello World"));
            await buffer.FlushAsync();

            await reading;
        }

        private class ThreadScheduler : IScheduler
        {
            private BlockingCollection<Action> _work = new BlockingCollection<Action>();

            public Thread Thread { get; }

            public ThreadScheduler()
            {
                Thread = new Thread(Work);
                Thread.Start();
            }

            public void Schedule(Action action)
            {
                _work.Add(action);
            }

            private void Work(object state)
            {
                foreach (var callback in _work.GetConsumingEnumerable())
                {
                    callback();
                }
            }
        }
    }
}
