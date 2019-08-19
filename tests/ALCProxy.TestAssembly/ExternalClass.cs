// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using ALCProxy.TestInterface;

namespace ALCProxy.TestAssembly
{
    public class ExternalClass : IExternalClass
    {
        private int _a;
        private string _b;
        public ExternalClass()
        {
            _a = 5;
            _b = "Hello world!";
        }
        public ExternalClass(int a, string b)
        {
            _a = a;
            _b = b;
        }
        public string GetCurrentContext()
        {
            var a = Assembly.GetExecutingAssembly();
            return AssemblyLoadContext.GetLoadContext(a).Name;
        }

        public int GetUserParameter(int a)
        {
            return a;
        }

        public IList<string> PassGenericObjects(IDictionary<string, string> a)
        {
            int count = a.Count;
            IList<string> toReturn = new List<string>();
            //toReturn.Add(a.ToString());
            for(int i = 0; i < count; i++)
            {
                toReturn.Add(_b);
            }
            return toReturn;
        }

        public void PrintToConsole()
        {
            var a = Assembly.GetExecutingAssembly();
            Console.WriteLine(AssemblyLoadContext.GetLoadContext(a).Name);
        }
    }
}
