// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Text.Formatting;
using System.Text;

namespace System.Text.Json
{
    public struct JsonWriter<TFormatter> where TFormatter: IFormatter
    {
        TFormatter _formatter;
        bool _wroteListItem;
        bool _prettyPrint;
        int _indent;

        public JsonWriter(TFormatter formatter, bool prettyPrint = false)
        {
            _wroteListItem = false;
            _prettyPrint = prettyPrint;
            _indent = 0;
            _formatter = formatter;
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
            if (_wroteListItem) {
                _formatter.Append(',');
                WriteNewline();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteNewline()
        {
            if (PrettyPrint) {
                _formatter.Append('\n');
            }
        }

        private void WriteIndent()
        {
            if (_prettyPrint) {
                for (int i = 0; i < _indent; i++) {
                    _formatter.Append("    ");
                }
            }
        }
    }
}

