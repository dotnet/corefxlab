// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace System.Text.Formatting
{
    // this interface would be implemented by types that want to support formatting, i.e. TextWriter/StringBuilder-like types.
    // the interface is used by an extension method in IFormatterExtensions.
    // One thing I am not sure here is if it's ok for these APIs to be synchronous, but I guess I will wait till I find a concrete issue with this.
    public interface IFormatter
    {
        Span<byte> FreeBuffer { get; }
        void CommitBytes(int bytes);

        void ResizeBuffer();

        FormattingData FormattingData { get; }
    }
}
