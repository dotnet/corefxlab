// Licensed to the .NET Foundation under one or more agreements. 
// The .NET Foundation licenses this file to you under the MIT license. 
// See the LICENSE file in the project root for more information. 

using ALCProxy.Communication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace ALCProxy.Proxy
{
    public class ServerDispatch<I> : ALCServer<I>
    {
        public ServerDispatch(Type instanceType, Type[] genericTypes, IList<object> constructorParams, IList<Type> constTypes)
            : base(instanceType, genericTypes, constructorParams, constTypes) { }

        /// <summary>
        /// Deserializes an memstream sent from the ClientDispatch object. Used when methods with arguments are sent over from the client to the server.
        /// </summary>
        protected override object DeserializeParameter(object serializedParam, Type t)
        {
            if (!serializedParam.GetType().Equals(typeof(MemoryStream)))
            {
                throw new Exception("The Server passed the wrong type to the client when returning an object");
            }
            var s = (MemoryStream)serializedParam;
            s.Position = 0;
            // Deserialize the Record object back into a new record object.  
            var newSerializer = new DataContractSerializer(t);
            object obj = newSerializer.ReadObject(s);
            return obj;
        }

        /// <summary>
        /// Once we've completed our method call to the real object, we need to convert the return type back into our type from the original ALC 
        /// the proxy is in, so we turn our returned object back into a stream that the client can decode
        /// </summary>
        protected override object SerializeReturnObject(object returnedObject, Type returnType)
        {
            var stream = new MemoryStream();
            var s = new DataContractSerializer(returnType);
            s.WriteObject(stream, returnedObject);
            return stream;
        }
    }
}
