using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToTypeScriptD.Core.TypeWriters;

namespace ToTypeScriptD.Core.TypeWriters
{
    public interface ITypeNotFoundErrorHandler
    {
        void Handle(TypeReference typeReference);
    }

    public class TypeWriterCollector
    {
        private ITypeNotFoundErrorHandler typeNotFoundErrorHandler;
        private ITypeWriterTypeSelector typeSelector;

        public TypeWriterCollector(ITypeNotFoundErrorHandler typeNotFoundErrorHandler, ITypeWriterTypeSelector typeSelector)
        {
            this.typeNotFoundErrorHandler = typeNotFoundErrorHandler;
            this.typeSelector = typeSelector;
        }
        public void Collect(IEnumerable<Mono.Cecil.TypeDefinition> tds, TypeCollection typeCollection)
        {
            foreach (var item in tds)
            {
                Collect(item, typeCollection);
            }
        }
        public void Collect(Mono.Cecil.TypeDefinition td, TypeCollection typeCollection)
        {
            if (td.ShouldIgnoreType())
            {
                return;
            }

            // don't duplicate types
            if (typeCollection.Contains(td.FullName))
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            var indentCount = 0;
            ITypeWriter typeWriter = typeSelector.PickTypeWriter(td, indentCount, typeCollection);

            td.Interfaces.Each(item =>
            {
                var foundType = typeCollection.LookupType(item);

                if (foundType == null)
                {
                    //TODO: This reporting a missing type is too early in the process.
                    // typeNotFoundErrorHandler.Handle(item);
                    return;
                }

                var itemWriter = typeSelector.PickTypeWriter(foundType, indentCount, typeCollection);
                typeCollection.Add(foundType.Namespace, foundType.Name, itemWriter);

            });

            typeCollection.Add(td.Namespace, td.Name, typeWriter);
        }
    }
}
