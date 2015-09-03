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
    }
}
