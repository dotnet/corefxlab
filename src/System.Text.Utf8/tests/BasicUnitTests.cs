using Xunit;

namespace System.Text.Utf8.Tests
{
    public class Utf8StringTests
    {
        [Fact]
        public unsafe void Length()
        {
            byte[] utf8Bytes = Encoding.UTF8.GetBytes("1258");
            var utf8String = new Utf8String(utf8Bytes);
            Assert.Equal(4, utf8Bytes.Length);
        }

        [Fact]
        public unsafe void ToStringTest()
        {
            byte[] utf8Bytes = Encoding.UTF8.GetBytes("1258Hello");
            var utf8String = new Utf8String(utf8Bytes);
            Assert.Equal("1258Hello", utf8String.ToString());
        }
    }
}
