// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Xunit;

namespace System.CommandLine.Tests
{
    public class ArgumentLexerTests
    {
        [Fact]
        public void Lex_Parameters()
        {
            var text = "abc def ghi";
            var actual = Lex(text);
            var expected = new[] {
                new ArgumentToken(null, "abc", null),
                new ArgumentToken(null, "def", null),
                new ArgumentToken(null, "ghi", null),
            };

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Lex_Options()
        {
            var text = "-a --b";
            var actual = Lex(text);
            var expected = new[] {
                new ArgumentToken("-", "a", null),
                new ArgumentToken("--", "b", null)
            };

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Lex_OptionArguments()
        {
            var text = "-a:va -b=vb --c vc";
            var actual = Lex(text);
            var expected = new[] {
                new ArgumentToken("-", "a", "va"),
                new ArgumentToken("-", "b", "vb"),
                new ArgumentToken("--", "c", null),
                new ArgumentToken(null, "vc", null)
            };

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Lex_ExpandsBundles()
        {
            var text = "-xdf";
            var actual = Lex(text);
            var expected = new[] {
                new ArgumentToken("-", "x", null),
                new ArgumentToken("-", "d", null),
                new ArgumentToken("-", "f", null)
            };

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Lex_ExpandsBundles_UnlessUsingDashDash()
        {
            var text = "--xdf";
            var actual = Lex(text);
            var expected = new[] {
                new ArgumentToken("--", "xdf", null)
            };

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Lex_ExpandsReponseFile()
        {
            var responseFileName = @"C:\src\magic.rsp";
            var responseFileContents = new[]
            {
                "-xdf",
                "--out:out.exe",
                "--responseFileReader",
                @"C:\Reference Assemblies\system.dll"
            };

            var responseFileReader = CreateMokeReponseFileReader(responseFileName, responseFileContents);
            var text = "--before @\"" + responseFileName + "\" --after";
            var actual = Lex(text, responseFileReader);
            var expected = new[] {
                new ArgumentToken("--", "before", null),
                new ArgumentToken("-", "x", null),
                new ArgumentToken("-", "d", null),
                new ArgumentToken("-", "f", null),
                new ArgumentToken("--", "out", "out.exe"),
                new ArgumentToken("--", "responseFileReader", null),
                new ArgumentToken(null, @"C:\Reference Assemblies\system.dll", null),
                new ArgumentToken("--", "after", null)
            };

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Lex_ExpandsReponseFile_UnlessNoReader()
        {
            var text = "--before @somefile --after";
            var actual = Lex(text);
            var expected = new[] {
                new ArgumentToken("--", "before", null),
                new ArgumentToken(null, "@somefile", null),
                new ArgumentToken("--", "after", null)
            };

            Assert.Equal(expected, actual);
        }

        private static IReadOnlyList<ArgumentToken> Lex(string commandLine)
        {
            var args = Splitter.Split(commandLine);
            return ArgumentLexer.Lex(args);
        }

        private static IReadOnlyList<ArgumentToken> Lex(string commandLine, Func<string, IEnumerable<string>> responseFileReader)
        {
            var args = Splitter.Split(commandLine);
            return ArgumentLexer.Lex(args, responseFileReader);
        }

        private static Func<string, IEnumerable<string>> CreateMokeReponseFileReader(string expectedFileName, string[] response)
        {
            return fileName =>
            {
                if (fileName == expectedFileName)
                    return response;

                throw new Exception("Unexpeced response file request:" + fileName);
            };
        }
    }
}