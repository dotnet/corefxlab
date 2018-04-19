// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Enumeration;
using System.IO.FileSystem.Watcher.Polling.Properties;
using System.Threading;

//TODO: add support for UNC paths
//TODO: write real tests

namespace System.IO.FileSystem
{
    /// <summary>
    /// PollingWatcher can be used to monitor changes to a file system directory
    /// </summary>
    /// <remarks>
    /// This type is similar to FileSystemWatcher, but unlike FileSystemWatcher it is fully reliable,
    /// at the cost of some performance overhead. 
    /// Instead of relying on Win32 file notification APIs, it periodically scans the watched directory to discover changes.
    /// This means that sooner or later it will discover every change.
    /// FileSystemWatcher's Win32 APIs can drop some events in rare circumstances, which is often an acceptable compromise.
    /// In scenarios where events cannot be missed, PollingWatcher should be used.
    /// Note: When a watched file is renamed, one or two notifications will be made.
    /// Note: When no changes are detected, PollingWatcher will not allocate memory on the GC heap.
    /// </remarks>
    public class PollingWatcher : IDisposable
    {
        public TraceSource Tracing { get; private set; }
        Stopwatch _stopwatch = new Stopwatch();
        Timer _timer;
        int _pollingIntervalInMilliseconds;
        bool _includeSubdirectories;

        List<string> _extensionsToWatch;
        string _directory;
        PathToFileStateHashtable _state; // stores state of the directory
        byte _version; // this is used to keep track of removals. // TODO: describe the algorithm
        static string[] SpecialFiles = new[] { "", ".", ".." };

        /// <summary>
        /// Creates an instance of a watcher
        /// </summary>
        /// <param name="directory">The directory to watch. It does not support UNC paths (yet).</param>
        /// <param name="includeSubdirectories">A bool controlling whether or not subdirectories will be watched too</param>
        /// <param name="pollingIntervalInMilliseconds">Polling interval</param>
        public PollingWatcher(string directory, bool includeSubdirectories = false, int pollingIntervalInMilliseconds = 1000)
        {
            if (!Directory.Exists(directory))
                throw new ArgumentException("Directory not found.", nameof(directory));

            Tracing = new TraceSource("PollingWatcher");
            _state = new PathToFileStateHashtable(Tracing);
            _pollingIntervalInMilliseconds = pollingIntervalInMilliseconds;
            _includeSubdirectories = includeSubdirectories;
            _directory = directory;
        }

        /// <summary>
        /// Extensions to watch
        /// </summary>
        /// <param name="extension">for example "txt", "doc", etc., i.e don't use wildcards</param>
        /// <remarks>
        /// By default all extensions are watched. Once this method is called, only extensions in added are watched.
        /// </remarks>
        public void AddExtension(string extension)
        {
            if (_timer != null)
            {
                throw new InvalidOperationException(Strings.InvalidOperation_Extension);
            }
            if (_extensionsToWatch == null)
            {
                _extensionsToWatch = new List<string>();
            }
            _extensionsToWatch.Add($".{extension}");
        }

        public void Start()
        {
            ComputeChangesAndUpdateState(); // captures the initial state
            _timer = new Timer(new TimerCallback(TimerHandler), null, _pollingIntervalInMilliseconds, Timeout.Infinite);
        }

        // This function walks all watched files, collects changes, and updates state
        private FileChangeList ComputeChangesAndUpdateState()
        {
            _version++;

            var enumerator = new FileSystemChangeEnumerator(this, _directory);
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
            if (_extensionsToWatch == null) return true;
            foreach (var extension in _extensionsToWatch)
            {
                if (Path.GetExtension(entry.FileName).SequenceEqual(extension))
                    return true;
            }
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
            try
            {
                _stopwatch.Restart();
                var changes = ComputeChangesAndUpdateState();
                var lastCycleTicks = _stopwatch.ElapsedTicks;
                if (Tracing.Switch.ShouldTrace(TraceEventType.Information))
                {
                    Tracing.TraceEvent(TraceEventType.Information, 1, "Last polling cycle {0}ms", lastCycleTicks * 1000 / Stopwatch.Frequency);
                    Tracing.TraceEvent(TraceEventType.Information, 6, "Changes detected {0}", changes.Count);
                }

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
            }
            catch (Exception e)
            {
                Tracing.TraceEvent(TraceEventType.Error, 0, e.ToString());
            }

            if (Tracing.Switch.ShouldTrace(TraceEventType.Verbose))
            {
                Tracing.TraceEvent(TraceEventType.Verbose, 3, "Number of names watched: {0}", _state.Count);
            }

            _timer.Change(_pollingIntervalInMilliseconds, Timeout.Infinite);
        }
    }
}
