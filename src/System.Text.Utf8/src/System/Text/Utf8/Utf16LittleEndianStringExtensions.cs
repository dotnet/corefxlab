namespace System.Text.Utf8
{
    public static class Utf16LittleEndianStringExtensions
    {
        // TODO: Naming it Equals causes picking up wrong overload when compiling (Equals(object))
        public static bool EqualsUtf8String(this string left, Utf8String right)
        {
            return right.Equals(left);
        }
    }
}
