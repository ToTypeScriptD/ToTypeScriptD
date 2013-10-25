using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToTypeScriptD.Core.TypeWriters;

namespace ToTypeScriptD
{
    public class Render
    {
        public static string FullAssembly(string assemblyPath)
        {
            var assembly = Mono.Cecil.AssemblyDefinition.ReadAssembly(assemblyPath);
            var typeCollection = new TypeCollection();
            var typeWriterGenerator = new TypeWriterCollector();
            foreach (var item in assembly.MainModule.Types)
            {
                typeWriterGenerator.Collect(item, typeCollection);
            }

            return typeCollection.Render();
        }
    }
}
