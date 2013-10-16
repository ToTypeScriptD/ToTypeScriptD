using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinmdToTypeScript.Core.TypeWriters
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
            step(sb); sb.Append("interface " + TypeDefinition.Name + " ");

            if (TypeDefinition.Interfaces.Any())
            {
                sb.Append("extends ");
                TypeDefinition.Interfaces.For((item, i, isLast) =>
                {
                    sb.AppendFormat(" {0}{1} ", item.FullName, isLast ? "" : ",");
                });
            }
            sb.AppendLine("{");

            base.WriteOutMethodSignatures(sb);
            step(sb); sb.AppendLine("}");
        }
    }
}
