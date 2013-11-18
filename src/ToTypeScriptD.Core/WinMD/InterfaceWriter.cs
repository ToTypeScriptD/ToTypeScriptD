using System.Text;
using ToTypeScriptD.Core.TypeWriters;

namespace ToTypeScriptD.Core.WinMD
{
    public class InterfaceWriter : TypeWriterBase
    {
        public InterfaceWriter(Mono.Cecil.TypeDefinition typeDefinition, int indentCount, TypeCollection typeCollection, ConfigBase config)
            : base(typeDefinition, indentCount, typeCollection, config)
        {
        }

        public override void Write(StringBuilder sb)
        {
            ++IndentCount;
            base.WriteOutMethodSignatures(sb, "interface", "extends");
        }
    }
}
