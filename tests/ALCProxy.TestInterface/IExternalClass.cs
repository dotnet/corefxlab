﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace ALCProxy.TestInterface
{
    public interface IExternalClass
    {
        void PrintToConsole();
        string GetCurrentContext();
        int GetUserParameter(int a);
        IList<string> PassGenericObjects(IDictionary<string, string> a);
    }
}
