// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.IO.FileSystem;
using System.Threading;
using Xunit;

public partial class PollingFileSystemWatcherUnitTests
{
    [Fact]
    public static void FileSystemWatcher_Created_File()
    {
        var currentDir = Directory.GetCurrentDirectory();
        string fileName = Path.GetRandomFileName();
        bool eventRaised = false;
        var signal = new AutoResetEvent(false);

        var watcher = new PollingFileSystemWatcher(currentDir) { PollingIntervalInMilliseconds = 0 };
        watcher.ChangedDetailed += (e, changes) =>
        {
            Assert.Equal(1, changes.Changes.Length);
            var change = changes.Changes[0];
            Assert.Equal(ChangeType.Created, change.ChangeType);
            Assert.Equal(fileName, change.Name);
            Assert.Equal(currentDir, change.Directory);
            eventRaised = true;
            signal.Set();
        };

        watcher.Start();
        using (var file = File.Create(fileName)) { }
        signal.WaitOne(1000);
        using (var waitHandle = new ManualResetEvent(false))
        {
            watcher.Dispose(waitHandle);
        }

        try
        {
            Assert.True(eventRaised);
        }
        finally
        {
            File.Delete(fileName);
        }
    }

    [Fact]
    public static void FileSystemWatcher_Deleted_File()
    {
        var currentDir = Directory.GetCurrentDirectory();
        string fileName = Path.GetRandomFileName();
        bool eventRaised = false;
        var signal = new AutoResetEvent(false);

        var watcher = new PollingFileSystemWatcher(currentDir) { PollingIntervalInMilliseconds = 0 };
        watcher.ChangedDetailed += (e, changes) =>
        {
            Assert.Equal(1, changes.Changes.Length);
            var change = changes.Changes[0];
            Assert.Equal(ChangeType.Deleted, change.ChangeType);
            Assert.Equal(fileName, change.Name);
            Assert.Equal(currentDir, change.Directory);
            eventRaised = true;
            signal.Set();
        };

        using (var file = File.Create(fileName)) { }
        watcher.Start();
        File.Delete(fileName);
        signal.WaitOne(1000);
        using (var waitHandle = new ManualResetEvent(false))
        {
            watcher.Dispose(waitHandle);
        }

        Assert.True(eventRaised);
    }

    [Fact]
    public static void FileSystemWatcher_Changed_File()
    {
        var currentDir = Directory.GetCurrentDirectory();
        string fileName = Path.GetRandomFileName();
        bool eventRaised = false;
        var signal = new AutoResetEvent(false);

        var watcher = new PollingFileSystemWatcher(currentDir) { PollingIntervalInMilliseconds = 0 };
        watcher.ChangedDetailed += (e, changes) =>
        {
            Assert.Equal(1, changes.Changes.Length);
            var change = changes.Changes[0];
            Assert.Equal(ChangeType.Changed, change.ChangeType);
            Assert.Equal(fileName, change.Name);
            Assert.Equal(currentDir, change.Directory);
            eventRaised = true;
            signal.Set();
        };

        using (var file = File.Create(fileName)) { }
        watcher.Start();
        File.AppendAllText(fileName, ".");
        signal.WaitOne(1000);
        using (var waitHandle = new ManualResetEvent(false))
        {
            watcher.Dispose(waitHandle);
        }

        try
        {
            Assert.True(eventRaised);
        }
        finally
        {
            File.Delete(fileName);
        }
    }
}
