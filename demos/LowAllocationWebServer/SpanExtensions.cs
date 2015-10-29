namespace System.Net.Http.Buffered
{
    public static class SpanExtensions
    {
        private const int EmptyCharacter = 32;
        private static readonly byte[] Empty = { EmptyCharacter };

        public static void SetFromRestOfSpanToEmpty(this Span<byte> span, int startingFrom)
        {
            for (var i = startingFrom; i < span.Length; i++)
            {
                span.Slice(i).Set(Empty);
            }
        }
    }
}
