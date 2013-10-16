using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinmdToTypeScript.Core.TypeWriters
{
    public class ClassWriter : TypeWriterBase
    {
        public ClassWriter(Mono.Cecil.TypeDefinition typeDefinition, int indentCount, TypeCollection typeCollection)
            : base(typeDefinition, indentCount, typeCollection)
        {
        }

        public override void Write(System.Text.StringBuilder sb)
        {
            ++IndentCount;
            sb.Append(Indent + "export class " + TypeDefinition.Name + " ");

            if (TypeDefinition.Interfaces.Any())
            {
                sb.Append("implements ");
                TypeDefinition.Interfaces.For((item, i, isLast) =>
                {
                    sb.AppendFormat(" {0}{1} ", item.FullName, isLast ? "" : ",");
                });
            }
            sb.AppendLine("{");

            ++IndentCount;
            base.WriteOutMethodSignatures(sb);
            --IndentCount;
            sb.AppendLine(Indent + "}");
        }
    }
}
