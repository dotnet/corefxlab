// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO.Enumeration;
using System.Runtime.InteropServices;
using System.Threading;

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
        /// <param name="path">The path to watch.</param>
        /// <param name="filter">The type of files to watch. For example, "*.txt" watches for changes to all text files.</param>
        public PollingFileSystemWatcher(string path, string filter = "*.*", EnumerationOptions options = null)
        {
            if (!Directory.Exists(path))
                throw new ArgumentException("Path not found.", nameof(path));

            _state = new PathToFileStateHashtable();
            Path = path;
            Filter = filter;
            EnumerationOptions = null ?? new EnumerationOptions();
        }

        public EnumerationOptions EnumerationOptions { get; set; } = new EnumerationOptions();
        public string Filter { get; set; } = "*.*";
        public string Path { get; set; }
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

            var enumerator = new FileSystemChangeEnumerator(this, Path, EnumerationOptions);
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

        protected internal virtual bool ShouldIncludeEntry(ref FileSystemEntry entry)
        {
            if (entry.IsDirectory) return false;
            if (Filter == null) return true;

            bool ignoreCase = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            if (FileSystemName.MatchesSimpleExpression(Filter, entry.FileName, ignoreCase: ignoreCase))
                return true;

            return false;
        }

        protected internal virtual bool ShouldRecurseIntoEntry(ref FileSystemEntry entry)
        {
            return true;
        }

        internal void UpdateState(string directory, ref FileChangeList changes, ref FileSystemEntry file)
        {
            int index = _state.IndexOf(directory, file.FileName);
            if (index == -1) // file added
            {
                string path = file.FileName.ToString();

                changes.AddAdded(directory, path.ToString());

                var newFileState = new FileState(directory, path);
                newFileState.LastWriteTimeUtc = file.LastWriteTimeUtc;
                newFileState.Length = file.Length;
                newFileState._version = _version;
                _state.Add(directory, path, newFileState);
                return;
            }

            _state.Values[index]._version = _version;

            var previousState = _state.Values[index];
            if (file.LastWriteTimeUtc != previousState.LastWriteTimeUtc || file.Length != previousState.Length)
            {
                changes.AddChanged(directory, previousState.Path);
                _state.Values[index].LastWriteTimeUtc = file.LastWriteTimeUtc;
                _state.Values[index].Length = file.Length;
            }
        }

        /// <summary>
        /// This callback is called when any change (Created, Deleted, Changed) is detected in any watched file.
        /// </summary>
        public event EventHandler Changed;

        public event PollingFileSystemEventHandler ChangedDetailed;

        public event ErrorEventHandler Error;

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
                var changes = ComputeChangesAndUpdateState();

                if (!changes.IsEmpty)
                {
                    Changed?.Invoke(this, EventArgs.Empty);
                    ChangedDetailed?.Invoke(this, new PollingFileSystemEventArgs(changes.ToArray()));
                }
            }
            catch (Exception e)
            {
                Error?.Invoke(this, new ErrorEventArgs(e));
            }

            _timer.Change(PollingIntervalInMilliseconds, Timeout.Infinite);
        }
    }
}
