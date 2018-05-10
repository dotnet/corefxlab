﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.IO.Enumeration;
using System.IO.FileSystem;
using Xunit;


public partial class PollingFileSystemWatcherDerivedTests
{
    [Fact]
    public static void ShouldIncludeEntryTest()
    {
        string currentDir = Environment.CurrentDirectory;
        string subDirectory = new DirectoryInfo(currentDir).CreateSubdirectory("sub").FullName;

        DerivedWatcher watcher2 = new DerivedWatcher(currentDir)
        {
            PollingInterval = 1
        };
        watcher2.Start();

        try
        {
            Assert.True(watcher2.ShouldIncludeEntryCalled);
            Assert.False(watcher2.ShouldRecurseIntoEntryCalled);
        }
        finally
        {
            Directory.Delete(subDirectory, true);
        }
    }

    [Fact]
    public static void ShouldRecurseIntoEntryTest()
    {
        string currentDir = Environment.CurrentDirectory;
        string subDirectory = new DirectoryInfo(currentDir).CreateSubdirectory("sub").FullName;

        DerivedWatcher watcher2 = new DerivedWatcher(currentDir, options: new EnumerationOptions { RecurseSubdirectories = true })
        {
            PollingInterval = 1
        };
        watcher2.Start();

        try
        {
            Assert.True(watcher2.ShouldIncludeEntryCalled);
            Assert.True(watcher2.ShouldRecurseIntoEntryCalled);
        }
        finally
        {
            Directory.Delete(subDirectory, true);
        }
    }
}


public class DerivedWatcher : PollingFileSystemWatcher
{
    public DerivedWatcher(string path, string filter = "*.*", EnumerationOptions options = null) : base(path, filter, options)
    {
    }

    public bool ShouldIncludeEntryOverride { get; set; }
    public bool ShouldRecurseIntoEntryOverride { get; set; }

    public bool ShouldIncludeEntryCalled { get; set; }
    public bool ShouldRecurseIntoEntryCalled { get; set; }

    protected override bool ShouldIncludeEntry(ref FileSystemEntry entry)
    {
        ShouldIncludeEntryCalled = true;

        return base.ShouldIncludeEntry(ref entry);
    }

    protected override bool ShouldRecurseIntoEntry(ref FileSystemEntry entry)
    {
        ShouldRecurseIntoEntryCalled = true;

        return base.ShouldRecurseIntoEntry(ref entry);
    }
}
