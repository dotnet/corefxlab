using System;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.Metadata.ILDasm.Decoder;
using System.Reflection.PortableExecutable;

namespace ILDasmLibrary
{
    internal struct Readers : IDisposable
    {
        private readonly PEReader _peReader;
        private readonly MetadataReader _mdReader;
        private readonly ILTypeProvider _provider;

        public PEReader PEReader { get { return _peReader; } }
        public MetadataReader MdReader { get { return _mdReader; } }
        public ILTypeProvider Provider { get { return _provider; } }

        private Readers(Stream fileStream)
        {
            _peReader = new PEReader(fileStream);
            _mdReader = _peReader.GetMetadataReader();
            _provider = new ILTypeProvider(_mdReader);
        }

        public static Readers Create(Stream fileStream)
        {
            return new Readers(fileStream);
        }

        public void Dispose()
        {
            PEReader.Dispose();
        }
    }
}
