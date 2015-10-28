using System.Text;

namespace System.Net.Http.Buffered
{ 
    public struct HttpHeaderBuffer
    {        
        private Span<byte> _valueByteSpan;

        public HttpHeaderBuffer(Span<byte> valueByteSpan)
        {
            _valueByteSpan = valueByteSpan;
        }

        public void UpdateValue(string newValue)
        {
            if (newValue.Length > _valueByteSpan.Length)
            {
                throw new ArgumentException("newValue");
            }

            _valueByteSpan.Set(new UTF8Encoding().GetBytes(newValue));

            _valueByteSpan.SetFromRestOfSpanToEmpty(newValue.Length);
        }        
    }
}
