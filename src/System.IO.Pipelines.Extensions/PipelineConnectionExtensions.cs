// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.IO.Pipelines
{
    public static class PipelineConnectionExtensions
    {
        public static Stream GetStream(this IDuplexPipe connection)
        {
            return new PipelineConnectionStream(connection);
        }
    }
}
