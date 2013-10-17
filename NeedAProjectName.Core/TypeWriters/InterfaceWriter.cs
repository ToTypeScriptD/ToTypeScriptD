using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeedAProjectName.Core.TypeWriters
{
    public class InterfaceWriter : TypeWriterBase
    {
        public InterfaceWriter(Mono.Cecil.TypeDefinition typeDefinition, int indentCount, TypeCollection typeCollection)
            : base(typeDefinition, indentCount, typeCollection)
        {
        }

        public override void Write(StringBuilder sb)
        {
            ++IndentCount;
            base.WriteOutMethodSignatures(sb, "interface", "extends");
            step(sb); sb.AppendLine("}");
        }
    }
}
