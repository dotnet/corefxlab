// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.IO.Pipelines
{
    public class DuplexPipe : IDuplexPipe
    {
        public DuplexPipe(PipeOptions pipeOptions)
        {
            Input = new Pipe(pipeOptions);
            Output = new Pipe(pipeOptions);
        }

        PipeReader IDuplexPipe.Input => Input.Reader;
        PipeWriter IDuplexPipe.Output => Output.Writer;

        public Pipe Input { get; }

        public Pipe Output { get; }

        public void Dispose()
        {
            Input.Reader.Complete();
            Input.Writer.Complete();
            Output.Reader.Complete();
            Output.Writer.Complete();
        }
    }
}
