using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinmdToTypeScript.Core.TypeWriters
{
    public interface ITypeWriter
    {
        void Write(StringBuilder sb);
    }
}
