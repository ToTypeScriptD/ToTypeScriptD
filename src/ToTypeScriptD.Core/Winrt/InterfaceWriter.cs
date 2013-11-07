using System.Text;
using ToTypeScriptD.Core.TypeWriters;

namespace ToTypeScriptD.Core.WinRT
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
        }
    }
}
