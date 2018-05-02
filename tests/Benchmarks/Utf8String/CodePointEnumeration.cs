// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Code;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Utf8StringRealType = System.Text.Utf8.Utf8String;

namespace Benchmarks.Utf8String
{
    public partial class Utf8String
    {
        [ParamsSource(nameof(GetConstructFromStringParameters))]
        public string ConstructFromStringData;

        public IEnumerable<IParam> GetConstructFromStringParameters()
        {
            yield return new ConstructFromStringParameter(5, 32, 126, "Short ASCII string");
            yield return new ConstructFromStringParameter(5, 32, 0xD7FF, "Short string");
            yield return new ConstructFromStringParameter(50000, 32, 126, "Long ASCII string");
            yield return new ConstructFromStringParameter(50000, 32, 0xD7FF, "Long string");
        }

        [Benchmark]
        public Utf8StringRealType ConstructFromString()
        {
            return new Utf8StringRealType(ConstructFromStringData);
        }

        [ParamsSource(nameof(GetEnumerateCodePointsParameters))]
        public Utf8StringRealType EnumerateCodePointsData;

        public IEnumerable<IParam> GetEnumerateCodePointsParameters()
        {
            yield return new EnumerateCodePointsParameter(5, 32, 126, "Short ASCII string");
            yield return new EnumerateCodePointsParameter(5, 32, 0xD7FF, "Short string");
            yield return new EnumerateCodePointsParameter(50000, 32, 126, "Long ASCII string");
            yield return new EnumerateCodePointsParameter(50000, 32, 0xD7FF, "Long string");
        }

        [Benchmark]
        public uint EnumerateCodePoints()
        {
            uint lastValue = default;
            foreach (var codePoint in EnumerateCodePointsData)
            {
                lastValue = codePoint;
            }
            return lastValue;
        }

        private static string GetRandomString(int length, int minCodePoint, int maxCodePoint)
        {
            Random r = new Random(42);
            StringBuilder sb = new StringBuilder(length);
            while (length-- != 0)
            {
                sb.Append((char)r.Next(minCodePoint, maxCodePoint));
            }
            return sb.ToString();
        }

        public class ConstructFromStringParameter : IParam
        {
            public ConstructFromStringParameter(int length, int minCodePoint, int maxCodePoint, string description)
            {
                DisplayText = description;
                Value = GetRandomString(length, minCodePoint, maxCodePoint);
            }

            public string DisplayText { get; }

            public object Value { get; }

            public string ToSourceCode()
            {
                string valueAsString = (string)Value;

                StringBuilder sb = new StringBuilder("\"");
                foreach (char ch in valueAsString)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "\\u{0:X4}", (uint)ch);
                }
                sb.Append("\"");

                return sb.ToString();
            }
        }

        public class EnumerateCodePointsParameter : IParam
        {
            private string _value;

            public EnumerateCodePointsParameter(int length, int minCodePoint, int maxCodePoint, string description)
            {
                DisplayText = description;
                _value = GetRandomString(length, minCodePoint, maxCodePoint);
            }

            public string DisplayText { get; }

            public object Value => new Utf8StringRealType(_value);

            public string ToSourceCode()
            {
                StringBuilder sb = new StringBuilder("new System.Text.Utf8.Utf8String(\"");
                foreach (char ch in _value)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "\\u{0:X4}", (uint)ch);
                }
                sb.Append("\")");

                return sb.ToString();
            }
        }

    }
}
