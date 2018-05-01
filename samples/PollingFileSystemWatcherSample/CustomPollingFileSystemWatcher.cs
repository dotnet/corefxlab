using System;
using System.IO;
using System.IO.Enumeration;
using System.IO.FileSystem;

namespace PollingFileSystemWatcherSample
{
    public class CustomPollingFileSystemWatcher : PollingFileSystemWatcher
    {
        public CustomPollingFileSystemWatcher(string path)
            : base(path, options: new EnumerationOptions { RecurseSubdirectories = true })
        { }

        [Obsolete]
        public new string Filter => throw new NotSupportedException("Use Filters instead");

        public string[] Filters { get; set; }

        protected override bool ShouldIncludeEntry(ref FileSystemEntry entry)
        {
            if (entry.IsDirectory) return false;
            if (Filters == null || Filters.Length == 0) return true;

            foreach (var filter in Filters)
            {
                if (FileSystemName.MatchesSimpleExpression(filter, entry.FileName, ignoreCase: true))
                    return true;
            }

            return false;
        }

        // Only enter subdirectories that start with an underscore
        protected override bool ShouldRecurseIntoEntry(ref FileSystemEntry entry)
        {
            return (entry.FileName[0] == '_');
        }
    }
}
