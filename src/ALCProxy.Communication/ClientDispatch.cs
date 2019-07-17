using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

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
        protected override object DeserializeReturnType(object s, Type returnType)
        {
            if (!s.GetType().Equals(typeof(MemoryStream)))
            {
                throw new Exception("The Server passed the wrong type to the client when returning an object"); 
            }
            MemoryStream stream = (MemoryStream)s;
            stream.Position = 0;
            //Deserialize the Record object back into a new record object.  
            DataContractSerializer newSerializer = new DataContractSerializer(returnType);
            return newSerializer.ReadObject(stream);
        }
    }
}
