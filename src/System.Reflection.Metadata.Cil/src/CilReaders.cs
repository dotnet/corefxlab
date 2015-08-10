using System.IO;
using System.Reflection.Metadata.Cil.Decoder;
using System.Reflection.PortableExecutable;

namespace System.Reflection.Metadata.Cil
{
    internal struct CilReaders : IDisposable
    {
        private readonly PEReader _peReader;
        private readonly MetadataReader _mdReader;
        private readonly CilTypeProvider _provider;

        public PEReader PEReader { get { return _peReader; } }
        public MetadataReader MdReader { get { return _mdReader; } }
        public CilTypeProvider Provider { get { return _provider; } }

        private CilReaders(Stream fileStream)
        {
            _peReader = new PEReader(fileStream);
            _mdReader = _peReader.GetMetadataReader();
            _provider = new CilTypeProvider(_mdReader);
        }

        public static CilReaders Create(Stream fileStream)
        {
            return new CilReaders(fileStream);
        }

        public void Dispose()
        {
            PEReader.Dispose();
        }
    }
}
