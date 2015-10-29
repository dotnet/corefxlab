using System.Text;
using System.Text.Formatting;

namespace System.Net.Http.Buffered
{ 
    public struct HttpHeaderBuffer
    {        
        private Span<byte> _valueByteSpan;
        private readonly FormattingData _formattingData;

        public HttpHeaderBuffer(Span<byte> valueByteSpan, FormattingData formattingData)
        {
            _valueByteSpan = valueByteSpan;
            _formattingData = formattingData;
        }

        public void UpdateValue(string newValue)
        {
            if (newValue.Length > _valueByteSpan.Length)
            {
                throw new ArgumentException("newValue");
            }

            int bytesWritten;
            newValue.TryFormat(_valueByteSpan, default(Format.Parsed), _formattingData, out bytesWritten);            

            _valueByteSpan.SetFromRestOfSpanToEmpty(newValue.Length);
        }        
    }
}
