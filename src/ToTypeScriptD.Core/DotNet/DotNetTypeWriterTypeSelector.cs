using System;
using ToTypeScriptD.Core.TypeWriters;

namespace ToTypeScriptD.Core.DotNet
{
    public class DotNetTypeWriterTypeSelector : ITypeWriterTypeSelector
    {
        public ITypeWriter PickTypeWriter(Mono.Cecil.TypeDefinition td, int indentCount, TypeCollection typeCollection, Config config)
        {
            if (td.IsEnum)
            {
                return new EnumWriter(td, indentCount, typeCollection, config);
            }

            if (td.IsInterface)
            {
                return new InterfaceWriter(td, indentCount, typeCollection, config);
            }

            if (td.IsClass)
            {
                return new ClassWriter(td, indentCount, typeCollection, config);
            }

            throw new NotImplementedException("Could not get a type to generate for:" + td.FullName);
        }
    }
}
