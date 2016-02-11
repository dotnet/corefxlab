// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.IO.Pipes;
using Xunit;

namespace System.Threading.Tasks.Channels.Tests
{
    public class SerializationChannelTests : TestBase
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Pipes_ReadWriteValues(bool firstWaitToRead)
        {
            await Pipes_ReadWriteValues(firstWaitToRead, 10,i => (byte)i);
            await Pipes_ReadWriteValues(firstWaitToRead, 10,i => (sbyte)i);
            await Pipes_ReadWriteValues(firstWaitToRead, 10,i => (uint)i);
            await Pipes_ReadWriteValues(firstWaitToRead, 10,i => (int)i);
            await Pipes_ReadWriteValues(firstWaitToRead, 10,i => (ushort)i);
            await Pipes_ReadWriteValues(firstWaitToRead, 10,i => (short)i);
            await Pipes_ReadWriteValues(firstWaitToRead, 10,i => (ulong)i);
            await Pipes_ReadWriteValues(firstWaitToRead, 10,i => (long)i);
            await Pipes_ReadWriteValues(firstWaitToRead, 10,i => (float)i);
            await Pipes_ReadWriteValues(firstWaitToRead, 10,i => (double)i);
            await Pipes_ReadWriteValues(firstWaitToRead, 10,i => (char)i);
            await Pipes_ReadWriteValues(firstWaitToRead, 10,i => (decimal)i);
            await Pipes_ReadWriteValues(firstWaitToRead, 10,i => i % 2 == 0);
            await Pipes_ReadWriteValues(firstWaitToRead, 10,i => i.ToString());
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => Pipes_ReadWriteValues(firstWaitToRead, 10, i => DateTime.UtcNow));
        }

        private static async Task Pipes_ReadWriteValues<T>(bool firstWaitToRead, int numItems, Func<int, T> getValue)
        {
            using (AnonymousPipeServerStream serverPipe = new AnonymousPipeServerStream(PipeDirection.Out))
            using (AnonymousPipeClientStream clientPipe = new AnonymousPipeClientStream(PipeDirection.In, serverPipe.ClientSafePipeHandle))
            {
                IWritableChannel<T> writer = Channel.WriteToStream<T>(serverPipe);
                IReadableChannel<T> reader = Channel.ReadFromStream<T>(clientPipe);

                for (int i = 0; i < numItems; i++)
                {
                    T itemToWrite = getValue(i);

                    Task<T> readItem = firstWaitToRead ?
                        reader.WaitToReadAsync().ContinueWith(_ => reader.ReadAsync().AsTask()).Unwrap() :
                        reader.ReadAsync().AsTask();
                    Task writeItem = writer.WriteAsync(itemToWrite);
                    await Task.WhenAll(readItem, writeItem);

                    Assert.Equal(itemToWrite, readItem.Result);
                }

                writer.Complete();
                Assert.False(await reader.WaitToReadAsync());
                await reader.Completion;
            }
        }

        [Fact]
        public void WaitToWriteAsync_Completed_SynchronousResult()
        {
            using (AnonymousPipeServerStream serverPipe = new AnonymousPipeServerStream(PipeDirection.Out))
            {
                IWritableChannel<int> writer = Channel.WriteToStream<int>(serverPipe);
                AssertSynchronousTrue(writer.WaitToWriteAsync());

                writer.Complete();
                AssertSynchronousFalse(writer.WaitToWriteAsync());

                var cts = new CancellationTokenSource();
                cts.Cancel();
                AssertSynchronouslyCanceled(writer.WaitToWriteAsync(cts.Token), cts.Token);
            }
        }

        [Fact]
        public void WriteToStream_Precancellation()
        {
            IWritableChannel<int> w = Channel.WriteToStream<int>(new MemoryStream());
            var cts = new CancellationTokenSource();
            cts.Cancel();
            AssertSynchronouslyCanceled(w.WaitToWriteAsync(cts.Token), cts.Token);
            AssertSynchronouslyCanceled(w.WriteAsync(42, cts.Token), cts.Token);
        }

        [Fact]
        public void ReadFromStream_Precancellation()
        {
            IReadableChannel<int> r = Channel.ReadFromStream<int>(new MemoryStream());
            var cts = new CancellationTokenSource();
            cts.Cancel();
            AssertSynchronouslyCanceled(r.WaitToReadAsync(cts.Token), cts.Token);
            AssertSynchronouslyCanceled(r.ReadAsync(cts.Token).AsTask(), cts.Token);
        }

        [Fact]
        private static void Pipes_EnumerateValues()
        {
            using (AnonymousPipeServerStream serverPipe = new AnonymousPipeServerStream(PipeDirection.Out))
            using (AnonymousPipeClientStream clientPipe = new AnonymousPipeClientStream(PipeDirection.In, serverPipe.ClientSafePipeHandle))
            {
                IWritableChannel<int> writer = Channel.WriteToStream<int>(serverPipe);
                IReadableChannel<int> reader = Channel.ReadFromStream<int>(clientPipe);

                Task.WaitAll(
                    Task.Run(async () =>
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            await writer.WriteAsync(i);
                        }
                        writer.Complete();
                        Assert.False(writer.TryWrite(100));
                    }),
                    Task.Run(async () =>
                    {
                        int i = 0;
                        IAsyncEnumerator<int> e = reader.GetAsyncEnumerator();
                        while (await e.MoveNextAsync())
                        {
                            Assert.Equal(i++, e.Current);
                        }
                    }));
            }
        }

        [Fact]
        private static void Pipes_WaitForReadThenTryReadValues()
        {
            using (AnonymousPipeServerStream serverPipe = new AnonymousPipeServerStream(PipeDirection.Out))
            using (AnonymousPipeClientStream clientPipe = new AnonymousPipeClientStream(PipeDirection.In, serverPipe.ClientSafePipeHandle))
            {
                IWritableChannel<int> writer = Channel.WriteToStream<int>(serverPipe);
                IReadableChannel<int> reader = Channel.ReadFromStream<int>(clientPipe);

                Task.WaitAll(
                    Task.Run(async () =>
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            await writer.WriteAsync(i);
                        }
                        writer.Complete();
                        Assert.False(writer.TryWrite(100));
                    }),
                    Task.Run(async () =>
                    {
                        int result;
                        int i = 0;
                        while (await reader.WaitToReadAsync())
                        {
                            if (reader.TryRead(out result))
                            {
                                Assert.Equal(i++, result);
                            }
                        }
                        Assert.False(reader.TryRead(out result));
                    }));
            }
        }

        [Fact]
        public async Task TryWrite_AfterComplete()
        {
            IWritableChannel<int> c = Channel.WriteToStream<int>(new MemoryStream());
            Assert.True(c.TryComplete());
            Assert.False(c.TryWrite(42));
            AssertSynchronousFalse(c.WaitToWriteAsync());
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => c.WriteAsync(42));
            Assert.False(c.TryComplete());
        }

    }
}