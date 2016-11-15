using System;
using System.Text;

namespace System.IO.Pipelines.Text.Primitives
{
    public static class PipelineWriterExtensions
    {
        public static PipelineTextOutput AsTextOutput(this IPipelineWriter writer, EncodingData formattingData)
        {
            return new PipelineTextOutput(writer, formattingData);
        }
    }
}
