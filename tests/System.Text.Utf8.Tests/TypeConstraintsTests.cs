using System.Reflection;
using Xunit;

namespace System.Text.Utf8.Tests
{
    public class TypeConstraintsTests
    {
        private Utf8String _anyUtf8String;

        public TypeConstraintsTests()
        {
            _anyUtf8String = new Utf8String("anyString");
        }

        [Fact]
        public void Utf8StringIsAStruct()
        {
            Assert.True(_anyUtf8String.GetType().GetTypeInfo().IsValueType);
        }

        [Fact]
        public void Utf8StringCodeUnitsEnumeratorIsAStruct()
        {
            var utf8CodeUnitsEnumerator = _anyUtf8String.GetEnumerator();
            Assert.True(_anyUtf8String.GetType().GetTypeInfo().IsValueType);
        }

        [Fact]
        public void Utf8StringCodePointEnumerableIsAStruct()
        {
            Assert.True(_anyUtf8String.CodePoints.GetType().GetTypeInfo().IsValueType);
        }

        [Fact]
        public void Utf8StringCodePointEnumeratorIsAStruct()
        {
            var utf8CodePointEnumerator = _anyUtf8String.CodePoints.GetEnumerator();
            Assert.True(utf8CodePointEnumerator.GetType().GetTypeInfo().IsValueType);
        }

        [Fact]
        public void Utf8StringReverseCodePointEnumeratorIsAStruct()
        {
            var utf8CodePointEnumerator = _anyUtf8String.CodePoints.GetReverseEnumerator();
            Assert.True(utf8CodePointEnumerator.GetType().GetTypeInfo().IsValueType);
        }
    }
}
