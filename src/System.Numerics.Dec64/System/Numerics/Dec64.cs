// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Runtime.InteropServices;

namespace System.Numerics
{
    /// <summary>
    /// Represents a decimal floating point value as defined by Douglas Crockford in https://github.com/douglascrockford/DEC64
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Dec64 : IComparable, IFormattable, IComparable<Dec64>, IEquatable<Dec64>
    {
        public readonly static Dec64 NaN = new Dec64() {  _value = 0x80UL };
        public readonly static Dec64 Zero = default;
        public readonly static Dec64 One = new Dec64() { _value = 0x100UL };
        public readonly static Dec64 True = new Dec64() { _value = 0x380UL };
        public readonly static Dec64 False = new Dec64() { _value = 0x280UL };
        public readonly static Dec64 MinusOne = new Dec64() {_value = 0xFFFFFFFFFFFFFF00UL };

        private ulong _value;

        public Dec64(byte[] value)
        {
            throw new NotImplementedException();
        }

        public Dec64(decimal value)
        {
            throw new NotImplementedException();
        }

        public Dec64(double value)
        {
            throw new NotImplementedException();
        }

        public Dec64(int value)
        {
            throw new NotImplementedException();
        }

        public Dec64(long value)
        {
            throw new NotImplementedException();
        }

        public Dec64(float value)
        {
            throw new NotImplementedException();
        }

        public Dec64(uint value)
        {
            throw new NotImplementedException();
        }

        public Dec64(ulong value)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return default;
        }
    }
}
