// Licensed to the .NET Foundation under one or more agreements. 
// The .NET Foundation licenses this file to you under the MIT license. 
// See the LICENSE file in the project root for more information. 

using System;
using System.Reflection;
using System.Runtime.Loader;

namespace ALCProxy.Communication
{
    /// <summary>
    /// The server interface we can wrap both in-proc and out-of-proc proxies around, will add more methods here as they are found needed by both versions
    /// </summary>
    public interface IProxyClient
    {
        /// <summary>
        /// Sends a message to the server to proc the method call, and return the result
        /// </summary>
        /// <param name="method">The method information of what needs to be called</param>
        /// <param name="args"></param>
        /// <returns>Whatever the target Method returns. We need to make sure that whatever gets returned is not of a type that is in our target ALC</returns>
        object SendMethod(MethodInfo method, object[] args);
        /// <summary>
        /// Creates the link between the client and the server, while also passing in all the information to the server for setup
        /// </summary>
        /// <param name="alc">The target AssemblyLoadContext</param>
        /// <param name="typeName">Name of the proxied type</param>
        /// <param name="assemblyPath">path of the assembly to the type</param>
        /// <param name="genericTypes">any generics that we need the proxy to work with</param>
        void SetUpServer(AssemblyLoadContext alc, string typeName, AssemblyName assemblyName, object[] constructorParams, Type[] genericTypes);
    }
}
