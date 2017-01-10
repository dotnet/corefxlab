// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

namespace System.Threading.Tasks.Channels
{
    public abstract class Channel<T> : Channel<T, T> { }

    public abstract class Channel<TWrite, TRead>
    {
        public abstract ReadableChannel<TRead> In { get; }
        public abstract WritableChannel<TWrite> Out { get; }

        // The following non-virtuals are all convenience members that wrap the corresponding
        // members on Out and In.

        public Task Completion => In.Completion;
        public ValueAwaiter<TRead> GetAwaiter() => In.GetAwaiter();
        public ValueTask<TRead> ReadAsync(CancellationToken cancellationToken = default(CancellationToken)) => In.ReadAsync(cancellationToken);
        public bool TryComplete(Exception error = null) => Out.TryComplete(error);
        public bool TryRead(out TRead item) => In.TryRead(out item);
        public bool TryWrite(TWrite item) => Out.TryWrite(item);
        public Task<bool> WaitToReadAsync(CancellationToken cancellationToken = default(CancellationToken)) => In.WaitToReadAsync(cancellationToken);
        public Task<bool> WaitToWriteAsync(CancellationToken cancellationToken = default(CancellationToken)) => Out.WaitToWriteAsync(cancellationToken);
        public Task WriteAsync(TWrite item, CancellationToken cancellationToken = default(CancellationToken)) => Out.WriteAsync(item, cancellationToken);
    }

    public abstract class ReadableChannel<T>
    {
        public abstract Task Completion { get; }
        public abstract ValueAwaiter<T> GetAwaiter();
        public abstract ValueTask<T> ReadAsync(CancellationToken cancellationToken = default(CancellationToken));
        public abstract bool TryRead(out T item);
        public abstract Task<bool> WaitToReadAsync(CancellationToken cancellationToken = default(CancellationToken));
    }

    public abstract class WritableChannel<T>
    {
        public abstract bool TryComplete(Exception error = null);
        public abstract bool TryWrite(T item);
        public abstract Task<bool> WaitToWriteAsync(CancellationToken cancellationToken = default(CancellationToken));
        public abstract Task WriteAsync(T item, CancellationToken cancellationToken = default(CancellationToken));
    }
}
