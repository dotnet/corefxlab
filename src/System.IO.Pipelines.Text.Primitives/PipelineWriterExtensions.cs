// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;

namespace System.IO.Pipelines.Text.Primitives
{
    public static class PipelineWriterExtensions
    {
        public static PipelineTextOutput AsTextOutput(this IPipeWriter writer, TextEncoder formattingData)
        {
            return new PipelineTextOutput(writer, formattingData);
        }
    }
}
