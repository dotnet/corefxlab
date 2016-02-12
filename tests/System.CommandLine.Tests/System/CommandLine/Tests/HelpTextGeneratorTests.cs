// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Xunit;

namespace System.CommandLine.Tests
{
    public class HelpTextGeneratorTests
    {
        [Fact]
        public void Help_Empty()
        {
            var expectedHelp = @"
                usage: tool
            ";

            var actualHelp = GetHelp(syntax => { });

            AssertMatch(expectedHelp, actualHelp);
        }

        [Fact]
        public void Help_Command_Overview()
        {
            var expectedHelp = @"
                usage: tool <command> [<args>]

                    pull      Gets commits from another repo
                    commit    Adds commits to the current repo
                    push      Transfers commits to another repo
            ";

            var actualHelp = GetHelp(string.Empty, syntax =>
            {
                var command = string.Empty;
                syntax.DefineCommand("pull", ref command, "Gets commits from another repo");
                syntax.DefineCommand("commit", ref command, "Adds commits to the current repo");
                syntax.DefineCommand("push", ref command, "Transfers commits to another repo");
            });

            AssertMatch(expectedHelp, actualHelp);
        }

        [Fact]
        public void Help_Command_Details()
        {
            var expectedHelp = @"
                usage: tool commit [-o <arg>]

                    -o <arg>    Some option
            ";

            var actualHelp = GetHelp("commit", syntax =>
            {
                var o = string.Empty;
                syntax.DefineCommand("pull");
                syntax.DefineCommand("commit");
                syntax.DefineOption("o", ref o, "Some option");
                syntax.DefineCommand("push");
            });

            AssertMatch(expectedHelp, actualHelp);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Help_Option(bool includeHidden)
        {
            var expectedHelp = @"
                usage: tool [-s] [--keyword] [-f] [-o <arg>] [--option <arg>]

                    -s                Single character flag
                    --keyword         Keyword flag
                    -f, --flag        A flag with with short and long name
                    -o <arg>          Single character option with value
                    --option <arg>    Keyword option with value
            ";

            var actualHelp = GetHelp(syntax =>
            {
                var s = false;
                var keyword = false;
                var flag = false;
                var o = string.Empty;
                var option = string.Empty;
                syntax.DefineOption("s", ref s, "Single character flag");
                syntax.DefineOption("keyword", ref keyword, "Keyword flag");
                syntax.DefineOption("f|flag", ref flag, "A flag with with short and long name");
                syntax.DefineOption("o", ref o, "Single character option with value");
                syntax.DefineOption("option", ref option, "Keyword option with value");

                if (includeHidden)
                {
                    var h = syntax.DefineParameter("h", string.Empty);
                    h.IsHidden = true;
                }
            });

            AssertMatch(expectedHelp, actualHelp);
        }

        [Fact]
        public void Help_Option_List()
        {
            var expectedHelp = @"
                usage: tool [-r <arg>...] [--color <arg>...]

                    -r <arg>...             The references
                    --color, -c <arg>...    The colors to use
            ";

            var actualHelp = GetHelp(syntax =>
            {
                var r = (IReadOnlyList<string>)null;
                var c = (IReadOnlyList<string>)null;

                syntax.DefineOptionList("r", ref r, "The references");
                syntax.DefineOptionList("color|c", ref c, "The colors to use");
            });

            AssertMatch(expectedHelp, actualHelp);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Help_Parameter(bool includeHidden)
        {
            var expectedHelp = @"
                usage: tool <p1> <p2>

                    <p1>    Parameter One
                    <p2>    Parameter Two
            ";

            var actualHelp = GetHelp(syntax =>
            {
                if (includeHidden)
                {
                    var h = syntax.DefineOption("h", string.Empty);
                    h.IsHidden = true;
                }

                var p1 = string.Empty;
                var p2 = false;
                syntax.DefineParameter("p1", ref p1, "Parameter One");
                syntax.DefineParameter("p2", ref p2, "Parameter Two");
            });

            AssertMatch(expectedHelp, actualHelp);
        }

        [Fact]
        public void Help_Parameter_List()
        {
            var expectedHelp = @"
                usage: tool <first> <second>...

                    <first>        The first
                    <second>...    The second
            ";

            var actualHelp = GetHelp(syntax =>
            {
                var r = string.Empty;
                var c = (IReadOnlyList<string>)null;

                syntax.DefineParameter("first", ref r, "The first");
                syntax.DefineParameterList("second", ref c, "The second");
            });

            AssertMatch(expectedHelp, actualHelp);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Help_OptionAndParameter(bool includeHidden)
        {
            var expectedHelp = @"
                usage: tool [-o] [-x <arg>] [--] <p1> <p2>

                    -o                     Option #1
                    -x, --option2 <arg>    Option #2
                    <p1>                   Parameter One
                    <p2>                   Parameter Two
            ";

            var actualHelp = GetHelp(syntax =>
            {
                var o1 = false;
                var o2 = string.Empty;
                var p1 = string.Empty;
                var p2 = false;

                syntax.DefineOption("o", ref o1, "Option #1");
                syntax.DefineOption("x|option2", ref o2, "Option #2");

                if (includeHidden)
                {
                    var h = syntax.DefineOption("h", string.Empty);
                    h.IsHidden = true;
                }

                syntax.DefineParameter("p1", ref p1, "Parameter One");
                syntax.DefineParameter("p2", ref p2, "Parameter Two");

                if (includeHidden)
                {
                    var h = syntax.DefineParameter("h", string.Empty);
                    h.IsHidden = true;
                }
            });

            AssertMatch(expectedHelp, actualHelp);
        }

        [Fact]
        public void Help_Wrapping_Usage()
        {
            var expectedHelp = @"
                usage: tool [-a] [-b] [-c] [-d] [-e] [-f] [-g] [-h]
                            [-i] [-j] [-k] [-l] [-m] [-n] [-o] [-p]
                            [-q]

                    -a    a
                    -b    b
                    -c    c
                    -d    d
                    -e    e
                    -f    f
                    -g    g
                    -h    h
                    -i    i
                    -j    j
                    -k    k
                    -l    l
                    -m    m
                    -n    n
                    -o    o
                    -p    p
                    -q    q
            ";

            var actualHelp = GetHelp(55, syntax =>
            {
                var f = false;
                for (var c = 'a'; c <= 'q'; c++)
                {
                    var cText = c.ToString();
                    syntax.DefineOption(cText, ref f, cText);
                }
            });

            AssertMatch(expectedHelp, actualHelp);
        }

        [Fact]
        public void Help_Wrapping_Usage_LongToken_First()
        {
            var expectedHelp = @"
                usage: tool [--Lorem_ipsum_dolor_sit_amet_consectetur_adipiscing_elit_sed_do_eiusmod_tempor]
                            [-a] [-b] [-c] [-d]

                    --Lorem_ipsum_dolor_sit_amet_consectetur_adipiscing_elit_sed_do_eiusmod_tempor    Lorem ipsum flag
                    -a                                                                                a
                    -b                                                                                b
                    -c                                                                                c
                    -d                                                                                d
            ";

            var actualHelp = GetHelp(55, syntax =>
            {
                var f = false;

                syntax.DefineOption("Lorem_ipsum_dolor_sit_amet_consectetur_adipiscing_elit_sed_do_eiusmod_tempor", ref f, "Lorem ipsum flag");

                for (var c = 'a'; c <= 'd'; c++)
                {
                    var cText = c.ToString();
                    syntax.DefineOption(cText, ref f, cText);
                }
            });

            AssertMatch(expectedHelp, actualHelp);
        }

        [Fact]
        public void Help_Wrapping_Usage_LongToken_Second()
        {
            var expectedHelp = @"
                usage: tool [-a]
                            [--Lorem_ipsum_dolor_sit_amet_consectetur_adipiscing_elit_sed_do_eiusmod_tempor]
                            [-b] [-c] [-d]

                    -a                                                                                a
                    --Lorem_ipsum_dolor_sit_amet_consectetur_adipiscing_elit_sed_do_eiusmod_tempor    Lorem ipsum flag
                    -b                                                                                b
                    -c                                                                                c
                    -d                                                                                d
            ";

            var actualHelp = GetHelp(55, syntax =>
            {
                var f = false;

                for (var c = 'a'; c <= 'd'; c++)
                {
                    var cText = c.ToString();
                    syntax.DefineOption(cText, ref f, cText);

                    if (c == 'a')
                        syntax.DefineOption("Lorem_ipsum_dolor_sit_amet_consectetur_adipiscing_elit_sed_do_eiusmod_tempor", ref f, "Lorem ipsum flag");
                }
            });

            AssertMatch(expectedHelp, actualHelp);
        }

        [Fact]
        public void Help_Wrapping_Text()
        {
            var expectedHelp = @"
                usage: tool [-s <arg>] [-f]

                    -s <arg>    Lorem ipsum dolor sit amet, consectetur
                                adipiscing elit, sed do eiusmod tempor
                                incididunt ut labore et dolore magna
                                aliqua. Ut enim ad minim veniam, quis
                                nostrud exercitation ullamco laboris
                                nisi ut aliquip ex ea commodo
                                consequat. Duis aute irure dolor in
                                reprehenderit in voluptate velit esse
                                cillum dolore eu fugiat nulla pariatur.
                                Excepteur sint occaecat cupidatat non
                                proident, sunt in culpa qui officia
                                deserunt mollit anim id est laborum.
                    -f          Lorem ipsum dolor sit amet.
            ";

            var actualHelp = GetHelp(55, syntax =>
            {
                var s = string.Empty;
                var f = false;
                syntax.DefineOption("s", ref s, "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
                syntax.DefineOption("f", ref f, "Lorem ipsum dolor sit amet.");
            });

            AssertMatch(expectedHelp, actualHelp);
        }

        [Fact]
        public void Help_Wrapping_Text_LongToken_First()
        {
            var expectedHelp = @"
                usage: tool [-s <arg>] [-f]

                    -s <arg>    Lorem_ipsum_dolor_sit_amet_consectetur_adipiscing_elit_sed_do_eiusmod_tempor
                                incididunt ut labore et dolore magna
                                aliqua. Ut enim ad minim veniam, quis
                                nostrud exercitation ullamco laboris
                                nisi ut aliquip ex ea commodo
                                consequat. Duis aute irure dolor in
                                reprehenderit in voluptate velit esse
                                cillum dolore eu fugiat nulla pariatur.
                                Excepteur sint occaecat cupidatat non
                                proident, sunt in culpa qui officia
                                deserunt mollit anim id est laborum.
                    -f          Lorem ipsum dolor sit amet.
            ";

            var actualHelp = GetHelp(55, syntax =>
            {
                var s = string.Empty;
                var f = false;
                syntax.DefineOption("s", ref s, "Lorem_ipsum_dolor_sit_amet_consectetur_adipiscing_elit_sed_do_eiusmod_tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
                syntax.DefineOption("f", ref f, "Lorem ipsum dolor sit amet.");
            });

            AssertMatch(expectedHelp, actualHelp);
        }

        [Fact]
        public void Help_Wrapping_Text_LongToken_Second()
        {
            var expectedHelp = @"
                usage: tool [-s <arg>] [-f]

                    -s <arg>    Lorem
                                ipsum_dolor_sit_amet_consectetur_adipiscing_elit_sed_do_eiusmod_tempor
                                incididunt ut labore et dolore magna
                                aliqua. Ut enim ad minim veniam, quis
                                nostrud exercitation ullamco laboris
                                nisi ut aliquip ex ea commodo
                                consequat. Duis aute irure dolor in
                                reprehenderit in voluptate velit esse
                                cillum dolore eu fugiat nulla pariatur.
                                Excepteur sint occaecat cupidatat non
                                proident, sunt in culpa qui officia
                                deserunt mollit anim id est laborum.
                    -f          Lorem ipsum dolor sit amet.
            ";

            var actualHelp = GetHelp(55, syntax =>
            {
                var s = string.Empty;
                var f = false;
                syntax.DefineOption("s", ref s, "Lorem ipsum_dolor_sit_amet_consectetur_adipiscing_elit_sed_do_eiusmod_tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
                syntax.DefineOption("f", ref f, "Lorem ipsum dolor sit amet.");
            });

            AssertMatch(expectedHelp, actualHelp);
        }

        private static void AssertMatch(string expectedText, string actualText)
        {
            var expectedLines = SplitLines(expectedText);
            RemoveLeadingBlankLines(expectedLines);
            RemoveTrailingBlankLines(expectedLines);
            UnindentLines(expectedLines);

            var actualLines = SplitLines(actualText);
            RemoveLeadingBlankLines(actualLines);
            RemoveTrailingBlankLines(actualLines);

            Assert.Equal(expectedLines.Count, actualLines.Count);

            for (var i = 0; i < expectedLines.Count; i++)
                Assert.Equal(expectedLines[i], actualLines[i]);
        }

        private static List<string> SplitLines(string expectedHelp)
        {
            var lines = new List<string>();

            // Get lines

            using (var stringReader = new StringReader(expectedHelp))
            {
                string line;
                while ((line = stringReader.ReadLine()) != null)
                    lines.Add(line);
            }
            return lines;
        }

        private static void RemoveLeadingBlankLines(List<string> lines)
        {
            while (lines.Count > 0 && lines.First().Trim().Length == 0)
                lines.RemoveAt(0);
        }

        private static void RemoveTrailingBlankLines(List<string> lines)
        {
            while (lines.Count > 0 && lines.Last().Trim().Length == 0)
                lines.RemoveAt(lines.Count - 1);
        }

        private static void UnindentLines(List<string> lines)
        {
            var minIndent = lines.Where(l => l.Length > 0)
                .Min(l => l.Length - l.TrimStart().Length);

            for (var i = 0; i < lines.Count; i++)
            {
                if (lines[i].Length >= minIndent)
                    lines[i] = lines[i].Substring(minIndent);
            }
        }

        private static string GetHelp(Action<ArgumentSyntax> defineAction)
        {
            return CreateSyntax(defineAction).GetHelpText();
        }

        private static string GetHelp(string commandLine, Action<ArgumentSyntax> defineAction)
        {
            return CreateSyntax(commandLine, defineAction).GetHelpText();
        }

        private static string GetHelp(int maxWidth, Action<ArgumentSyntax> defineAction)
        {
            return CreateSyntax(defineAction).GetHelpText(maxWidth);
        }

        private static ArgumentSyntax CreateSyntax(Action<ArgumentSyntax> defineAction)
        {
            return CreateSyntax(new string[0], defineAction);
        }

        private static ArgumentSyntax CreateSyntax(string commandLine, Action<ArgumentSyntax> defineAction)
        {
            var args = Splitter.Split(commandLine);
            return CreateSyntax(args, defineAction);
        }

        private static ArgumentSyntax CreateSyntax(IEnumerable<string> arguments, Action<ArgumentSyntax> defineAction)
        {
            var syntax = new ArgumentSyntax(arguments);
            syntax.ApplicationName = "tool";
            defineAction(syntax);
            return syntax;
        }
    }
}