// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Threading.Tasks.Channels.Tests
{
    public class ObservableObserverTests : TestBase
    {
        [Fact]
        public void AsObservable_InvalidSubscribe_ThrowsException()
        {
            IChannel<int> c = Channel.CreateUnbounded<int>();
            IObservable<int> o = c.AsObservable();
            Assert.Throws<ArgumentNullException>("observer", () => o.Subscribe(null));
        }

        [Fact]
        public void AsObservable_Subscribe_Dispose_Success()
        {
            IChannel<int> c = Channel.CreateUnbounded<int>();
            IObservable<int> o = c.AsObservable();

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
            IChannel<int> c = Channel.CreateUnbounded<int>();
            IObservable<int> o = c.AsObservable();

            IDisposable d = o.Subscribe(new DelegateObserver<int>());
            d.Dispose();
            d.Dispose();
        }

        [Fact]
        public async Task AsObservable_SubscribeUnsubscribeSubscribe_NoItemsMissed()
        {
            IChannel<int> c = Channel.CreateUnbounded<int>();

            int total = 0;
            Action<int> addToTotal = i => Interlocked.Add(ref total, i);
            var tcs = new TaskCompletionSource<bool>();

            await c.WriteAsync(1);

            IObservable<int> o = c.AsObservable();

            await c.WriteAsync(2);

            IDisposable d = o.Subscribe(new DelegateObserver<int> { OnNextDelegate = addToTotal });

            await c.WriteAsync(3);

            d.Dispose();

            await c.WriteAsync(4);
            await Task.Delay(250);

            d = o.Subscribe(new DelegateObserver<int> { OnNextDelegate = addToTotal, OnCompletedDelegate = () => tcs.SetResult(true) });

            await c.WriteAsync(5);

            c.Complete();
            await tcs.Task;

            Assert.Equal(15, total);
        }
    }
}
