using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILDasmLibrary.Visitor
{
    public interface IVisitable
    {
        void Accept(IVisitor visitor);
    }
}
