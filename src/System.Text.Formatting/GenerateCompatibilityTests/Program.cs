// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static Random random = new Random(1000);
    static void Main(string[] args)
    {
        GenerateNumericTypesTests();
        GenerateTimeSpanTests();
    }

    private static void GenerateTimeSpanTests()
    {
        var file = new StreamWriter(@"..\..\..\tests\TimeSpanFormattingTests.Generated.cs");
        var writer = new CodeWriter(file);

        GenerateFileHeader(writer);

        GenerateCheck(writer, "TimeSpan");
        GenerateTest(writer, "TimeSpan", 'G');
        //GenerateTest(writer, "TimeSpan", 'g');
        GenerateTest(writer, "TimeSpan", 'c');

        GenerateFileFooter(file, writer);
    }

    private static void GenerateNumericTypesTests()
    {
        var file = new StreamWriter(@"..\..\..\tests\NumberFormattingTests.Generated.cs");
        var writer = new CodeWriter(file);

        GenerateFileHeader(writer);
        GenerateNumericTypeTests(writer, "Byte", 5);
        GenerateNumericTypeTests(writer, "SByte", 5);

        GenerateNumericTypeTests(writer, "UInt16", 5);
        GenerateNumericTypeTests(writer, "Int16", 5);

        GenerateNumericTypeTests(writer, "UInt32", 5);
        GenerateNumericTypeTests(writer, "Int32", 5);

        GenerateNumericTypeTests(writer, "UInt64", 5);
        GenerateNumericTypeTests(writer, "Int64", 5);

        GenerateFileFooter(file, writer);
    }

    // TODO: all of the formats should work
    private static void GenerateNumericTypeTests(CodeWriter writer, string typeName, int maxPrecision = -1)
    {
        GenerateCheck(writer, typeName);

        //GenerateTest(writer, typeName, 'C', maxPrecision);
        GenerateTest(writer, typeName, 'D', maxPrecision);
        GenerateTest(writer, typeName, 'd', maxPrecision);
        //GenerateTest(writer, typeName, 'E', maxPrecision);
        //GenerateTest(writer, typeName, 'F', maxPrecision);
        GenerateTest(writer, typeName, 'G');
        //GenerateTest(writer, typeName, 'g', maxPrecision);
        GenerateTest(writer, typeName, 'N', maxPrecision);
        //GenerateTest(writer, typeName, 'P', maxPrecision);
        //GenerateTest(writer, typeName, 'R', maxPrecision);
        GenerateTest(writer, typeName, 'X', maxPrecision);
        GenerateTest(writer, typeName, 'x', maxPrecision);
    }

    private static void GenerateCheck(CodeWriter writer, string typeName)
    {
        var helperName = "Check" + typeName;
        writer.WriteLine("public void {0}({1} value, string format)", helperName, typeName);
        writer.WriteLine("{");
        writer.WriteLine("    var parsed = TextFormat.Parse(format);");
        writer.WriteLine("    var formatter = new StringFormatter();");
        writer.WriteLine("    formatter.Append(value, parsed);");
        writer.WriteLine("    var result = formatter.ToString();");
        writer.WriteLine("    var clrResult = value.ToString(format, CultureInfo.InvariantCulture);");
        writer.WriteLine("    Assert.Equal(clrResult, result);");
        writer.WriteLine("}");
        writer.WriteLine("");

    }

    private static void GenerateTest(CodeWriter writer, string typeName, char format, int maxPrecision = -1)
    {
        writer.WriteLine("[Fact]");
        var testName = typeName + "Format" + format;
        writer.WriteLine("public void {0}()", testName);
        writer.WriteLine("{");
        writer.Indent++;
        var helperName = "Check" + typeName;
        GenerateSpecificFormatTests(writer, typeName, format.ToString(), helperName);
        if (maxPrecision > -1)
        {
            for (int precision = 0; precision <= maxPrecision; precision++)
            {
                GenerateSpecificFormatTests(writer, typeName, format.ToString() + precision.ToString(), helperName);
            }
        }

        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine("");
    }

    private static void GenerateSpecificFormatTests(CodeWriter writer, string typeName, string format, string checkMethodName)
    {
        writer.WriteLine("");
        writer.WriteLine("// format {0}", format);
        foreach (var testValue in GetTestValues(typeName)) {
            writer.WriteLine("{0}({1}, \"{2}\");", checkMethodName, testValue, format);
        }
    }

    private static IEnumerable<string> GetTestValues(string typeName)
    {
        int min = 0;
        int max = int.MaxValue;
        switch (typeName)
        {
            case "UInt32":
            case "UInt64":
                break;

            case "TimeSpan":
            case "Int32":
            case "Int64":
                min = int.MinValue;
                break;
            case "UInt16":
                max = ushort.MaxValue;
                break;
            case "Byte":
                max = byte.MaxValue;
                break;
            case "Int16":
                min = short.MinValue;
                max = short.MaxValue;
                break;
            case "SByte":
                min = sbyte.MinValue;
                max = sbyte.MaxValue;
                break;
            default: throw new NotImplementedException();
        }

        yield return typeName + ".MinValue";
        yield return typeName + ".MaxValue";
        if (typeName != "TimeSpan")
        {
            yield return "0";
        }

        if (typeName == "TimeSpan")
        {
            for (int test = 3; test <= 20; test++)
            {
                yield return String.Format("new {0}({1})", typeName, random.Next(min, max));
            }
        }
        else
        {
            for (int test = 4; test <= 8; test++)
            {
                yield return random.Next(min, max).ToString();
            }
        }
    }

    private static void GenerateFileHeader(CodeWriter writer)
    {
        writer.WriteLine("// Licensed to the .NET Foundation under one or more agreements.");
        writer.WriteLine("// The .NET Foundation licenses this file to you under the MIT license.");
        writer.WriteLine("// See the LICENSE file in the project root for more information.");
        writer.WriteLine("");
        writer.WriteLine("// THIS FILE IS AUTOGENERATED");
        writer.WriteLine("");
        writer.WriteLine("using System;");
        writer.WriteLine("using System.Globalization;");
        writer.WriteLine("using Xunit;");
        writer.WriteLine("");
        writer.WriteLine("namespace System.Text.Formatting.Tests");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine("public partial class SystemTextFormattingTests");
        writer.WriteLine("{");
        writer.Indent++;
    }

    private static void GenerateFileFooter(StreamWriter file, CodeWriter writer)
    {
        writer.Indent--;
        writer.WriteLine("}"); // end of test type
        writer.Indent--;
        writer.WriteLine("}"); // namesapce
        file.Close();
    }

    class CodeWriter
    {
        TextWriter _writer;
        public int Indent = 0;

        public CodeWriter(TextWriter writer)
        {
            _writer = writer;
        }
        internal void WriteLine(string text)
        {
            _writer.Write(new String(' ', Indent * 4));
            _writer.WriteLine(text);
        }

        internal void WriteLine(string format, params object[] args)
        {
            WriteLine(String.Format(format, args));
        }
    }
}

