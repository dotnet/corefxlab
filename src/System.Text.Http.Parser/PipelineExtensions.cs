// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Text.Http.Parser.Internal
{
    internal static class PipelineExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> ToSpan(this ReadableBuffer buffer)
        {
            if (buffer.IsSingleSpan)
            {
                return buffer.First.Span;
            }
            return buffer.ToArray();
        }
    }
}