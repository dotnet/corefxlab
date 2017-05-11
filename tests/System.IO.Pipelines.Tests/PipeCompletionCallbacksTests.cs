// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
        public void OnReaderCompletedExecutedInlineIfCompleted()
        {
            var callbackRan = false;
            var scheduler = new CallCountScheduler();
            var pipe = _pipeFactory.Create(new PipeOptions() { WriterScheduler = scheduler });
            pipe.Reader.Complete();

            pipe.Writer.OnReaderCompleted((exception, state) =>
            {
                callbackRan = true;
            }, null);

            Assert.True(callbackRan);
            Assert.Equal(0, scheduler.CallCount);
        }

        [Fact]
        public void OnWriterCompletedExecutedInlineIfCompleted()
        {
            var callbackRan = false;
            var scheduler = new CallCountScheduler();
            var pipe = _pipeFactory.Create(new PipeOptions() { ReaderScheduler = scheduler });
            pipe.Writer.Complete();

            pipe.Reader.OnWriterCompleted((exception, state) =>
            {
                callbackRan = true;
            }, null);

            Assert.True(callbackRan);
            Assert.Equal(0, scheduler.CallCount);
        }

        [Fact]
        public void OnReaderCompletedThrowsWithNullCallback()
        {
            var pipe = _pipeFactory.Create();

            Assert.Throws<ArgumentNullException>(() => pipe.Writer.OnReaderCompleted(null, null));
        }

        [Fact]
        public void OnWriterCompletedThrowsWithNullCallback()
        {
            var pipe = _pipeFactory.Create();

            Assert.Throws<ArgumentNullException>(() => pipe.Reader.OnWriterCompleted(null, null));
        }

        [Fact]
        public void OnReaderCompletedUsingWriterScheduler()
        {
            var callbackRan = false;
            var scheduler = new CallCountScheduler();
            var pipe = _pipeFactory.Create(new PipeOptions() { WriterScheduler = scheduler });
            pipe.Writer.OnReaderCompleted((exception, state) =>
            {
                callbackRan = true;
            }, null);
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
            pipe.Reader.OnWriterCompleted((exception, state) =>
            {
                callbackRan = true;
            }, null);
            pipe.Writer.Complete();

            Assert.True(callbackRan);
            Assert.Equal(1, scheduler.CallCount);
        }
        [Fact]
        public void OnReaderCompletedIsDetachedDuringReset()
        {
            var callbackRan = false;
            var scheduler = new CallCountScheduler();
            var pipe = _pipeFactory.Create(new PipeOptions() { WriterScheduler = scheduler });
            pipe.Writer.OnReaderCompleted((exception, state) =>
            {
                callbackRan = true;
            }, null);

            pipe.Reader.Complete();
            pipe.Writer.Complete();
            pipe.Reset();

            Assert.True(callbackRan);
            callbackRan = false;

            pipe.Reader.Complete();
            pipe.Writer.Complete();

            Assert.Equal(1, scheduler.CallCount);
        }

        [Fact]
        public void OnWriterCompletedIsDetachedDuringReset()
        {
            var callbackRan = false;
            var scheduler = new CallCountScheduler();
            var pipe = _pipeFactory.Create(new PipeOptions() { ReaderScheduler = scheduler });
            pipe.Reader.OnWriterCompleted((exception, state) =>
            {
                callbackRan = true;
            }, null);
            pipe.Reader.Complete();
            pipe.Writer.Complete();
            pipe.Reset();

            Assert.True(callbackRan);
            callbackRan = false;

            pipe.Reader.Complete();
            pipe.Writer.Complete();

            Assert.Equal(1, scheduler.CallCount);
        }

        [Fact]
        public void OnReaderCompletedPassesState()
        {
            var callbackRan = false;
            var callbackState = new object();
            var pipe = _pipeFactory.Create();
            pipe.Writer.OnReaderCompleted((exception, state) =>
            {
                Assert.Equal(callbackState, state);
                callbackRan = true;
            }, callbackState);
            pipe.Reader.Complete();

            Assert.True(callbackRan);
        }

        [Fact]
        public void OnWriterCompletedPassesState()
        {
            var callbackRan = false;
            var callbackState = new object();
            var pipe = _pipeFactory.Create();
            pipe.Reader.OnWriterCompleted((exception, state) =>
            {
                Assert.Equal(callbackState, state);
                callbackRan = true;
            }, callbackState);
            pipe.Writer.Complete();

            Assert.True(callbackRan);
        }

        [Fact]
        public void OnReaderCompletedRunsInRegistrationOrder()
        {
            var callbackState1 = new object();
            var callbackState2 = new object();
            var callbackState3 = new object();
            var counter = 0;

            var pipe = _pipeFactory.Create();
            pipe.Writer.OnReaderCompleted((exception, state) =>
            {
                Assert.Equal(callbackState1, state);
                Assert.Equal(0, counter);
                counter++;
            }, callbackState1);

            pipe.Writer.OnReaderCompleted((exception, state) =>
            {
                Assert.Equal(callbackState2, state);
                Assert.Equal(1, counter);
                counter++;
            }, callbackState2);

            pipe.Writer.OnReaderCompleted((exception, state) =>
            {
                Assert.Equal(callbackState3, state);
                Assert.Equal(2, counter);
                counter++;
            }, callbackState3);

            pipe.Reader.Complete();

            Assert.Equal(3, counter);
        }

        [Fact]
        public void OnWriterCompletedRunsInRegistrationOrder()
        {
            var callbackState1 = new object();
            var callbackState2 = new object();
            var callbackState3 = new object();
            var counter = 0;

            var pipe = _pipeFactory.Create();
            pipe.Reader.OnWriterCompleted((exception, state) =>
            {
                Assert.Equal(callbackState1, state);
                Assert.Equal(0, counter);
                counter++;
            }, callbackState1);

            pipe.Reader.OnWriterCompleted((exception, state) =>
            {
                Assert.Equal(callbackState2, state);
                Assert.Equal(1, counter);
                counter++;
            }, callbackState2);

            pipe.Reader.OnWriterCompleted((exception, state) =>
            {
                Assert.Equal(callbackState3, state);
                Assert.Equal(2, counter);
                counter++;
            }, callbackState3);

            pipe.Writer.Complete();

            Assert.Equal(3, counter);
        }

        [Fact]
        public void OnWriterCompletedPassesException()
        {
            var callbackRan = false;
            var pipe = _pipeFactory.Create();
            var readerException = new Exception();

            pipe.Reader.OnWriterCompleted((exception, state) =>
            {
                callbackRan = true;
                Assert.Same(readerException, exception);
            }, null);
            pipe.Writer.Complete(readerException);

            Assert.True(callbackRan);
        }

        [Fact]
        public void OnReaderCompletedPassesException()
        {
            var callbackRan = false;
            var pipe = _pipeFactory.Create();
            var readerException = new Exception();

            pipe.Writer.OnReaderCompleted((exception, state) =>
            {
                callbackRan = true;
                Assert.Same(readerException, exception);
            }, null);
            pipe.Reader.Complete(readerException);

            Assert.True(callbackRan);
        }

        [Fact]
        public void OnWriterCompletedRanBeforeReadContinuation()
        {
            var callbackRan = false;
            var continuationRan = false;
            var pipe = _pipeFactory.Create();

            pipe.Reader.OnWriterCompleted((exception, state) =>
            {
                callbackRan = true;
                Assert.False(continuationRan);
            }, null);

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
            var pipe = _pipeFactory.Create(new PipeOptions() { MaximumSizeHigh = 5 });

            pipe.Writer.OnReaderCompleted((exception, state) =>
            {
                callbackRan = true;
                Assert.False(continuationRan);
            }, null);

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


        private class CallCountScheduler : IScheduler
        {
            public int CallCount { get; set; }

            public void Schedule(Action<object> action, object state)
            {
                CallCount++;
                action(state);
            }
        }
    }
}
