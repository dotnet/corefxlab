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
    public abstract class ALCServer<I> : IProxyServer
    {
        public I instance;
        public Type instanceIntType;
        public AssemblyLoadContext currentLoadContext;

        /// <summary>
        /// ALCServer, contacted by the client to take methods and serialized parameters and run them on a created proxy object.
        /// </summary>
        /// <param name="instanceType">The type of the object that is to be loaded in the ALC</param>
        /// <param name="genericTypes">Any generic types that are used within the instance type if instance is generic</param>
        /// <param name="serializedConstParams">Serialized versions of objects that need to be passed to the constructor</param>
        /// <param name="constArgTypes">The given types of the serialized constructor parameters</param>
        /// <exception cref="ArgumentNullException">Throws if there is no instanceType that is given ie. there is no object type defined to be proxied</exception>
        /// <exception cref="ArgumentException">Throws if there are a different number of serialized constructor parameters to listed types for them</exception>
        public ALCServer(Type instanceType, Type[] genericTypes, IList<object> serializedConstParams, IList<Type> constArgTypes)
        {
            if (instanceType == null)
                throw new ArgumentNullException();
            if (serializedConstParams.Count != constArgTypes.Count)
                throw new ArgumentException("Different number of passed streams to argument types");

            currentLoadContext = AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly());
            instanceIntType = typeof(I);
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
            instance = (I)ci.Invoke(constructorArgs);
        }

        /// <summary>
        /// Takes a Type that's been passed from the user ALC, and loads it into the current ALC for use. 
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws if there is no type given to convert</exception>
        protected Type ConvertType(Type toConvert)
        {
            if (toConvert == null)
                throw new ArgumentNullException();
            AssemblyName assemblyName = Assembly.GetAssembly(toConvert).GetName();
            if (assemblyName.Name.Equals("System.Private.CoreLib"))
                return toConvert;
            Assembly foundAssembly = currentLoadContext.Assemblies.ToList().Find(x => x.FullName.Equals(assemblyName));
            if(foundAssembly == null)
                return currentLoadContext.LoadFromAssemblyName(assemblyName).GetType(toConvert.FullName);
            return foundAssembly.GetType(toConvert.FullName);
        }
        
        /// <summary>
        /// Calls a method of the stored proxy object
        /// </summary>
        /// <param name="targetMethod">The method from the Proxy to be called</param>
        /// <param name="serializedObjects">Parameter arguments for the target method, serialized in some way</param>
        /// <param name="argTypes">In the same order as the serialized objects, the respective type for each serialized object</param>
        /// <exception cref="ArgumentNullException">Throws if there is no given method to call</exception>
        /// <exception cref="ArgumentException">Throws if there are a different number of serialized parameters to given types for said parameters</exception>
        /// <returns>A serialized version of the returned object from the method</returns>
        public object CallObject(MethodInfo targetMethod, IList<object> serializedObjects, IList<Type> argTypes)
        {
            if (targetMethod == null)
                throw new ArgumentNullException();
            if (serializedObjects.Count != argTypes.Count)
                throw new ArgumentException("Different number of serialized arguments to types");

            //Turn the serialized objects into their respective objects
            argTypes = argTypes.Select(x => ConvertType(x)).ToList();
            object[] args = DeserializeParameters(serializedObjects, argTypes);
            MethodInfo[] methods = instanceIntType.GetRuntimeMethods().ToArray();
            MethodInfo m = FindMethod(methods, targetMethod, argTypes.ToArray());
            if (m.ContainsGenericParameters)
            {
                // While this may work without the conversion, we want it to uphold the type-load boundary, don't let the passed in method use anything from outside the target ALC
                m = m.MakeGenericMethod(targetMethod.GetGenericArguments());
            }
            return SerializeReturnObject(m.Invoke(instance, args), m.ReturnType);
        }

        /// <summary>
        /// Searches for methods within the type to find the one that matches our passed in type. Since the types are technically different,
        /// using a .Equals() on the methods doesn't have the comparison work correctly, so the first if statement does that manually for us.
        /// </summary>
        /// <exception cref="MissingMethodException">Throws if we can't find the given method from the parameters</exception>
        /// <returns>The MethodInfo object representing the method we want to call</returns>
        private MethodInfo FindMethod(MethodInfo[] methods, MethodInfo targetMethod, Type[] parameterTypes/*These have already been converted so no issues with compatibility*/)
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
                {
                    continue;
                }
                return m;
            }
            throw new MissingMethodException("Error in ALCProxy: Method Not found for " + instance.ToString() + ": " + methodName);
        }

        /// <summary>
        /// If a parameter of a function isn't the direct type that we've passed in, this function should find that the type we've passed is correct.
        /// </summary>
        private bool RecursivelyCheckForTypes(Type sentParameterType, Type toCompare)
        {
            if ((sentParameterType ?? toCompare) == null)
                return false;

            Type[] interfaces = sentParameterType.GetInterfaces();

            if (interfaces == null)
                return false;

            if (sentParameterType.Equals(toCompare))
                return true;
            else if (sentParameterType.BaseType == null && interfaces.Length == 0)
                return false;
            else
            {
                bool baseType = false;
                if (sentParameterType.BaseType != null)
                {
                    baseType = RecursivelyCheckForTypes(sentParameterType.BaseType, toCompare);
                }
                return baseType || interfaces.Any(x => RecursivelyCheckForTypes(ConvertType(x), toCompare));
            }
        }

        /// <summary>
        /// Takes the serialized objects passed into the server and turns them into the specific objects we want, in the desired types we want
        /// </summary>
        /// <param name="streams">A list of serialized objects</param>
        /// <param name="argTypes">The types for each serialized object</param>
        /// <exception cref="ArgumentException">Throws if there are a different number of serialized objects to the given list of types that are to deserialize them</exception>
        /// <returns>the list of deserialized objects to be sent to the targetObject</returns>
        protected object[] DeserializeParameters(IList<object> streams, IList<Type> argTypes)
        {
            if (streams.Count != argTypes.Count)
                throw new ArgumentException("Different number of passed streams to argument types");
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
        
        /// <summary>
        /// Deserializes an object into the required type for the ALC. Used when methods with arguments are sent over from the client to the server.
        /// </summary>
        protected abstract object DeserializeParameter(object serializedParam, Type paramType);

        /// <summary>
        /// Once we've completed our method call to the real object, we need to convert the return type back into our type from the original ALC 
        /// the proxy is in, so we turn our returned object back into a stream that the client can decode
        /// </summary>
        protected abstract object SerializeReturnObject(object returnedObject, Type returnType);
    }
}
