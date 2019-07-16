using System;
using System.Collections;
using System.Collections.Generic;

namespace ALCProxy.TestAssembly
{
    public interface IExternalClass
    {
        void PrintToConsole();
        string GetCurrentContext();
        int GetUserParameter(int a);
        IEnumerable<string> PassGenericObjects(IDictionary<string, string> a);
    }
}
