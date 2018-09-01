// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Microsoft.Collections.Extensions.Tests
{
    public class RefDictionaryTests
    {
        [Fact]
        public void FirstTest()
        {
            var d = new RefDictionary<ulong, int>();
            d[7]++;
            d[7]++;
            Assert.Equal(2, d[7]);
        }
    }
}
