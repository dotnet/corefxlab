// Licensed to the .NET Foundation under one or more agreements. 
// The .NET Foundation licenses this file to you under the MIT license. 
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace ALCProxy.Communication
{
    public class ClientDispatch : ALCClient
    {
        public ClientDispatch(Type intType) : base(intType, "ServerDispatch`1") { }
        protected override object SerializeParameter(object param, Type paramType)
        {
            MemoryStream stream = new MemoryStream();
            //Serialize the Record object to a memory stream using DataContractSerializer.  
            DataContractSerializer serializer = new DataContractSerializer(paramType);
            serializer.WriteObject(stream, param);
            return stream;

        }
        protected override object DecryptReturnType(object s, Type returnType)
        {
            MemoryStream stream = (MemoryStream)s;
            stream.Position = 0;
            //Deserialize the Record object back into a new record object.  
            DataContractSerializer newSerializer = new DataContractSerializer(returnType);
            return newSerializer.ReadObject(stream);
        }
    }
    public class ServerDispatch<InterfaceType> : ALCServer<InterfaceType>
    {
        public ServerDispatch(Type instanceType, Type[] genericTypes, IList<object> constructorParams, IList<Type> constTypes) 
            : base(instanceType, genericTypes, constructorParams, constTypes) { }

        protected override object DecryptParameter(object serializedParam, Type t)
        {
            MemoryStream s = (MemoryStream)serializedParam;
            s.Position = 0;
            //Deserialize the Record object back into a new record object.  
            DataContractSerializer newSerializer = new DataContractSerializer(t);
            object obj = newSerializer.ReadObject(s);
            return obj;
        }

        /// <summary>
        /// Once we've completed our method call to the real object, we need to convert the return type back into our type from the original ALC 
        /// the proxy is in, so we turn our returned object back into a stream that the client can decode
        /// </summary>
        /// <param name="returnedObject"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        protected override object EncryptReturnObject(object returnedObject, Type returnType)
        {
            MemoryStream stream = new MemoryStream();
            DataContractSerializer s = new DataContractSerializer(returnType);
            s.WriteObject(stream, returnedObject);
            return stream;
        }
    }

}
