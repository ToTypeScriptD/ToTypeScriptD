using Mono.Cecil;

namespace ToTypeScriptD.Core.TypeWriters
{
    public interface ITypeWriterTypeSelector
    {
        ITypeWriter PickTypeWriter(TypeDefinition td, int indentCount, TypeCollection typeCollection);
    }
}
