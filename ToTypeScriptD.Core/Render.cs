using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToTypeScriptD.Core.TypeWriters;

namespace ToTypeScriptD
{
    public class Render
    {
        public static bool AllAssemblies(IEnumerable<string> assemblyPaths, TextWriter textWriter)
        {
            if (assemblyPaths == null) assemblyPaths = new string[0];
           var wroteAnyTypes = false;

            var filesAlreadyProcessed = new HashSet<string>(new IgnoreCaseStringEqualityComparer());
            if (assemblyPaths.Any())
            {
                wroteAnyTypes = true;

                assemblyPaths.Each(file =>
                {
                    if (filesAlreadyProcessed.Contains(file))
                        return;

                    filesAlreadyProcessed.Add(file);
                    var x = ToTypeScriptD.Render.FullAssembly(file);

                    textWriter.WriteLine("");
                    textWriter.WriteLine(x);
                });
            }

            return wroteAnyTypes;
        }

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


    public class IgnoreCaseStringEqualityComparer : EqualityComparer<string>
    {

        public override bool Equals(string x, string y)
        {
            if (x == null && y == null) return true;
            if (x == null) return false;
            return x.Equals(y);
        }

        public override int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}
