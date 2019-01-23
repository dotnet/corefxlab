// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace System.Text.ValueBuilder.Tests
{
    public class HardCasts
    {
        [Fact]
        public void Cast()
        {
            long longValue = (long)(Variant)(-1);
        }
    }
}
