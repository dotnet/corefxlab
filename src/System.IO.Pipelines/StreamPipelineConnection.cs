// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

namespace System.IO.Pipelines
{
    internal class StreamPipelineConnection : IPipelineConnection
    {
        public StreamPipelineConnection(PipelineFactory factory, Stream stream)
        {
            Input = factory.CreateReader(stream);
            Output = factory.CreateWriter(stream);
        }

        public IPipelineReader Input { get; }

        public IPipelineWriter Output { get; }

        public void Dispose()
        {
            Input.Complete();
            Output.Complete();
        }
    }
}