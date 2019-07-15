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
    /// This currently is designed to only work in-process
    /// TODO: set up to allow for construction of out-of-proc proxies
    /// </summary>
    public abstract class ALCClient : IClientObject
    {
        //Can't make this an IServerObject directly due to the type-loading barrier
        protected object _server;
        protected string _serverTypeName;
        protected Type _intType;
        protected MethodInfo _callMethod;
        public ALCClient(Type interfaceType, string serverName)
        {
            _intType = interfaceType;
            _serverTypeName = serverName;
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
            //find the type we're going to proxy inside the loaded assembly
            Type objType = FindTypeInAssembly(typeName, a);
            //Load *this* (The CommObjects) assembly into the ALC so we can get the server into the ALC
            Assembly aa = alc.LoadFromAssemblyPath(Assembly.GetAssembly(typeof(ClientDispatch)).CodeBase.Substring(8));
            Type serverType = FindTypeInAssembly(_serverTypeName, aa);
            //Set up all the generics to allow for the serverDispatch to be created correctly
            Type constructedType = serverType.MakeGenericType(_intType);
            //Give the client its reference to the server
            _server = constructedType.GetConstructor(new Type[] { typeof(Type), typeof(Type[]) }).Invoke(new object[] { objType, genericTypes });
            _callMethod = _server.GetType().GetMethod("CallObject");
            //Attach to the unloading event
            alc.Unloading += UnloadClient;
        }
        private void UnloadClient(object sender)
        {
            _server = null; //unload only removes the reference to the proxy, doesn't do anything else, since the ALCs need to be cleaned up by the users before the GC can collect.
        }
        /// <summary>
        /// Converts each argument into a serialized version of the object so it can be sent over in a call-by-value fashion
        /// </summary>
        /// <param name="method">the methodInfo of the target method</param>
        /// <param name="args">the current objects assigned as arguments to send</param>
        /// <returns></returns>
        public object SendMethod(MethodInfo method, object[] args)
        {
            if (_server == null) //We've called the ALC unload, so the proxy has been cut off
            {
                throw new InvalidOperationException("Error in ALCClient: Proxy has been unloaded, or communication server was never set up correctly");
            }
            SerializeParameters(args, out IList<object> streams, out IList<Type> argTypes);
            object encryptedReturn = _callMethod.Invoke(_server, new object[] { method, streams, argTypes });
            return DecryptReturnType(encryptedReturn, method.ReturnType);
        }
        protected void SerializeParameters(object[] arguments, out IList<object> serializedArgs, out IList<Type> argTypes)
        {
            argTypes = new List<Type>();
            serializedArgs = new List<object>();
            for (int i = 0; i < arguments.Length; i++)
            {
                object arg = arguments[i];
                Type t = arg.GetType();
                //Serialize the argument
                object serialArg = SerializeParameter(arg, t);
                serializedArgs.Add(serialArg);
                argTypes.Add(t);
            }
        }
        protected abstract object SerializeParameter(object param, Type paramType);
        protected abstract object DecryptReturnType(object returnedObject, Type returnType);
    }
    public abstract class ALCServer<ObjectInterface> : IServerObject
    {
        public object instance;
        public AssemblyLoadContext currentLoadContext;
        public ALCServer(Type instanceType, Type[] genericTypes)
        {
            currentLoadContext = AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly());
            if (genericTypes != null)
            {
                instanceType = instanceType.MakeGenericType(genericTypes.Select(x => ConvertType(x)).ToArray());
            }
            SetParameters(instanceType, new Type[] { }, new object[] { });
        }
        protected void SetParameters(Type instanceType, Type[] constructorTypes, object[] constructorArgs)
        {
            instance = instanceType.GetConstructor(constructorTypes).Invoke(constructorArgs);
        }
        protected Type ConvertType(Type toConvert)
        {
            string assemblyPath = Assembly.GetAssembly(toConvert).CodeBase.Substring(8);
            if (toConvert.IsPrimitive || assemblyPath.Contains("System.Private.CoreLib")) //Can't load/dont want to load extra types from System.Private.CoreLib
            {
                return toConvert;
            }
            return currentLoadContext.LoadFromAssemblyPath(assemblyPath).GetType(toConvert.FullName);
        }
        public object CallObject(MethodInfo targetMethod, List<object> streams, List<Type> argTypes)
        {
            //Turn the memstreams into their respective objects
            argTypes = argTypes.Select(x => ConvertType(x)).ToList();
            object[] args = DecryptParameters(streams, argTypes);

            MethodInfo[] methods = instance.GetType().GetMethods();
            MethodInfo m = FindMethod(methods, targetMethod.Name, argTypes.ToArray());
            if (m.ContainsGenericParameters)
            {
                //While this may work without the conversion, we want it to uphold the type-load boundary, don't let the passed in method use anything from outside the target ALC
                m = m.MakeGenericMethod(targetMethod.GetGenericArguments().Select(x => ConvertType(x)).ToArray());
            }
            return EncryptReturnObject(m.Invoke(instance, args), m.ReturnType);
        }
        protected MethodInfo FindMethod(MethodInfo[] methods, string methodName, Type[] parameterTypes/*These have already been converted so no issues with compatibility*/)
        {
            foreach (MethodInfo m in methods)
            {
                if (!m.Name.Equals(methodName) || parameterTypes.Length != m.GetParameters().Length)
                {
                    continue;
                }
                bool methodParamsAlligned = true;
                for (int i = 0; i < parameterTypes.Length; i++)
                {
                    if (!(m.GetParameters()[i].ParameterType.Equals(parameterTypes[i])))
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
        protected object[] DecryptParameters(List<object> streams, List<Type> argTypes)
        {
            var convertedObjects = new List<object>();
            for (int i = 0; i < streams.Count; i++)
            {
                object s = streams[i];
                Type t = argTypes[i];
                object obj = DecryptParameter(s, t);
                convertedObjects.Add(obj);
            }
            return convertedObjects.ToArray();
        }
        protected abstract object DecryptParameter(object serializedParam, Type paramType);
        /// <summary>
        /// Once we've completed our method call to the real object, we need to convert the return type back into our type from the original ALC 
        /// the proxy is in, so we turn our returned object back into a stream that the client can decode
        /// </summary>
        /// <param name="returnedObject"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        protected abstract object EncryptReturnObject(object returnedObject, Type returnType);
    }
    public interface IServerObject
    {
        /// <summary>
        /// Sends a message to the server to proc the method call, and return the result
        /// </summary>
        /// <param name="method">the method that needs to be called</param>
        /// <param name="streams">The parameters for the given method, converted into MemoryStreams by the client that now need to be decoded</param>
        /// <param name="types">The types of each stream, so the server knows how to decode the streams</param>
        /// <returns></returns>
        object CallObject(MethodInfo method, List<object> streams, List<Type> types);
    }
    public interface IClientObject
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
        void SetUpServer(AssemblyLoadContext alc, string typeName, string assemblyPath, Type[] genericTypes);

    }
}
