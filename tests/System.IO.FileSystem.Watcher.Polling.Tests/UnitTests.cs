// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.IO.FileSystem;
using System.Threading;
using Xunit;

public partial class PollingWatcherUnitTests
{
    [Fact]
    public static void FileSystemWatcher_Created_File()
    {
        var currentDir = Directory.GetCurrentDirectory();
        string fileName = Guid.NewGuid().ToString();

        var watcher = new PollingWatcher(currentDir, false, 100);
        watcher.ChangedDetailed += (changes) =>
        {
            Assert.Equal(1, changes.Length);
            var change = changes[0];
            Assert.Equal(ChangeType.Created, change.ChangeType);
            Assert.Equal(fileName, change.Name);
            Assert.Equal(currentDir, change.Directory);
        };
        watcher.Start();
        Thread.Sleep(200);
        using (var file = new TemporaryTestFile(fileName))
        {
            Thread.Sleep(200);
            watcher.Dispose();
            Thread.Sleep(200);
        }
    }

    [Fact]
    public static void FileSystemWatcher_Deleted_File() 
    {
        var currentDir = Directory.GetCurrentDirectory();
        string fileName = Guid.NewGuid().ToString();

        var watcher = new PollingWatcher(currentDir, false, 100);

        using (var file = new TemporaryTestFile(fileName))
        {
            watcher.ChangedDetailed += (changes) =>
            {
                Assert.Equal(1, changes.Length);
                var change = changes[0];
                Assert.Equal((byte)ChangeType.Deleted, (byte)change.ChangeType);
                Assert.Equal(fileName, change.Name);
                Assert.Equal(currentDir, change.Directory);
            };
            Thread.Sleep(100);
            watcher.Start();
            Thread.Sleep(200);
        }

        Thread.Sleep(200);
        watcher.Dispose();
        Thread.Sleep(200);
    }

    [Fact]
    public static void FileSystemWatcher_Changed_File() 
    {
        var currentDir = Directory.GetCurrentDirectory();
        string fileName = Guid.NewGuid().ToString();

        var watcher = new PollingWatcher(currentDir, false, 100);

        using (var file = new TemporaryTestFile(fileName))
        {
            watcher.Start();
            Thread.Sleep(200);
            watcher.ChangedDetailed += (changes) =>
            {
                Assert.Equal(1, changes.Length);
                var change = changes[0];
                Assert.Equal(ChangeType.Changed, change.ChangeType);
                Assert.Equal(fileName, change.Name);
                Assert.Equal(currentDir, change.Directory);
            };
            Thread.Sleep(200);
            file.WriteByte(100);
            Thread.Sleep(200);
            watcher.Dispose();
            Thread.Sleep(200);
        }
    }
}
