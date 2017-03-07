// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net.Sockets;

namespace System.IO.Pipelines
{
    public static class PipeFactoryExtensions
    {
        public static IPipeReader CreateReader(this PipeFactory factory, Stream stream)
        {
            if (!stream.CanRead)
            {
                throw new NotSupportedException();
            }

            var pipe = factory.Create();
            var ignore = stream.CopyToEndAsync(pipe.Writer);
            return pipe.Reader;
        }

        public static IPipeConnection CreateConnection(this PipeFactory factory, NetworkStream stream)
        {
            return new StreamPipeConnection(factory, stream);
        }

        public static IPipeWriter CreateWriter(this PipeFactory factory, Stream stream)
        {
            if (!stream.CanWrite)
            {
                throw new NotSupportedException();
            }

            var pipe = factory.Create();
            var ignore = pipe.Reader.CopyToEndAsync(stream);

            return pipe.Writer;
        }
    }


}