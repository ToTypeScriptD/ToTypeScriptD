using Mono.Cecil;
using System.Text;

namespace WinmdToTypeScript.Core.TypeWriters
{
    public class EnumWriter : TypeWriterBase
    {
        public EnumWriter(TypeDefinition typeDefinition, int indentCount, TypeCollection typeCollection)
            : base(typeDefinition, indentCount, typeCollection)
        { }

        public override void Write(StringBuilder sb)
        {
            ++IndentCount;
            sb.AppendLine(Indent + "enum " + TypeDefinition.Name + " {");
            ++IndentCount;
            TypeDefinition.Fields.For((item, i, isLast) =>
            {
                if (item.Name == "value__") return;
                sb.AppendFormat("{0}{1}", Indent, item.Name);
                sb.AppendLine(isLast ? "" : ",");
            });
            --IndentCount;
            sb.AppendLine(Indent + "}");
        }
    }
}
