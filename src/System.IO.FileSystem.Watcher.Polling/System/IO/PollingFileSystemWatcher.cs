// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO.Enumeration;
using System.Runtime.InteropServices;
using System.Threading;

//TODO: add support for UNC paths
//TODO: write real tests

namespace System.IO.FileSystem
{
    /// <summary>
    /// PollingFileSystemWatcher can be used to monitor changes to a file system directory
    /// </summary>
    /// <remarks>
    /// This type is similar to FileSystemWatcher, but unlike FileSystemWatcher it is fully reliable,
    /// at the cost of some performance overhead. 
    /// Instead of relying on Win32 file notification APIs, it periodically scans the watched directory to discover changes.
    /// This means that sooner or later it will discover every change.
    /// FileSystemWatcher's Win32 APIs can drop some events in rare circumstances, which is often an acceptable compromise.
    /// In scenarios where events cannot be missed, PollingFileSystemWatcher should be used.
    /// Note: When a watched file is renamed, one or two notifications will be made.
    /// Note: When no changes are detected, PollingFileSystemWatcher will not allocate memory on the GC heap.
    /// </remarks>
    public class PollingFileSystemWatcher : IDisposable
    {
        Timer _timer;
        PathToFileStateHashtable _state; // stores state of the directory
        byte _version; // this is used to keep track of removals. // TODO: describe the algorithm

        /// <summary>
        /// Creates an instance of a watcher
        /// </summary>
        /// <param name="directory">The directory to watch. It does not support UNC paths (yet).</param>
        /// <param name="filter">The type of files to watch. For example, "*.txt" watches for changes to all text files.</param>
        public PollingFileSystemWatcher(string directory, string filter = "*.*")
        {
            if (!IO.Directory.Exists(directory))
                throw new ArgumentException("Directory not found.", nameof(directory));

            _state = new PathToFileStateHashtable();
            Directory = directory;
            Filter = filter;
        }

        /// <summary>
        /// Creates an instance of a watcher
        /// </summary>
        /// <param name="directory">The directory to watch. It does not support UNC paths (yet).</param>
        /// <param name="includeSubdirectories">A bool controlling whether or not subdirectories will be watched too</param>
        /// <param name="pollingIntervalInMilliseconds">Polling interval</param>
        public PollingFileSystemWatcher(string directory, bool includeSubdirectories = false, int pollingIntervalInMilliseconds = 1000)
        {
            if (!IO.Directory.Exists(directory))
                throw new ArgumentException("Directory not found.", nameof(directory));

            _state = new PathToFileStateHashtable();
            PollingIntervalInMilliseconds = pollingIntervalInMilliseconds;
            IncludeSubdirectories = includeSubdirectories;
            Directory = directory;
            }

        public string Filter { get; set; } = "*.*";
        public bool IncludeSubdirectories { get; set; } = false;
        public string Directory { get; set; } = "";
        public int PollingIntervalInMilliseconds { get; set; } = 1000;

        public void Start()
        {
            ComputeChangesAndUpdateState(); // captures the initial state
            _timer = new Timer(new TimerCallback(TimerHandler), null, PollingIntervalInMilliseconds, Timeout.Infinite);
        }

        // This function walks all watched files, collects changes, and updates state
        private FileChangeList ComputeChangesAndUpdateState()
        {
            _version++;

            var enumerator = new FileSystemChangeEnumerator(this, Directory, new EnumerationOptions { RecurseSubdirectories = IncludeSubdirectories });
            while (enumerator.MoveNext())
            {
                // Ignore `.Current`
            }
            var changes = enumerator.Changes;

            foreach (var value in _state)
            {
                if (value._version != _version)
                {
                    changes.AddRemoved(value.Directory, value.Path);
                    _state.Remove(value.Directory, value.Path);
                }
            }

            return changes;
        }

        internal bool IsWatched(ref FileSystemEntry entry)
        {
            if (entry.IsDirectory) return false;
            if (Filter == null) return true;

            bool ignoreCase = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            if (FileSystemName.MatchesSimpleExpression(Filter, entry.FileName, ignoreCase: ignoreCase))
                    return true;

            return false;
        }

        internal void UpdateState(string directory, ref FileChangeList changes, ref FileSystemEntry file)
        {
            int index = _state.IndexOf(directory, file.FileName);
            if (index == -1) // file added
            {
                string path = file.FileName.ToString();

                changes.AddAdded(directory, path.ToString());

                var newFileState = new FileState(directory, path);
                newFileState.LastWrite = file.LastWriteTimeUtc;
                newFileState.FileSize = file.Length;
                newFileState._version = _version;
                _state.Add(directory, path, newFileState);
                return;
            }

            _state.Values[index]._version = _version;

            var previousState = _state.Values[index];
            if (file.LastWriteTimeUtc != previousState.LastWrite || file.Length != previousState.FileSize)
            {
                changes.AddChanged(directory, previousState.Path);
                _state.Values[index].LastWrite = file.LastWriteTimeUtc;
                _state.Values[index].FileSize = file.Length;
            }
        }

        /// <summary>
        /// This callback is called when any change (Created, Deleted, Changed) is detected in any watched file.
        /// </summary>
        public Action Changed;

        public Action<FileChange[]> ChangedDetailed;

        /// <summary>
        /// Disposes the timer used for polling.
        /// </summary>
        public void Dispose()
        {
            _timer.Dispose();
        }

        private void TimerHandler(object context)
        {
            var changes = ComputeChangesAndUpdateState();

            var changedHandler = Changed;
            var ChangedDetailedHandler = ChangedDetailed;

            if (changedHandler != null || ChangedDetailedHandler != null)
            {
                if (!changes.IsEmpty)
                {
                    changedHandler?.Invoke();
                    ChangedDetailedHandler?.Invoke(changes.ToArray());
                }
            }

            _timer.Change(PollingIntervalInMilliseconds, Timeout.Infinite);
        }
    }
}
