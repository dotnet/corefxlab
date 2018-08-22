// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

#pragma warning disable 0618   // Obsolete members

namespace System.Reflection.TypeLoading.Ecma
{
    internal static class EcmaCustomAttributeHelpers
    {
        /// <summary>
        /// Converts ECMA-encoded custom attributes into a freshly allocated CustomAttributeData object suitable for direct return 
        /// from the CustomAttributes api.
        /// </summary>
        public static IEnumerable<CustomAttributeData> ToTrueCustomAttributes(this CustomAttributeHandleCollection handles, EcmaModule module)
        {
            foreach (CustomAttributeHandle handle in handles)
            {
                yield return handle.ToCustomAttributeData(module);
            }
        }

        public static CustomAttributeData ToCustomAttributeData(this CustomAttributeHandle handle, EcmaModule module) => new EcmaCustomAttributeData(handle, module);

        public static bool IsCustomAttributeDefined(this CustomAttributeHandleCollection handles, ReadOnlySpan<byte> ns, ReadOnlySpan<byte> name, EcmaModule module)
        {
            return !handles.FindCustomAttributeByName(ns, name, module).IsNil;
        }

        public static CustomAttributeData TryFindCustomAttribute(this CustomAttributeHandleCollection handles, ReadOnlySpan<byte> ns, ReadOnlySpan<byte> name, EcmaModule module)
        {
            CustomAttributeHandle handle = handles.FindCustomAttributeByName(ns, name, module);
            if (handle.IsNil)
                return null;
            return new EcmaCustomAttributeData(handle, module);
        }

        private static CustomAttributeHandle FindCustomAttributeByName(this CustomAttributeHandleCollection handles, ReadOnlySpan<byte> ns, ReadOnlySpan<byte> name, EcmaModule module)
        {
            MetadataReader reader = module.Reader;
            foreach (CustomAttributeHandle handle in handles)
            {
                CustomAttribute ca = handle.GetCustomAttribute(module.Reader);
                EntityHandle ctorHandle = ca.Constructor;
                switch (ctorHandle.Kind)
                {
                    case HandleKind.MethodDefinition:
                        {
                            MethodDefinitionHandle mh = (MethodDefinitionHandle)ctorHandle;
                            EntityHandle declaringType = mh.GetMethodDefinition(reader).GetDeclaringType();
                            if (declaringType.TypeMatchesNameAndNamespace(ns, name, reader))
                                return handle;
                            break;
                        }

                    case HandleKind.MemberReference:
                        {
                            MemberReference mr = ((MemberReferenceHandle)ctorHandle).GetMemberReference(reader);
                            EntityHandle declaringType = mr.Parent;
                            if (declaringType.TypeMatchesNameAndNamespace(ns, name, reader))
                                return handle;
                            break;
                        }

                    default:
                        break;
                }
            }

            return default;
        }

        public static bool TypeMatchesNameAndNamespace(this EntityHandle handle, ReadOnlySpan<byte> ns, ReadOnlySpan<byte> name, MetadataReader reader)
        {
            switch (handle.Kind)
            {
                case HandleKind.TypeDefinition: // Not clear if this can happen but as fail-safe....
                    {
                        TypeDefinition td = ((TypeDefinitionHandle)handle).GetTypeDefinition(reader);
                        return td.Name.Equals(name, reader) && td.Namespace.Equals(ns, reader);
                    }

                case HandleKind.TypeReference:
                    {
                        TypeReference tr = ((TypeReferenceHandle)handle).GetTypeReference(reader);
                        return tr.ResolutionScope.Kind != HandleKind.TypeReference && tr.Name.Equals(name, reader) && tr.Namespace.Equals(ns, reader);
                    }

                default:
                    return false;
            }
        }

        /// <summary>
        /// Converts a System.Reflection.Metadata CustomAttributeTypedArgument&lt;&gt; into a freshly allocated CustomAttributeTypedArgument 
        /// object suitable for direct return from the CustomAttributes api.
        /// </summary>
        public static CustomAttributeTypedArgument ToApiForm(this CustomAttributeTypedArgument<RoType> catg) => ToApiForm(catg.Type, catg.Value);

        private static CustomAttributeTypedArgument ToApiForm(Type type, object value)
        {
            if (!(value is IList<CustomAttributeTypedArgument<RoType>> catgs))
            {
                return new CustomAttributeTypedArgument(type, value);
            }

            int count = catgs.Count;
            CustomAttributeTypedArgument[] cats = new CustomAttributeTypedArgument[count];
            for (int i = 0; i < count; i++)
            {
                cats[i] = catgs[i].ToApiForm();
            }
            return new CustomAttributeTypedArgument(type, cats.ToReadOnlyCollection());
        }

        /// <summary>
        /// Converts a System.Reflection.Metadata CustomAttributeNamedArgument&lt;&gt; into a freshly allocated CustomAttributeNamedArgument 
        /// object suitable for direct return from the CustomAttributes api.
        /// </summary>
        public static CustomAttributeNamedArgument ToApiForm(this CustomAttributeNamedArgument<RoType> cang, RoType attributeType)
        {
            MemberInfo member;
            switch (cang.Kind)
            {
                case CustomAttributeNamedArgumentKind.Field:
                    member = attributeType.GetField(cang.Name, BindingFlags.Public | BindingFlags.Instance);
                    break;

                case CustomAttributeNamedArgumentKind.Property:
                    member = attributeType.GetProperty(cang.Name, BindingFlags.Public | BindingFlags.Instance);
                    break;

                default:
                    Debug.Fail("Invalid CustomAttributeNamedArgumentKind value: " + cang.Kind);
                    throw new BadImageFormatException();
            }

            return new CustomAttributeNamedArgument(member, ToApiForm(cang.Type, cang.Value));
        }

        //
        // Logic ported from ParseNativeTypeInfo()
        //
        // https://github.com/dotnet/coreclr/blob/ab9b4511180d1dfde09d1480c29a7bbacf3587dd/src/vm/mlinfo.cpp#L512
        //
        public static MarshalAsAttribute ToMarshalAsAttribute(this BlobHandle blobHandle, EcmaModule module)
        {
            MetadataReader reader = module.Reader;
            BlobReader br = blobHandle.GetBlobReader(reader);
            UnmanagedType unmgdType = (UnmanagedType)br.ReadByte();
            MarshalAsAttribute ma = new MarshalAsAttribute(unmgdType);
            switch (unmgdType)
            {
                case UnmanagedType.Interface:
                case UnmanagedType.IUnknown:
                case UnmanagedType.IDispatch:
                    if (br.RemainingBytes == 0)
                        break;
                    ma.IidParameterIndex = br.ReadCompressedInteger();
                    break;

                case UnmanagedType.ByValArray:
                    if (br.RemainingBytes == 0)
                        break;
                    ma.SizeConst = br.ReadCompressedInteger();

                    if (br.RemainingBytes == 0)
                        break;
                    ma.ArraySubType = (UnmanagedType)br.ReadCompressedInteger();

                    break;

                case UnmanagedType.SafeArray:
                    if (br.RemainingBytes == 0)
                        break;
                    ma.SafeArraySubType = (VarEnum)br.ReadCompressedInteger();

                    if (br.RemainingBytes == 0)
                        break;
                    string udtName = br.ReadSerializedString();
                    ma.SafeArrayUserDefinedSubType = Helpers.LoadTypeFromAssemblyQualifiedName(udtName, module.GetRoAssembly(), ignoreCase: false, throwOnError: false);
                    break;

                case UnmanagedType.LPArray:
                    if (br.RemainingBytes == 0)
                        break;
                    ma.ArraySubType = (UnmanagedType)br.ReadCompressedInteger();

                    if (br.RemainingBytes == 0)
                        break;
                    ma.SizeParamIndex = (short)br.ReadCompressedInteger();

                    if (br.RemainingBytes == 0)
                        break;
                    ma.SizeConst = br.ReadCompressedInteger();
                    break;

                case UnmanagedType.CustomMarshaler:
                    if (br.RemainingBytes == 0)
                        break;
                    br.ReadSerializedString(); // Skip the typelib guid.

                    if (br.RemainingBytes == 0)
                        break;
                    br.ReadSerializedString(); // Skip name of native type.

                    if (br.RemainingBytes == 0)
                        break;
                    ma.MarshalType  = br.ReadSerializedString();
                    ma.MarshalTypeRef = Helpers.LoadTypeFromAssemblyQualifiedName(ma.MarshalType, module.GetRoAssembly(), ignoreCase: false, throwOnError: false);

                    if (br.RemainingBytes == 0)
                        break;
                    ma.MarshalCookie = br.ReadSerializedString();
                    break;

                default:
                    break;
            }

            return ma;
        }
    }
}
