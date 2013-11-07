using ToTypeScriptD.Core.TypeWriters;

namespace ToTypeScriptD.Core.DotNet
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
            base.WriteOutMethodSignatures(sb, "interface", "extends");
        }
    }
}
