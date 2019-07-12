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
    public class ClientDispatch : ALCProxy.Communication.DispatchProxy, IClientObject
    {
        //Can't make this an IServerObject directly due to the type-loading barrier
        private object _server;
        private Type _intType;
        public ClientDispatch(Type interfaceType)
        {
            _intType = interfaceType;
        }
        private Type FindTypeInAssembly(string typeName, Assembly a)
        {
            //find the type we're looking for
            Type t = null;
            foreach (Type ty in a.GetTypes())
            {
                if (ty.Name.Equals(typeName) || (ty.Name.StartsWith(typeName) && ty.Name.Contains("`"))) // will happen for non-generic types, generics we need to find the additional "`1" afterwards
                {
                    t = ty;
                    break;
                }
            }
            if (t == null)
            {
                //no type asked for in the assembly
                throw new Exception("Proxy creation exception: No valid type while searching for the given type");
            }
            return t;
        }
        /// <summary>
        /// Creates the link between the client and the server, while also passing in all the information to the server for setup
        /// </summary>
        /// <param name="alc">The target AssemblyLoadContext</param>
        /// <param name="typeName">Name of the proxied type</param>
        /// <param name="assemblyPath">path of the assembly to the type</param>
        /// <param name="genericTypes">any generics that we need the proxy to work with</param>
        public void SetUpServer(AssemblyLoadContext alc, string typeName, string assemblyPath, Type[] genericTypes)
        {
            Assembly a = alc.LoadFromAssemblyPath(assemblyPath);
            //find the type we're looking for
            Type objType = FindTypeInAssembly(typeName, a);
            //Load this assembly in so we can get the server into the ALC
            Assembly aa = alc.LoadFromAssemblyPath(Assembly.GetAssembly(typeof(ClientDispatch)).CodeBase.Substring(8));
            Type serverType = FindTypeInAssembly("ServerDispatch`1", aa);
            //Set up all the generics to allow for the serverDispatch to be created correctly
            Type constructedType = serverType.MakeGenericType(_intType);
            //Give the client its reference to the server
            _server = constructedType.GetConstructor(new Type[] { typeof(Type), typeof(Type[]) }).Invoke(new object[] { objType, genericTypes });
            //Attach to the unloading event
            //TODO debug unloading correctly
            //Delegate handler = Delegate.CreateDelegate(typeof(EventHandler), this, this.GetType().GetMethod("UnloadClient", Type.EmptyTypes));
            //alc.Unloading += handler;
            alc.Unloading += UnloadClient;
        }
        private void UnloadClient(object sender)
        {
            _server = null; //unload only removes the reference to the proxy, doesn't do anything else, since the ALCs need to be cleaned up by the users before the GC can collect.
        }
        /// <summary>
        /// Converts each argument into a stream using DataContractSerializer so it can be sent over in a call-by-value fashion
        /// </summary>
        /// <param name="method">the methodInfo of the target method</param>
        /// <param name="args">the current objects assigned as arguments to send</param>
        /// <returns></returns>
        public object SendMethod(MethodInfo method, object[] args)
        {
            if(_server == null) //We've called the ALC unload, so the proxy has been cut off
            {
                throw new InvalidOperationException("Error in ALCProxy: Proxy has been unloaded, or communication server was never set up correctly");
            }
            List<MemoryStream> streams = new List<MemoryStream>();
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
                //serializers.Add(serializer);
                types.Add(t);
            }
            MethodInfo callMethod = _server.GetType().GetMethod("CallObject");
            callMethod.GetParameters();
            MemoryStream encryptedReturn = (MemoryStream) /*always returns a MemoryStream*/callMethod.Invoke(_server, new object[] { method, streams, /*serializers,*/ types });
            return DecryptReturnType(encryptedReturn, method.ReturnType);
        }
        private object DecryptReturnType(MemoryStream stream, Type returnType)
        {
            stream.Position = 0;

            //Deserialize the Record object back into a new record object.  
            DataContractSerializer newSerializer = new DataContractSerializer(returnType);
            object obj = newSerializer.ReadObject(stream);
            return obj;

        }
        //This should always be a "SendMethod" option, with 2 args, the methodinfo of what needs to be sent and the args for said method
        protected override object Invoke(MethodInfo method, object[] args)
        {
            return SendMethod(method, args);
        }
    }

    public class ServerDispatch<ObjectInterface> : IServerObject
    {
        public object instance;
        public ServerDispatch(Type instanceType, Type[] genericTypes)
        {
            if (genericTypes != null)
            {
                instanceType = instanceType.MakeGenericType(genericTypes.Select(x => ConvertType(x)).ToArray());
            }
            SetParameters(instanceType, new Type[] { }, new object[] { });
        }
        internal static IServerObject Create<Dispatch>(Type type, Type[] genericTypes)
        {
            return new ServerDispatch<Dispatch>(type, genericTypes);
        }
        private void SetParameters(Type instanceType, Type[] constructorTypes, object[] constructorArgs)
        {
            instance = instanceType.GetConstructor(constructorTypes).Invoke(constructorArgs);
        }
        public Type ConvertType(Type toConvert)
        {
            string assemblyPath = Assembly.GetAssembly(toConvert).CodeBase.Substring(8);
            if (toConvert.IsPrimitive || assemblyPath.Contains("System.Private.CoreLib")) //Can't load/dont want to load extra types from System.Private.CoreLib
            {
                return toConvert;
            }
            return AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly()).LoadFromAssemblyPath(assemblyPath).GetType(toConvert.FullName);
        }
        public object CallObject(MethodInfo targetMethod, List<MemoryStream> streams, List<Type> argTypes)
        {
            //Turn the memstreams into their respective objects
            argTypes = argTypes.Select(x => ConvertType(x)).ToList();
            object[] args = DecryptStreams(streams, argTypes);

            MethodInfo[] methods = instance.GetType().GetMethods();
            MethodInfo m = FindMethod(methods, targetMethod.Name, argTypes.ToArray());
            if (m.ContainsGenericParameters)
            {
                //While this may work without the conversion, we want it to uphold the type-load boundary, don't let the passed in method use anything from outside the target ALC
                m = m.MakeGenericMethod(targetMethod.GetGenericArguments().Select(x => ConvertType(x)).ToArray());
            }
            return EncryptReturnObject(m.Invoke(instance, args), m.ReturnType);
        }
        private MethodInfo FindMethod(MethodInfo[] methods, string methodName, Type[] parameterTypes)
        {
            foreach(MethodInfo m in methods)
            {
                if (!m.Name.Equals(methodName) || parameterTypes.Length != m.GetParameters().Length)//|| m.GetParameters().Select(x => x.ParameterType) != parameterTypes)
                {
                    continue;
                }
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
            throw new Exception("Error in ALCProxy: Method Not found for " + instance.ToString() + ": " + methodName);
        }
        /// <summary>
        /// Takes the memory streams passed into the server and turns them into the specific objects we want, in the desired types we want
        /// </summary>
        /// <param name="streams"></param>
        /// <param name="argTypes"></param>
        /// <returns></returns>
        private object[] DecryptStreams(List<MemoryStream> streams, List<Type> argTypes)
        {
            var convertedObjects = new List<object>();

            for (int i = 0; i <streams.Count; i++)
            {
                MemoryStream s = streams[i];
                Type t = argTypes[i];
                s.Position = 0;

                //Deserialize the Record object back into a new record object.  
                DataContractSerializer newSerializer = new DataContractSerializer(t);
                object obj = newSerializer.ReadObject(s);
                convertedObjects.Add(obj);
            }
            return convertedObjects.ToArray();
        }
        private object EncryptReturnObject(object returnedObject, Type returnType)
        {
            MemoryStream stream = new MemoryStream();
            DataContractSerializer s = new DataContractSerializer(returnType);
            s.WriteObject(stream, returnedObject);
            return stream;
        }
    }
}
