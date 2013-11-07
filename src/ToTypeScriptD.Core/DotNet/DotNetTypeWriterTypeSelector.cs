using System;
using ToTypeScriptD.Core.TypeWriters;

namespace ToTypeScriptD.Core.DotNet
{
    public class DotNetTypeWriterTypeSelector : ITypeWriterTypeSelector
    {
        public ITypeWriter PickTypeWriter(Mono.Cecil.TypeDefinition td, int indentCount, TypeCollection typeCollection)
        {
            if (td.IsEnum)
            {
                return new EnumWriter(td, indentCount, typeCollection);
            }

            if (td.IsInterface)
            {
                return new InterfaceWriter(td, indentCount, typeCollection);
            }

            if (td.IsClass)
            {
                return new ClassWriter(td, indentCount, typeCollection);
            }

            throw new NotImplementedException("Could not get a type to generate for:" + td.FullName);
        }
    }
}
