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
    public class MSBuildVariant
    {
        [ThreadStatic]
        private static Variant[] t_threadVariants = new Variant[11];

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

        unsafe internal static string FormatEventMessage
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
            char* c = stackalloc char[72];
            ValueStringBuilder format = new ValueStringBuilder(new Span<char>(c, 72));

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

            char* f = stackalloc char[256];
            ValueStringBuilder formattedMessage = new ValueStringBuilder(new Span<char>(f, 256));

            Variant[] variants = t_threadVariants;

            variants[0] = threadId;
            variants[1] = file;
            variants[2] = lineNumber;
            variants[3] = columnNumber;
            variants[4] = category;
            variants[5] = code;
            variants[7] = endLineNumber;
            variants[8] = endColumnNumber;
            variants[9] = subcategory;
            variants[10] = projectFile;

            // If there are multiple lines, show each line as a separate message.

            int index = message.IndexOf('\n');
            if (index < 0)
            {
                variants[6] = message;
                formattedMessage.Append(format.AsSpan(), variants, CultureInfo.CurrentCulture);
            }
            else
            {
                string[] lines = SplitStringOnNewLines(message);
                for (int i = 0; i < lines.Length; i++)
                {
                    variants[6] = lines[i];
                    formattedMessage.Append(format.AsSpan(), variants);
                    if (i < (lines.Length - 1))
                    {
                        formattedMessage.Append(Environment.NewLine);
                    }
                }
            }

            format.Dispose();
            string result = formattedMessage.ToString();
            formattedMessage.Dispose();
            return result;
        }

        private static string[] SplitStringOnNewLines(string s)
        {
            string[] subStrings = s.Split(s_newLines, StringSplitOptions.None);
            return subStrings;
        }

        private static readonly string[] s_newLines = { "\r\n", "\n" };
    }
}
