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
        public async Task ReadAsyncCallbackRunsOnReaderScheduler()
        {
            using (var factory = new PipelineFactory())
            {
                var scheduler = new ThreadScheduler();
                var pipe = factory.Create(new PipeOptions
                {
                    ReaderScheduler = scheduler
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
        }

        [Fact]
        public async Task FlushCallbackRunsOnWriterScheduler()
        {
            using (var factory = new PipelineFactory())
            {
                var scheduler = new ThreadScheduler();
                var pipe = factory.Create(new PipeOptions
                {
                    MaximumSizeLow = 32,
                    MaximumSizeHigh = 64,
                    WriterScheduler = scheduler
                });

                var writableBuffer = pipe.Alloc(64);
                writableBuffer.Advance(64);
                var flushAsync = writableBuffer.FlushAsync();

                Assert.False(flushAsync.IsCompleted);

                var writing = Task.Run(async () =>
                {
                    await flushAsync;

                    pipe.CompleteWriter();

                    Assert.Equal(Thread.CurrentThread.ManagedThreadId, scheduler.Thread.ManagedThreadId);
                });

                var result = await pipe.ReadAsync();

                pipe.AdvanceReader(result.Buffer.End, result.Buffer.End);

                pipe.CompleteReader();

                await writing;
            }
        }

        [Fact]
        public async Task DefaultReaderSchedulerRunsInline()
        {
            using (var factory = new PipelineFactory())
            {
                var pipe = factory.Create();

                var id = Thread.CurrentThread.ManagedThreadId;

                var reading = Task.Run(async () =>
                {
                    await pipe.ReadAsync();

                    Assert.Equal(Thread.CurrentThread.ManagedThreadId, id);

                    pipe.CompleteReader();
                });

                var buffer = pipe.Alloc();
                buffer.Write(Encoding.UTF8.GetBytes("Hello World"));
                await buffer.FlushAsync();

                pipe.CompleteWriter();

                await reading;
            }
        }

        [Fact]
        public async Task DefaultWriterSchedulerRunsInline()
        {
            using (var factory = new PipelineFactory())
            {
                var pipe = factory.Create(new PipeOptions
                {
                    MaximumSizeLow = 32,
                    MaximumSizeHigh = 64
                });

                var writableBuffer = pipe.Alloc(64);
                writableBuffer.Advance(64);
                var flushAsync = writableBuffer.FlushAsync();

                Assert.False(flushAsync.IsCompleted);

                int id = 0;

                var writing = Task.Run(async () =>
                {
                    await flushAsync;

                    pipe.CompleteWriter();

                    Assert.Equal(Thread.CurrentThread.ManagedThreadId, id);
                });

                var result = await pipe.ReadAsync();

                id = Thread.CurrentThread.ManagedThreadId;

                pipe.AdvanceReader(result.Buffer.End, result.Buffer.End);

                pipe.CompleteReader();

                await writing;
            }
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
