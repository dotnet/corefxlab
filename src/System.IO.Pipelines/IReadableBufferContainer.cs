using System;
using System.Collections.Generic;
using System.Text;

namespace System.IO.Pipelines
{
    public interface IReadableBufferContainer
    {
        ReadableBuffer Buffer { get; }
    }
}
