using System;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.Metadata.ILDasm.Decoder;
using System.Text;

namespace ILDasmLibrary
{
    public struct ILMethodImport
    {
        private Readers _readers;
        private string _name;
        private MethodImport _methodImport;
        private ILModuleReference _moduleReference;
        private MethodDefinition _methodDef;
        private bool _isModuleReferenceInitialized;
        private MethodImportAttributes _attributes;
        private bool _isAttributesInitialized;

        internal static ILMethodImport Create(MethodImport methodImport, ref Readers readers, MethodDefinition methodDef)
        {
            ILMethodImport import = new ILMethodImport();
            import._methodImport = methodImport;
            import._readers = readers;
            import._isModuleReferenceInitialized = false;
            import._isAttributesInitialized = false;
            import._methodDef = methodDef;
            return import;
        }

        public string Name
        {
            get
            {
                return ILDecoder.GetCachedValue(_methodImport.Name, _readers, ref _name);
            }
        }

        public bool IsNil
        {
            get
            {
                return _methodImport.Module.IsNil;
            }
        }

        public ILModuleReference ModuleReference
        {
            get
            {
                if (!_isModuleReferenceInitialized)
                {
                    if (IsNil)
                    {
                        throw new InvalidOperationException("Method Import is nil");
                    }
                    _isModuleReferenceInitialized = true;
                    ModuleReferenceHandle handle = _methodImport.Module;
                    _moduleReference = ILModuleReference.Create(_readers.MdReader.GetModuleReference(handle), ref _readers, MetadataTokens.GetToken(handle));
                }
                return _moduleReference;
            }
        }

        public MethodImportAttributes Attributes
        {
            get
            {
                if (!_isAttributesInitialized)
                {
                    _isAttributesInitialized = true;
                    _attributes = _methodImport.Attributes;
                }
                return _attributes;
            }
        }

        public string GetMethodImportDeclaration()
        {
            if (IsNil)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("\"{0}\"", ModuleReference.Name);
            if (Name != _readers.MdReader.GetString(_methodDef.Name))
            {
                sb.AppendFormat(" as \"{0}\"", Name);
            }
            sb.AppendFormat(" {0}", GetFlags());
            return sb.ToString();
        }

        public string GetFlags()
        {
            string noMangle = Attributes.HasFlag(MethodImportAttributes.ExactSpelling) ? "nomangle " : string.Empty;
            string lastError = Attributes.HasFlag(MethodImportAttributes.SetLastError) ? "lasterr " : string.Empty;
            return string.Format("{0}{1}{2}{3}{4}{5}", noMangle, GetFormatFlags(), lastError, GetCallingConventionFlags(), GetBestFitFlags(), GetCharMapErrorFlag());
        }

        private object GetCharMapErrorFlag()
        {
            if (Attributes.HasFlag(MethodImportAttributes.ThrowOnUnmappableCharDisable))
            {
                return "charmaperror:off ";
            }
            if (Attributes.HasFlag(MethodImportAttributes.ThrowOnUnmappableCharEnable))
            {
                return "charmaperror:on ";
            }
            return string.Empty;
        }

        private string GetFormatFlags()
        {
            if (Attributes.HasFlag(MethodImportAttributes.CharSetAuto))
            {
                return "autochar ";
            }
            if (Attributes.HasFlag(MethodImportAttributes.CharSetUnicode))
            {
                return "unicode ";
            }
            if (Attributes.HasFlag(MethodImportAttributes.CharSetAnsi))
            {
                return "ansi ";
            }
            return string.Empty;
        }

        private string GetCallingConventionFlags()
        {
            if (Attributes.HasFlag(MethodImportAttributes.CallingConventionFastCall))
            {
                return "fastcall ";
            }
            if (Attributes.HasFlag(MethodImportAttributes.CallingConventionThisCall))
            {
                return "thiscall ";
            }
            if (Attributes.HasFlag(MethodImportAttributes.CallingConventionStdCall))
            {
                return "stdcall ";
            }
            if (Attributes.HasFlag(MethodImportAttributes.CallingConventionCDecl))
            {
                return "cdecl ";
            }
            if (Attributes.HasFlag(MethodImportAttributes.CallingConventionWinApi))
            {
                return "winapi ";
            }
            return string.Empty;
        }

        private string GetBestFitFlags()
        {
            if (Attributes.HasFlag(MethodImportAttributes.BestFitMappingDisable))
            {
                return "bestfit:off ";
            }
            if (Attributes.HasFlag(MethodImportAttributes.BestFitMappingEnable))
            {
                return "bestfit:on ";
            }
            return string.Empty;
        }
    }
}
