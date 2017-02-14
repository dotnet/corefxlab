// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection.Emit;

namespace System.Reflection.Metadata.Cil.Instructions
{
    public abstract class CilInstructionWithValue<T> : CilInstruction
    {
        private T _value;
        private int _token;

        internal CilInstructionWithValue(OpCode opCode, T value, int token, int size)
            : base(opCode, size)
        {
            _value = value;
            _token = token;
        }

        public T Value
        {
            get
            {
                return _value;
            }
        }

        public int Token
        {
            get
            {
                return _token;
            }
        }
    }
}
