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
            step(sb); sb.Append("export class " + TypeDefinition.Name + " ");

            if (TypeDefinition.Interfaces.Any())
            {
                sb.Append("implements");
                TypeDefinition.Interfaces.For((item, i, isLast) =>
                {
                    sb.AppendFormat(" {0}{1}", item.FullName, isLast ? " " : ",");
                });
            }
            sb.AppendLine("{");

            base.WriteOutMethodSignatures(sb);
            step(sb); sb.AppendLine("}");
        }
    }
}
