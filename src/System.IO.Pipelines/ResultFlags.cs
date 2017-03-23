namespace System.IO.Pipelines
{
    [Flags]
    internal enum ResultFlags : byte
    {
        None = 0,
        Cancelled = 1,
        Completed = 2
    }
}