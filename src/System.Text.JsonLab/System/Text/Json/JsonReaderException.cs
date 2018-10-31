// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.JsonLab
{
    public class JsonReaderException : Exception
    {
        //TODO: Consider adding line number and position to the message itself
        public JsonReaderException(string message, long lineNumber, long lineBytePosition) : base(message)
        {
            LineNumber = lineNumber;
            LineBytePosition = lineBytePosition;
        }

        public long LineNumber { get; }

        public long LineBytePosition { get; }

        //TODO: Should we add a path string (allocating a stack/etc)?
    }
}
