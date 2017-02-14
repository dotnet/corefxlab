// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace NetCoreXunitTestLibrary
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal("Hello", NetCoreLibrary.Class1.SayHello());
        }
    }
}
