// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using System.Reflection.Emit;

namespace System.Text.Json.Serialization
{
    internal class JsonReflectionEmitMaterializer : JsonMemberBasedClassMaterializer
    {
        public override JsonClassInfo.ConstructorDelegate CreateConstructor(Type type)
        {
#if !BUILDING_INBOX_LIBRARY
            throw new NotImplementedException("TODO: JsonReflectionEmitMaterializer is not yet supported on .NET Standard 2.0.");
#else
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
                throw new InvalidOperationException();

            ILGenerator generator = dynamicMethod?.GetILGenerator();
            if (generator == null)
                throw new InvalidOperationException();

            generator.Emit(OpCodes.Newobj, realMethod);
            generator.Emit(OpCodes.Ret);

            var result = (JsonClassInfo.ConstructorDelegate)dynamicMethod.CreateDelegate(typeof(JsonClassInfo.ConstructorDelegate));
            return result;
#endif
        }

        public override JsonPropertyInfo<TValue>.GetterDelegate CreateGetter<TValue>(PropertyInfo propertyInfo)
        {
#if !BUILDING_INBOX_LIBRARY
            throw new NotImplementedException("TODO: JsonReflectionEmitMaterializer is not yet supported on .NET Standard 2.0.");
#else
            MethodInfo realMethod = propertyInfo.GetGetMethod();
            if (realMethod == null)
                return null; // Exception will be raised later if called

            var dynamicMethod = new DynamicMethod(
                realMethod.Name,
                typeof(TValue),
                new Type[] { typeof(object) },
                typeof(JsonReflectionEmitMaterializer).Module,
                skipVisibility: true);

            if (dynamicMethod == null)
                throw new InvalidOperationException();

            ILGenerator generator = dynamicMethod?.GetILGenerator();
            if (generator == null)
                throw new InvalidOperationException();

            generator.Emit(OpCodes.Ldarg_0);
            generator.EmitCall(OpCodes.Callvirt, realMethod, null);
            generator.Emit(OpCodes.Ret);

            var result = (JsonPropertyInfo<TValue>.GetterDelegate)dynamicMethod.CreateDelegate(typeof(JsonPropertyInfo<TValue>.GetterDelegate));
            return result;
#endif
        }

        public override JsonPropertyInfo<TValue>.SetterDelegate CreateSetter<TValue>(PropertyInfo propertyInfo)
        {
#if !BUILDING_INBOX_LIBRARY
            throw new NotImplementedException("TODO: JsonReflectionEmitMaterializer is not yet supported on .NET Standard 2.0.");
#else
            MethodInfo realMethod = propertyInfo.GetSetMethod();
            if (realMethod == null)
                return null; // Exception will be raised later if called

            var dynamicMethod = new DynamicMethod(
                realMethod.Name,
                typeof(void),
                new Type[] { typeof(object), typeof(TValue) },
                typeof(JsonReflectionEmitMaterializer).Module,
                skipVisibility: true);

            if (dynamicMethod == null)
                throw new InvalidOperationException();

            ILGenerator generator = dynamicMethod?.GetILGenerator();
            if (generator == null)
                throw new InvalidOperationException();

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);

            if (propertyInfo.PropertyType.IsEnum)
            {
                generator.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
            }

            generator.EmitCall(OpCodes.Callvirt, realMethod, null);
            generator.Emit(OpCodes.Ret);

            var result = (JsonPropertyInfo<TValue>.SetterDelegate)dynamicMethod.CreateDelegate(typeof(JsonPropertyInfo<TValue>.SetterDelegate));
            return result;
#endif
        }
    }
}
