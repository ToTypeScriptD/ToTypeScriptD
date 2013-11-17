using System;
using ToTypeScriptD.Core.TypeWriters;

namespace ToTypeScriptD.Core.DotNet
{
    public class DotNetTypeWriterTypeSelector : ITypeWriterTypeSelector
    {
        public ITypeWriter PickTypeWriter(Mono.Cecil.TypeDefinition td, int indentCount, TypeCollection typeCollection, Config config)
        {
            var castedConfig = (DotNetConfig)config;
            if (td.IsEnum)
            {
                return new EnumWriter(td, indentCount, typeCollection, config);
            }

            if (td.IsInterface)
            {
                return new InterfaceWriter(td, indentCount, typeCollection, castedConfig);
            }

            if (td.IsClass)
            {
                return new ClassWriter(td, indentCount, typeCollection, castedConfig);
            }

            throw new NotImplementedException("Could not get a type to generate for:" + td.FullName);
        }
    }
}
