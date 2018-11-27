// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Xunit;

public class PollingFileSystemWatcherSerializableTests
{
    [Fact]
    public void RoundTripPropertyTest()
    {
        string path = Environment.CurrentDirectory;
        string filter = "*.abc";
        EnumerationOptions options = new EnumerationOptions { RecurseSubdirectories = true };
        var watcher = new PollingFileSystemWatcher(path, filter, options)
        {
            PollingInterval = Timeout.Infinite
        };

        using (PollingFileSystemWatcher deserialized = RoundTrip(watcher))
        {
            Assert.Equal(path, deserialized.Path);
            Assert.Equal(filter, deserialized.Filter);
            Assert.Equal(Timeout.Infinite, deserialized.PollingInterval);
            Assert.Equal(options.RecurseSubdirectories, deserialized.EnumerationOptions.RecurseSubdirectories);
        }
        watcher.Dispose();
    }

    [Fact]
    public void RoundTripReturnSeparateObjectTest()
    {
        var watcher = new PollingFileSystemWatcher(Environment.CurrentDirectory) { PollingInterval = Timeout.Infinite };

        using (PollingFileSystemWatcher deserialized = RoundTrip(watcher))
        {
            watcher.PollingInterval = 0;

            Assert.False(ReferenceEquals(watcher, deserialized));
            Assert.Equal(Timeout.Infinite, deserialized.PollingInterval);
        }
        watcher.Dispose();
    }

    [Fact(Skip = "Active issue: https://github.com/dotnet/corefxlab/issues/420")]
    public void RoundTripBeforeStartedDoesNotAutomaticallyStartTest()
    {
        string currentDir = Utility.GetRandomDirectory();
        string fileName = Path.GetRandomFileName();
        string fullName = Path.Combine(currentDir, fileName);
        bool eventRaised = false;
        AutoResetEvent signal = new AutoResetEvent(false);
        var watcher = new PollingFileSystemWatcher(currentDir) { PollingInterval = 0 };

        using (PollingFileSystemWatcher deserialized = RoundTrip(watcher))
        {
            watcher.Dispose();

            deserialized.Changed += (e, args) =>
            {
                eventRaised = true;
                watcher.PollingInterval = Timeout.Infinite;
                signal.Set();
            };

            using (var file = File.Create(fullName)) { }
            signal.WaitOne(1000);
        }

        try
        {
            Assert.False(eventRaised);  // watcher did not automatically start
        }
        finally
        {
            Directory.Delete(currentDir, true);
        }
    }

    [Fact(Skip = "Active issue: https://github.com/dotnet/corefxlab/issues/420")]
    public void RoundTripBeforeStartedTest()
    {
        string currentDir = Utility.GetRandomDirectory();
        string fileName = Path.GetRandomFileName();
        string fullName = Path.Combine(currentDir, fileName);
        bool eventRaised = false;
        AutoResetEvent signal = new AutoResetEvent(false);
        var watcher = new PollingFileSystemWatcher(currentDir) { PollingInterval = 0 };

        using (PollingFileSystemWatcher deserialized = RoundTrip(watcher))
        {
            watcher.Dispose();

            deserialized.Changed += (e, args) =>
            {
                eventRaised = true;
                watcher.PollingInterval = Timeout.Infinite;
                signal.Set();
            };
            deserialized.Start();

            using (var file = File.Create(fullName)) { }
            signal.WaitOne(1000);
        }

        try
        {
            Assert.True(eventRaised);
        }
        finally
        {
            Directory.Delete(currentDir, true);
        }
    }

    [Fact(Skip = "Active issue: https://github.com/dotnet/corefxlab/issues/420")]
    public void RoundTripAfterStartedTest()
    {
        string currentDir = Utility.GetRandomDirectory();
        string fileName = Path.GetRandomFileName();
        string fullName = Path.Combine(currentDir, fileName);
        bool eventRaised = false;
        AutoResetEvent signal = new AutoResetEvent(false);
        var watcher = new PollingFileSystemWatcher(currentDir) { PollingInterval = 0 };
        watcher.Start();

        using (PollingFileSystemWatcher deserialized = RoundTrip(watcher))
        {
            watcher.Dispose();

            deserialized.Changed += (e, args) =>
            {
                eventRaised = true;
                watcher.PollingInterval = Timeout.Infinite;
                signal.Set();
            };

            using (var file = File.Create(fullName)) { }
            signal.WaitOne(1000);
        }

        try
        {
            Assert.True(eventRaised);
        }
        finally
        {
            Directory.Delete(currentDir, true);
        }
    }

    [Fact(Skip = "Active issue: https://github.com/dotnet/corefxlab/issues/420")]
    public void RoundTripAfterStartedDoesNotAffectOriginalTest()
    {
        string currentDir = Utility.GetRandomDirectory();
        string fileName = Path.GetRandomFileName();
        string fullName = Path.Combine(currentDir, fileName);
        bool eventRaised = false;
        AutoResetEvent signal = new AutoResetEvent(false);
        var watcher = new PollingFileSystemWatcher(currentDir) { PollingInterval = 0 };
        watcher.Start();

        using (PollingFileSystemWatcher deserialized = RoundTrip(watcher))
        {
            watcher.Changed += (e, args) =>
            {
                eventRaised = true;
                watcher.PollingInterval = Timeout.Infinite;
                signal.Set();
            };

            using (var file = File.Create(fullName)) { }
            signal.WaitOne(1000);
        }

        try
        {
            Assert.True(eventRaised);
        }
        finally
        {
            Directory.Delete(currentDir, true);
            watcher.Dispose();
        }
    }

    [Fact]
    public void RoundTripAfterDisposedTest()
    {
        var watcher = new PollingFileSystemWatcher(Environment.CurrentDirectory);
        watcher.Dispose();

        PollingFileSystemWatcher deserialized = RoundTrip(watcher);

        Assert.Throws<ObjectDisposedException>(() => deserialized.Start());
        deserialized.Dispose();
    }

    private static PollingFileSystemWatcher RoundTrip(PollingFileSystemWatcher watcher)
    {
        PollingFileSystemWatcher deserialized;
        IFormatter formatter = new BinaryFormatter();
        using (MemoryStream stream = new MemoryStream())
        {
            formatter.Serialize(stream, watcher);

            stream.Position = 0;
            deserialized = (PollingFileSystemWatcher)formatter.Deserialize(stream);
        }

        return deserialized;
    }
}
