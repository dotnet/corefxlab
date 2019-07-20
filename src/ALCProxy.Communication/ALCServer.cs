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
    public abstract class ALCServer : IProxyServer
    {
        public object instance;
        public AssemblyLoadContext currentLoadContext;
        public ALCServer(Type instanceType, Type[] genericTypes, IList<object> serializedConstParams, IList<Type> constArgTypes)
        {
            currentLoadContext = AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly());
            if (genericTypes != null && genericTypes.Length > 0)
            {
                instanceType = instanceType.MakeGenericType(genericTypes.Select(x => ConvertType(x)).ToArray());
            }
            constArgTypes = constArgTypes.Select(x => ConvertType(x)).ToList();
            var constructorParams = DeserializeParameters(serializedConstParams, constArgTypes);
            SetInstance(instanceType, constArgTypes.ToArray(), constructorParams);
        }
        /// <summary>
        /// Create the instance of the object we want to proxy
        /// </summary>
        /// <param name="instanceType">the type of the object we want</param>
        /// <param name="constructorTypes">The list of types that the constructor of the object takes in as an argument</param>
        /// <param name="constructorArgs">The physical objects that are the parameters to the constructor</param>
        protected void SetInstance(Type instanceType, Type[] constructorTypes, object[] constructorArgs)
        {
            var ci = instanceType.GetConstructor(constructorTypes);
            instance = ci.Invoke(constructorArgs);
        }
        /// <summary>
        /// Takes a Type that's been passed from the user ALC, and loads it into the current ALC for use. 
        /// </summary>
        protected Type ConvertType(Type toConvert)
        {
            AssemblyName assemblyName = Assembly.GetAssembly(toConvert).GetName();
            return currentLoadContext.LoadFromAssemblyName(assemblyName).GetType(toConvert.FullName);
        }
        public object CallObject(MethodInfo targetMethod, IList<object> streams, IList<Type> argTypes)
        {
            if (targetMethod == null || streams.Count != argTypes.Count)
                throw new ArgumentNullException();

            //Turn the memstreams into their respective objects
            argTypes = argTypes.Select(x => ConvertType(x)).ToList();
            object[] args = DeserializeParameters(streams, argTypes);
            MethodInfo[] methods = instance.GetType().GetMethods();
            MethodInfo m = FindMethod(methods, targetMethod, argTypes.ToArray());
            if (m.ContainsGenericParameters)
            {
                //While this may work without the conversion, we want it to uphold the type-load boundary, don't let the passed in method use anything from outside the target ALC
                //m = m.MakeGenericMethod(targetMethod.GetGenericArguments().Select(x => ConvertType(x)).ToArray());
                m = m.MakeGenericMethod(targetMethod.GetGenericArguments());
            }
            return SerializeReturnObject(m.Invoke(instance, args), m.ReturnType);
        }
        /// <summary>
        /// Searches for methods within the type to find the one that matches our passed in type. Since the types are technically different,
        /// using a .Equals() on the methods doesn't have the comparison work correctly, so the first if statement does that manually for us.
        /// </summary>
        protected MethodInfo FindMethod(MethodInfo[] methods, MethodInfo targetMethod, Type[] parameterTypes/*These have already been converted so no issues with compatibility*/)
        {
            string methodName = targetMethod.Name;
            foreach (MethodInfo m in methods)
            {
                if (!m.Name.Equals(methodName) || parameterTypes.Length != m.GetParameters().Length)
                {
                    continue;
                }
                bool methodParamsAlligned = true;
                for (int i = 0; i < parameterTypes.Length; i++)
                {
                    if (!RecursivelyCheckForTypes(parameterTypes[i], m.GetParameters()[i].ParameterType))
                    {
                        methodParamsAlligned = false;
                        break;
                    }
                }
                if (!methodParamsAlligned)
                    continue;
                return m;
            }
            throw new MissingMethodException("Error in ALCProxy: Method Not found for " + instance.ToString() + ": " + methodName);
        }
        /// <summary>
        /// If a parameter of a function isn't the direct type that we've passed in, this function should find that the type we've passed is correct.
        /// </summary>
        private bool RecursivelyCheckForTypes(Type sentParameterType, Type toCompare)
        {
            Type[] interfaces = sentParameterType.GetInterfaces();
            if (sentParameterType.Equals(toCompare))
            {
                return true;
            }
            else if (sentParameterType.BaseType == null && interfaces.Length == 0)
            {
                return false;
            }
            else
            {
                return RecursivelyCheckForTypes(sentParameterType.BaseType, toCompare) || interfaces.Any(x => RecursivelyCheckForTypes(x, toCompare));
            }
        }
        /// <summary>
        /// Takes the serialized objects passed into the server and turns them into the specific objects we want, in the desired types we want
        /// </summary>
        /// <param name="streams"></param>
        /// <param name="argTypes"></param>
        /// <returns></returns>
        protected object[] DeserializeParameters(IList<object> streams, IList<Type> argTypes)
        {
            var convertedObjects = new List<object>();
            for (int i = 0; i < streams.Count; i++)
            {
                object s = streams[i];
                Type t = argTypes[i];
                object obj = DeserializeParameter(s, t);
                convertedObjects.Add(obj);
            }
            return convertedObjects.ToArray();
        }
        protected abstract object DeserializeParameter(object serializedParam, Type paramType);
        /// <summary>
        /// Once we've completed our method call to the real object, we need to convert the return type back into our type from the original ALC 
        /// the proxy is in, so we turn our returned object back into a stream that the client can decode
        /// </summary>
        /// <param name="returnedObject"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        protected abstract object SerializeReturnObject(object returnedObject, Type returnType);
    }
}

