// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace System.CommandLine
{
    internal static class Splitter
    {
        internal static IReadOnlyList<string> Split(string commandLine)
        {
            var result = new List<string>();
            var sb = new StringBuilder();
            var pos = 0;

            while (pos < commandLine.Length)
            {
                var c = commandLine[pos];

                if (c == ' ')
                {
                    AddArgument(result, sb);
                }
                else if (c == '"')
                {
                    var openingQuote = pos++;

                    while (pos < commandLine.Length)
                    {
                        if (commandLine[pos] == '"')
                        {
                            // Check if quote is escaped
                            if (pos + 1 < commandLine.Length && commandLine[pos + 1] == '"')
                                pos++;
                            else
                                break;
                        }

                        // Check for backslash quote
                        if (commandLine[pos] == '\\' && pos + 1 < commandLine.Length && commandLine[pos + 1] == '"')
                            pos++;

                        sb.Append(commandLine[pos]);
                        pos++;
                    }

                    if (pos >= commandLine.Length)
                        throw new FormatException(string.Format(Strings.UnmatchedQuoteFmt, openingQuote));
                }
                else
                {
                    sb.Append(commandLine[pos]);
                }

                pos++;
            }

            AddArgument(result, sb);

            return result.ToArray();
        }

        private static void AddArgument(ICollection<string> receiver, StringBuilder sb)
        {
            if (sb.Length == 0)
                return;

            var token = sb.ToString().Trim();
            if (token.Length > 0)
                receiver.Add(token);

            sb.Clear();
        }
    }
}