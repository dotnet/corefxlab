// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace ALCProxy.Communication
{
    internal delegate object ServerCall(MethodInfo info, IList<object> args, IList<Type> types);
    /// <summary>
    /// This currently is designed to only work in-process
    /// TODO: set up to allow for construction of out-of-proc proxies
    /// </summary>
    public abstract class ALCClient : IProxyClient
    {
        //Can't make this an IServerObject directly due to the type-loading barrier
        protected object _server;
        protected string _serverTypeName;
        protected Type _intType;
        internal ServerCall _serverDelegate;

        private StackTrace _stackTrace;
        public ALCClient(Type interfaceType, string serverName)
        {
            _intType = interfaceType;
            _serverTypeName = serverName;
#if DEBUG
            _stackTrace = new StackTrace(true); //holds information for debugging purposes
#endif
        }
        private Type FindTypeInAssembly(string typeName, Assembly a)
        {
            //find the type we're looking for
            Type t = a.GetType(typeName);
            if (t == null)
            {
                t = a.GetType(a.GetName().Name + "." + typeName);
                if (t == null)
                {
                    throw new TypeLoadException("Proxy creation exception: No valid type while searching for the given type: " + typeName + " || " + a.GetName().Name + "." + typeName);
                }
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
        public void SetUpServer(AssemblyLoadContext alc, string typeName, string assemblyPath, object[] constructorParams, Type[] genericTypes)
        {
            if (alc == null || (typeName ?? assemblyPath) == null)
                throw new ArgumentNullException();
            if (genericTypes == null)
                genericTypes = new Type[] { };
            if (constructorParams == null)
                constructorParams = new object[] { };

            Assembly a = alc.LoadFromAssemblyPath(assemblyPath);
            //find the type we're going to proxy inside the loaded assembly
            Type objType = FindTypeInAssembly(typeName, a);
            //Load *this* (ALCProxy.Communication) assembly into the ALC so we can get the server into the ALC
            Assembly aa = alc.LoadFromAssemblyPath(Assembly.GetAssembly(typeof(ServerDispatch)).CodeBase.Substring(8));
            //Get the server type, then make it generic with the interface we're using
            Type constructedType = FindTypeInAssembly(_serverTypeName, aa);//.MakeGenericType(_intType);
            //Give the client its reference to the server
            SerializeParameters(constructorParams, out IList<object> serializedConstArgs, out IList<Type> argTypes);
            _server = constructedType.GetConstructor(
                new Type[] { typeof(Type), typeof(Type[]), typeof(IList<object>), typeof(IList<Type>) })
                .Invoke(new object[] { objType, genericTypes, serializedConstArgs.ToList(), argTypes });
            _serverDelegate = (ServerCall)Delegate.CreateDelegate(typeof(ServerCall), _server, constructedType.GetMethod("CallObject"));
            //Attach to the unloading event
            alc.Unloading += UnloadClient;
        }
        private void UnloadClient(object sender)
        {
            _server = null; //unload only removes the reference to the proxy, doesn't do anything else, since the ALCs need to be cleaned up by the users before the GC can collect.
            _serverDelegate = null; //TODO: to Delegates to a specific object hold a strong or weak reference to said object?
        }
        /// <summary>
        /// Converts each argument into a serialized version of the object so it can be sent over in a call-by-value fashion
        /// </summary>
        /// <param name="method">the methodInfo of the target method</param>
        /// <param name="args">the current objects assigned as arguments to send</param>
        /// <returns></returns>
        public object SendMethod(MethodInfo method, object[] args)
        {
            if (_serverDelegate == null) //We've called the ALC unload, so the proxy has been cut off
            {
                throw new InvalidOperationException("Error in ALCClient: Proxy has been unloaded, or communication server was never set up correctly");
            }
            SerializeParameters(args, out IList<object> streams, out IList<Type> argTypes);
            object encryptedReturn = _serverDelegate( method, streams, argTypes );
            return DeserializeReturnType(encryptedReturn, method.ReturnType);
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
        protected abstract object DeserializeReturnType(object returnedObject, Type returnType);
    }
}
