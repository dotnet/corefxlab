// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Threading;

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

        string _directory;
        PathToFileStateHashtable _state = new PathToFileStateHashtable(); // stores state of the directory
        byte _version; // this is used to keep track of removals. // TODO: describe the algorithm

        /// <summary>
        /// Creates an instance of a watcher
        /// </summary>
        /// <param name="rootDirectory">The directory to watch. It does not support UNC paths (yet).</param>
        /// <param name="pollingIntervalInMilliseconds">Polling interval</param>
        public PollingWatcher(string rootDirectory, int pollingIntervalInMilliseconds = 1000)
        {
            _pollingIntervalInMilliseconds = pollingIntervalInMilliseconds;
            _directory = @"\\?\" + rootDirectory + @"\*"; // TODO: this needs to be smarter for UNC paths
            ComputeChangesAndUpdateState(_directory); // captures the initial state

            _timer = new Timer(new TimerCallback(TimerHandler), null, pollingIntervalInMilliseconds, Timeout.Infinite);
        }

        /// <summary>
        /// This callback is called when any change (Created, Deleted, Changed) is detected.
        /// </summary>
        public Action Changed;

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
            var handler = Changed;
            if (handler != null)
            {
                _stopwatch.Restart();
                var changes = ComputeChangesAndUpdateState(_directory);
                _lastCycleTicks = _stopwatch.ElapsedTicks;
                if (!changes.IsEmpty)
                {
                    handler();
                }
            }

            _timer.Change(_pollingIntervalInMilliseconds, Timeout.Infinite);
        }
        private FileChangeList ComputeChangesAndUpdateState(string directory)
        {
            _version++;
            var changes = new FileChangeList();

            WIN32_FIND_DATAW fileData = new WIN32_FIND_DATAW();
            unsafe
            {
                WIN32_FIND_DATAW* pFileData = &fileData;

                var file = DllImports.FindFirstFileW(_directory, pFileData);
                if (file == DllImports.INVALID_HANDLE_VALUE) {
                    throw new IOException();
                }

                do
                {
                    changes = UpdateState(ref changes, ref fileData, fileData.cFileName);
                }
                while (DllImports.FindNextFileW(file, pFileData));
                DllImports.FindClose(file);
            }

            foreach(var value in _state)
            {
                if(value._version != _version)
                {
                    changes.AddRemoved(value.Path);
                    _state.Remove(value.Path);
                }
            }

            return changes;
        }
        private unsafe FileChangeList UpdateState(ref FileChangeList changes, ref WIN32_FIND_DATAW file, char* filename)
        {
            int index = _state.IndexOf(filename);
            if(index == -1) // file added
            {
                string path = new string(filename);

                var newFileState = new FileState(path);
                newFileState.LastWrite = file.LastWrite;
                newFileState.FileSize = file.FileSize;
                newFileState._version = _version;

                _state.Add(path, newFileState);
                changes.AddAdded(path);
                return changes;
            }

            var previousState = _state.Values[index];
            if (file.LastWrite != previousState.LastWrite || file.FileSize != previousState.FileSize)
            {
                changes.AddChanged(previousState.Path);
                _state.Values[index].LastWrite = file.LastWrite;
                _state.Values[index].FileSize = file.FileSize;
            }

            _state.Values[index]._version = _version;
            return changes;
        }
    }
}
