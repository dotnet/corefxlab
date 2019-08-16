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
    /// </summary>
    public abstract class ALCClient : IProxyClient
    {
        protected object _server; // Can't make this an IServerObject directly due to the type-loading barrier
        protected string _serverTypeName;
        protected Type _intType;
        internal ServerCall _serverDelegate;
        protected Type _serverType;
#if DEBUG
        private StackTrace _stackTrace; // Holds information for debugging purposes
#endif
        /// <summary>
        /// ALCClient gets the information that is needed to set up the ALCServer in the target ALC
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws if missing an interface type/server type to work with or a server name</exception>
        public ALCClient(Type interfaceType, string serverName, Type serverType)
        {
            if ((interfaceType ?? serverType) == null || serverName == null)
                throw new ArgumentNullException();
            _serverType = serverType;
            _intType = interfaceType;
            _serverTypeName = serverName;
#if DEBUG
            _stackTrace = new StackTrace(true);
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
        /// <param name="assemblyName">path of the assembly to the type</param>
        /// <param name="genericTypes">any generics that we need the proxy to work with</param>
        /// <exception cref="ArgumentNullException">Throws if we're missing either the given ALC, the AssemblyName to load, or the type to load from the assembly</exception>
        public void SetUpServer(AssemblyLoadContext alc, string typeName, AssemblyName assemblyName, object[] constructorParams, Type[] genericTypes)
        {
            if (alc == null || typeName == null || assemblyName == null)
                throw new ArgumentNullException();
            if (genericTypes == null)
                genericTypes = new Type[] { };
            if (constructorParams == null)
                constructorParams = new object[] { };
            Assembly a = alc.LoadFromAssemblyName(assemblyName);
            
            // Find the type we're going to proxy inside the loaded assembly
            Type objType = FindTypeInAssembly(typeName, a);
            // Get the interface of the object so we can set it as the server's generic type
            Type interfaceType = FindInterfaceType(_intType.Name, objType);
            if (interfaceType.IsGenericType)
            {
                interfaceType = interfaceType.MakeGenericType(genericTypes.Select(x => ConvertType(x, alc)).ToArray());
            }
            // Load *this* (ALCProxy.Communication) assembly into the ALC so we can get the server into the ALC
            Assembly serverAssembly = alc.LoadFromAssemblyName(Assembly.GetAssembly(_serverType).GetName());
            // Get the server type, then make it generic with the interface we're using
            Type serverType = FindTypeInAssembly(_serverTypeName, serverAssembly).MakeGenericType(interfaceType);
            // Give the client its reference to the server
            SerializeParameters(constructorParams, out IList<object> serializedConstArgs, out IList<Type> argTypes);
            ConstructorInfo ci = serverType.GetConstructor(
                new Type[] { typeof(Type), typeof(Type[]), typeof(IList<object>), typeof(IList<Type>) });
            _server = ci.Invoke(new object[] { objType, genericTypes, serializedConstArgs.ToList(), argTypes });
            _serverDelegate = (ServerCall)Delegate.CreateDelegate(typeof(ServerCall), _server, serverType.GetMethod("CallObject"));
            // Attach to the unloading event
            alc.Unloading += UnloadClient;
        }

        /// <summary>
        /// Takes a Type that's been passed from the user ALC, and loads it into the current ALC for use. 
        /// </summary>
        private Type ConvertType(Type toConvert, AssemblyLoadContext currentLoadContext)
        {
            AssemblyName assemblyName = Assembly.GetAssembly(toConvert).GetName();
            return currentLoadContext.LoadFromAssemblyName(assemblyName).GetType(toConvert.FullName);
        }

        private Type FindInterfaceType(string interfaceName, Type objType)
        {
            Type[] interfaces;
            if (objType.IsGenericType)
                interfaces = objType.GetGenericTypeDefinition().GetInterfaces();
            else
                interfaces = objType.GetInterfaces();

            // Type[] interfaces = objType.GetInterfaces();
            foreach(Type t in interfaces)
            {
                if (t.Name.Equals(interfaceName))
                    return GetModType(t.Name, t.Module);
            }
            throw new Exception("Interface not found, error");
        }

        private Type GetModType(string name, Module m)
        {
            foreach (Type t in m.GetTypes())
            {
                if (t.Name.Equals(name))
                    return t;
            }
            throw new Exception("Type not found in module");
        }

        private void UnloadClient(object sender)
        {
            _server = null; // Unload only removes the reference to the proxy, doesn't do anything else, since the ALCs need to be cleaned up by the users before the GC can collect.
            _serverDelegate = null;
        }

        /// <summary>
        /// Converts each argument into a serialized version of the object so it can be sent over in a call-by-value fashion
        /// </summary>
        /// <param name="method">the methodInfo of the target method</param>
        /// <param name="args">the current objects assigned as arguments to send</param>
        /// <exception cref="ArgumentNullException">Throws if we're missing a method to call</exception>
        /// <exception cref="InvalidOperationException">Throws if the client doesn't have a link to a server, usually due to the ALC being unloaded</exception>
        /// <returns>the Deserialized return object to give back to the proxy</returns>
        public object SendMethod(MethodInfo method, object[] args)
        {
            if (method == null)
                throw new ArgumentNullException();
            if (_serverDelegate == null) //We've called the ALC unload, so the proxy has been cut off
                throw new InvalidOperationException("Error in ALCClient: Proxy has been unloaded, or communication server was never set up correctly");
            if (args == null)
                args = new object[] { };

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

        /// <summary>
        /// Serializes an object that's being used as a parameter for a method call. Will be sent to the ALCServer to be deserialized and used in the method call.
        /// </summary>
        protected abstract object SerializeParameter(object param, Type paramType);

        /// <summary>
        /// Deserializes the object that is returned from the ALCServer once a method call is finished.
        /// </summary>
        protected abstract object DeserializeReturnType(object returnedObject, Type returnType);
    }
}
