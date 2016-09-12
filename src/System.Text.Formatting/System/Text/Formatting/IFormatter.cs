// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace System.Text.Formatting
{
    // this interface would be implemented by types that want to support formatting, i.e. TextWriter/StringBuilder-like types.
    // the interface is used by an extension method in IFormatterExtensions.
    // One thing I am not sure here is if it's ok for these APIs to be synchronous, but I guess I will wait till I find a concrete issue with this.
    public interface IStream
    {
        Span<byte> AvaliableBytes { get; }
        void Advance(int byteCount);

        /// <summary>Returns false if there is backpressure and the request can be fullfiled later. Throws if the request cannot be fullfiled.</summary>
        bool TryEnsureAvaliable(int minimunByteCount = 1);
    }

    // this interface would be implemented by types that want to support formatting, i.e. TextWriter/StringBuilder-like types.
    // the interface is used by an extension method in IFormatterExtensions.
    // One thing I am not sure here is if it's ok for these APIs to be synchronous, but I guess I will wait till I find a concrete issue with this.
    public interface IFormatter : IStream
    {       
        FormattingData FormattingData { get; }
    }
}
