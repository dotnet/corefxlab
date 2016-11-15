namespace System.IO.Pipelines
{
    public struct ReadResult
    {
        public ReadResult(ReadableBuffer buffer, bool isCompleted)
        {
            Buffer = buffer;
            IsCompleted = isCompleted;
        }

        public ReadableBuffer Buffer { get; }

        public bool IsCompleted { get; }
    }
}