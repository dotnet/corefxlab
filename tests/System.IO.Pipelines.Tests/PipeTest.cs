// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.IO.Pipelines.Tests
{
    public class PipeTest: IDisposable
    {
        protected const int DefaultMaximumSizeHigh = 65;
        protected const int DefaultMaximumSizeLow = 6;

        protected IPipe Pipe;
        private readonly PipeFactory _pipeFactory;

        public PipeTest(long maximumSizeHigh = DefaultMaximumSizeHigh, long maximumSizeLow = DefaultMaximumSizeLow)
        {
            _pipeFactory = new PipeFactory();
            Pipe = _pipeFactory.Create(new PipeOptions()
            {
                MaximumSizeHigh = maximumSizeHigh,
                MaximumSizeLow = maximumSizeLow
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