// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace System.CommandLine
{
    public sealed class ArgumentCommand<T> : ArgumentCommand
    {
        internal ArgumentCommand(string name, T value)
            : base(name)
        {
            Value = value;
        }

        public new T Value { get; private set; }

        internal override object GetValue()
        {
            return Value;
        }
    }
}