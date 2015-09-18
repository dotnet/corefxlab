// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Formatting;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public class Json
    {
        [Fact]
        public void WriteJsonUtf8()
        {
            var buffer = new byte[1024];
            var stream = new MemoryStream(buffer);
            var json = new JsonWriter(stream, FormattingData.Encoding.Utf8, prettyPrint:true);
            Write(json);
            var str = Encoding.UTF8.GetString(buffer, 0, (int)stream.Position);
            Console.WriteLine(str);
            Assert.Equal(expected, str.Replace("\n", "").Replace(" ",""));
        } 

        [Fact]
        public void WriteJsonUtf16()
        {
            var buffer = new byte[1024];
            var stream = new MemoryStream(buffer);
            var json = new JsonWriter(stream, FormattingData.Encoding.Utf16);
            Write(json);
            var written = buffer.Slice(0, (int)stream.Position);
            var str = written.CreateString();
            Assert.Equal(expected, str.Replace(" ", ""));
        }

        static string expected = "{\"age\":30,\"first\":\"John\",\"last\":\"Smith\",\"phoneNumbers\":[\"425-000-1212\",\"425-000-1213\"],\"address\":{\"street\":\"1MicrosoftWay\",\"city\":\"Redmond\",\"zip\":98052}}";
        static void Write(JsonWriter json)
        {
            json.WriteObjectStart();
            json.WriteAttribute("age", 30);
            json.WriteAttribute("first", "John");
            json.WriteAttribute("last", "Smith");
            json.WriteMember("phoneNumbers");
            json.WriteArrayStart();
            json.WriteString("425-000-1212");
            json.WriteString("425-000-1213");
            json.WriteArrayEnd();
            json.WriteMember("address");
            json.WriteObjectStart();
            json.WriteAttribute("street", "1 Microsoft Way");
            json.WriteAttribute("city", "Redmond");
            json.WriteAttribute("zip", 98052);
            json.WriteObjectEnd();
            json.WriteObjectEnd();
        }
    }

    struct JsonWriter : IDisposable
    {
        StreamFormatter _formatter;
        bool _wroteListItem;
        bool _prettyPrint;
        int _indent;

        public JsonWriter(Stream stream, FormattingData.Encoding encoding, bool prettyPrint = false)
        {
            _wroteListItem = false;
            _prettyPrint = prettyPrint;
            _indent = 0;
            _formatter = new StreamFormatter(stream, encoding == FormattingData.Encoding.Utf16 ? FormattingData.InvariantUtf16 : FormattingData.InvariantUtf8);
        }
        
        public void Dispose() {
            _formatter.Dispose();
        }

        public bool PrettyPrint { get { return _prettyPrint; } set { _prettyPrint = value; } }

        public void WriteObjectStart()
        {
            _wroteListItem = false;
            _formatter.Append('{');
            WriteNewline();
            _indent++;
        }

        public void WriteObjectEnd()
        {
            _indent--;
            WriteNewline();
            WriteIndent();
            _formatter.Append('}');
            _wroteListItem = true;
        }

        public void WriteArrayStart()
        {
            _wroteListItem = false;
            _formatter.Append('[');
            WriteNewline();
            _indent++;
        }

        public void WriteArrayEnd()
        {
            _indent--;
            WriteNewline();
            WriteIndent();
            _formatter.Append(']');
            _wroteListItem = true;
        }

        public void WriteAttribute(string name, byte value)
        {
            WriteMember(name);
            _formatter.Append(value);
            WriteAttributeEnd();
        }

        public void WriteAttribute(string name, sbyte value)
        {
            WriteMember(name);
            _formatter.Append(value);
            WriteAttributeEnd();
        }

        public void WriteAttribute(string name, long value)
        {
            WriteMember(name);
            _formatter.Append(value);
            WriteAttributeEnd();
        }

        public void WriteAttribute(string name, ulong value)
        {
            WriteMember(name);
            _formatter.Append(value);
            WriteAttributeEnd();
        }

        public void WriteAttribute(string name, uint value)
        {
            WriteMember(name);
            _formatter.Append(value);
            WriteAttributeEnd();
        }

        public void WriteAttribute(string name, int value)
        {
            WriteMember(name);
            _formatter.Append(value);
            WriteAttributeEnd();
        }

        public void WriteAttribute(string name, ushort value)
        {
            WriteMember(name);
            _formatter.Append(value);
            WriteAttributeEnd();
        }

        public void WriteAttribute(string name, short value)
        {
            WriteMember(name);
            _formatter.Append(value);
            WriteAttributeEnd();
        }

        public void WriteAttribute(string name, char value)
        {
            WriteMember(name);
            _formatter.Append('"');
            _formatter.Append(value);
            _formatter.Append('"');
            WriteAttributeEnd();
        }

        public void WriteAttribute(string name, string value)
        {
            WriteMember(name);
            _formatter.Append('"');
            _formatter.Append(value);
            _formatter.Append('"');
            WriteAttributeEnd();
        }

        public void WriteAttribute(string name, bool value)
        {
            WriteMember(name);
            _formatter.Append(value ? "true" : "false");
            WriteAttributeEnd();
        }

        public void WriteAttributeNull(string name)
        {
            WriteMember(name);
            _formatter.Append("null");
            WriteAttributeEnd();
        }

        public void WriteString(string value)
        {
            ConsideComma();
            WriteIndent();
            _formatter.Append('"');
            _formatter.Append(value);
            _formatter.Append('"');
            _wroteListItem = true;
        }

        private void WriteAttributeEnd()
        {
            _wroteListItem = true;

        }
        public void WriteMember(string name)
        {
            ConsideComma();
            WriteIndent();
            _formatter.Append('"');
            _formatter.Append(name);
            _formatter.Append("\":");
        }

        private void ConsideComma()
        {
            if (_wroteListItem)
            {
                _formatter.Append(',');
                WriteNewline();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteNewline()
        {
            if (PrettyPrint)
            {
                _formatter.Append('\n');
            }
        }

        private void WriteIndent()
        {
            if (_prettyPrint)
            {
                for (int i = 0; i < _indent; i++)
                {
                    _formatter.Append("    ");
                }
            }
        }
    }
}
