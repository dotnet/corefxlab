using Xunit;

namespace System.Text.Utf8.Tests
{
    public class Utf8StringTests
    {
        [Fact]
        public unsafe void LengthTest()
        {
            byte[] utf8Bytes = Encoding.UTF8.GetBytes("1258");
            var utf8String = new Utf8String(utf8Bytes);
            Assert.Equal(4, utf8Bytes.Length);
        }

        [Fact]
        public unsafe void LengthPointerTest()
        {
            byte[] utf8Bytes = Encoding.UTF8.GetBytes("1258");
            fixed (byte* bytes = utf8Bytes)
            {
                var utf8String = new Utf8String(bytes, utf8Bytes.Length);
                Assert.Equal(4, utf8Bytes.Length);
            }
        }

        [Fact]
        public unsafe void ToStringTest()
        {
            byte[] utf8Bytes = Encoding.UTF8.GetBytes("1258Hello");
            var utf8String = new Utf8String(utf8Bytes);
            Assert.Equal("1258Hello", utf8String.ToString());
        }

        [Fact]
        public unsafe void ToStringPointerTest()
        {
            byte[] utf8Bytes = Encoding.UTF8.GetBytes("1258Hello");
            fixed(byte* bytes = utf8Bytes)
            {
                var utf8String = new Utf8String(bytes, utf8Bytes.Length);
                Assert.Equal("1258Hello", utf8String.ToString());
            }
        }
    }
}
