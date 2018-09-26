// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.JsonLab
{
    public class JsonReaderException : Exception
    {
        //TODO: Consider adding line number and position to the message itself
        public JsonReaderException(string message, int lineNumber, int position) : base(message)
        {
            LineNumber = lineNumber;
            Position = position;
        }

        public int LineNumber { get; }

        public int Position { get; }

        //TODO: Should we add a path string (allocating a stack/etc)?
    }
}
