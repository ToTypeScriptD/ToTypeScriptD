using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinmdToTypeScript.Core.TypeWriters;

namespace WinmdToTypeScript.Core.TypeWriters
{
    public class TypeWriterGenerator
    {
        public void Generate(IEnumerable<Mono.Cecil.TypeDefinition> tds, TypeCollection typeCollection)
        {
            foreach (var item in tds)
            {
                Generate(item, typeCollection);
            }
        }
        public void Generate(Mono.Cecil.TypeDefinition td, TypeCollection typeCollection)
        {
            // don't duplicate types
            if (typeCollection.Contains(td.FullName))
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            var indentCount = 0;
            ITypeWriter typeWriter = PickTypeWriter(td, indentCount, typeCollection);

            td.Interfaces.Each(item =>
            {
                var foundType = item.Module.Types.SingleOrDefault(w => w.FullName == item.FullName);
                if (foundType == null)
                {
                    Console.Error.WriteLine("Could not find type: " + item.FullName);
                    //throw new Exception("Could not find type: " + item.FullName);
                    return;
                }

                var itemWriter = new InterfaceWriter(foundType, indentCount, typeCollection);
                typeCollection.Add(foundType.Namespace, foundType.Name, itemWriter);

            });

            typeCollection.Add(td.Namespace, td.Name, typeWriter);
        }

        ITypeWriter PickTypeWriter(TypeDefinition td, int indentCount, TypeCollection typeCollection)
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
