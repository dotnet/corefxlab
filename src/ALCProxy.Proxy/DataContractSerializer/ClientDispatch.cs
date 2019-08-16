// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using ALCProxy.Communication;
using System;
using System.IO;
using System.Runtime.Serialization;

namespace ALCProxy.Proxy
{
    public class ClientDispatch : ALCClient
    {
        /// <summary>
        /// Inherits directly from ALCClient, it's partner server is ServerDispatch<Interface>, 
        /// which is why it's being called from the base class in the constructor
        /// </summary>
        /// <param name="intType">The interface type of the proxied object</param>
        public ClientDispatch(Type intType) : base(intType, "ALCProxy.Proxy.ServerDispatch`1", typeof(ServerDispatch<>)) { }

        /// <summary>
        /// Serializes a parameter that needs to be sent to the server
        /// </summary>
        /// <param name="param">The object to be serialized</param>
        /// <param name="paramType">The base type of the object</param>
        /// <returns>A memory stream holding the serialized object</returns>
        protected override object SerializeParameter(object param, Type paramType)
        {
            var stream = new MemoryStream();
            // Serialize the Record object to a memory stream using DataContractSerializer.  
            var serializer = new DataContractSerializer(paramType);
            serializer.WriteObject(stream, param);
            return stream;
        }

        /// <summary>
        /// Deserializes any return type from the called method
        /// </summary>
        /// <param name="s">The memoryStream that represents the serialized object</param>
        /// <param name="returnType">The return type to convert our serialized stream to</param>
        /// <returns>The deserialized object</returns>
        protected override object DeserializeReturnType(object s, Type returnType)
        {
            if (!s.GetType().Equals(typeof(MemoryStream)))
            {
                throw new Exception("The Server passed the wrong type to the client when returning an object");
            }
            var stream = (MemoryStream)s;
            stream.Position = 0;
            // Deserialize the Record object back into a new record object.  
            var newSerializer = new DataContractSerializer(returnType);
            return newSerializer.ReadObject(stream);
        }
    }
}

