// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using Xunit;
using System.Drawing.Graphics;

public partial class GraphicsUnitTests
{
    [Fact]
    public void Test()
    {
        System.Console.WriteLine("hi");

        IntPtr imgPtr = DLLImports.gdImageCreate(1, 1);
        System.Console.WriteLine(imgPtr.ToInt64());
    }
    
}