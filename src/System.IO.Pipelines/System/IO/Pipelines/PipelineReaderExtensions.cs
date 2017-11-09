// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.IO.Pipelines
{
    public static class PipelineReaderExtensions
    {
        public static void Advance(this IPipeReader input, ReadCursor cursor)
        {
            input.Advance(cursor, cursor);
        }
    }
}