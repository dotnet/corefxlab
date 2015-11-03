using System.Text.Utf16;
using Xunit;

namespace System.Text.Utf8.Tests
{
    public class Utf8Utf16ConversionTests
    {
        [Theory]
        [InlineData("abcdefghijklmnopqrstuvwxyz")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        [InlineData("0123456789")]
        [InlineData(" ,.\r\n[]<>()")]
        [InlineData("")]
        [InlineData("1258")]
        [InlineData("1258Hello")]
        [InlineData("\uABCD")]
        [InlineData("\uABEE")]
        [InlineData("a\uABEE")]
        [InlineData("a\uABEEa")]
        [InlineData("a\uABEE\uABCDa")]
        public void Utf16StringToUtf8StringToUtf16StringRoundTrip(string utf16String)
        {
            Utf8String utf8String = new Utf8String(utf16String);
            Assert.Equal(utf16String, utf8String.ToString());
        }

        public static object[][] Utf8StringToUtf16StringToUtf8StringRoundTripTestCases = {
            new object[] { "abcdefghijklmnopqrstuvwxyz"u8 },
            new object[] { "ABCDEFGHIJKLMNOPQRSTUVWXYZ"u8 },
            new object[] { "0123456789"u8 },
            new object[] { " ,.\r\n[]<>()"u8 },
            new object[] { ""u8 },
            new object[] { "1258"u8 },
            new object[] { "1258Hello"u8 },
            new object[] { "\uABCD"u8 },
            new object[] { "\uABEE"u8 },
            new object[] { "a\uABEE"u8 },
            new object[] { "a\uABEEa"u8 },
            new object[] { "a\uABEE\uABCDa"u8 }
        };
        [Theory, MemberData("Utf8StringToUtf16StringToUtf8StringRoundTripTestCases")]
        public void Utf8StringToUtf16StringToUtf8StringRoundTrip(Utf8String utf8String)
        {
            string utf16String = utf8String.ToString();
            Assert.Equal(utf8String, new Utf8String(utf16String));
        }

        public static object[][] EnumerateAndEnsureCodePointsOfTheSameUtf8AndUtf16StringsAreTheSameTestCases = {
            new object[] { "", default(Utf8String) },
            new object[] { "", Utf8String.Empty },
            new object[] { "", ""u8 },
            new object[] { "1258", "1258"u8 },
            new object[] { "\uABCD", "\uABCD"u8 },
            new object[] { "\uABEE", "\uABEE"u8 },
            new object[] { "a\uABEE", "a\uABEE"u8 },
            new object[] { "a\uABEEa", "a\uABEEa"u8 },
            new object[] { "a\uABEE\uABCDa", "a\uABEE\uABCDa"u8 }
        };
        [Theory, MemberData("EnumerateAndEnsureCodePointsOfTheSameUtf8AndUtf16StringsAreTheSameTestCases")]
        public void EnumerateAndEnsureCodePointsOfTheSameUtf8AndUtf16StringsAreTheSame(string utf16String, Utf8String utf8String)
        {
            var utf16StringCodePoints = new Utf16LittleEndianCodePointEnumerable(utf16String);

            var utf16CodePointEnumerator = utf16StringCodePoints.GetEnumerator();
            var utf8CodePointEnumerator = utf8String.CodePoints.GetEnumerator();

            bool moveNext;
            while (true)
            {
                moveNext = utf16CodePointEnumerator.MoveNext();
                Assert.Equal(moveNext, utf8CodePointEnumerator.MoveNext());
                if (!moveNext)
                {
                    break;
                }
                Assert.Equal(utf16CodePointEnumerator.Current, utf8CodePointEnumerator.Current);
            }
        }
    }
}
