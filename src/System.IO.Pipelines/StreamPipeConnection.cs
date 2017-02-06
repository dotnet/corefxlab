// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

namespace System.IO.Pipelines
{
    internal class StreamPipeConnection : IPipeConnection
    {
        public StreamPipeConnection(PipeFactory factory, Stream stream)
        {
            Input = factory.CreateReader(stream);
            Output = factory.CreateWriter(stream);
        }

        public IPipeReader Input { get; }

        public IPipeWriter Output { get; }

        public void Dispose()
        {
            Input.Complete();
            Output.Complete();
        }
    }
}