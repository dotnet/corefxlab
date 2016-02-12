// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace System.CommandLine.Tests
{
    public class ArgumentSyntaxTests
    {
        [Fact]
        public void Parse_Throws_IfArgumentsIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                ArgumentSyntax.Parse(null, syntax => { });
            });

            Assert.Equal("arguments", ex.ParamName);
        }

        [Fact]
        public void Parse_Throws_IfDefineActionIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                ArgumentSyntax.Parse(new string[0], null);
            });

            Assert.Equal("defineAction", ex.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Command_Definition_Error_NoName(string name)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                Parse(string.Empty, syntax =>
                {
                    syntax.DefineCommand(name, string.Empty);
                });
            });

            Assert.Equal("You must specify a name." + Environment.NewLine + "Parameter name: name", ex.Message);
        }

        [Theory]
        [InlineData("-c")]
        [InlineData("/c")]
        [InlineData("--c")]
        [InlineData("<c>")]
        [InlineData("c|d")]
        public void Command_Definition_Error_IllegalName(string name)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                Parse(string.Empty, syntax =>
                {
                    syntax.DefineCommand(name, string.Empty);
                });
            });

            Assert.Equal("Name '" + name + "' cannot be used for a command." + Environment.NewLine + "Parameter name: name", ex.Message);
        }

        [Fact]
        public void Command_Definition_Error_AfterOption()
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                Parse(string.Empty, syntax =>
                {
                    syntax.DefineOption("o", string.Empty);
                    syntax.DefineCommand("c", string.Empty);
                });
            });

            Assert.Equal("Cannot define commands if global options or parameters exist.", ex.Message);
        }

        [Fact]
        public void Command_Definition_Error_AfterParameter()
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                Parse(string.Empty, syntax =>
                {
                    syntax.DefineParameter("p", string.Empty);
                    syntax.DefineCommand("c", string.Empty);
                });
            });

            Assert.Equal("Cannot define commands if global options or parameters exist.", ex.Message);
        }

        [Fact]
        public void Command_Definition_Error_Duplicate()
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                Parse(string.Empty, syntax =>
                {
                    syntax.DefineCommand("c", string.Empty);
                    syntax.DefineCommand("c", string.Empty);
                });
            });

            Assert.Equal("Command 'c' is already defined.", ex.Message);
        }

        [Fact]
        public void Command_Definition()
        {
            var c1 = (ArgumentCommand)null;
            var c2 = (ArgumentCommand)null;
            var c3 = (ArgumentCommand)null;
            var c = string.Empty;

            var result = Parse("c2", syntax =>
            {
                c1 = syntax.DefineCommand("c1", ref c, string.Empty);
                c2 = syntax.DefineCommand("c2", ref c, string.Empty);
                c3 = syntax.DefineCommand("c3", ref c, string.Empty);
            });

            Assert.Equal("c2", c);
            Assert.Equal(c2, result.ActiveCommand);

            Assert.Equal("c1", c1.Value);
            Assert.Equal("c2", c2.Value);
            Assert.Equal("c3", c3.Value);
        }

        [Fact]
        public void Command_Definition_WithCustomValue()
        {
            var c1 = (ArgumentCommand)null;
            var c2 = (ArgumentCommand)null;
            var c3 = (ArgumentCommand)null;
            var c = 0;

            var result = Parse("c2", syntax =>
            {
                c1 = syntax.DefineCommand("c1", ref c, 1, string.Empty);
                c2 = syntax.DefineCommand("c2", ref c, 2, string.Empty);
                c3 = syntax.DefineCommand("c3", ref c, 3, string.Empty);
            });

            Assert.Equal(2, c);
            Assert.Equal(c2, result.ActiveCommand);

            Assert.Equal(1, c1.Value);
            Assert.Equal(2, c2.Value);
            Assert.Equal(3, c3.Value);
        }

        [Fact]
        public void Command_Usage_Error_Missing()
        {
            var ex = Assert.Throws<ArgumentSyntaxException>(() =>
            {
                Parse(string.Empty, syntax =>
                {
                    syntax.DefineCommand("c", string.Empty);
                });
            });

            Assert.Equal("missing command", ex.Message);
        }

        [Fact]
        public void Command_Usage_Error_Invalid()
        {
            var ex = Assert.Throws<ArgumentSyntaxException>(() =>
            {
                Parse("d", syntax =>
                {
                    syntax.DefineCommand("c", string.Empty);
                });
            });

            Assert.Equal("unknown command 'd'", ex.Message);
        }

        [Theory]
        [MemberData("GetNonLetterNames")]
        public void Command_Usage_ViaNonLetterName(string name)
        {
            var o = string.Empty;

            Parse(name, syntax =>
            {
                syntax.DefineCommand(name, ref o, string.Empty);
            });

            Assert.Equal(name, o);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Option_Definition_Error_NoName(string name)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                Parse(string.Empty, syntax =>
                {
                    syntax.DefineOption(name, string.Empty);
                });
            });

            Assert.Equal("You must specify a name." + Environment.NewLine + "Parameter name: name", ex.Message);
        }

        [Theory]
        [InlineData("-o")]
        [InlineData("/o")]
        [InlineData("--o")]
        [InlineData("<o>")]
        public void Option_Definition_Error_IllegalName(string name)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                Parse(string.Empty, syntax =>
                {
                    syntax.DefineOption(name, string.Empty);
                });
            });

            Assert.Equal("Name '" + name + "' cannot be used for an option." + Environment.NewLine + "Parameter name: name", ex.Message);
        }

        [Fact]
        public void Option_Definition_MultipleNames()
        {
            var a = "standard-a";
            var b = "standard-b";
            var d = "standard-c";

            Parse("--opta arga -c argb -e argd", syntax =>
            {
                syntax.DefineOption("a|opta", ref a, string.Empty);
                syntax.DefineOption("b|optb|c", ref b, string.Empty);
                syntax.DefineOption("d|optd|e", ref d, string.Empty);
            });

            Assert.Equal("arga", a);
            Assert.Equal("argb", b);
            Assert.Equal("argd", d);
        }

        [Fact]
        public void Option_Definition_AfterParameters()
        {
            Parse("c1 p", syntax =>
            {
                var c1 = syntax.DefineCommand("c1");
                var p = syntax.DefineParameter("p", string.Empty);

                var c2 = syntax.DefineCommand("c2");
                var o = syntax.DefineOption("o", string.Empty);

                Assert.Equal(c1, p.Command);
                Assert.Equal(c2, o.Command);
            });
        }

        [Fact]
        public void Option_Definition_SameNameInAnotherCommand()
        {
            var o1 = "o1";
            var o2 = "o2";

            Parse("c1 -o x", syntax =>
            {
                syntax.DefineCommand("c1");
                syntax.DefineOption("o", ref o1, string.Empty);

                syntax.DefineCommand("c2");
                syntax.DefineOption("o", ref o2, string.Empty);

                Assert.Equal("x", o1);
                Assert.Equal("o2", o2);
            });
        }

        [Fact]
        public void Option_Definition_Error_AfterParameter()
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                Parse(string.Empty, syntax =>
                {
                    syntax.DefineParameter("p", string.Empty);
                    syntax.DefineOption("o", string.Empty);
                });
            });

            Assert.Equal("Options must be defined before any parameters.", ex.Message);
        }

        [Fact]
        public void Option_Definition_Error_AfterParameter_WithCommand()
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                Parse("c", syntax =>
                {
                    syntax.DefineCommand("c");
                    syntax.DefineParameter("p", string.Empty);
                    syntax.DefineOption("o", string.Empty);
                });
            });

            Assert.Equal("Options must be defined before any parameters.", ex.Message);
        }

        [Fact]
        public void Option_Definition_Error_Duplicate()
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                Parse(string.Empty, syntax =>
                {
                    syntax.DefineOption("o", string.Empty);
                    syntax.DefineOption("o", string.Empty);
                });
            });

            Assert.Equal("Option 'o' is already defined.", ex.Message);
        }

        [Fact]
        public void Option_Definition_HasDefault_IfNotSpecified()
        {
            var o = "standard";

            Parse(string.Empty, syntax =>
            {
                syntax.DefineOption("o", ref o, string.Empty);
            });

            Assert.Equal("standard", o);
        }

        [Fact]
        public void Option_Usage_Error_Invalid()
        {
            var ex = Assert.Throws<ArgumentSyntaxException>(() =>
            {
                Parse("-e -d", syntax =>
                {
                    var exists = false;
                    syntax.DefineOption("e", ref exists, "Some qualifier");
                });
            });

            Assert.Equal("invalid option -d", ex.Message);
        }

        [Fact]
        public void Option_Usage_Error_Conversion_Int32()
        {
            var ex = Assert.Throws<ArgumentSyntaxException>(() =>
            {
                Parse("-o abc", syntax =>
                {
                    var o = 0;
                    syntax.DefineOption("o", ref o, string.Empty);
                });
            });

            Assert.Equal("value 'abc' isn't valid for -o: Input string was not in a correct format.", ex.Message);
        }

        [Fact]
        public void Option_Usage_Error_Conversion_Boolean()
        {
            var ex = Assert.Throws<ArgumentSyntaxException>(() =>
            {
                Parse("-o:abc", syntax =>
                {
                    var o = false;
                    syntax.DefineOption("o", ref o, string.Empty);
                });
            });

            Assert.Equal("value 'abc' isn't valid for -o: String was not recognized as a valid Boolean.", ex.Message);
        }

        [Fact]
        public void Option_Usage_Error_Conversion_CustomConverter()
        {
            Func<string, Guid> converter = s =>
            {
                throw new FormatException("invalid format");
            };

            var ex = Assert.Throws<ArgumentSyntaxException>(() =>
            {
                Parse("-o abc", syntax =>
                {
                    var o = Guid.Empty;
                    syntax.DefineOption("o", ref o, converter, string.Empty);
                });
            });

            Assert.Equal("value 'abc' isn't valid for -o: invalid format", ex.Message);
        }

        [Theory]
        [InlineData("-a")]
        [InlineData("-a:")]
        [InlineData("-a : ")]
        [InlineData("-a : -b")]
        public void Option_Usage_Error_RequiresValue(string commandLine)
        {
            var ex = Assert.Throws<ArgumentSyntaxException>(() =>
            {
                Parse(commandLine, syntax =>
                {
                    var arg1 = string.Empty;
                    syntax.DefineOption("a", ref arg1, string.Empty);
                });
            });

            Assert.Equal("option -a requires a value", ex.Message);
        }

        [Fact]
        public void Option_Usage_Error_Flag_Bundle_ViaDashDash()
        {
            var ex = Assert.Throws<ArgumentSyntaxException>(() =>
            {
                Parse("--opq", syntax =>
                {
                    syntax.DefineOption("o", false);
                    syntax.DefineOption("p", false);
                    syntax.DefineOption("q", false);
                });
            });

            Assert.Equal("invalid option --opq", ex.Message);
        }

        [Theory]
        [MemberData("GetNonLetterNames")]
        public void Option_Usage_ViaNonLetterName(string name)
        {
            var o = false;

            Parse("--" + name, syntax =>
            {
                syntax.DefineOption(name, ref o, "");
            });

            Assert.True(o);
        }

        [Theory]
        [InlineData("-")]
        [InlineData("--")]
        public void Option_Usage_ViaModifer(string modifier)
        {
            var o = false;

            Parse(modifier + "o", syntax =>
            {
                syntax.DefineOption("o", ref o, "");
            });

            Assert.True(o);
        }

        [Theory]
        [InlineData("-a x -b -a y -b:false")]
        [InlineData("-a x -a y -b -b:false")]
        public void Option_Usage_LastOneWins(string commandLine)
        {
            var a = string.Empty;
            var b = false;

            Parse(commandLine, syntax =>
            {
                syntax.DefineOption("a", ref a, string.Empty);
                syntax.DefineOption("b", ref b, string.Empty);
            });

            Assert.Equal("y", a);
            Assert.False(b);
        }

        [Fact]
        public void Option_Usage_Flag_DoesNotRequireValue()
        {
            var f1 = false;
            var f2 = true;

            Parse("-f --flag", syntax =>
            {
                syntax.DefineOption("f", ref f1, string.Empty);
                syntax.DefineOption("flag", ref f2, string.Empty);
            });

            Assert.True(f1);
            Assert.True(f2);
        }

        [Fact]
        public void Option_Usage_Flag_AcceptsValue()
        {
            var f1 = false;
            var f2 = false;
            var f3 = true;

            Parse("--f1 --f2:true --f3 = false", syntax =>
            {
                syntax.DefineOption("f1", ref f1, string.Empty);
                syntax.DefineOption("f2", ref f2, string.Empty);
                syntax.DefineOption("f3", ref f3, string.Empty);
            });

            Assert.True(f1);
            Assert.True(f2);
            Assert.False(f3);
        }

        [Fact]
        public void Option_Usage_Flag_Bundled()
        {
            var a = false;
            var b = false;
            var c = false;
            var d = false;
            var e = false;
            var f = false;

            Parse("-bdf", syntax =>
            {
                syntax.DefineOption("a", ref a, string.Empty);
                syntax.DefineOption("b", ref b, string.Empty);
                syntax.DefineOption("c", ref c, string.Empty);
                syntax.DefineOption("d", ref d, string.Empty);
                syntax.DefineOption("e", ref e, string.Empty);
                syntax.DefineOption("f", ref f, string.Empty);
            });

            Assert.False(a);
            Assert.True(b);
            Assert.False(c);
            Assert.True(d);
            Assert.False(e);
            Assert.True(f);
        }

        [Fact]
        public void Option_Usage_Flag_Bundled_WithValue()
        {
            var a = false;
            var b = "";

            Parse("-ab message", syntax =>
            {
                syntax.DefineOption("a", ref a, string.Empty);
                syntax.DefineOption("b", ref b, string.Empty);
            });

            Assert.True(a);
            Assert.Equal("message", b);
        }

        [Fact]
        public void Option_Usage_Flag_DoesNotConsumeParameter()
        {
            var a = false;
            var b = "";

            Parse("-a false", syntax =>
            {
                syntax.DefineOption("a", ref a, string.Empty);
                syntax.DefineParameter("b", ref b, string.Empty);
            });

            Assert.True(a);
            Assert.Equal("false", b);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Option_List_Definition_Error_NoName(string name)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                Parse(string.Empty, syntax =>
                {
                    syntax.DefineOptionList(name, new string[0]);
                });
            });

            Assert.Equal("You must specify a name." + Environment.NewLine + "Parameter name: name", ex.Message);
        }

        [Theory]
        [InlineData("-o")]
        [InlineData("/o")]
        [InlineData("--o")]
        [InlineData("<o>")]
        public void Option_List_Definition_Error_IllegalName(string name)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                Parse(string.Empty, syntax =>
                {
                    syntax.DefineOptionList(name, new string[0]);
                });
            });

            Assert.Equal("Name '" + name + "' cannot be used for an option." + Environment.NewLine + "Parameter name: name", ex.Message);
        }

        [Fact]
        public void Option_List_Definition_Error_AfterParameter()
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                Parse(string.Empty, syntax =>
                {
                    syntax.DefineParameter("p", string.Empty);
                    syntax.DefineOptionList("o", new string[0]);
                });
            });

            Assert.Equal("Options must be defined before any parameters.", ex.Message);
        }

        [Fact]
        public void Option_List_Definition()
        {
            var arg1 = (IReadOnlyList<string>)new string[0];
            var arg2 = false;

            Parse("-a x -b -a y", syntax =>
            {
                syntax.DefineOptionList("a", ref arg1, string.Empty);
                syntax.DefineOption("b", ref arg2, string.Empty);
            });

            Assert.Equal(new[] { "x", "y" }, arg1);
            Assert.True(arg2);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Parameter_Definition_Error_NoName(string name)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                Parse(string.Empty, syntax =>
                {
                    syntax.DefineParameter(name, string.Empty);
                });
            });

            Assert.Equal("You must specify a name." + Environment.NewLine + "Parameter name: name", ex.Message);
        }

        [Theory]
        [InlineData("-p")]
        [InlineData("/p")]
        [InlineData("--p")]
        [InlineData("p|para")]
        [InlineData("<p>")]
        public void Parameter_Definition_Error_IllegalName(string name)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                Parse(string.Empty, syntax =>
                {
                    syntax.DefineParameter(name, string.Empty);
                });
            });

            Assert.Equal("Name '" + name + "' cannot be used for a parameter." + Environment.NewLine + "Parameter name: name", ex.Message);
        }

        [Fact]
        public void Parameter_Definition_Error_Duplicate()
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                Parse(string.Empty, syntax =>
                {
                    syntax.DefineParameter("p", string.Empty);
                    syntax.DefineParameter("p", string.Empty);
                });
            });

            Assert.Equal("Parameter 'p' is already defined.", ex.Message);
        }

        [Fact]
        public void Parameter_Definition_Error_AfterList()
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                Parse(string.Empty, syntax =>
                {
                    syntax.DefineParameterList("a", new string[0]);
                    syntax.DefineParameter("b", string.Empty);
                });
            });

            Assert.Equal("Parameters cannot be defined after parameter lists.", ex.Message);
        }

        [Fact]
        public void Parameter_Definition_SameNameAsOption()
        {
            var o1 = "o1";
            var o2 = "o2";

            Parse("-o x y", syntax =>
            {
                syntax.DefineOption("o", ref o1, string.Empty);
                syntax.DefineParameter("o", ref o2, string.Empty);

                Assert.Equal("x", o1);
                Assert.Equal("y", o2);
            });
        }

        [Fact]
        public void Parameter_Usage_Error_Extra()
        {
            var ex = Assert.Throws<ArgumentSyntaxException>(() =>
            {
                Parse("-a a -b b c", syntax =>
                {
                    var arg1 = string.Empty;
                    var arg2 = string.Empty;
                    syntax.DefineOption("a", ref arg1, string.Empty);
                    syntax.DefineOption("b", ref arg2, string.Empty);
                });
            });

            Assert.Equal("extra parameter 'c'", ex.Message);
        }

        [Fact]
        public void Parameter_Usage_Error_Conversion_Int32()
        {
            var ex = Assert.Throws<ArgumentSyntaxException>(() =>
            {
                Parse("abc", syntax =>
                {
                    var o = 0;
                    syntax.DefineParameter("p", ref o, string.Empty);
                });
            });

            Assert.Equal("value 'abc' isn't valid for <p>: Input string was not in a correct format.", ex.Message);
        }

        [Fact]
        public void Parameter_Usage_Error_Conversion_Boolean()
        {
            var ex = Assert.Throws<ArgumentSyntaxException>(() =>
            {
                Parse("abc", syntax =>
                {
                    var o = false;
                    syntax.DefineParameter("p", ref o, string.Empty);
                });
            });

            Assert.Equal("value 'abc' isn't valid for <p>: String was not recognized as a valid Boolean.", ex.Message);
        }

        [Fact]
        public void Parameter_Usage_Error_Conversion_CustomConverter()
        {
            Func<string, Guid> converter = s =>
            {
                throw new FormatException("invalid format");
            };

            var ex = Assert.Throws<ArgumentSyntaxException>(() =>
            {
                Parse("abc", syntax =>
                {
                    var o = Guid.Empty;
                    syntax.DefineParameter("p", ref o, converter, string.Empty);
                });
            });

            Assert.Equal("value 'abc' isn't valid for <p>: invalid format", ex.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Parameter_Definition_List_Error_NoName(string name)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                Parse(string.Empty, syntax =>
                {
                    syntax.DefineParameterList(name, new string[0]);
                });
            });

            Assert.Equal("You must specify a name." + Environment.NewLine + "Parameter name: name", ex.Message);
        }

        [Theory]
        [InlineData("-p")]
        [InlineData("/p")]
        [InlineData("--p")]
        [InlineData("p|para")]
        [InlineData("<p>")]
        public void Parameter_Definition_List_Error_IllegalName(string name)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                Parse(string.Empty, syntax =>
                {
                    syntax.DefineParameterList(name, new string[0]);
                });
            });

            Assert.Equal("Name '" + name + "' cannot be used for a parameter." + Environment.NewLine + "Parameter name: name", ex.Message);
        }

        [Fact]
        public void Parameter_List_Definition_Error_Duplicate_WithCommand()
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                Parse("c", syntax =>
                {
                    var p1 = (IReadOnlyList<string>)null;
                    var p2 = (IReadOnlyList<string>)null;
                    syntax.DefineCommand("c");
                    syntax.DefineParameterList("p1", ref p1, string.Empty);
                    syntax.DefineParameterList("p2", ref p2, string.Empty);
                });
            });

            Assert.Equal("Cannot define multiple parameter lists.", ex.Message);
        }

        [Fact]
        public void Parameter_List_Definition_Error_Duplicate()
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                Parse(string.Empty, syntax =>
                {
                    var p1 = (IReadOnlyList<string>)null;
                    var p2 = (IReadOnlyList<string>)null;
                    syntax.DefineParameterList("p1", ref p1, string.Empty);
                    syntax.DefineParameterList("p2", ref p2, string.Empty);
                });
            });

            Assert.Equal("Cannot define multiple parameter lists.", ex.Message);
        }

        [Fact]
        public void Parameter_List_Definition()
        {
            var sources = (IReadOnlyList<string>)new string[0];

            Parse("source1.cs source2.cs", syntax =>
            {
                syntax.DefineParameterList("sources", ref sources, string.Empty);
            });

            var expected = new[] { "source1.cs", "source2.cs" };
            var actual = sources;
            Assert.Equal(expected.AsEnumerable(), actual);
        }

        [Fact]
        public void Parameter_List_Definition_AfterParamater()
        {
            var p = string.Empty;
            var ps = (IReadOnlyList<string>)new string[0];

            Parse("single p1 p2", syntax =>
            {
                syntax.DefineParameter("p", ref p, string.Empty);
                syntax.DefineParameterList("ps", ref ps, string.Empty);
            });

            Assert.Equal("single", p);
            Assert.Equal(new[] { "p1", "p2" }.AsEnumerable(), ps);
        }

        [Fact]
        public void Parameter_List_Definition_AnotherListInAnotherCommand()
        {
            var p1 = (IReadOnlyList<string>)null;
            var p2 = (IReadOnlyList<string>)null;

            Parse("c2 a1 a2", syntax =>
            {
                syntax.DefineCommand("c1");
                syntax.DefineParameterList("p1", ref p1, string.Empty);

                syntax.DefineCommand("c2");
                syntax.DefineParameterList("p2", ref p2, string.Empty);
            });

            Assert.Null(p1);
            Assert.Equal(new[] { "a1", "a2" }, p2.AsEnumerable());
        }

        [Fact]
        public void ResponseFiles_Error_DoesNotExist()
        {
            var ex = Assert.Throws<ArgumentSyntaxException>(() =>
            {
                Parse("@6FF54573-5066-46BA-8457-0CF8AA402561", syntax => { });
            });

            Assert.Equal("Response file '6FF54573-5066-46BA-8457-0CF8AA402561' doesn't exist.", ex.Message);
        }

        [Fact]
        public void ResponseFiles_CanBeDisabled()
        {
            var p = string.Empty;

            Parse("@foo", syntax =>
            {
                syntax.HandleResponseFiles = false;
                syntax.DefineParameter("p", ref p, string.Empty);
            });

            Assert.Equal("@foo", p);
        }

        private static ArgumentSyntax Parse(string commandLine, Action<ArgumentSyntax> defineAction)
        {
            var args = Splitter.Split(commandLine);

            return ArgumentSyntax.Parse(args, syntax =>
            {
                syntax.HandleHelp = false;
                syntax.HandleErrors = false;
                defineAction(syntax);
            });
        }

        public static IEnumerable<object[]> GetNonLetterNames()
        {
            return new[]
            {
                new[] {"do-stuff"},
                new[] {"do123"},
                new[] {"123_do"},
                new[] {"_test"}
            };
        }
    }
}
