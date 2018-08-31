// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;

namespace System.Text.Formatting
{
    // this interface would be implemented by types that want to support formatting, i.e. TextWriter/StringBuilder-like types.
    // the interface is used by an extension method in IFormatterExtensions.
    // One thing I am not sure here is if it's ok for these APIs to be synchronous, but I guess I will wait till I find a concrete issue with this.
    public interface ITextBufferWriter : IBufferWriter<byte>
    {
        SymbolTable SymbolTable { get; }
    }
}
