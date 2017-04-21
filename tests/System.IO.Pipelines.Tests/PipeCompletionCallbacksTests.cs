using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class PipeCompletionCallbacksTests
    {
        private readonly PipeFactory _pipeFactory;

        public PipeCompletionCallbacksTests()
        {
            _pipeFactory = new PipeFactory();
        }

        public void Dispose()
        {
            _pipeFactory.Dispose();
        }

        [Fact]
        public void OnReaderCompletedUsingWriterScheduler()
        {
            var callbackRan = false;
            var scheduler = new CallCountScheduler();
            var pipe = _pipeFactory.Create(new PipeOptions() { WriterScheduler = scheduler });
            pipe.Writer.OnReaderCompleted(exception =>
            {
                callbackRan = true;
            });
            pipe.Reader.Complete();

            Assert.True(callbackRan);
            Assert.Equal(1, scheduler.CallCount);
        }

        [Fact]
        public void OnWriterCompletedUsingReaderScheduler()
        {
            var callbackRan = false;
            var scheduler = new CallCountScheduler();
            var pipe = _pipeFactory.Create(new PipeOptions() { ReaderScheduler = scheduler });
            pipe.Reader.OnWriterCompleted(exception =>
            {
                callbackRan = true;
            });
            pipe.Writer.Complete();

            Assert.True(callbackRan);
            Assert.Equal(1, scheduler.CallCount);
        }

        [Fact]
        public void OnWriterCompletedPassesException()
        {
            var callbackRan = false;
            var pipe = _pipeFactory.Create();
            var readerException = new Exception();

            pipe.Reader.OnWriterCompleted(exception =>
            {
                callbackRan = true;
                Assert.Same(readerException, exception);
            });
            pipe.Writer.Complete(readerException);

            Assert.True(callbackRan);
        }

        [Fact]
        public void OnReaderCompletedPassesException()
        {
            var callbackRan = false;
            var pipe = _pipeFactory.Create();
            var readerException = new Exception();

            pipe.Writer.OnReaderCompleted(exception =>
            {
                callbackRan = true;
                Assert.Same(readerException, exception);
            });
            pipe.Reader.Complete(readerException);

            Assert.True(callbackRan);
        }

        [Fact]
        public void OnWriterCompletedRanBeforeReadContinuation()
        {
            var callbackRan = false;
            var continuationRan = false;
            var pipe = _pipeFactory.Create();

            pipe.Reader.OnWriterCompleted(exception =>
            {
                callbackRan = true;
                Assert.False(continuationRan);
            });

            var awaiter = pipe.Reader.ReadAsync();
            Assert.False(awaiter.IsCompleted);
            awaiter.OnCompleted(() =>
            {
                continuationRan = true;
            });
            pipe.Writer.Complete();

            Assert.True(callbackRan);
        }

        [Fact]
        public void OnReaderCompletedRanBeforeFlushContinuation()
        {
            var callbackRan = false;
            var continuationRan = false;
            var pipe = _pipeFactory.Create(new PipeOptions() { MaximumSizeHigh = 5});

            pipe.Writer.OnReaderCompleted(exception =>
            {
                callbackRan = true;
                Assert.False(continuationRan);
            });

            var buffer = pipe.Writer.Alloc(10);
            buffer.Advance(10);
            var awaiter = buffer.FlushAsync();

            Assert.False(awaiter.IsCompleted);
            awaiter.OnCompleted(() =>
            {
                continuationRan = true;
            });
            pipe.Reader.Complete();

            Assert.True(callbackRan);
        }


        private class CallCountScheduler:IScheduler
        {
            public int CallCount { get; set; }

            public void Schedule(Action action)
            {
                CallCount++;
                action();
            }


        }
    }
}
