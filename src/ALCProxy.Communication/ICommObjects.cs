// Licensed to the .NET Foundation under one or more agreements. 
// The .NET Foundation licenses this file to you under the MIT license. 
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace ALCProxy.Communication
{
    public interface IServerObject
    {
        object CallObject(MethodInfo method, List<MemoryStream> streams, List<Type> types);
    }
    public interface IClientObject
    {
        object SendMethod(MethodInfo method, object[] args);
        void SetUpServer(AssemblyLoadContext alc, string typeName, string assemblyPath, bool isGeneric);
    }
}
