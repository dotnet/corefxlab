// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace System.Text.Formatting
{
    public interface IBufferFormattable
    {
        // I went back and forth between bytesWritten being out and ref. Ref makes it easier to implement the interface.
        // Out makes so that callers don't have to trust calees doing the right thing with the bytesWritten value. 
        // I prefer correctness here over ease of use (as this is a low level API), so I decided on out parameter. 

            /// <summary>
            /// This interface should be implemented by types that want to support allocation-free formatting.
            /// </summary>
            /// <param name="buffer">The buffer to format the value into</param>
            /// <param name="format">This is a pre-parsed representation of the formatting string. It's preparsed for efficiency.</param>
            /// <param name="formattingData">This object provides bytes representing digits and symbols used during number formatting.</param>
            /// <param name="written">This parameter is used to return the number of bytes that were written to the buffer</param>
            /// <returns>False if the buffer was to small, otherwise true.</returns>
        bool TryFormat(Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int written);
    }
}
