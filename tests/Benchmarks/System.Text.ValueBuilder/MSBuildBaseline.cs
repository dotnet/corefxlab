// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System;
using System.Globalization;
using System.Text;

namespace Benchmarks.System.Text.ValueBuilder
{
    [MemoryDiagnoser]
    public class MSBuildBaseline
    {
        [Benchmark]
        public void ExactLocationFormat()
        {
            string result = FormatEventMessage("error", "CS", "Missing ;", "312", "source.cs", 233, 236, 4, 8, 0);
        }

        [Benchmark]
        public void ExactLocationFormat_Loop()
        {
            for (int i = 0; i < 10000; i++)
            {
                string result = FormatEventMessage("error", "CS", "Missing ;", "312", "source.cs", 233, 236, 4, 8, 0);
            }
        }

        // From MSBuild sources https://github.com/Microsoft/msbuild/blob/e70a3159d64f9ed6ec3b60253ef863fa883a99b1/src/Shared/EventArgsFormatting.cs
        internal static string FormatEventMessage
        (
            string category,
            string subcategory,
            string message,
            string code,
            string file,
            int lineNumber,
            int endLineNumber,
            int columnNumber,
            int endColumnNumber,
            int threadId
        )
        {
            return FormatEventMessage(category, subcategory, message, code, file, null, lineNumber, endLineNumber, columnNumber, endColumnNumber, threadId);
        }

        internal static string FormatEventMessage
        (
            string category,
            string subcategory,
            string message,
            string code,
            string file,
            string projectFile,
            int lineNumber,
            int endLineNumber,
            int columnNumber,
            int endColumnNumber,
            int threadId
        )
        {
            StringBuilder format = new StringBuilder();

            // Uncomment these lines to show show the processor, if present.
            /*
            if (threadId != 0)
            {
                format.Append("{0}>");
            }
            */

            if ((file == null) || (file.Length == 0))
            {
                format.Append("MSBUILD : ");    // Should not be localized.
            }
            else
            {
                format.Append("{1}");

                if (lineNumber == 0)
                {
                    format.Append(" : ");
                }
                else
                {
                    if (columnNumber == 0)
                    {
                        if (endLineNumber == 0)
                        {
                            format.Append("({2}): ");
                        }
                        else
                        {
                            format.Append("({2}-{7}): ");
                        }
                    }
                    else
                    {
                        if (endLineNumber == 0)
                        {
                            if (endColumnNumber == 0)
                            {
                                format.Append("({2},{3}): ");
                            }
                            else
                            {
                                format.Append("({2},{3}-{8}): ");
                            }
                        }
                        else
                        {
                            if (endColumnNumber == 0)
                            {
                                format.Append("({2}-{7},{3}): ");
                            }
                            else
                            {
                                format.Append("({2},{3},{7},{8}): ");
                            }
                        }
                    }
                }
            }

            if ((subcategory != null) && (subcategory.Length != 0))
            {
                format.Append("{9} ");
            }

            // The category as a string (should not be localized)
            format.Append("{4} ");

            // Put a code in, if available and necessary.
            if (code == null)
            {
                format.Append(": ");
            }
            else
            {
                format.Append("{5}: ");
            }

            // Put the message in, if available.
            if (message != null)
            {
                format.Append("{6}");
            }

            // If the project file was specified, tack that onto the very end.
            if (projectFile != null && !String.Equals(projectFile, file))
            {
                format.Append(" [{10}]");
            }

            // A null message is allowed and is to be treated as a blank line.
            if (null == message)
            {
                message = String.Empty;
            }

            string finalFormat = format.ToString();

            // If there are multiple lines, show each line as a separate message.
            string[] lines = SplitStringOnNewLines(message);
            StringBuilder formattedMessage = new StringBuilder();

            for (int i = 0; i < lines.Length; i++)
            {
                formattedMessage.Append(String.Format(
                        CultureInfo.CurrentCulture, finalFormat,
                        threadId, file,
                        lineNumber, columnNumber, category, code,
                        lines[i], endLineNumber, endColumnNumber,
                        subcategory, projectFile));

                if (i < (lines.Length - 1))
                {
                    formattedMessage.AppendLine();
                }
            }

            return formattedMessage.ToString();
        }

        private static string[] SplitStringOnNewLines(string s)
        {
            string[] subStrings = s.Split(s_newLines, StringSplitOptions.None);
            return subStrings;
        }

        private static readonly string[] s_newLines = { "\r\n", "\n" };
    }
}
