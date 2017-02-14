// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection.Emit;

namespace System.Reflection.Metadata.Cil.Instructions
{
    public abstract class CilNumericValueInstruction<T> : CilInstructionWithValue<T>
    {
        private string _bytes;
        internal CilNumericValueInstruction(OpCode opCode, T value, int token, int size)
            :base(opCode, value, token, size)
        {
        }

        public string Bytes
        {
            get
            {
                if(_bytes == null)
                {
                    _bytes = GetBytes();
                }
                return _bytes;
            }
        }

        protected abstract string GetBytes();
    }
}
