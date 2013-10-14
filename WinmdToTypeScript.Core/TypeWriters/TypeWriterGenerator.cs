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
        public void Generate(Mono.Cecil.TypeDefinition td, StringBuilder sb)
        {
            var indentCount = 0;
            TypeWriterBase typeWriter = null;
            if (td.IsEnum)
            {
                typeWriter = new EnumWriter(td, indentCount);
            }
            else if (td.IsClass)
            {
                typeWriter = new ClassWriter(td, indentCount);
            }

            if (typeWriter == null)
            {
                throw new NotImplementedException("Could not get a type to generate for:" + td.FullName);
            }

            typeWriter.Write(sb);
        }
    }
}
