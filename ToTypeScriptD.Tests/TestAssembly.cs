using Mono.Cecil;
using System.Collections.Generic;
using System.Linq;

namespace ToTypeScriptD.Tests
{
    public class TestAssembly
    {
        private AssemblyDefinition _nativeAssemblyDefinition;
        private string path;

        public TestAssembly(string path)
        {
            this.path = path;
        }
        public string ComponentPath
        {
            get { return path; }
        }

        public AssemblyDefinition AssemblyDefinition
        {
            get { return _nativeAssemblyDefinition ?? (_nativeAssemblyDefinition = Mono.Cecil.AssemblyDefinition.ReadAssembly(ComponentPath)); }
        }
        public ModuleDefinition ModuleDefinition { get { return AssemblyDefinition.MainModule; } }


        public TypeDefinition GetNativeType(string name)
        {
            return ModuleDefinition.Types.Where(s => s.FullName == name).Single();
        }

        protected IEnumerable<TypeDefinition> GetAllTypesThatStartsWith(string name)
        {
            return ModuleDefinition.Types.Where(s => s.FullName.StartsWith(name));
        }
    }
}
