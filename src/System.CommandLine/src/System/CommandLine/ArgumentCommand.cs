// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace System.CommandLine
{
    public abstract class ArgumentCommand
    {
        internal ArgumentCommand(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public string Help { get; set; }

        public object Value
        {
            get { return GetValue(); }
        }

        public bool IsHidden { get; set; }

        public bool IsActive { get; private set; }

        internal abstract object GetValue();

        internal void MarkActive()
        {
            IsActive = true;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}