// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Text;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Text.JsonLab
{
    public partial class JsonDynamicObject : DynamicObject, IBufferFormattable
    {
        private static class Deserializer<T>
        {
            private static readonly Func<T> _createInstance = CreateDelegate();

            private static Func<T> CreateDelegate()
            {
                DynamicMethod method = new DynamicMethod("CreateInstanceDynamicMethod", typeof(T), null, restrictedSkipVisibility: true);

                // GetConstructors() is only available on netstandard 2.0
                IEnumerable<ConstructorInfo> ctors = typeof(T).GetTypeInfo().DeclaredConstructors;
                ConstructorInfo constructor = default;
                foreach (ConstructorInfo ci in ctors)
                {
                    constructor = ci;
                    break;
                }

                ILGenerator generator = method.GetILGenerator();
                generator.Emit(OpCodes.Newobj, constructor);
                generator.Emit(OpCodes.Ret);
                return (Func<T>)method.CreateDelegate(typeof(Func<T>));
            }

            public static T Deserialize(ReadOnlySpan<byte> data) => _createInstance();
        }
    }
}
