// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;
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
        public TraceSource Tracing { get; private set; }
        Stopwatch _stopwatch = new Stopwatch();
        Timer _timer;
        int _pollingIntervalInMilliseconds;
        bool _includeSubdirectories;

        List<string> _extensionsToWatch;
        List<string> _directories = new List<string>();
        PathToFileStateHashtable _state; // stores state of the directory
        byte _version; // this is used to keep track of removals. // TODO: describe the algorithm

        /// <summary>
        /// Creates an instance of a watcher
        /// </summary>
        /// <param name="rootDirectory">The directory to watch. It does not support UNC paths (yet).</param>
        /// <param name="includeSubdirectories">A bool controlling whether or not subdirectories will be watched too</param>
        /// <param name="pollingIntervalInMilliseconds">Polling interval</param>
        public PollingWatcher(string rootDirectory, bool includeSubdirectories = false, int pollingIntervalInMilliseconds = 1000)
        {
            Tracing = new TraceSource("PollingWatcher");
            _state = new PathToFileStateHashtable(Tracing); 
            _pollingIntervalInMilliseconds = pollingIntervalInMilliseconds;
            _includeSubdirectories = includeSubdirectories;
            _directories.Add(ToDirectoryFormat(rootDirectory));
        }

        /// <summary>
        /// Extensions to watch
        /// </summary>
        /// <param name="extension">for examople "txt", "doc", etc., i.e don't use wildcards</param>
        /// <remarks>
        /// By default all extensions are watched. Once this method is called, only extensions in added are watched.
        /// </remarks>
        public void AddExtension(string extension)
        {
            if(_timer != null)
            {
                throw new InvalidOperationException(Strings.InvalidOperation_Extension);
            }
            if(_extensionsToWatch == null)
            {
                _extensionsToWatch = new List<string>();
            }
            _extensionsToWatch.Add(extension);
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
            var changes = new FileChangeList();

            WIN32_FIND_DATAW fileData = new WIN32_FIND_DATAW();
            unsafe
            {
                WIN32_FIND_DATAW* pFileData = &fileData;

                for(int index=0; index < _directories.Count; index++) {
                    var directory = _directories[index];

                    var handle = DllImports.FindFirstFileExW(directory, FINDEX_INFO_LEVELS.FindExInfoBasic, pFileData, FINDEX_SEARCH_OPS.FindExSearchNameMatch, IntPtr.Zero, DllImports.FIND_FIRST_EX_LARGE_FETCH);
                    if (handle == DllImports.INVALID_HANDLE_VALUE) { // directory got deleted 
                        if (Tracing.Switch.ShouldTrace(TraceEventType.Information))
                        {
                            Tracing.TraceEvent(TraceEventType.Warning, 2, "Directory could not be opened {0}", directory);
                        }
                        _directories.Remove(directory);
                        continue;
                    }

                    try
                    {
                        do
                        {
                            if (IsSpecial(fileData.cFileName)) continue;
                            if (!IsWatched(fileData.cFileName)) continue;
                            UpdateState(directory, ref changes, ref fileData, fileData.cFileName);
                        }
                        while (DllImports.FindNextFileW(handle, pFileData));
                    }
                    finally
                    {
                        DllImports.FindClose(handle);
                    }
                }
            }

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

        private unsafe static int GetLength(char* nullTerminatedString)
        {
            int length = -1;
            while (true)
            {
                if (nullTerminatedString[++length] == 0)
                {
                    break;
                }
            }
            return length;
        }
        private unsafe static bool EndsWith(char* nullTerminatedString, int length, string possibleEnding)
        {
            if(possibleEnding.Length > length)
            {
                return false;
            }

            var start = nullTerminatedString + (length - possibleEnding.Length);

            for(int i=0; i<possibleEnding.Length; i++)
            {
                if (start[i] != possibleEnding[i]) return false;
            }
            return true;
        }

        private unsafe bool IsWatched(char* filename)
        {
            if (_extensionsToWatch == null) return true;
            var length = GetLength(filename);
            foreach(var extension in _extensionsToWatch)
            {
                if(EndsWith(filename, length, extension))
                {
                    return true;
                }
            }
            return false;
        }

        private unsafe void UpdateState(string directory, ref FileChangeList changes, ref WIN32_FIND_DATAW file, char* filename)
        {
            int index = _state.IndexOf(directory, filename);
            if (index == -1) // file added
            {
                string path = new string(filename);

                if (file.IsDirectory) {
                    if (_includeSubdirectories) {
                        _directories.Add(Path.Combine(directory.TrimEnd('*'), path, "*"));
                    }
                }

                changes.AddAdded(directory, path);
                
                var newFileState = new FileState(directory, path);
                newFileState.LastWrite = file.LastWrite;
                newFileState.FileSize = file.FileSize;
                newFileState._version = _version;
                _state.Add(directory, path, newFileState);
                return;
            }

            _state.Values[index]._version = _version;

            if (file.IsDirectory) {
                return;
            }

            var previousState = _state.Values[index];
            if (file.LastWrite != previousState.LastWrite || file.FileSize != previousState.FileSize) {
                changes.AddChanged(directory, previousState.Path);
                _state.Values[index].LastWrite = file.LastWrite;
                _state.Values[index].FileSize = file.FileSize;
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
            try {
                _stopwatch.Restart();
                var changes = ComputeChangesAndUpdateState();
                var lastCycleTicks = _stopwatch.ElapsedTicks;
                if (Tracing.Switch.ShouldTrace(TraceEventType.Information))
                {
                    Tracing.TraceEvent(TraceEventType.Information, 1, "Last polling cycle {0}ms", lastCycleTicks*1000/Stopwatch.Frequency);
                    Tracing.TraceEvent(TraceEventType.Information, 6, "Changes detected {0}", changes.Count);
                }

                var changedHandler = Changed;
                var ChangedDetailedHandler = ChangedDetailed;

                if (changedHandler != null || ChangedDetailedHandler != null)
                {
                    if (!changes.IsEmpty)
                    {
                        if (changedHandler != null)
                        {
                            changedHandler();
                        }
                        if (ChangedDetailedHandler != null)
                        {
                            ChangedDetailedHandler(changes.ToArray());
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Tracing.TraceEvent(TraceEventType.Error, 0, e.ToString());
            }

            if (Tracing.Switch.ShouldTrace(TraceEventType.Verbose))
            {
                Tracing.TraceEvent(TraceEventType.Verbose, 3, "Number of names watched: {0}", _state.Count);
                Tracing.TraceEvent(TraceEventType.Verbose, 4, "Number of directories watched: {0}", _directories.Count);
            }

            _timer.Change(_pollingIntervalInMilliseconds, Timeout.Infinite);
        }

        private static string ToDirectoryFormat(string path)
        {
            return @"\\?\" + path + @"\*";
        }
    }
}
