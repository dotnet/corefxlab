using Xunit;
using System.Reflection;

namespace System.Text.Utf8.Tests
{
    public partial class Utf8String2Tests
    {
        [Fact]
        public void Utf8StringBasics()
        {
            Utf8String2 hello = "hello world!";
            Assert.True(hello.GetType().GetTypeInfo().IsClass);

            Assert.Equal(Encoding.UTF8.GetBytes("hello world!"), hello.Span.ToArray());

            Utf8String2 alsoHello = "hello world!";
            Utf8String2 world = "world";

            Assert.True(hello == alsoHello);
            Assert.True(hello != world);

            var substring = hello.Substring(6, 5);
            Assert.Equal(substring.ToArray(), world.Memory.ToArray());
        }

        [Fact]
        public void Utf8StringMemory()
        {
            string str = "hello world";

            Utf8String2 utf8 = str;
            Assert.True(utf8 == str);
            
            var utf16 = utf8.ToString();
            Assert.Equal(str, utf16);

            ReadOnlyMemory<byte> memory = utf8.Memory;
            using (memory.Reserve()) {
                ReadOnlySpan<byte> bytes = memory.Span;
                Assert.Equal(Encoding.UTF8.GetBytes(str), bytes.ToArray());
            }
        }
    }
}
