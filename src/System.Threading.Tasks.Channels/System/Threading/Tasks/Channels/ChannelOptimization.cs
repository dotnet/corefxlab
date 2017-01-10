﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Threading.Tasks.Channels
{
    public sealed class ChannelOptimizations
    {
        public bool SingleWriter { get; set; } = false;
        public bool SingleReader { get; set; } = false;
        public bool SingleReaderWriter
        {
            get { return SingleWriter & SingleReader; }
            set
            {
                SingleWriter = value;
                SingleReader = value;
            }
        }
    }
}
