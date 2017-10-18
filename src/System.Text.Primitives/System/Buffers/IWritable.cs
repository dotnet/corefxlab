// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Buffers.Text;

namespace System.Buffers
{
    public interface IWritable
    {
        bool TryWrite(Span<byte> buffer, out int written, ParsedFormat format = default);
    }
}
