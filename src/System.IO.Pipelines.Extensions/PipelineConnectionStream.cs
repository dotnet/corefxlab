// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    public class PipelineConnectionStream : Stream
    {
        private readonly static Task<int> _initialCachedTask = Task.FromResult(0);
        private Task<int> _cachedTask = _initialCachedTask;

        private readonly IPipeConnection _connection;

        public PipelineConnectionStream(IPipeConnection connection)
        {
            _connection = connection;
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            // ValueTask uses .GetAwaiter().GetResult() if necessary
            // https://github.com/dotnet/corefx/blob/f9da3b4af08214764a51b2331f3595ffaf162abe/src/System.Threading.Tasks.Extensions/src/System/Threading/Tasks/ValueTask.cs#L156
            return ReadAsync(new ArraySegment<byte>(buffer, offset, count)).Result;
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            var task = ReadAsync(new ArraySegment<byte>(buffer, offset, count));

            if (task.IsCompletedSuccessfully)
            {
                if (_cachedTask.Result != task.Result)
                {
                    // Needs .AsTask to match Stream's Async method return types
                    _cachedTask = task.AsTask();
                }
            }
            else
            {
                // Needs .AsTask to match Stream's Async method return types
                _cachedTask = task.AsTask();
            }

            return _cachedTask;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _connection.Output.WriteAsync(new ArraySegment<byte>(buffer, offset, count)).GetAwaiter().GetResult();
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken token)
        {
            return _connection.Output.WriteAsync(new ArraySegment<byte>(buffer, offset, count));
        }

        public override void Flush()
        {
            // No-op since writes are immediate.
        }

        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            // No-op since writes are immediate.
            return Task.FromResult(0);
        }

        private ValueTask<int> ReadAsync(ArraySegment<byte> buffer)
        {
            return _connection.Input.ReadAsync(buffer);
        }

#if NET451
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            var task = ReadAsync(buffer, offset, count, default(CancellationToken), state);
            return TaskToApm.Begin(task, callback, state);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            return TaskToApm.End<int>(asyncResult);
        }

        private Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken, object state)
        {
            var tcs = new TaskCompletionSource<int>(state);
            var task = ReadAsync(buffer, offset, count, cancellationToken);
            task.ContinueWith((task2, state2) =>
            {
                var tcs2 = (TaskCompletionSource<int>)state2;
                if (task2.IsCanceled)
                {
                    tcs2.SetCanceled();
                }
                else if (task2.IsFaulted)
                {
                    tcs2.SetException(task2.Exception);
                }
                else
                {
                    tcs2.SetResult(task2.Result);
                }
            }, tcs, cancellationToken);
            return tcs.Task;
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            var task = WriteAsync(buffer, offset, count, default(CancellationToken), state);
            return TaskToApm.Begin(task, callback, state);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            TaskToApm.End(asyncResult);
        }

        private Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken, object state)
        {
            var tcs = new TaskCompletionSource<object>(state);
            var task = WriteAsync(buffer, offset, count, cancellationToken);
            task.ContinueWith((task2, state2) =>
            {
                var tcs2 = (TaskCompletionSource<object>)state2;
                if (task2.IsCanceled)
                {
                    tcs2.SetCanceled();
                }
                else if (task2.IsFaulted)
                {
                    tcs2.SetException(task2.Exception);
                }
                else
                {
                    tcs2.SetResult(null);
                }
            }, tcs, cancellationToken);
            return tcs.Task;
        }
#endif
        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            return _connection.Input.CopyToAsync(destination, bufferSize, cancellationToken);
        }

        protected override void Dispose(bool disposing)
        {
            _connection.Dispose();
        }
    }
}