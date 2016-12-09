using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Cil.Decoder;
using System.Reflection.Metadata.Cil.Visitor;
using System.Reflection.Metadata.Ecma335;

namespace System.Reflection.Metadata.Cil
{
    /// <summary>
    /// Class representing an assembly.
    /// </summary>
    public struct CilAssembly : ICilVisitable, IDisposable 
    {
        private CilReaders _readers;
        private AssemblyDefinition _assemblyDefinition;
        private byte[] _publicKey;
        private IEnumerable<CilTypeDefinition> _typeDefinitions;
        private string _name;
        private string _culture;
        private int _hashAlgorithm;
        private Version _version;
        private IEnumerable<CilAssemblyReference> _assemblyReferences;
        private IEnumerable<CilTypeReference> _typeReferences;
        private IEnumerable<CilCustomAttribute> _customAttribues;
        private IEnumerable<CilModuleReference> _moduleReferences;
        private CilModuleDefinition _moduleDefinition;
        private CilHeaderOptions _headerOptions;
        private bool _isHeaderInitialized;
        private bool _isModuleInitialized;
        private bool _disposed;

        #region Public APIs

        public static CilAssembly Create(Stream stream)
        {
            CilAssembly assembly = new CilAssembly();
            CilReaders readers = CilReaders.Create(stream);
            assembly._readers = readers;
            assembly._hashAlgorithm = -1;
            assembly._assemblyDefinition = readers.MdReader.GetAssemblyDefinition();
            assembly._isModuleInitialized = false;
            assembly._isHeaderInitialized = false;
            assembly._disposed = false;
            return assembly;
        }

        public static CilAssembly Create(string path)
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
                return CilDecoder.GetCachedValue(_assemblyDefinition.Name, _readers, ref _name);
            }
        }

        /// <summary>
        /// Property containing the assembly culture. known as locale, such as en-US or fr-CA.
        /// </summary>
        public string Culture
        {
            get
            {
                return CilDecoder.GetCachedValue(_assemblyDefinition.Culture, _readers, ref _culture);
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

        public CilModuleDefinition ModuleDefinition
        {
            get
            {
                if (!_isModuleInitialized)
                {
                    _isModuleInitialized = true;
                    _moduleDefinition = CilModuleDefinition.Create(_readers.MdReader.GetModuleDefinition(), ref _readers);
                }
                return _moduleDefinition;
            }
        }

        public CilHeaderOptions HeaderOptions
        {
            get
            {
                if (!_isHeaderInitialized)
                {
                    _isHeaderInitialized = true;
                    _headerOptions = CilHeaderOptions.Create(ref _readers);
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
        public IEnumerable<CilTypeDefinition> TypeDefinitions
        {
            get
            {
                if (_typeDefinitions == null)
                {
                    _typeDefinitions = GetTypeDefinitions();
                }
                return _typeDefinitions.AsEnumerable<CilTypeDefinition>();
            }
        }

        public IEnumerable<CilTypeReference> TypeReferences
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

        public IEnumerable<CilAssemblyReference> AssemblyReferences
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

        public IEnumerable<CilModuleReference> ModuleReferences
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

        public IEnumerable<CilCustomAttribute> CustomAttributes
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
            return CilDecoder.CreateByteArrayString(PublicKey);
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
            return CilDecoder.CreateVersionString(Version);
        }

        public void WriteTo(TextWriter writer)
        {
            this.Accept(new CilToStringVisitor(new CilVisitorOptions(true), writer));
        }

        public void Accept(ICilVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private Methods

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _readers.Dispose();
                }

                _disposed = true;
            }
        }

        private IEnumerable<CilTypeDefinition> GetTypeDefinitions()
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
                    yield return CilTypeDefinition.Create(typeDefinition, ref _readers, MetadataTokens.GetToken(handle));
            }
        }

        private IEnumerable<CilTypeReference> GetTypeReferences()
        {
            var handles = _readers.MdReader.TypeReferences;
            foreach(var handle in handles)
            {
                var typeReference = _readers.MdReader.GetTypeReference(handle);
                yield return CilTypeReference.Create(typeReference, ref _readers, MetadataTokens.GetToken(handle));
            }
        }

        private IEnumerable<CilAssemblyReference> GetAssemblyReferences()
        {
            foreach(var handle in _readers.MdReader.AssemblyReferences)
            {
                var assembly = _readers.MdReader.GetAssemblyReference(handle);
                int token = MetadataTokens.GetToken(handle);
                yield return CilAssemblyReference.Create(assembly, token, ref _readers, this);
            }
        }

        private IEnumerable<CilCustomAttribute> GetCustomAttributes()
        {
            foreach(var handle in _assemblyDefinition.GetCustomAttributes())
            {
                var attribute = _readers.MdReader.GetCustomAttribute(handle);
                yield return new CilCustomAttribute(attribute, ref _readers);
            }
        }

        private IEnumerable<CilModuleReference> GetModuleReferences()
        {
            for(int rid = 1, rowCount = _readers.MdReader.GetTableRowCount(TableIndex.ModuleRef); rid <= rowCount; rid++)
            {
                var handle = MetadataTokens.ModuleReferenceHandle(rid);
                var moduleReference = _readers.MdReader.GetModuleReference(handle);
                int token = MetadataTokens.GetToken(handle);
                yield return CilModuleReference.Create(moduleReference, ref _readers, token);
            }
        }

        #endregion
    }
}
