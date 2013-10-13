using Mono.Cecil;
using System;
using System.Text;

namespace WinmdToTypeScript.TypeWriters
{
    public class NamespaceWriter : TypeWriterBase
    {
        public NamespaceWriter(TypeDefinition typeDefinition, int indentCount)
            : base(typeDefinition, indentCount)
        {
        }

        public override void Write(StringBuilder sb, Action midWrite)
        {
            sb.Append(Indent); sb.AppendFormat("declare module {0} {{", TypeDefinition.Namespace);
            sb.AppendLine();
            midWrite();
            sb.Append(Indent); sb.AppendLine("}");
        }
    }

    public class EnumWriter : TypeWriterBase
    {
        public EnumWriter(TypeDefinition typeDefinition, int indentCount)
            : base(typeDefinition, indentCount)
        { }

        public override void Write(StringBuilder sb)
        {
            var namespaceWriter = new NamespaceWriter(TypeDefinition, IndentCount);
            namespaceWriter.Write(sb, () =>
            {
                ++IndentCount;
                sb.AppendLine(Indent + "enum " + TypeDefinition.Name + "{");
                foreach (var item in TypeDefinition.Fields)
                {
                    if (item.Name == "value__") continue;
                    sb.AppendLine(Indent + Indent + item.Name + " = " + item.Constant);
                }
                sb.AppendLine(Indent + "}");
            });
        }
    }
}
