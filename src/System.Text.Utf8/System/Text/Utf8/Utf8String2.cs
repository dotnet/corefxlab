using System.Buffers;
using System.Runtime.InteropServices;

namespace System.Text.Utf8
{
    [StructLayout(LayoutKind.Sequential)]
    public class Utf8String2 : IReadOnlyMemory<byte>, IComparable<Utf8String2>, IEquatable<Utf8String2>
    {
        int _length;
        byte _b0;
        byte _b1;
        byte _b2;
        byte _b3;
        byte _b4;
        byte _b5;
        byte _b6;
        byte _b7;
        byte _b8;
        byte _b9;
        byte _b10;
        byte _b11;
        byte _b12;
        byte _b13;
        byte _b14;
        byte _b15;
        byte _b16;
        byte _b17;
        byte _b18;
        byte _b19;

        public Utf8String2(string text)
        {
            var utf8 = new Utf8String(text);
            if (utf8.Length > 20) throw new NotImplementedException("this type does not support strings longer than 20 bytes");
            var span = Buffer;
            utf8.CopyTo(Buffer);
            _length = utf8.Length;
        }

        public Utf8String2(byte[] bytes, int offset = 0, int length = -1)
        {
            if (length == -1) length = bytes.Length;
            if (length > 20) throw new NotImplementedException("this type does not support strings longer than 20 bytes");
            var span = Buffer;
            bytes.Slice(offset, length).CopyTo(Buffer);
            _length = bytes.Length;
        }

        public static implicit operator Utf8String2(string text)
        {
            return new Utf8String2(text);
        }

        public ReadOnlyMemory<byte> Substring(int index, int length)
        {
            return Memory.Slice(index, length);
        }
        public ReadOnlyMemory<byte> Substring(int index)
        {
            return Memory.Slice(index);
        }

        Span<byte> Buffer => new Span<byte>(this, new UIntPtr(12), 20);

        ReadOnlySpan<byte> IReadOnlyMemory<byte>.GetSpan(long id)
        {
            return new ReadOnlySpan<byte>(this, new UIntPtr(12), _length);
        }

        public ReadOnlyMemory<byte> Memory {
            get {
                return new ReadOnlyMemory<byte>(this, 0).Slice(0, _length);
            }
        }
        public ReadOnlySpan<byte> Span => Buffer.Slice(0, _length);

        public int Length => _length;

        void IReferenceCounted.AddReference(long id)
        { }

        void IReferenceCounted.Release(long id)
        { }

        unsafe bool IReadOnlyMemory<byte>.TryGetPointer(long id, out void* pointer)
        {
            throw new NotImplementedException();
        }

        public DisposableReservation Reserve(ref ReadOnlyMemory<byte> memory)
        {
            return new DisposableReservation(this, 0);
        }

        public override string ToString()
        {
            return new Utf8String(Buffer.Slice(0, _length)).ToString();
        }

        public override int GetHashCode()
        {
            return ToUtf8String().GetHashCode();
        }
        public override bool Equals(object obj)
        {
            Utf8String2 utf8 = obj as Utf8String2;
            if (utf8 == null) return false;
            return Equals(utf8);
        }

        Utf8String ToUtf8String()
        {
            return new Utf8String(Buffer.Slice(0, _length));

        }
        public int CompareTo(Utf8String2 other)
        {
            return ToUtf8String().CompareTo(other.ToUtf8String());
        }

        public bool Equals(Utf8String2 other)
        {
            return ToUtf8String().Equals(other.ToUtf8String());
        }

        public static bool operator ==(Utf8String2 left, Utf8String2 right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Utf8String2 left, Utf8String2 right)
        {
            return !left.Equals(right);
        }

        public static bool operator ==(Utf8String2 left, string right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Utf8String2 left, string right)
        {
            return !left.Equals(right);
        }
    }
}
