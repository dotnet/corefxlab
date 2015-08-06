using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace ILDasmLibrary
{
    public struct ILHeaderOptions
    {

        private Readers _readers;
        private ulong _imageBase;
        private ulong _stackReserve;
        private bool _isImageBaseInitialized;
        private bool _isStackReserveInitialized;
        private bool _isSubsystemInitialized;
        private bool _isCorflagsInitialized;
        private int _fileAlignment;
        private Subsystem _subsystem;
        private CorFlags _corflags;

        internal static ILHeaderOptions Create(ref Readers readers)
        {
            ILHeaderOptions options = new ILHeaderOptions();
            options._readers = readers;
            options._fileAlignment = -1;
            options._isImageBaseInitialized = false;
            options._isStackReserveInitialized = false;
            options._isSubsystemInitialized = false;
            options._isCorflagsInitialized = false;
            return options;
        }

        public ulong ImageBase
        {
            get
            {
                if (!_isImageBaseInitialized)
                {
                    _isImageBaseInitialized = true;
                    _imageBase = _readers.PEReader.PEHeaders.PEHeader.ImageBase;
                }
                return _imageBase;
            }
        }

        public int FileAlignment
        {
            get
            {
                if(_fileAlignment == -1)
                {
                    _fileAlignment = _readers.PEReader.PEHeaders.PEHeader.FileAlignment;
                }
                return _fileAlignment;
            }
        }

        public ulong StackReserve
        {
            get
            {
                if (!_isStackReserveInitialized)
                {
                    _isStackReserveInitialized = true;
                    _stackReserve = _readers.PEReader.PEHeaders.PEHeader.SizeOfStackReserve;
                }
                return _stackReserve;
            }
        }

        public Subsystem SubSystem
        {
            get
            {
                if (!_isSubsystemInitialized)
                {
                    _isSubsystemInitialized = true;
                    _subsystem = _readers.PEReader.PEHeaders.PEHeader.Subsystem;
                }
                return _subsystem;
            }
        }

        public CorFlags Corflags
        {
            get
            {
                if (!_isCorflagsInitialized)
                {
                    _isCorflagsInitialized = true;
                    _corflags = _readers.PEReader.PEHeaders.CorHeader.Flags;
                }
                return _corflags;
            }
        }

    }
}
