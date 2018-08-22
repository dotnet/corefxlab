// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

using SampleMetadata;

using Xunit;

namespace System.Reflection.Tests
{
    public static partial class CustomAttributeTests
    {
        [Fact]
        public static void TestDllImportPseudoCustomAttribute()
        {
            TypeInfo runtimeType = typeof(DllImportHolders).GetTypeInfo();  // Intentionally not projected - using to get expected results.
            MethodInfo[] runtimeMethods = runtimeType.DeclaredMethods.OrderBy(m => m.Name).ToArray();

            TypeInfo ecmaType = runtimeType.Project().GetTypeInfo();
            MethodInfo[] ecmaMethods = ecmaType.DeclaredMethods.OrderBy(m => m.Name).ToArray();

            Assert.Equal(runtimeMethods.Length, ecmaMethods.Length);
            for (int i = 0; i < runtimeMethods.Length; i++)
            {
                DllImportAttribute expected = runtimeMethods[i].GetCustomAttribute<DllImportAttribute>();
                CustomAttributeData cad = ecmaMethods[i].CustomAttributes.Single(c => c.AttributeType.Name == nameof(DllImportAttribute));
                DllImportAttribute actual = cad.UnprojectAndInstantiate<DllImportAttribute>();
                AssertEqual(expected, actual);
            }
        }

        private static void AssertEqual(DllImportAttribute d1, DllImportAttribute d2)
        {
            Assert.Equal(d1.BestFitMapping, d2.BestFitMapping);
            Assert.Equal(d1.CallingConvention, d2.CallingConvention);
            Assert.Equal(d1.CharSet, d2.CharSet);
            Assert.Equal(d1.EntryPoint, d2.EntryPoint);
            Assert.Equal(d1.ExactSpelling, d2.ExactSpelling);
            Assert.Equal(d1.PreserveSig, d2.PreserveSig);
            Assert.Equal(d1.SetLastError, d2.SetLastError);
            Assert.Equal(d1.ThrowOnUnmappableChar, d2.ThrowOnUnmappableChar);
            Assert.Equal(d1.Value, d2.Value);
        }

        [Fact]
        public static void TestMarshalAsPseudoCustomAttribute()
        {
            TypeInfo runtimeType = typeof(MarshalAsHolders).GetTypeInfo();  // Intentionally not projected - using to get expected results.
            FieldInfo[] runtimeFields = runtimeType.DeclaredFields.OrderBy(f => f.Name).ToArray();

            TypeInfo ecmaType = runtimeType.Project().GetTypeInfo();
            FieldInfo[] ecmaFields = ecmaType.DeclaredFields.OrderBy(f => f.Name).ToArray();

            Assert.Equal(runtimeFields.Length, ecmaFields.Length);
            for (int i = 0; i < runtimeFields.Length; i++)
            {
                MarshalAsAttribute expected = runtimeFields[i].GetCustomAttribute<MarshalAsAttribute>();
                CustomAttributeData cad = ecmaFields[i].CustomAttributes.Single(c => c.AttributeType.Name == nameof(MarshalAsAttribute));
                MarshalAsAttribute actual = cad.UnprojectAndInstantiate<MarshalAsAttribute>();
                AssertEqual(expected, actual);
            }
        }

        private static void AssertEqual(MarshalAsAttribute m1, MarshalAsAttribute m2)
        {
            Assert.Equal(m1.ArraySubType, m2.ArraySubType);
            Assert.Equal(m1.IidParameterIndex, m2.IidParameterIndex);
            Assert.Equal(m1.MarshalCookie, m2.MarshalCookie);
            Assert.Equal(m1.MarshalType, m2.MarshalType);
            Assert.Equal(m1.MarshalTypeRef, m2.MarshalTypeRef);
            Assert.Equal(m1.SafeArraySubType, m2.SafeArraySubType);
            Assert.Equal(m1.SafeArrayUserDefinedSubType, m2.SafeArrayUserDefinedSubType);
            Assert.Equal(m1.SizeConst, m2.SizeConst);
            Assert.Equal(m1.SizeParamIndex, m2.SizeParamIndex);
            Assert.Equal(m1.Value, m2.Value);
        }
    }
}
