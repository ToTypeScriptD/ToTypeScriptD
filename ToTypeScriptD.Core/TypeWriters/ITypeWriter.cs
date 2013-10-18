using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToTypeScriptD.Core.TypeWriters
{
    public interface ITypeWriter
    {
        void Write(StringBuilder sb);
    }
}
