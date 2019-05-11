// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if BUILDING_INBOX_LIBRARY

using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Text.JsonLab.Serialization
{
    internal class JsonReflectionEmitMaterializer : JsonMemberBasedClassMaterializer
    {
        public override JsonClassInfo.ConstructorDelegate CreateConstructor(Type type)
        {
            ConstructorInfo realMethod = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic| BindingFlags.Instance, binder: null, Type.EmptyTypes, modifiers: null); //todo: verify json.net semantics
            if (realMethod == null)
                return null; // Exception will be raised later if called

            var dynamicMethod = new DynamicMethod(
                realMethod.Name,
                type,
                Type.EmptyTypes,
                typeof(JsonReflectionEmitMaterializer).Module,
                skipVisibility: true);

            if (dynamicMethod == null)
                throw new InvalidOperationException("todo - unable to create DynamicMethod");

            ILGenerator generator = dynamicMethod?.GetILGenerator();
            if (generator == null)
                throw new InvalidOperationException("todo - unable to create DynamicMethod");

            generator.Emit(OpCodes.Newobj, realMethod);
            generator.Emit(OpCodes.Ret);

            var result = (JsonClassInfo.ConstructorDelegate)dynamicMethod.CreateDelegate(typeof(JsonClassInfo.ConstructorDelegate));
            return result;
        }

        public override JsonPropertyInfo<TValue>.GetterDelegate CreateGetter<TValue>(PropertyInfo propertyInfo)
        {
            MethodInfo realMethod = propertyInfo.GetGetMethod();

            Debug.Assert(realMethod != null); // We previously verified

            var dynamicMethod = new DynamicMethod(
                realMethod.Name,
                typeof(TValue),
                new Type[] { typeof(object) },
                typeof(JsonReflectionEmitMaterializer).Module,
                skipVisibility: true);

            if (dynamicMethod == null)
                throw new InvalidOperationException("todo - unable to create DynamicMethod");

            ILGenerator generator = dynamicMethod?.GetILGenerator();
            if (generator == null)
                throw new InvalidOperationException("todo - unable to create DynamicMethod");

            generator.Emit(OpCodes.Ldarg_0);
            generator.EmitCall(OpCodes.Callvirt, realMethod, null);
            generator.Emit(OpCodes.Ret);

            var result = (JsonPropertyInfo<TValue>.GetterDelegate)dynamicMethod.CreateDelegate(typeof(JsonPropertyInfo<TValue>.GetterDelegate));
            return result;
        }

        public override JsonPropertyInfo<TValue>.SetterDelegate CreateSetter<TValue>(PropertyInfo propertyInfo)
        {
            MethodInfo realMethod = propertyInfo.GetSetMethod();

            Debug.Assert(realMethod != null); // We previously verified

            var dynamicMethod = new DynamicMethod(
                realMethod.Name,
                typeof(void),
                new Type[] { typeof(object), typeof(TValue) },
                typeof(JsonReflectionEmitMaterializer).Module,
                skipVisibility: true);

            if (dynamicMethod == null)
                throw new InvalidOperationException("todo - unable to create DynamicMethod");

            ILGenerator generator = dynamicMethod?.GetILGenerator();
            if (generator == null)
                throw new InvalidOperationException("todo - unable to create DynamicMethod");

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.EmitCall(OpCodes.Callvirt, realMethod, null);
            generator.Emit(OpCodes.Ret);

            var result = (JsonPropertyInfo<TValue>.SetterDelegate)dynamicMethod.CreateDelegate(typeof(JsonPropertyInfo<TValue>.SetterDelegate));
            return result;
        }
    }
}
#endif