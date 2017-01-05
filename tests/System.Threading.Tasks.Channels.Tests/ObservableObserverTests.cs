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

    }
}
