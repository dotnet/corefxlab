// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    public class WriteableBufferStream : Stream
    {
        private readonly static Task _completedTask = Task.FromResult(0);

        private WritableBuffer _buffer;

        public WriteableBufferStream(WritableBuffer buffer)
        {
            _buffer = buffer;
        }

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length
        {
            get
            {
                ThrowHelper.ThrowNotSupportedException();
                return 0;
            }
        }

        public override long Position
        {
            get
            {
                ThrowHelper.ThrowNotSupportedException();
                return 0;
            }
            set
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            ThrowHelper.ThrowNotSupportedException();
            return 0;
        }

        public override void SetLength(long value)
        {
            ThrowHelper.ThrowNotSupportedException();
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            ThrowHelper.ThrowNotSupportedException();
            return 0;
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            ThrowHelper.ThrowNotSupportedException();
            return null;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _buffer.Write(new Span<byte>(buffer, offset, count));
            // No Flush or Commit since caller may want to turn stream writes into a readable buffer.
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken token)
        {
            _buffer.Write(new Span<byte>(buffer, offset, count));
            // No Flush or Commit since caller may want to turn stream writes into a readable buffer.
            return _completedTask;
        }

        public override void Flush()
        {
            // No Flush since caller may want to turn stream writes into a readable buffer.
        }

        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            // No Flush since caller may want to turn stream writes into a readable buffer.
            return _completedTask;
        }

        private ValueTask<int> ReadAsync(ArraySegment<byte> buffer)
        {
            ThrowHelper.ThrowNotSupportedException();
            return default(ValueTask<int>);
        }

#if NET451
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            ThrowHelper.ThrowNotSupportedException();
            return null;
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            ThrowHelper.ThrowNotSupportedException();
            return 0;
        }

        private Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken, object state)
        {
            ThrowHelper.ThrowNotSupportedException();
            return null;
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            var task = WriteAsync(buffer, offset, count, default(CancellationToken), state);
            if (callback != null)
            {
                task.ContinueWith(t => callback.Invoke(t));
            }
            return task;
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            ((Task<object>)asyncResult).GetAwaiter().GetResult();
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
            ThrowHelper.ThrowNotSupportedException();
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            // No Flush or Commit since caller may want to turn stream writes into a readable buffer.
        }
    }
}