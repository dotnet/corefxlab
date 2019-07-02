// Licensed to the .NET Foundation under one or more agreements. 
// The .NET Foundation licenses this file to you under the MIT license. 
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Runtime.Serialization;

namespace ALCProxy.Communication
{
    public class ClientObject : ALCProxy.Communication.DispatchProxy, IClientObject
    {
        public static void PrintALC()
        {
            var a = Assembly.GetExecutingAssembly();
            Console.WriteLine(AssemblyLoadContext.GetLoadContext(a).Name);

        }
        //Can't make this an IServerObject directly due to the type-isolation barrier
        private object _server;
        //private ConditionalWeakTable<ClientObject<InterfaceType>, ServerDispatch<InterfaceType>> _serverTable;
        private Type _intType;
        public ClientObject(Type interfaceType)
        {
            _intType = interfaceType;
        }
        private Type FindType(string typeName, Assembly a)
        {
            //find the type we're looking for
            Type t = null;
            foreach (Type ty in a.GetTypes())
            {
                if (ty.Name.Equals(typeName)) // will happen for non-generic types, generics we need to find the additional "`1" afterwards
                {
                    t = ty;
                    break;
                }
            }
            if (t == null)
            {
                //no type asked for in the assembly
                throw new Exception();
            }
            return t;
        }
        public void SetUpServer(AssemblyLoadContext alc, string typeName, string assemblyPath, bool isGeneric)
        {
            Assembly a = alc.LoadFromAssemblyPath(assemblyPath);
            //find the type we're looking for
            Type objType = FindType(typeName, a);
            //Load this assembly in so we can get the server into the ALC
            Assembly aa = alc.LoadFromAssemblyPath(Assembly.GetAssembly(typeof(ClientObject)).CodeBase.Substring(8));
            Type serverType = FindType("ServerDispatch`1", aa);
            //Set up all the generics to allow for the serverDispatch to be created correctly
            Type constructedType = serverType.MakeGenericType(_intType);
            //Give the client its reference to the server
            _server = constructedType.GetConstructor(new Type[] { typeof(Type) }).Invoke(new object[] { objType });
            //Attach to the unloading event
            alc.Unloading += Unload;
        }
        private void Unload(object sender)
        {
            _server = null;
            //GC.Collect(); //Probably don't want to force anything here but it could be an option.
        }

        /// <summary>
        /// Converts each argument into a stream using DataContractSerializer so it can be sent over in a call-by-value fashion
        /// </summary>
        /// <param name="method">the methodInfo of the target method</param>
        /// <param name="args">the current objects assigned as arguments to send</param>
        /// <returns></returns>
        public object SendMethod(MethodInfo method, object[] args)
        {
            //List<(MemoryStream, DataContractSerializer, Type)> streams = new List<(MemoryStream, DataContractSerializer, Type)>();
            List<MemoryStream> streams = new List<MemoryStream>();
            List<DataContractSerializer> serializers = new List<DataContractSerializer>();
            List<Type> types = new List<Type>();
            Type[] argTypes = args.Select(obj => obj.GetType()).ToArray();

            for (int i = 0; i < args.Length; i++)
            {
                object arg = args[i];
                MemoryStream stream = new MemoryStream();
                Type t = arg.GetType();
                //Serialize the Record object to a memory stream using DataContractSerializer.  
                DataContractSerializer serializer = new DataContractSerializer(arg.GetType());
                serializer.WriteObject(stream, arg);
                //streams.Add( (stream, serializer, argTypes[i]) );
                streams.Add(stream);
                serializers.Add(serializer);
                types.Add(t);
            }
            MethodInfo callMethod = _server.GetType().GetMethod("CallObject");
            callMethod.GetParameters();
            return callMethod.Invoke(_server, new object[] { method, streams, serializers, types });
            //return method.Invoke(_server, streams.ToArray());
        }
        //This should always be a "SendMethod" option, with 2 args, the methodinfo of what needs to be sent and the args for said method
        protected override object Invoke(MethodInfo method, object[] args)
        {
            return method.Invoke(_server, args);
        }
    }
    public class ServerDispatch<ObjectInterface> : IServerObject
    {
        public object instance;

        public ServerDispatch(Type type)
        {
            SetParameters(type, new Type[] { }, new object[] { });
        }
        internal static IServerObject Create<Dispatch>(Type type)
        {
            return new ServerDispatch<Dispatch>(type);
        }
        private void SetParameters(Type instanceType, Type[] constructorTypes, object[] constructorArgs)
        {
            instance = instanceType.GetConstructor(constructorTypes).Invoke(constructorArgs);
        }

        //TODO Get type conversion working
        public Type ConvertType(Type toConvert)
        {
            string assemblyPath = Assembly.GetAssembly(toConvert).CodeBase.Substring(8);
            if (toConvert.IsPrimitive || assemblyPath.Contains("System.Private.CoreLib")) //Can't load/dont want to load extra types from System.Private.CoreLib
            {
                return toConvert;
            }
            return AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly()).LoadFromAssemblyPath(assemblyPath).GetType(toConvert.FullName);
        }

        public object CallObject(MethodInfo targetMethod, List<MemoryStream> streams, List<DataContractSerializer> serializers, List<Type> argTypes)
        {
            ClientObject.PrintALC();
            //Turn the memstreams into their respective objects
            argTypes = argTypes.Select(x => ConvertType(x)).ToList();
            //TODO user made classes still don't work, need to figure out why
            object[] args = DecryptStreams(streams, serializers, argTypes);

            MethodInfo[] methods = instance.GetType().GetMethods();
            MethodInfo m = FindMethod(methods, targetMethod.Name, argTypes.ToArray());
            return m.Invoke(instance, args);
        }

        private MethodInfo FindMethod(MethodInfo[] methods, string methodName, Type[] parameterTypes)
        {
            foreach(MethodInfo m in methods)
            {
                if (!m.Name.Equals(methodName) || parameterTypes.Length != m.GetParameters().Length)//|| m.GetParameters().Select(x => x.ParameterType) != parameterTypes)
                {
                    continue;
                }
                // List<Type> methodParams = new List<Type>();
                bool methodParamsAlligned = true;
                for(int i = 0; i < parameterTypes.Length; i++)
                {
                    //TODO find a better way to match method types then comparing their full names
                    if (!(m.GetParameters()[i].ParameterType.FullName.Equals(parameterTypes[i].FullName)))
                    {
                        methodParamsAlligned = false;
                        break;
                    }
                }
                if (!methodParamsAlligned)
                    continue;
                return m;
            }
            throw new Exception("Method Not found");
        }

        private object[] DecryptStreams(List<MemoryStream> streams, List<DataContractSerializer> serializers, List<Type> argTypes)
        {
            var convertedObjects = new List<object>();

            for (int i = 0; i <streams.Count; i++)
            {
                MemoryStream s = streams[i];
                //TODO remove datacontractserializer from the transfer, we don't need it since we make new ones on the deserialization side
                //DataContractSerializer serializer = serializers[i];
                Type t = argTypes[i];
                s.Position = 0;

                //Deserialize the Record object back into a new record object.  
                DataContractSerializer newSerializer = new DataContractSerializer(t);
                object obj = newSerializer.ReadObject(s);
                convertedObjects.Add(obj);
            }
            return convertedObjects.ToArray();
        }
    }
}
