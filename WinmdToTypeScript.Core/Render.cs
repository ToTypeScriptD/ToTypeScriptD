using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinmdToTypeScript.Core.TypeWriters;

namespace WinmdToTypeScript
{
    public class Render
    {
        public static string FullAssembly(string assemblyPath)
        {
            var assembly = Mono.Cecil.AssemblyDefinition.ReadAssembly(assemblyPath);
            var typeCollection = new TypeCollection();
            var typeWriterGenerator = new TypeWriterGenerator();
            foreach (var item in assembly.MainModule.Types)
            {
                typeWriterGenerator.Generate(item, typeCollection);
            }

            return typeCollection.Render();
        }
    }
}
