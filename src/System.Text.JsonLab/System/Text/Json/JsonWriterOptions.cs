// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.JsonLab
{
    public struct JsonWriterOptions
    {
        // Or Minified, Minimzed, PrettyPrinted,
        public bool Formatted { get; set; }

        public bool SkipValidation { get; set; }

        internal bool SlowPath => Formatted || !SkipValidation;
    }
}
