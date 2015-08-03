using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILDasmLibrary.Visitor
{
    public struct ILVisitorOptions
    {
        private readonly bool _showBytes;

        public ILVisitorOptions(bool showBytes)
        {
            _showBytes = showBytes;
        }

        public bool ShowBytes
        {
            get
            {
                return _showBytes;
            }
        }
    }
}
