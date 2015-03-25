// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text.Formatting {

    // This whole API is very speculative, i.e. I am not sure I am happy with the design
    // This API is trying to do composite formatting without boxing (or any other allocations).
    // And because not all types in the platfrom implement IBufferFormattable (in particular built-in primitives don't), 
    // it needs to play some tricks with generic type parameters. But as you can see at the end of AppendUntyped, I am not sure how to tick the type system
    // not never box.
    public static class CompositeFormattingExtensions
    {
        public static void Format<TFormatter, T0>(this TFormatter formatter, string compositeFormat, T0 arg0) where TFormatter : IFormatter
        {
            var reader = new CompositeFormatReader(compositeFormat);
            while (reader.Next() != CompositeFormatReader.State.End)
            {
                if (reader.Current == CompositeFormatReader.State.Literal) formatter.Append(reader.Literal);
                else if (reader.Current == CompositeFormatReader.State.InsertionPoint)
                {
                    if (reader.Insertion.ArgIndex == 0) formatter.AppendUntyped(arg0);
                    else throw new Exception("invalid insertion point");
                }
            }
        }

        public static void Format<TFormatter, T0, T1>(this TFormatter formatter, string compositeFormat, T0 arg0, T1 arg1) where TFormatter : IFormatter
        {
            var reader = new CompositeFormatReader(compositeFormat);
            while (reader.Next() != CompositeFormatReader.State.End)
            {
                if (reader.Current == CompositeFormatReader.State.Literal) formatter.Append(reader.Literal);
                else if (reader.Current == CompositeFormatReader.State.InsertionPoint)
                {
                    if (reader.Insertion.ArgIndex == 0) formatter.AppendUntyped(arg0);
                    else if (reader.Insertion.ArgIndex == 1) formatter.AppendUntyped(arg1);
                    else throw new Exception("invalid insertion point");
                }
            }
        }

        public static void Format<TFormatter, T0, T1, T2>(this TFormatter formatter, string compositeFormat, T0 arg0, T1 arg1, T2 arg2) where TFormatter : IFormatter
        {
            var reader = new CompositeFormatReader(compositeFormat);
            while (reader.Next() != CompositeFormatReader.State.End)
            {
                if (reader.Current == CompositeFormatReader.State.Literal) formatter.Append(reader.Literal);
                else if (reader.Current == CompositeFormatReader.State.InsertionPoint)
                {
                    if (reader.Insertion.ArgIndex == 0) formatter.AppendUntyped(arg0);
                    else if (reader.Insertion.ArgIndex == 1) formatter.AppendUntyped(arg1);
                    else if (reader.Insertion.ArgIndex == 2) formatter.AppendUntyped(arg2);
                    else throw new Exception("invalid insertion point");
                }
            }
        }

        public static void Format<TFormatter, T0, T1, T2, T3>(this TFormatter formatter, string compositeFormat, T0 arg0, T1 arg1, T2 arg2, T3 arg3) where TFormatter : IFormatter
        {
            var reader = new CompositeFormatReader(compositeFormat);
            while (reader.Next() != CompositeFormatReader.State.End)
            {
                if (reader.Current == CompositeFormatReader.State.Literal) formatter.Append(reader.Literal);
                else if (reader.Current == CompositeFormatReader.State.InsertionPoint)
                {
                    if (reader.Insertion.ArgIndex == 0) formatter.AppendUntyped(arg0);
                    else if (reader.Insertion.ArgIndex == 1) formatter.AppendUntyped(arg1);
                    else if (reader.Insertion.ArgIndex == 2) formatter.AppendUntyped(arg2);
                    else if (reader.Insertion.ArgIndex == 3) formatter.AppendUntyped(arg3);
                    else throw new Exception("invalid insertion point");
                }
            }
        }

        public static void Format<TFormatter, T0, T1, T2, T3, T4>(this TFormatter formatter, string compositeFormat, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where TFormatter : IFormatter
        {
            var reader = new CompositeFormatReader(compositeFormat);
            while (reader.Next() != CompositeFormatReader.State.End)
            {
                if (reader.Current == CompositeFormatReader.State.Literal) formatter.Append(reader.Literal);
                else if (reader.Current == CompositeFormatReader.State.InsertionPoint)
                {
                    if (reader.Insertion.ArgIndex == 0) formatter.AppendUntyped(arg0);
                    else if (reader.Insertion.ArgIndex == 1) formatter.AppendUntyped(arg1);
                    else if (reader.Insertion.ArgIndex == 2) formatter.AppendUntyped(arg2);
                    else if (reader.Insertion.ArgIndex == 3) formatter.AppendUntyped(arg3);
                    else if (reader.Insertion.ArgIndex == 4) formatter.AppendUntyped(arg4);
                    else throw new Exception("invalid insertion point");
                }
            }
        }

        static void AppendUntyped<TFormatter, T>(this TFormatter formatter, T value) where TFormatter : IFormatter
        {
            #region Built in types
            var i32 = value as int?;
            if (i32 != null)
            {
                formatter.Append(i32.Value);
                return;
            }
            var i64 = value as long?;
            if (i64 != null)
            {
                formatter.Append(i64.Value);
                return;
            }
            var i16 = value as short?;
            if (i16 != null)
            {
                formatter.Append(i16.Value);
                return;
            }
            var b = value as byte?;
            if (b != null)
            {
                formatter.Append(b.Value);
                return;
            }
            var c = value as char?;
            if (c != null)
            {
                formatter.Append(c.Value);
                return;
            }
            var u32 = value as uint?;
            if (u32 != null)
            {
                formatter.Append(u32.Value);
                return;
            }
            var u64 = value as ulong?;
            if (u64 != null)
            {
                formatter.Append(u64.Value);
                return;
            }
            var u16 = value as ushort?;
            if (u16 != null)
            {
                formatter.Append(u16.Value);
                return;
            }
            var sb = value as sbyte?;
            if (sb != null)
            {
                formatter.Append(sb.Value);
                return;
            }
            var str = value as string;
            if (str != null)
            {
                formatter.Append(str);
                return;
            }
            var dt = value as DateTime?;
            if (dt != null)
            {
                formatter.Append(dt.Value);
                return;
            }
            var ts = value as TimeSpan?;
            if (ts != null)
            {
                formatter.Append(ts.Value);
                return;
            }
            #endregion

            // I could uncomment the code below and then not throw for types that implement IBufferFormattable.
            // Unfortunatelly this would cause silent boxing, which I don't like.
            // To avoid breaking changes, I will just preemptivelly throw here instead of boxing
            throw new NotSupportedException("only built-in types are supported in composite formatting");

            //if(value is IBufferFormattable)
            //{
            //    formatter.Append((IBufferFormattable)value); // this is boxing. not sure how to avoid it.
            //    return;
            //}

            //else throw new NotSupportedException("value is not formattable.");
        }

        // this is just a state machine walking the composite format and instructing CompositeFormattingExtensions.Format overloads on what to do.
        // this whole type is not just a hacky prototype.
        // I will clean it up later if I decide that I like this whole composite format model.
        struct CompositeFormatReader
        {
            public CompositeFormatReader(string format)
            {
                _format = format;
                _index = -1;
                _insertionPointStart = -1;
                _state = State.New;
                _arg = 0;
            }

            string _format;
            int _index;
            int _insertionPointStart;
            State _state;
            uint _arg;

            public char Literal
            {
                get { return _format[_index]; }
            }
            public InsertionPoint Insertion
            {
                get { return new InsertionPoint() { ArgIndex = _arg }; }
            }
            public State Current
            {
                get
                {
                    return _state;
                }
            }

            public State Next()
            {
                _index++;
                if (_index == _format.Length) return State.End;

                char c = _format[_index];
                switch (c)
                {
                    case '{':
                        _insertionPointStart = _index + 1;
                        _index++;
                        _state = State.Ignore;
                        break;
                    case '}':
                        if (!InvariantParser.TryParse(_format, _insertionPointStart, _index - _insertionPointStart, out _arg))
                        {
                            throw new Exception("invalid insertion point");
                        }

                        _insertionPointStart = -1;
                        _state = State.InsertionPoint;
                        break;
                    default:
                        if (_insertionPointStart == -1)
                        {
                            _state = State.Literal;
                        }
                        else
                        {
                            _state = State.Ignore;
                        }
                        break;
                }
                return _state;
            }

            public enum State : byte
            {
                New,
                Ignore,
                Literal, // TODO: this should not return single chars. it should be span<char> 
                InsertionPoint,
                End
            }

            public struct InsertionPoint
            {
                public uint ArgIndex;
            }
        }
    }
}