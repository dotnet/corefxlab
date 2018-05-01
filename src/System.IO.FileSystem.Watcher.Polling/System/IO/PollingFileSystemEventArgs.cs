namespace System.IO.FileSystem
{
    public delegate void PollingFileSystemEventHandler(object sender, PollingFileSystemEventArgs e);

    public class PollingFileSystemEventArgs : EventArgs
    {
        public PollingFileSystemEventArgs(FileChange[] changes)
        {
            Changes = changes;
        }

        public FileChange[] Changes { get; }
    }
}
