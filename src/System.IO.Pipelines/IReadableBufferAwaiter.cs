using System;

namespace System.IO.Pipelines
{
    public interface IReadableBufferAwaiter
    {
        bool IsCompleted { get; }

        ReadResult GetResult();

        void OnCompleted(Action continuation);
    }
}
