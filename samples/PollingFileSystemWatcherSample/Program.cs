using System;
using System.IO;
using System.IO.FileSystem;

namespace PollingFileSystemWatcherSample
{
    class Program
    {
        static void Main(string[] args)
        {
            string Path = Environment.CurrentDirectory;
            Console.WriteLine($"Current directory: {Path}");

            var watcher = new PollingFileSystemWatcher(Path, "*.txt", new EnumerationOptions { RecurseSubdirectories = true });
            watcher.ChangedDetailed += Watcher_ChangedDetailed;
            watcher.Start();

            var customWatcher = new CustomPollingFileSystemWatcher(Path)
            {
                Filters = new string[] { "*.txt", "*.csv" }
            };
            customWatcher.ChangedDetailed += CustomWatcher_ChangedDetailed;
            customWatcher.Start();

            Console.WriteLine("Ready!");
            Console.ReadKey();
        }

        private static void Watcher_ChangedDetailed(object sender, PollingFileSystemEventArgs e)
        {
            var ec = e.Changes[0];
            Console.WriteLine($"Watcher: File \"{ec.Name}\" in \"{ec.Directory}\" in \"{ec.Path}\" was \"{ec.ChangeType}\"");
        }

        private static void CustomWatcher_ChangedDetailed(object sender, PollingFileSystemEventArgs e)
        {
            var ec = e.Changes[0];
            Console.WriteLine($"Custom Watcher: File \"{ec.Name}\" in \"{ec.Directory}\" in \"{ec.Path}\" was \"{ec.ChangeType}\"");
        }
    }
}
