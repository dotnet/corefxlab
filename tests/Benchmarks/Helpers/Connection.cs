// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.IO.Pipelines
{
    public class PipeConnection : IPipeConnection
    {
        public PipeConnection(PipeOptions pipeOptions)
        {
            Input = new ResetablePipe(pipeOptions);
            Output = new ResetablePipe(pipeOptions);
        }

        PipeReader IPipeConnection.Input => Input.Reader;
        PipeWriter IPipeConnection.Output => Output.Writer;

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
