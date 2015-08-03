using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ILDasmLibrary
{
    public struct ILTypeLayout
    {
        private readonly TypeLayout _layout;

        public ILTypeLayout(TypeLayout layout)
        {
            _layout = layout;
        }

        public int Size
        {
            get { return _layout.Size; }
        }

        public int PackingSize
        {
            get { return _layout.PackingSize; }
        }

        public bool IsDefault
        {
            get { return _layout.IsDefault; }
        }
    }
}
