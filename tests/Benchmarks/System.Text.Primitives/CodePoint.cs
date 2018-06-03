using BenchmarkDotNet.Code;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Text.Primitives.Tests.Encoding.TextEncoderTestHelper;

namespace Benchmarks.System.Text.Primitives
{
    public class CodePoint : IParam
    {
        public CodePoint(int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            this.MinCodePoint = minCodePoint;
            this.MaxCodePoint = maxCodePoint;
            this.Special = special;
        }

        public object Value => this;

        public string DisplayText
        {
            get
            {
                switch (Special)
                {
                    case SpecialTestCases.None:
                        return $"({MinCodePoint},{MaxCodePoint},None)";
                    case SpecialTestCases.AlternatingASCIIAndNonASCII:
                        return $"({MinCodePoint},{MaxCodePoint},Alternating)";
                    case SpecialTestCases.MostlyASCIIAndSomeNonASCII:
                        return $"({MinCodePoint},{MaxCodePoint},Mostly)";
                    default:
                        return $"({MinCodePoint},{MaxCodePoint},{Special})";
                }
            }
        }

        public int MinCodePoint { get; }
        public int MaxCodePoint { get; }
        public SpecialTestCases Special { get; }

        public string ToSourceCode() => $"new Benchmarks.System.Text.Primitives.CodePoint({MinCodePoint}, {MaxCodePoint}, System.Text.Primitives.Tests.Encoding.TextEncoderTestHelper.SpecialTestCases.{Special})";
    }
}
