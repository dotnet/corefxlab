// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading;
using Xunit;

public partial class PollingFileSystemWatcherUnitTests
{
    private const int MillisecondsTimeout = 1000;

    [Fact]
    public static void FileSystemWatcher_ctor_Defaults()
    {
        string path = Environment.CurrentDirectory;
        using (var watcher = new PollingFileSystemWatcher(path))
        {
            Assert.Equal(path, watcher.Path);
            Assert.Equal("*", watcher.Filter);
            Assert.NotNull(watcher.EnumerationOptions);
            Assert.Equal(1000, watcher.PollingInterval);
        }
    }

    [Fact]
    public static void FileSystemWatcher_ctor_OptionalParams()
    {
        string currentDir = Directory.GetCurrentDirectory();
        const string filter = "*.csv";
        using (var watcher = new PollingFileSystemWatcher(currentDir, filter, new EnumerationOptions { RecurseSubdirectories = true }))
        {
            Assert.Equal(currentDir, watcher.Path);
            Assert.Equal(filter, watcher.Filter);
            Assert.True(watcher.EnumerationOptions.RecurseSubdirectories);
        }
    }

    [Fact]
    public static void FileSystemWatcher_ctor_Null()
    {
        // Not valid
        Assert.Throws<ArgumentNullException>("path", () => new PollingFileSystemWatcher(null));
        Assert.Throws<ArgumentNullException>("filter", () => new PollingFileSystemWatcher(Environment.CurrentDirectory, null));

        // Valid
        using (var watcher = new PollingFileSystemWatcher(Environment.CurrentDirectory, options: null)) { }
    }

    [Fact]
    public static void FileSystemWatcher_ctor_PathDoesNotExist()
    {
        Assert.Throws<ArgumentException>(() => new PollingFileSystemWatcher(@"Z:\RandomPath\sdsdljdkkjdfsdlcjfskdcvnj"));
    }

    [Fact(Skip = "Active issue: https://github.com/dotnet/corefxlab/issues/420")]
    public static void FileSystemWatcher_Created_File()
    {
        string currentDir = Utility.GetRandomDirectory();
        string fileName = Path.GetRandomFileName();
        string fullName = Path.Combine(currentDir, fileName);
        bool eventRaised = false;

        using (PollingFileSystemWatcher watcher = new PollingFileSystemWatcher(currentDir) { PollingInterval = 0 })
        {
            AutoResetEvent signal = new AutoResetEvent(false);

            watcher.ChangedDetailed += (e, changes) =>
            {
                Assert.Equal(1, changes.Changes.Length);
                FileChange change = changes.Changes[0];
                Assert.Equal(WatcherChangeTypes.Created, change.ChangeType);
                Assert.Equal(fileName, change.Name);
                Assert.Equal(currentDir, change.Directory);
                eventRaised = true;
                watcher.PollingInterval = Timeout.Infinite;
                signal.Set();
            };

            watcher.Start();
            using (FileStream file = File.Create(fullName)) { }
            signal.WaitOne(MillisecondsTimeout);
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
    public static void FileSystemWatcher_Deleted_File()
    {
        string currentDir = Utility.GetRandomDirectory();
        string fileName = Path.GetRandomFileName();
        string fullName = Path.Combine(currentDir, fileName);
        bool eventRaised = false;

        using (PollingFileSystemWatcher watcher = new PollingFileSystemWatcher(currentDir) { PollingInterval = 0 })
        {
            AutoResetEvent signal = new AutoResetEvent(false);

            watcher.ChangedDetailed += (e, changes) =>
            {
                Assert.Equal(1, changes.Changes.Length);
                FileChange change = changes.Changes[0];
                Assert.Equal(WatcherChangeTypes.Deleted, change.ChangeType);
                Assert.Equal(fileName, change.Name);
                Assert.Equal(currentDir, change.Directory);
                eventRaised = true;
                watcher.PollingInterval = Timeout.Infinite;
                signal.Set();
            };

            using (FileStream file = File.Create(fullName)) { }
            watcher.Start();
            File.Delete(fullName);
            signal.WaitOne(MillisecondsTimeout);
        }

        Assert.True(eventRaised);

        Directory.Delete(currentDir, true);
    }

    [Fact(Skip = "Active issue: https://github.com/dotnet/corefxlab/issues/420")]
    public static void FileSystemWatcher_Changed_File()
    {
        string currentDir = Utility.GetRandomDirectory();
        string fileName = Path.GetRandomFileName();
        string fullName = Path.Combine(currentDir, fileName);
        bool eventRaised = false;

        using (PollingFileSystemWatcher watcher = new PollingFileSystemWatcher(currentDir) { PollingInterval = 0 })
        {
            AutoResetEvent signal = new AutoResetEvent(false);

            watcher.ChangedDetailed += (e, changes) =>
            {
                Assert.Equal(1, changes.Changes.Length);
                FileChange change = changes.Changes[0];
                Assert.Equal(WatcherChangeTypes.Changed, change.ChangeType);
                Assert.Equal(fileName, change.Name);
                Assert.Equal(currentDir, change.Directory);
                eventRaised = true;
                watcher.PollingInterval = Timeout.Infinite;
                signal.Set();
            };

            using (FileStream file = File.Create(fullName)) { }
            watcher.Start();
            File.AppendAllText(fullName, ".");
            signal.WaitOne(MillisecondsTimeout);
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
    public static void FileSystemWatcher_Filter()
    {
        string currentDir = Utility.GetRandomDirectory();
        string fileName = $"{Path.GetRandomFileName()}.csv";
        string fullName = Path.Combine(currentDir, fileName);
        bool eventRaised = false;

        using (PollingFileSystemWatcher watcher = new PollingFileSystemWatcher(currentDir, filter: "*.csv") { PollingInterval = 1 })
        {
            AutoResetEvent signal = new AutoResetEvent(false);

            watcher.ChangedDetailed += (e, changes) =>
            {
                Assert.Equal(1, changes.Changes.Length);
                FileChange change = changes.Changes[0];
                Assert.Equal(WatcherChangeTypes.Created, change.ChangeType);
                Assert.Equal(fileName, change.Name);
                Assert.Equal(currentDir, change.Directory);
                eventRaised = true;
                watcher.PollingInterval = Timeout.Infinite;
                signal.Set();
            };

            watcher.Start();

            using (FileStream file = File.Create(fullName)) { }
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
    public static void FileSystemWatcher_PollingInterval_ChangeBeforeStart()
    {
        string currentDir = Utility.GetRandomDirectory();
        string fileName = Path.GetRandomFileName();
        string fullName = Path.Combine(currentDir, fileName);
        bool eventRaised = false;

        using (PollingFileSystemWatcher watcher = new PollingFileSystemWatcher(currentDir) { PollingInterval = Timeout.Infinite })
        {
            AutoResetEvent signal = new AutoResetEvent(false);

            watcher.Changed += (e, changes) =>
            {
                eventRaised = true;
                watcher.PollingInterval = Timeout.Infinite;
                signal.Set();
            };

            watcher.PollingInterval = 0;
            watcher.Start();

            using (FileStream file = File.Create(fullName)) { }
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
    public static void FileSystemWatcher_PollingInterval_ChangeAfterStart()
    {
        string currentDir = Utility.GetRandomDirectory();
        string fileName = Path.GetRandomFileName();
        string fullName = Path.Combine(currentDir, fileName);
        bool eventRaised = false;

        using (PollingFileSystemWatcher watcher = new PollingFileSystemWatcher(currentDir) { PollingInterval = Timeout.Infinite })
        {
            AutoResetEvent signal = new AutoResetEvent(false);

            watcher.Changed += (e, changes) =>
            {
                eventRaised = true;
                watcher.PollingInterval = Timeout.Infinite;
                signal.Set();
            };

            watcher.Start();

            using (FileStream file = File.Create(fullName)) { }
            watcher.PollingInterval = 0;
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
    public static void FileSystemWatcher_Recursive()
    {
        string currentDir = Utility.GetRandomDirectory();
        string fileName = Path.GetRandomFileName();
        string subDirectory = new DirectoryInfo(currentDir).CreateSubdirectory("sub").FullName;
        string fullName = Path.Combine(subDirectory, fileName);

        bool eventRaised = false;

        using (PollingFileSystemWatcher watcher = new PollingFileSystemWatcher(currentDir, options: new EnumerationOptions { RecurseSubdirectories = true }) { PollingInterval = 1 })
        {
            AutoResetEvent signal = new AutoResetEvent(false);

            watcher.Error += (e, error) =>
            {
                throw  error.GetException();
            };

            watcher.ChangedDetailed += (e, changes) =>
            {
                Assert.Equal(1, changes.Changes.Length);
                FileChange change = changes.Changes[0];
                Assert.Equal(WatcherChangeTypes.Created, change.ChangeType);
                Assert.Equal(fileName, change.Name);
                Assert.Equal(subDirectory, change.Directory);
                eventRaised = true;
                watcher.PollingInterval = Timeout.Infinite;
                signal.Set();
            };

            watcher.Start();

            using (FileStream file = File.Create(fullName)) { }
            signal.WaitOne(10000);
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

    [Fact]
    public static void FileSystemWatcher_MultipleDispose()
    {
        var watcher = new PollingFileSystemWatcher(Environment.CurrentDirectory);
        watcher.Dispose();
        watcher.Dispose();
    }

    [Fact]
    public static void FileSystemWatcher_DisposeBeforeStart()
    {
        var watcher = new PollingFileSystemWatcher(Environment.CurrentDirectory);
        watcher.Dispose();
        Assert.Throws<ObjectDisposedException>(() => watcher.Start());
    }

    [Fact]
    public static void FileSystemWatcher_DisposeAfterStart()
    {
        var watcher = new PollingFileSystemWatcher(Environment.CurrentDirectory);
        watcher.Start();
        watcher.Dispose();
    }

    [Fact]
    public static void FileSystemWatcher_MultipleDisposeWithWaitHandle()
    {
        ManualResetEvent resetEvent = new ManualResetEvent(false);
        var watcher = new PollingFileSystemWatcher(Environment.CurrentDirectory);

        var isSuccessful = watcher.Dispose(resetEvent);
        Assert.True(isSuccessful);

        resetEvent.Reset();
        isSuccessful = watcher.Dispose(resetEvent);
        Assert.False(isSuccessful);
    }

    [Fact]
    public static void FileSystemWatcher_DisposeBeforeStartWithWaitHandle()
    {
        ManualResetEvent resetEvent = new ManualResetEvent(false);
        var watcher = new PollingFileSystemWatcher(Environment.CurrentDirectory);
        watcher.Dispose(resetEvent);
        Assert.Throws<ObjectDisposedException>(() => watcher.Start());
    }

    [Fact]
    public static void FileSystemWatcher_DisposeAfterStartWithWaitHandle()
    {
        ManualResetEvent resetEvent = new ManualResetEvent(false);
        var watcher = new PollingFileSystemWatcher(Environment.CurrentDirectory) { PollingInterval = 1 };
        watcher.Start();
        var isSuccessful = watcher.Dispose(resetEvent);
        Assert.True(isSuccessful);
    }
}
