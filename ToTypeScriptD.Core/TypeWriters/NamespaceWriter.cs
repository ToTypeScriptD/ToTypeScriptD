using Mono.Cecil;
using System;
using System.Text;

namespace ToTypeScriptD.Core.TypeWriters
{
    public class NamespaceWriter : TypeWriterBase
    {
        public NamespaceWriter(TypeDefinition typeDefinition, int indentCount, TypeCollection typeCollection)
            : base(typeDefinition, indentCount, typeCollection)
        {
        }

        public override void Write(StringBuilder sb, Action midWrite)
        {
            sb.Append(IndentValue); sb.AppendFormat("declare module {0} {{", TypeDefinition.Namespace);
            sb.AppendLine();
            midWrite();
            sb.Append(IndentValue); sb.AppendLine("}");
        }
    }
}
