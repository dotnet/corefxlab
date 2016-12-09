// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;

namespace System.Threading.Tasks.Channels
{
    // TODO: This implementation currently uses BinaryReader and BinaryWriter for serialization.
    // This is fundamentally limited in several ways, and serves purely as an example for
    // how channels can be implemented on top of Streams and a serialization mechanism.
    // It's also limited in that these types don't currently provide async versions of the read
    // and write methods.

    public static partial class Channel
    {
        /// <summary>Provides a channel for serializing data to a stream.</summary>
        private sealed class SerializationChannel<T> : IWritableChannel<T>
        {
            /// <summary>The writer to which data is written.</summary>
            private readonly BinaryWriter _destination;
            /// <summary>Whether <see cref="TryComplete(Exception)"/> has been called.</summary>
            private bool _isCompleted;

            /// <summary>Initialize the channel.</summary>
            /// <param name="destination">The destination stream.</param>
            internal SerializationChannel(Stream destination)
            {
                _destination = new BinaryWriter(destination);
            }

            public bool TryComplete(Exception error = null)
            {
                // Complete the channel by disposing of the stream.
                lock (_destination)
                {
                    if (_isCompleted)
                    {
                        return false;
                    }
                    _isCompleted = true;
                    _destination.Dispose();
                }

                return true;
            }

            public bool TryWrite(T item)
            {
                // Write it out
                lock (_destination)
                {
                    if (_isCompleted)
                    {
                        return false;
                    }

                    if (typeof(T) == typeof(bool)) _destination.Write((bool)(object)item);
                    else if (typeof(T) == typeof(byte)) _destination.Write((byte)(object)item);
                    else if (typeof(T) == typeof(char)) _destination.Write((char)(object)item);
                    else if (typeof(T) == typeof(decimal)) _destination.Write((decimal)(object)item);
                    else if (typeof(T) == typeof(double)) _destination.Write((double)(object)item);
                    else if (typeof(T) == typeof(float)) _destination.Write((float)(object)item);
                    else if (typeof(T) == typeof(int)) _destination.Write((int)(object)item);
                    else if (typeof(T) == typeof(long)) _destination.Write((long)(object)item);
                    else if (typeof(T) == typeof(sbyte)) _destination.Write((sbyte)(object)item);
                    else if (typeof(T) == typeof(short)) _destination.Write((short)(object)item);
                    else if (typeof(T) == typeof(string)) _destination.Write((string)(object)item);
                    else if (typeof(T) == typeof(uint)) _destination.Write((uint)(object)item);
                    else if (typeof(T) == typeof(ulong)) _destination.Write((ulong)(object)item);
                    else if (typeof(T) == typeof(ushort)) _destination.Write((ushort)(object)item);
                    else throw new InvalidOperationException(Properties.Resources.InvalidOperationException_TypeNotSerializable);
                }

                // And always return true.  We have no mechanism on Stream for attempting to write
                // without actually doing it.
                return true;
            }

            public Task<bool> WaitToWriteAsync(CancellationToken cancellationToken)
            {
                // We assume we can always write to the stream (unless we're already canceled).
                return 
                    cancellationToken.IsCancellationRequested ? Task.FromCanceled<bool>(cancellationToken) :
                    _isCompleted ? s_falseTask :
                    s_trueTask;
            }

            public Task WriteAsync(T item, CancellationToken cancellationToken)
            {
                // Fast-path cancellation check
                if (cancellationToken.IsCancellationRequested)
                {
                    return Task.FromCanceled(cancellationToken);
                }

                return Task.Run(() =>
                {
                    bool result = TryWrite(item);
                    if (!result)
                    {
                        throw CreateInvalidCompletionException();
                    }
                    return result;
                });
            }
        }

        /// <summary>Provides a channel for deserializing data from a stream.</summary>
        private sealed class DeserializationChannel<T> : IReadableChannel<T> 
        {
            /// <summary>The stream from which to read data.</summary>
            private readonly BinaryReader _source;
            /// <summary>A task that completes when the stream is at its end.</summary>
            private readonly TaskCompletionSource<VoidResult> _completion = new TaskCompletionSource<VoidResult>(TaskCreationOptions.RunContinuationsAsynchronously);
            /// <summary>Semaphore used to protect access to the underlying stream.</summary>
            private readonly SemaphoreSlim _asyncGate = new SemaphoreSlim(1, 1);
            /// <summary>A task for the next asynchronous read operation.</summary>
            private Task<KeyValuePair<bool, T>> _nextAvailable;

            /// <summary>The object to use to synchronize all state on this channel.</summary>
            private object SyncObj => _source;

            internal DeserializationChannel(Stream source)
            {
                _source = new BinaryReader(source);
            }

            public Task Completion => _completion.Task;

            public ValueTask<T> ReadAsync(CancellationToken cancellationToken)
            {
                // Fast-path cancellation check
                if (cancellationToken.IsCancellationRequested)
                    return new ValueTask<T>(Task.FromCanceled<T>(cancellationToken));

                lock (SyncObj)
                {
                    // If there's a task available, grab it, otherwise create one.
                    Task<KeyValuePair<bool, T>> next;
                    if (_nextAvailable != null)
                    {
                        next = _nextAvailable;
                        _nextAvailable = null;
                    }
                    else
                    {
                        next = ReadNextAsync();
                    }

                    // If the task has completed, return its results synchronously
                    if (next.IsCompleted)
                    {
                        if (next.Status == TaskStatus.RanToCompletion)
                        {
                            if (next.Result.Key)
                            {
                                return new ValueTask<T>(next.Result.Value);
                            }
                            else
                            {
                                return new ValueTask<T>(Task.FromException<T>(CreateInvalidCompletionException()));
                            }
                        }
                        else
                        {
                            return new ValueTask<T>(PropagateErrorAsync<T>(next));
                        }
                    }

                    // Otherwise, wait for it asynchronously
                    return new ValueTask<T>(next.ContinueWith(t =>
                    {
                        KeyValuePair<bool, T> result = t.GetAwaiter().GetResult();
                        if (!result.Key)
                            throw CreateInvalidCompletionException();
                        return result.Value;
                    }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default));
                }
            }

            public bool TryRead(out T item)
            {
                lock (SyncObj)
                {
                    // If there's a task available, it completed successfully, and it came back with results,
                    // grab it and return those results.
                    Task<KeyValuePair<bool, T>> next = _nextAvailable;
                    if (next != null && next.Status == TaskStatus.RanToCompletion && next.Result.Key)
                    {
                        _nextAvailable = null;
                        item = next.Result.Value;
                        return true;
                    }
                }

                item = default(T);
                return false;
            }

            public Task<bool> WaitToReadAsync(CancellationToken cancellationToken)
            {
                if (cancellationToken.IsCancellationRequested)
                    return Task.FromCanceled<bool>(cancellationToken);

                lock (SyncObj)
                {
                    // If there's no current task, create one.
                    Task<KeyValuePair<bool, T>> next = _nextAvailable;
                    if (next == null)
                    {
                        _nextAvailable = next = ReadNextAsync();
                    }

                    // If the task is completed, we can return synchronously.
                    if (next.IsCompleted)
                    {
                        return next.Status == TaskStatus.RanToCompletion ?
                            (next.Result.Key ? s_trueTask : s_falseTask) :
                            PropagateErrorAsync<bool>(next);
                    }

                    // Otherwise, return asynchronously.
                    return next.ContinueWith(t => t.GetAwaiter().GetResult().Key,
                        cancellationToken, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
                }
            }

            /// <summary>Reads the next T item from the stream asynchronously.</summary>
            /// <returns>A task whose result is a key/value pair indicating whether there was more data or not and the T item result.</returns>
            private Task<KeyValuePair<bool, T>> ReadNextAsync()
            {
                // Ensure there's only one read on the underlying stream at a time, and that if we need to make
                // multiple reads to get enough data for the T instance that we can do them without intervening reads
                // from other calls.
                return _asyncGate.WaitAsync().ContinueWith((t, s) =>
                {
                    var thisRef = (DeserializationChannel<T>)s;
                    var reader = thisRef._source;
                    try
                    {
                        T item;
                        if (typeof(T) == typeof(bool)) item = (T)(object)reader.ReadBoolean();
                        else if (typeof(T) == typeof(byte)) item = (T)(object)reader.ReadByte();
                        else if (typeof(T) == typeof(char)) item = (T)(object)reader.ReadChar();
                        else if (typeof(T) == typeof(decimal)) item = (T)(object)reader.ReadDecimal();
                        else if (typeof(T) == typeof(double)) item = (T)(object)reader.ReadDouble();
                        else if (typeof(T) == typeof(float)) item = (T)(object)reader.ReadSingle();
                        else if (typeof(T) == typeof(int)) item = (T)(object)reader.ReadInt32();
                        else if (typeof(T) == typeof(long)) item = (T)(object)reader.ReadInt64();
                        else if (typeof(T) == typeof(sbyte)) item = (T)(object)reader.ReadSByte();
                        else if (typeof(T) == typeof(short)) item = (T)(object)reader.ReadInt16();
                        else if (typeof(T) == typeof(string)) item = (T)(object)reader.ReadString();
                        else if (typeof(T) == typeof(uint)) item = (T)(object)reader.ReadUInt32();
                        else if (typeof(T) == typeof(ulong)) item = (T)(object)reader.ReadUInt64();
                        else if (typeof(T) == typeof(ushort)) item = (T)(object)reader.ReadUInt16();
                        else throw new InvalidOperationException(Properties.Resources.InvalidOperationException_TypeNotSerializable);
                        return new KeyValuePair<bool, T>(true, item);
                    }
                    catch (EndOfStreamException)
                    {
                        CompleteWithOptionalError(_completion, null);
                        return new KeyValuePair<bool, T>(false, default(T));
                    }
                    finally
                    {
                        thisRef._asyncGate.Release();
                    }
                }, this, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.Default);
            }

        }
    }
}
