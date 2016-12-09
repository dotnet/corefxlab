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
        [InlineData("a\uABEE")]
        [InlineData("a\uABEEa")]
        [InlineData("a\uABEE\uABCDa")]
        public void Utf16StringToUtf8StringToUtf16StringRoundTrip(string utf16String)
        {
            Utf8String utf8String = new Utf8String(utf16String);
            Assert.Equal(utf16String, utf8String.ToString());
        }

        public static object[][] Utf8StringToUtf16StringToUtf8StringRoundTripTestCases = {
            new object[] { new Utf8String("abcdefghijklmnopqrstuvwxyz")},
            new object[] { new Utf8String("ABCDEFGHIJKLMNOPQRSTUVWXYZ")},
            new object[] { new Utf8String("0123456789")},
            new object[] { new Utf8String(" ,.\r\n[]<>()")},
            new object[] { new Utf8String("")},
            new object[] { new Utf8String("1258")},
            new object[] { new Utf8String("1258Hello")},
            new object[] { new Utf8String("\uABCD")},
            new object[] { new Utf8String("a\uABEE")},
            new object[] { new Utf8String("a\uABEEa")},
            new object[] { new Utf8String("a\uABEE\uABCDa")}
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
            new object[] { "", new Utf8String("")},
            new object[] { "1258", new Utf8String("1258")},
            new object[] { "\uABCD", new Utf8String("\uABCD")},
            new object[] { "a\uABEE", new Utf8String("a\uABEE")},
            new object[] { "a\uABEEa", new Utf8String("a\uABEEa")},
            new object[] { "a\uABEE\uABCDa", new Utf8String("a\uABEE\uABCDa")}
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
