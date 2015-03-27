// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

class Program
{
    static Random random = new Random(1000);
    static void Main(string[] args)
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

        writer.Indent--;
        writer.WriteLine("}"); // end of test type
        writer.Indent--;
        writer.WriteLine("}"); // namesapce

        file.Close();
    }

    // TODO: all of the formats should work
    private static void GenerateNumericTypeTests(CodeWriter writer, string type, int maxPrecision = -1)
    {
        GenerateCheck(writer, type);

        //GenerateTest(writer, type, 'C', maxPrecision);
        GenerateTest(writer, type, 'D', maxPrecision);
        //GenerateTest(writer, type, 'E', maxPrecision);
        //GenerateTest(writer, type, 'F', maxPrecision);
        GenerateTest(writer, type, 'G');
        //GenerateTest(writer, type, 'N', maxPrecision); // TODO: this is implemented, but has bugs
        //GenerateTest(writer, type, 'P', maxPrecision);
        //GenerateTest(writer, type, 'R', maxPrecision);
        GenerateTest(writer, type, 'X', maxPrecision);
    }

    private static void GenerateCheck(CodeWriter writer, string type)
    {
        var helperName = "Check" + type;
        writer.WriteLine("public void {0}({1} value, string format)", helperName, type);
        writer.WriteLine("{");
        writer.WriteLine("    var parsed = Format.Parse(format);");
        writer.WriteLine("    formatter.Clear();");
        writer.WriteLine("    formatter.Append(value, parsed);");
        writer.WriteLine("    var result = formatter.ToString();");
        writer.WriteLine("    var clrResult = value.ToString(format, CultureInfo.InvariantCulture);");
        writer.WriteLine("    Assert.Equal(clrResult, result);");
        writer.WriteLine("}");
        writer.WriteLine("");

    }

    private static void GenerateTest(CodeWriter writer, string type, char format, int maxPrecision = -1)
    {
        writer.WriteLine("[Fact]");
        var testName = type + "Format" + format;
        writer.WriteLine("public void {0}()", testName);
        writer.WriteLine("{");
        writer.Indent++;
        var helperName = "Check" + type;
        GenerateSpecificFormatTersts(writer, type, format.ToString(), helperName);
        GenerateSpecificFormatTersts(writer, type, format.ToString().ToLowerInvariant(), helperName);
        if (maxPrecision > -1)
        {
            for (int precision = 0; precision <= maxPrecision; precision++)
            {
                GenerateSpecificFormatTersts(writer, type, format.ToString() + precision.ToString(), helperName);
                GenerateSpecificFormatTersts(writer, type, format.ToString().ToLowerInvariant() + precision.ToString(), helperName);
            }
        }

        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine("");
    }

    private static void GenerateSpecificFormatTersts(CodeWriter writer, string type, string format, string helperName)
    {
        writer.WriteLine("// format {0}", format);
        writer.WriteLine("{0}({1}, \"{2}\");", helperName, type + ".MinValue", format);
        writer.WriteLine("{0}({1}, \"{2}\");", helperName, type + ".MaxValue", format);
        writer.WriteLine("{0}({1}, \"{2}\");", helperName, "0", format);
        writer.WriteLine("// some random numbers");
        for (int randomNumber = 0; randomNumber < 5; randomNumber++)
        {
            writer.WriteLine("{0}(({3}){1}, \"{2}\");", helperName, GetRandomNumber(type), format, type);
        }
    }

    private static string GetRandomNumber(string numericTypeName)
    {
        switch (numericTypeName)
        {
            case "Int32":
            case "Int64":
                return random.Next().ToString();
            case "UInt32":
            case "UInt64":
                return random.Next(0, int.MaxValue).ToString();
            case "UInt16":
                return random.Next(0, ushort.MaxValue).ToString();
            case "Int16":
                return random.Next(0, short.MaxValue).ToString();
            case "Byte":
                return random.Next(0, byte.MaxValue).ToString();
            case "SByte":
                return random.Next(0, sbyte.MaxValue).ToString();
            default: throw new NotImplementedException();
        }
    }

    private static void GenerateFileHeader(CodeWriter writer)
    {
        writer.WriteLine("// Copyright (c) Microsoft. All rights reserved.");
        writer.WriteLine("// Licensed under the MIT license. See LICENSE file in the project root for full license information.");
        writer.WriteLine("");
        writer.WriteLine("// THIS FILE IS AUTOGENERATED");
        writer.WriteLine("");
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

