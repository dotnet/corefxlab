using System.Reflection.Metadata.ILDasm.Decoder;
using ILDasmLibrary.Visitor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace ILDasmLibrary
{
    /// <summary>
    /// Class representing an assembly.
    /// </summary>
    public struct ILAssembly : IVisitable, IDisposable 
    {
        private Readers _readers;
        private AssemblyDefinition _assemblyDefinition;
        private byte[] _publicKey;
        private IEnumerable<ILTypeDefinition> _typeDefinitions;
        private string _name;
        private string _culture;
        private int _hashAlgorithm;
        private Version _version;
        private IEnumerable<ILAssemblyReference> _assemblyReferences;
        private IEnumerable<ILTypeReference> _typeReferences;
        private IEnumerable<ILCustomAttribute> _customAttribues;
        private IEnumerable<ILModuleReference> _moduleReferences;
        private ILModuleDefinition _moduleDefinition;
        private ILHeaderOptions _headerOptions;
        private bool _isHeaderInitialized;
        private bool _isModuleInitialized;

        #region Public APIs

        public static ILAssembly Create(Stream stream)
        {
            ILAssembly assembly = new ILAssembly();
            Readers readers = Readers.Create(stream);
            assembly._readers = readers;
            assembly._hashAlgorithm = -1;
            assembly._assemblyDefinition = readers.MdReader.GetAssemblyDefinition();
            assembly._isModuleInitialized = false;
            assembly._isHeaderInitialized = false;
            return assembly;
        }

        public static ILAssembly Create(string path)
        {
            if (!File.Exists(path))
            {
                throw new ArgumentException("File doesn't exist in path");
            }

            return Create(File.OpenRead(path));
        }

        /// <summary>
        /// Property that represent the Assembly name.
        /// </summary>
        public string Name
        {
            get
            {
                return ILDecoder.GetCachedValue(_assemblyDefinition.Name, _readers, ref _name);
            }
        }

        /// <summary>
        /// Property containing the assembly culture. known as locale, such as en-US or fr-CA.
        /// </summary>
        public string Culture
        {
            get
            {
                return ILDecoder.GetCachedValue(_assemblyDefinition.Culture, _readers, ref _culture);
            }
        }

        /// <summary>
        /// Property that represents the hash algorithm used on this assembly to hash the files.
        /// </summary>
        public int HashAlgorithm
        {
            get
            {
                if(_hashAlgorithm == -1)
                {
                    _hashAlgorithm = Convert.ToInt32(_assemblyDefinition.HashAlgorithm);
                }
                return _hashAlgorithm;
            }
        }

        /// <summary>
        /// Version of the assembly.
        /// Containing:
        ///    MajorVersion
        ///    MinorVersion
        ///    BuildNumber
        ///    RevisionNumber
        /// </summary>
        public Version Version
        {
            get
            {
                if(_version == null)
                {
                    _version = _assemblyDefinition.Version;
                }
                return _version;
            }
        }

        public ILModuleDefinition ModuleDefinition
        {
            get
            {
                if (!_isModuleInitialized)
                {
                    _isModuleInitialized = true;
                    _moduleDefinition = ILModuleDefinition.Create(_readers.MdReader.GetModuleDefinition(), ref _readers);
                }
                return _moduleDefinition;
            }
        }

        public ILHeaderOptions HeaderOptions
        {
            get
            {
                if (!_isHeaderInitialized)
                {
                    _isHeaderInitialized = true;
                    _headerOptions = ILHeaderOptions.Create(ref _readers);
                }
                return _headerOptions;
            }
        }

        /// <summary>
        /// A binary object representing a public encryption key for a strong-named assembly.
        /// Represented as a byte array on a string format (00 00 00 00 00 00 00 00 00)
        /// </summary>
        public byte[] PublicKey
        {
            get
            {
                if (_publicKey == null)
                {
                    _publicKey = _readers.MdReader.GetBlobBytes(_assemblyDefinition.PublicKey);
                }
                return _publicKey;
            }
        }

        public bool HasPublicKey
        {
            get
            {
                return PublicKey.Length != 0;
            }
        }
        
        public bool HasCulture
        {
            get
            {
                return !_assemblyDefinition.Culture.IsNil;
            }
        }

        public bool IsDll
        {
            get
            {
                return _readers.PEReader.PEHeaders.IsDll;
            }
        }

        public bool IsExe
        {
            get
            {
                return _readers.PEReader.PEHeaders.IsExe;
            }
        }

        /// <summary>
        /// Assembly flags if it is strong named, whether the JIT tracking and optimization is enabled, and if the assembly can be retargeted at run time to a different assembly version.
        /// </summary>
        public string Flags
        {
            get
            {
                if (_assemblyDefinition.Flags.HasFlag(AssemblyFlags.Retargetable))
                {
                    return "retargetable ";
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// The type definitions contained on the current assembly.
        /// </summary>
        public IEnumerable<ILTypeDefinition> TypeDefinitions
        {
            get
            {
                if (_typeDefinitions == null)
                {
                    _typeDefinitions = GetTypeDefinitions();
                }
                return _typeDefinitions.AsEnumerable<ILTypeDefinition>();
            }
        }

        public IEnumerable<ILTypeReference> TypeReferences
        {
            get
            {
                if(_typeReferences == null)
                {
                    _typeReferences = GetTypeReferences();
                }
                return _typeReferences;
            }
        }

        public IEnumerable<ILAssemblyReference> AssemblyReferences
        {
            get
            {
                if(_assemblyReferences == null)
                {
                    _assemblyReferences = GetAssemblyReferences();
                }
                return _assemblyReferences;
            }
        }

        public IEnumerable<ILModuleReference> ModuleReferences
        {
            get
            {
                if(_moduleReferences == null)
                {
                    _moduleReferences = GetModuleReferences();
                }
                return _moduleReferences;
            }
        }

        public IEnumerable<ILCustomAttribute> CustomAttributes
        {
            get
            {
                if(_customAttribues == null)
                {
                    _customAttribues = GetCustomAttributes();
                }
                return _customAttribues;
            }
        }

        public string GetPublicKeyString()
        {
            if (!HasPublicKey)
                return string.Empty;
            return ILDecoder.GetByteArrayString(PublicKey);
        }

        /// <summary>
        /// Method to get the hash algorithm formatted in the MSIL syntax.
        /// </summary>
        /// <returns>string representing the hash algorithm.</returns>
        public string GetFormattedHashAlgorithm()
        {
            return String.Format("0x{0:x8}", HashAlgorithm);
        }

        /// <summary>
        /// Method to get the version formatted to the MSIL syntax. MajorVersion:MinorVersion:BuildVersion:RevisionVersion.
        /// </summary>
        /// <returns>string representing the version.</returns>
        public string GetFormattedVersion()
        {
            return ILDecoder.GetVersionString(Version);
        }

        public void WriteTo(TextWriter writer)
        {
            this.Accept(new ILToStringVisitor(new ILVisitorOptions(true), writer));
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Dispose()
        {
            _readers.Dispose();
        }

        #endregion

        #region Private Methods

        private IEnumerable<ILTypeDefinition> GetTypeDefinitions()
        {
            var handles = _readers.MdReader.TypeDefinitions;
            foreach(var handle in handles)
            {
                if (handle.IsNil)
                {
                    continue;
                }
                var typeDefinition = _readers.MdReader.GetTypeDefinition(handle);
                if(typeDefinition.GetDeclaringType().IsNil)
                    yield return ILTypeDefinition.Create(typeDefinition, ref _readers, MetadataTokens.GetToken(handle));
            }
        }

        private IEnumerable<ILTypeReference> GetTypeReferences()
        {
            var handles = _readers.MdReader.TypeReferences;
            foreach(var handle in handles)
            {
                var typeReference = _readers.MdReader.GetTypeReference(handle);
                yield return ILTypeReference.Create(typeReference, ref _readers, MetadataTokens.GetToken(handle));
            }
        }

        private IEnumerable<ILAssemblyReference> GetAssemblyReferences()
        {
            foreach(var handle in _readers.MdReader.AssemblyReferences)
            {
                var assembly = _readers.MdReader.GetAssemblyReference(handle);
                int token = MetadataTokens.GetToken(handle);
                yield return ILAssemblyReference.Create(assembly, token, ref _readers, this);
            }
        }

        private IEnumerable<ILCustomAttribute> GetCustomAttributes()
        {
            foreach(var handle in _assemblyDefinition.GetCustomAttributes())
            {
                var attribute = _readers.MdReader.GetCustomAttribute(handle);
                yield return new ILCustomAttribute(attribute, ref _readers);
            }
        }

        private IEnumerable<ILModuleReference> GetModuleReferences()
        {
            for(int rid = 1, rowCount = _readers.MdReader.GetTableRowCount(TableIndex.ModuleRef); rid <= rowCount; rid++)
            {
                var handle = MetadataTokens.ModuleReferenceHandle(rid);
                var moduleReference = _readers.MdReader.GetModuleReference(handle);
                int token = MetadataTokens.GetToken(handle);
                yield return ILModuleReference.Create(moduleReference, ref _readers, token);
            }
        }

        #endregion
    }
}
