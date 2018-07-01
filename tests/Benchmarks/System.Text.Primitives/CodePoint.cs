// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Code;

namespace System.Text.Primitives.Benchmarks
{
    public class CodePoint : IParam
    {
        public CodePoint(int minCodePoint, int maxCodePoint, EncoderHelper.SpecialTestCases special = EncoderHelper.SpecialTestCases.None)
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
                    case EncoderHelper.SpecialTestCases.None:
                        return $"({MinCodePoint},{MaxCodePoint},None)";
                    case EncoderHelper.SpecialTestCases.AlternatingASCIIAndNonASCII:
                        return $"({MinCodePoint},{MaxCodePoint},Alternating)";
                    case EncoderHelper.SpecialTestCases.MostlyASCIIAndSomeNonASCII:
                        return $"({MinCodePoint},{MaxCodePoint},Mostly)";
                    default:
                        return $"({MinCodePoint},{MaxCodePoint},{Special})";
                }
            }
        }

        public int MinCodePoint { get; }
        public int MaxCodePoint { get; }
        public EncoderHelper.SpecialTestCases Special { get; }

        public string ToSourceCode() => $"new System.Text.Primitives.Benchmarks.CodePoint({MinCodePoint}, {MaxCodePoint}, System.Text.Primitives.Benchmarks.UtfEncoderHelper.SpecialTestCases.{Special})";

    }
}
