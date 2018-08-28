// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
            d.GetRef(7)++;
            d.GetRef(7)++;
            Assert.Equal(2, d.GetRef(7));
        }
    }
}
