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
        [Fact(Skip = "Trying to find a hang")]
        public async Task ReadAsyncCallbackRunsOnReaderScheduler()
        {
            using (var factory = new PipeFactory())
            {
                using (var scheduler = new ThreadScheduler())
                {
                    var pipe = factory.Create(new PipeOptions
                    {
                        ReaderScheduler = scheduler
                    });

                    Func<Task> doRead = async () =>
                    {
                        var oid = Thread.CurrentThread.ManagedThreadId;

                        var result = await pipe.Reader.ReadAsync();

                        Assert.NotEqual(oid, Thread.CurrentThread.ManagedThreadId);

                        Assert.Equal(Thread.CurrentThread.ManagedThreadId, scheduler.Thread.ManagedThreadId);

                        pipe.Reader.Advance(result.Buffer.End, result.Buffer.End);

                        pipe.Reader.Complete();
                    };

                    var reading = doRead();

                    var buffer = pipe.Writer.Alloc();
                    buffer.Write(Encoding.UTF8.GetBytes("Hello World"));
                    await buffer.FlushAsync();

                    await reading;
                }
            }
        }

        [Fact(Skip = "Trying to find a hang")]
        public async Task FlushCallbackRunsOnWriterScheduler()
        {
            using (var factory = new PipeFactory())
            {
                using (var scheduler = new ThreadScheduler())
                {
                    var pipe = factory.Create(new PipeOptions
                    {
                        MaximumSizeLow = 32,
                        MaximumSizeHigh = 64,
                        WriterScheduler = scheduler
                    });

                    var writableBuffer = pipe.Writer.Alloc(64);
                    writableBuffer.Advance(64);
                    var flushAsync = writableBuffer.FlushAsync();

                    Assert.False(flushAsync.IsCompleted);

                    Func<Task> doWrite = async () =>
                    {
                        var oid = Thread.CurrentThread.ManagedThreadId;

                        await flushAsync;

                        Assert.NotEqual(oid, Thread.CurrentThread.ManagedThreadId);

                        pipe.Writer.Complete();

                        Assert.Equal(Thread.CurrentThread.ManagedThreadId, scheduler.Thread.ManagedThreadId);
                    };

                    var writing = doWrite();

                    var result = await pipe.Reader.ReadAsync();

                    pipe.Reader.Advance(result.Buffer.End, result.Buffer.End);

                    pipe.Reader.Complete();

                    await writing;
                }
            }
        }

        [Fact(Skip = "Trying to find a hang")]
        public async Task DefaultReaderSchedulerRunsInline()
        {
            using (var factory = new PipeFactory())
            {
                var pipe = factory.Create();

                var id = 0;

                Func<Task> doRead = async () =>
                {
                    var result = await pipe.Reader.ReadAsync();

                    Assert.Equal(Thread.CurrentThread.ManagedThreadId, id);

                    pipe.Reader.Advance(result.Buffer.End, result.Buffer.End);

                    pipe.Reader.Complete();
                };

                var reading = doRead();

                id = Thread.CurrentThread.ManagedThreadId;

                var buffer = pipe.Writer.Alloc();
                buffer.Write(Encoding.UTF8.GetBytes("Hello World"));
                await buffer.FlushAsync();

                pipe.Writer.Complete();

                await reading;
            }
        }

        [Fact(Skip = "Trying to find a hang")]
        public async Task DefaultWriterSchedulerRunsInline()
        {
            using (var factory = new PipeFactory())
            {
                var pipe = factory.Create(new PipeOptions
                {
                    MaximumSizeLow = 32,
                    MaximumSizeHigh = 64
                });

                var writableBuffer = pipe.Writer.Alloc(64);
                writableBuffer.Advance(64);
                var flushAsync = writableBuffer.FlushAsync();

                Assert.False(flushAsync.IsCompleted);

                int id = 0;

                Func<Task> doWrite = async () =>
                {
                    await flushAsync;

                    pipe.Writer.Complete();

                    Assert.Equal(Thread.CurrentThread.ManagedThreadId, id);
                };

                var writing = doWrite();

                var result = await pipe.Reader.ReadAsync();

                id = Thread.CurrentThread.ManagedThreadId;

                pipe.Reader.Advance(result.Buffer.End, result.Buffer.End);

                pipe.Reader.Complete();

                await writing;
            }
        }

        private class ThreadScheduler : IScheduler, IDisposable
        {
            private BlockingCollection<Action> _work = new BlockingCollection<Action>();

            public Thread Thread { get; }

            public ThreadScheduler()
            {
                Thread = new Thread(Work) { IsBackground = true };
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

            public void Dispose()
            {
                _work.CompleteAdding();
            }
        }
    }
}
