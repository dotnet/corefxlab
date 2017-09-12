// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Xunit;

namespace System.Threading.Tasks.Channels.Tests
{
    public class ObservableObserverTests : TestBase
    {
        [Fact]
        public void AsObservable_InvalidSubscribe_ThrowsException()
        {
            var c = Channel.CreateUnbounded<int>();
            IObservable<int> o = c.In.AsObservable();
            Assert.Throws<ArgumentNullException>("observer", () => o.Subscribe(null));
        }

        [Fact]
        public void AsObservable_Subscribe_Dispose_Success()
        {
            var c = Channel.CreateUnbounded<int>();
            IObservable<int> o = c.In.AsObservable();

            using (o.Subscribe(new DelegateObserver<int>()))
            {
            }

            using (o.Subscribe(new DelegateObserver<int>()))
            using (o.Subscribe(new DelegateObserver<int>()))
            using (o.Subscribe(new DelegateObserver<int>()))
            {
            }
        }

        [Fact]
        public void AsObservable_Subscribe_DisposeMultipleTimes_Success()
        {
            var c = Channel.CreateUnbounded<int>();
            IObservable<int> o = c.In.AsObservable();

            IDisposable d = o.Subscribe(new DelegateObserver<int>());
            d.Dispose();
            d.Dispose();
        }

        [Fact]
        public async Task AsObservable_SubscribeUnsubscribeSubscribe_NoItemsMissed()
        {
            var c = Channel.CreateUnbounded<int>();

            int total = 0;
            Action<int> addToTotal = i => Interlocked.Add(ref total, i);
            var tcs = new TaskCompletionSource<bool>();

            await c.Out.WriteAsync(1);

            IObservable<int> o = c.In.AsObservable();

            await c.Out.WriteAsync(2);

            IDisposable d = o.Subscribe(new DelegateObserver<int> { OnNextDelegate = addToTotal });

            await c.Out.WriteAsync(3);

            d.Dispose();

            await c.Out.WriteAsync(4);
            await Task.Delay(250);

            d = o.Subscribe(new DelegateObserver<int> { OnNextDelegate = addToTotal, OnCompletedDelegate = () => tcs.SetResult(true) });

            await c.Out.WriteAsync(5);

            c.Out.Complete();
            await tcs.Task;

            Assert.Equal(15, total);
        }

        [Fact]
        public async Task ReaderCompetingWithObserver_AllItemsConsumed()
        {
            const int Items = 1000;
            var results = new HashSet<int>();
            var tcs = new TaskCompletionSource<bool>();

            var c = Channel.CreateBounded<int>(1);

            var writer = Task.Run(async () =>
            {
                for (int i = 0; i < Items; i++)
                {
                    await c.Out.WriteAsync(i);
                }
                c.Out.Complete();
            });

            c.In.AsObservable().Subscribe(new DelegateObserver<int>
            {
                OnNextDelegate = i => 
                {
                    lock (results)
                    {
                        results.Add(i);
                    }
                },
                OnCompletedDelegate = () => tcs.TrySetResult(true)
            });

            while (await c.In.WaitToReadAsync())
            {
                if (c.In.TryRead(out int item))
                {
                    lock (results)
                    {
                        results.Add(item);
                    }
                }
            }

            await tcs.Task;
            await writer;

            Assert.Equal(Items, results.Count);
        }
    }
}
