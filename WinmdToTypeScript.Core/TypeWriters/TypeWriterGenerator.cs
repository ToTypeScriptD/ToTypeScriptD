using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinmdToTypeScript.Core.TypeWriters;

namespace WinmdToTypeScript.Core.TypeWriters
{
    public class TypeWriterGenerator
    {
        public string Generate(Mono.Cecil.TypeDefinition td)
        {
            TypeWriterBase typeWriter = null;
            if (td.IsEnum)
            {
                typeWriter = new EnumWriter(td, 0);
            }
            var sb = new StringBuilder();
            typeWriter.Write(sb);
            return sb.ToString();
        }
    }
}
