// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;

namespace System.IO
{
    public class FormattingTextWriter : TextWriter
    {
        private Encoding _encoding;

        public FormattingTextWriter(Encoding encoding)
        {
            _encoding = encoding;
        }

        public override Encoding Encoding => _encoding;

        /// <remarks>
        /// If you were to call Write($"Some string {value}"), C# would see that we have a Write() that takes
        /// <see cref="FormatString"/> and would prefer that. It would create a <see cref="Variant"/> around
        /// value and pass it via <see cref="Variant.ToSpan(in Variant)"/>.
        /// 
        /// Additionally, if you call Write($"Some string {value}", CultureInfo.CurrentCulture) it would get
        /// transformed into a call to this specific method. General rules:
        /// 
        ///  1. When invoking a method with an interpolated string...
        ///  2. If there is an overload that matches with FormatString, ReadOnlySpan(Variant) in the $ position...
        ///  3. Use it
        /// </remarks>
        public void Write(FormatString format, ReadOnlySpan<Variant> args, IFormatProvider formatProvider = null)
        {
            ValueStringBuilder vsb = new ValueStringBuilder(format.Length);
            vsb.Append(format, args, formatProvider);

            Write(vsb.AsSpan());
            vsb.Dispose();
        }
    }
}
