// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Runtime.Serialization;

namespace ALCProxy.Communication
{
    public class ClientDispatch : ALCClient
    {
        public ClientDispatch(Type intType) : base(intType, "ServerDispatch`1") { }
        protected override object SerializeParameter(object param, Type paramType)
        {
            var stream = new MemoryStream();
            //Serialize the Record object to a memory stream using DataContractSerializer.  
            var serializer = new DataContractSerializer(paramType);
            serializer.WriteObject(stream, param);
            return stream;

        }
        protected override object DeserializeReturnType(object s, Type returnType)
        {
            if (!s.GetType().Equals(typeof(MemoryStream)))
            {
                throw new Exception("The Server passed the wrong type to the client when returning an object"); 
            }
            var stream = (MemoryStream)s;
            stream.Position = 0;
            //Deserialize the Record object back into a new record object.  
            var newSerializer = new DataContractSerializer(returnType);
            return newSerializer.ReadObject(stream);
        }
    }
}
