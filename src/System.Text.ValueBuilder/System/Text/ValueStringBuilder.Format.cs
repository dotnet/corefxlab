// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace System.Text
{
    public ref partial struct ValueStringBuilder
    {
        // Undocumented exclusive limits on the range for Argument Hole Index and Argument Hole Alignment.
        private const int IndexLimit = 1000000; // Note:            0 <= ArgIndex < IndexLimit
        private const int WidthLimit = 1000000; // Note:  -WidthLimit <  ArgAlign < WidthLimit

        // This is a copy of the StringBuilder.AppendFormatHelper with minor functional tweaks:
        //
        //  1. Has a small stackalloc span for formatting Variant value types into.
        //  2. Doesn't work with ISpanFormattable (the interface is currently internal).
        //  3. Uses Variant to format with no allocations for value types.
        //  4. Takes FormatString instead of string for format.
        //  5. Takes ReadOnlySpan<Variant> instead of ParamsArray.
        //  6. Code formatting is scrubbed a bit for clarity.

        // Note that Argument Hole parsing can be factored into a helper, perhaps taking a callback
        // delegate with (int index, int width, ReadOnlySpan<char> itemFormat) or something along those
        // lines. This would, of course, make the code a little slower, but the advantage of having
        // shareable logic (between StringBuilder, ValueStringBuilder, etc.) may be worth it.

        public unsafe void Append(FormatString format, ReadOnlySpan<Variant> args, IFormatProvider provider = null)
        {
            ReadOnlySpan<char> formatSpan = format.Format;

            int position = 0;
            int length = formatSpan.Length;
            char current = '\x0';
            ValueStringBuilder unescapedItemFormat = default;

            // Can't do an inline stackalloc as we can't express that we won't capture the span.
            char* c = stackalloc char[32];
            Span<char> initialBuffer = new Span<char>(c, 32);
            ValueStringBuilder formatBuilder = new ValueStringBuilder(initialBuffer);

            ICustomFormatter customFormatter = null;
            if (provider != null)
            {
                customFormatter = (ICustomFormatter)provider.GetFormat(typeof(ICustomFormatter));
            }

            while (true)
            {
                // Scan for an argument hole (braces)
                while (position < length)
                {
                    current = formatSpan[position];
                    position++;

                    if (current == '}')
                    {
                        if (position < length && formatSpan[position] == '}')
                        {
                            // Escaped brace (}}), skip
                            position++;
                        }
                        else
                        {
                            // Mismatched closing brace
                            FormatError();
                        }
                    }

                    if (current == '{')
                    {
                        if (position < length && formatSpan[position] == '{')
                        {
                            // Escaped brace ({{), skip
                            position++;
                        }
                        else
                        {
                            // Opening brace of an argument hole, fall out
                            position--;
                            break;
                        }
                    }

                    // Plain text (or escaped brace).
                    Append(current);
                }

                if (position == length)
                {
                    // No arguments, exit
                    break;
                }

                //
                // Start of parsing of Argument Hole.
                // Argument Hole ::= { Index (, WS* Alignment WS*)? (: Formatting)? }
                //

                int index = 0;

                // Parse required Index parameter.
                // Index ::= ('0'-'9')+ WS*
                {
                    position++;
                    if (position == length || (current = formatSpan[position]) < '0' || current > '9')
                    {
                        // Need at least one digit
                        FormatError();
                    }

                    do
                    {
                        index = index * 10 + current - '0';
                        position++;
                        if (position == length)
                        {
                            // End of text (can't have a closing brace)
                            FormatError();
                        }
                        current = formatSpan[position];
                    } while (current >= '0' && current <= '9' && index < IndexLimit);

                    if (index >= args.Length)
                    {
                        throw new FormatException("Index (zero based) must be greater than or equal to zero and less than the size of the argument list.");
                    }

                    // Consume optional whitespace.
                    while (position < length && (current = formatSpan[position]) == ' ')
                    {
                        position++;
                    }
                }

                bool leftJustify = false;
                int width = 0;

                // Parse optional Alignment
                //  Alignment ::= comma WS* minus? ('0'-'9')+ WS*
                {
                    // Is the character a comma, which indicates the start of alignment parameter.
                    if (current == ',')
                    {
                        position++;

                        // Consume optional whitespace
                        while (position < length && formatSpan[position] == ' ')
                        {
                            position++;
                        }

                        if (position == length)
                        {
                            // End of text (can't have a closing brace)
                            FormatError();
                        }

                        current = formatSpan[position];
                        if (current == '-')
                        {
                            // Minus sign means alignment is left justified.
                            leftJustify = true;
                            position++;
                            if (position == length)
                            {
                                // End of text (can't have a closing brace)
                                FormatError();
                            }
                            current = formatSpan[position];
                        }

                        if (current < '0' || current > '9')
                        {
                            // Need at least one digit
                            FormatError();
                        }

                        do
                        {
                            width = width * 10 + current - '0';
                            position++;
                            if (position == length)
                            {
                                // End of text (can't have a closing brace)
                                FormatError();
                            }

                            current = formatSpan[position];
                        }
                        while (current >= '0' && current <= '9' && width < WidthLimit);
                    }

                    // Consume optional whitespace
                    while (position < length && (current = formatSpan[position]) == ' ')
                    {
                        position++;
                    }
                }

                ReadOnlySpan<char> itemFormatSpan = default;

                // Parse optional formatting parameter. (colon)
                if (current == ':')
                {
                    position++;
                    int startPosition = position;

                    while (true)
                    {
                        if (position == length)
                        {
                            // End of text (can't have a closing brace)
                            FormatError();
                        }
                        current = formatSpan[position];
                        position++;

                        // Is character a opening or closing brace?
                        if (current == '}' || current == '{')
                        {
                            if (current == '{')
                            {
                                if (position < length && formatSpan[position] == '{')
                                {
                                    // Escaped brace ({{), skip
                                    position++;
                                }
                                else
                                {
                                    // Error Argument Holes can not be nested.
                                    FormatError();
                                }
                            }
                            else
                            {
                                // Closing brace

                                if (position < length && formatSpan[position] == '}')
                                {
                                    // Escaped brace (}}), skip
                                    position++;
                                }
                                else
                                {
                                    // Closing brace of the argument hole.
                                    position--;
                                    break;
                                }
                            }

                            // Reaching here means the brace has been escaped
                            // so we need to build up the format string in segments
                            unescapedItemFormat.Append(formatSpan.Slice(startPosition, position - startPosition - 1));
                            startPosition = position;
                        }
                    }

                    if (unescapedItemFormat.Length == 0)
                    {
                        if (startPosition != position)
                        {
                            // There was no brace escaping, extract the item format as a single string
                            itemFormatSpan = formatSpan.Slice(startPosition, position - startPosition);
                        }
                    }
                    else
                    {
                        unescapedItemFormat.Append(formatSpan.Slice(startPosition, position - startPosition));
                        itemFormatSpan = unescapedItemFormat.ToString();
                        unescapedItemFormat.Length = 0;
                    }
                }

                if (current != '}')
                {
                    // Missing closing argument brace
                    FormatError();
                }

                Variant arg = args[index];

                // Construct the output for this argument hole.
                position++;
                ReadOnlySpan<char> formattedSpan = default;

                if (customFormatter != null)
                {
                    string itemFormat = null;
                    if (itemFormatSpan.Length != 0)
                    {
                        itemFormat = new string(itemFormatSpan);
                    }
                    formattedSpan = customFormatter.Format(itemFormat, arg.Box(), provider);
                }
                else
                {
                    if (!arg.TryFormat(ref formatBuilder, itemFormatSpan, provider))
                    {
                        Debug.Fail($"Failed to format index {index} with format span of '{new string(itemFormatSpan)}'");
                    }
                    formattedSpan = formatBuilder.AsSpan();
                }

                int padding = width - formattedSpan.Length;

                if (!leftJustify && padding > 0)
                {
                    Append(' ', padding);
                }

                Append(formattedSpan);

                if (leftJustify && padding > 0)
                {
                    Append(' ', padding);
                }

                // Continue to parse other characters.
            }

            unescapedItemFormat.Dispose();
            formatBuilder.Dispose();
        }

        private static void FormatError()
        {
            throw new FormatException("Input string was not in a correct format.");
        }
    }
}
