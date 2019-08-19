// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace ALCProxy.Communication
{
    /// <summary>
    /// The client interface we can wrap both in-proc and out-of-proc proxies around, will add more methods here as they are found needed by both versions
    /// </summary>
    public interface IProxyServer
    {
        /// <summary>
        /// Sends a message to the server to proc the method call, and return the result
        /// </summary>
        /// <param name="method">the method that needs to be called</param>
        /// <param name="streams">The parameters for the given method, converted into a serialized object by the client that now need to be deserialized</param>
        /// <param name="types">The types of each stream, so the server knows how to decode the streams</param>
        /// <returns></returns>
        object CallObject(MethodInfo method, IList<object> streams, IList<Type> types);
    }
}
