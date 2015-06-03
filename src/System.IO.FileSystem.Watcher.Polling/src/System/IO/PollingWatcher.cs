// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

//TODO: there is a bug where directory or file is creted, deleted, recreated and then deleted again

namespace System.IO.FileSystem
{
    /// <summary>
    /// PollingWatcher can be used to monitor changes to a file system directory
    /// </summary>
    /// <remarks>
    /// This type is similar to FileSystemWatcher, but unlike FileSystemWatcher it is fully reliable,
    /// at the cost of some perfromance overhead. 
    /// Instead of relying on Win32 file notification APIs, it preodically scans the watched directory to discover changes.
    /// This means that sooner or later it will discover every change.
    /// FileSystemWatcher's Win32 APIs can drop some events in rare circumstances, which is often acceptable compromise.
    /// In scenarios where events cannot be missed, PollingWatcher should be used.
    /// Note: When a watched file is renamed, one or two notifications will be made.
    /// Note: When no changes are detected, PollingWatcher will not allocate memory on the GC heap.
    /// </remarks>
    public class PollingWatcher : IDisposable
    {
        Stopwatch _stopwatch = new Stopwatch();
        long _lastCycleTicks;

        Timer _timer;
        int _pollingIntervalInMilliseconds;
        bool _includeSubdirectories;

        List<string> _directories = new List<string>();
        PathToFileStateHashtable _state = new PathToFileStateHashtable(); // stores state of the directory
        byte _version; // this is used to keep track of removals. // TODO: describe the algorithm

        /// <summary>
        /// Creates an instance of a watcher
        /// </summary>
        /// <param name="rootDirectory">The directory to watch. It does not support UNC paths (yet).</param>
        /// <param name="pollingIntervalInMilliseconds">Polling interval</param>
        public PollingWatcher(string rootDirectory, bool includeSubdirectories = false, int pollingIntervalInMilliseconds = 1000)
        {
            _pollingIntervalInMilliseconds = pollingIntervalInMilliseconds;
            _includeSubdirectories = includeSubdirectories;
            _directories.Add(ToDirectoryFormat(rootDirectory));

            // TODO: this needs to be updated when directories are added or removed
            if (includeSubdirectories) {
                DiscoverAllSubdirectories(rootDirectory);
            }

            ComputeChangesAndUpdateState(); // captures the initial state

            _timer = new Timer(new TimerCallback(TimerHandler), null, pollingIntervalInMilliseconds, Timeout.Infinite);
        }

        private FileChangeList ComputeChangesAndUpdateState()
        {
            _version++;
            var changes = new FileChangeList();

            WIN32_FIND_DATAW fileData = new WIN32_FIND_DATAW();
            unsafe
            {
                WIN32_FIND_DATAW* pFileData = &fileData;

                foreach (var directory in _directories) {
                    var file = DllImports.FindFirstFileExW(directory, FINDEX_INFO_LEVELS.FindExInfoBasic, pFileData, FINDEX_SEARCH_OPS.FindExSearchNameMatch, IntPtr.Zero, DllImports.FIND_FIRST_EX_LARGE_FETCH);
                    if (file == DllImports.INVALID_HANDLE_VALUE) {
                        throw new IOException();
                    }

                    do {
                        if (IsSpecial(fileData.cFileName)) continue;
                        UpdateState(directory, ref changes, ref fileData, fileData.cFileName);
                    }
                    while (DllImports.FindNextFileW(file, pFileData));

                    // TODO: this should be safe handle probably
                    DllImports.FindClose(file);
                }
            }

            // TODO: should these be moved to the front of the change list? Otherwise it might be difficult to detect moves/renames
            foreach (var value in _state) {
                if (value._version != _version) {
                    changes.AddRemoved(value.Directory, value.Path);
                    _state.Remove(value.Directory, value.Path);
                }
            }

            return changes;
        }

        // returns true for empty, '.', and '..' 
        private unsafe bool IsSpecial(char* cFileName)
        {
            if (cFileName[0] == 0) return true;
            if (cFileName[0] == '.') {
                if (cFileName[1] == 0) return true;
                if (cFileName[1] == '.' && cFileName[2] == 0) return true;
            }
            return false;
        }

        private unsafe void UpdateState(string directory, ref FileChangeList changes, ref WIN32_FIND_DATAW file, char* filename)
        {
            int index = _state.IndexOf(directory, filename);
            if (index == -1) // file added
            {
                string path = new string(filename);

                var newFileState = new FileState(directory, path);
                newFileState.LastWrite = file.LastWrite;
                newFileState.FileSize = file.FileSize;
                newFileState._version = _version;

                _state.Add(directory, path, newFileState);
                changes.AddAdded(directory, path);
                return;
            }

            var previousState = _state.Values[index];
            if (file.LastWrite != previousState.LastWrite || file.FileSize != previousState.FileSize) {
                changes.AddChanged(directory, previousState.Path);
                _state.Values[index].LastWrite = file.LastWrite;
                _state.Values[index].FileSize = file.FileSize;
            }

            _state.Values[index]._version = _version;
        }

        /// <summary>
        /// This callback is called when any change (Created, Deleted, Changed) is detected in any watched file.
        /// </summary>
        public Action Changed;

        public Action<FileChange[]> ChangedDetailed;

        public long LastCycleTicks
        {
            get
            {
                return _lastCycleTicks;
            }
        }

        /// <summary>
        /// Disposes the timer used for polling.
        /// </summary>
        public void Dispose()
        {
            _timer.Dispose();
        }

        private void TimerHandler(object context)
        {
            var changedHandler = Changed;
            var ChangedDetailedHandler = ChangedDetailed;

            if (changedHandler != null || ChangedDetailedHandler != null) {
                _stopwatch.Restart();
                var changes = ComputeChangesAndUpdateState();
                _lastCycleTicks = _stopwatch.ElapsedTicks;
                if (!changes.IsEmpty) {
                    if (changedHandler != null) {
                        changedHandler();
                    }
                    if (ChangedDetailedHandler != null) {
                        ChangedDetailedHandler(changes.ToArray());
                    }
                }
            }

            _timer.Change(_pollingIntervalInMilliseconds, Timeout.Infinite);
        }

        private void DiscoverAllSubdirectories(string root)
        {
            // TODO: maybe this should use the Win32 APIs too?
            // TODO: this need sot guard against exceptions from access denied 
            foreach (var directory in Directory.EnumerateDirectories(root)) {
                _directories.Add(ToDirectoryFormat(directory));
                DiscoverAllSubdirectories(directory);
            }
        }

        // TODO: this needs to be smarter for UNC paths
        private static string ToDirectoryFormat(string path)
        {
            return @"\\?\" + path + @"\*";
        }
    }
}
