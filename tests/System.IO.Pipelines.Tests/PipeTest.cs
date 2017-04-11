// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.IO.Pipelines.Tests
{
    public class PipeTest: IDisposable
    {
        protected const int MaximumSizeHigh = 65;

        protected IPipe Pipe;
        private readonly PipeFactory _pipeFactory;

        public PipeTest()
        {
            _pipeFactory = new PipeFactory();
            Pipe = _pipeFactory.Create(new PipeOptions()
            {
                MaximumSizeHigh = 65,
                MaximumSizeLow = 6
            });
        }

        public void Dispose()
        {
            Pipe.Writer.Complete();
            Pipe.Reader.Complete();
            _pipeFactory.Dispose();
        }
    }
}