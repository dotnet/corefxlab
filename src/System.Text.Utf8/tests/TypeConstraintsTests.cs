using System.Reflection;
using Xunit;

namespace System.Text.Utf8.Tests
{
    public class TypeConstraintsTests
    {
        [Fact]
        public void Utf8StringIsAStruct()
        {
            var utf8String = "anyString"u8;
            Assert.True(utf8String.GetType().GetTypeInfo().IsValueType);
        }

        [Fact]
        public void Utf8StringCodeUnitsEnumeratorIsAStruct()
        {
            var utf8String = "anyString"u8;
            var utf8CodeUnitsEnumerator = utf8String.GetEnumerator();
            Assert.True(utf8String.GetType().GetTypeInfo().IsValueType);
        }

        [Fact]
        public void Utf8StringCodePointEnumerableIsAStruct()
        {
            var utf8String = "anyString"u8;
            Assert.True(utf8String.CodePoints.GetType().GetTypeInfo().IsValueType);
        }

        [Fact]
        public void Utf8StringCodePointEnumeratorIsAStruct()
        {
            var utf8String = "anyString"u8;
            var utf8CodePointEnumerator = utf8String.CodePoints.GetEnumerator();
            Assert.True(utf8CodePointEnumerator.GetType().GetTypeInfo().IsValueType);
        }

        [Fact]
        public void Utf8StringReverseCodePointEnumeratorIsAStruct()
        {
            var utf8String = "anyString"u8;
            var utf8CodePointEnumerator = utf8String.CodePoints.GetReverseEnumerator();
            Assert.True(utf8CodePointEnumerator.GetType().GetTypeInfo().IsValueType);
        }
    }
}
