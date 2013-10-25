using System.Linq;
using Mono.Cecil;
using ApprovalTests.Reporters;
using System.Collections.Generic;

namespace ToTypeScriptD.Tests
{
    public class WinmdTestBase
    {
        private AssemblyDefinition _nativeAssemblyDefinition;
        public string NativeComponentPath { get { return @"ToTypeScriptD.Native.winmd"; } }
        public AssemblyDefinition NativeAssembly
        {
            get { return _nativeAssemblyDefinition ?? (_nativeAssemblyDefinition = Mono.Cecil.AssemblyDefinition.ReadAssembly(NativeComponentPath)); }
        }
        public ModuleDefinition NativeModule { get { return NativeAssembly.MainModule; } }


        private AssemblyDefinition _windowsAssemblyDefinition;
        public string WindowsComponentPath { get { return @"C:\Program Files (x86)\Windows Kits\8.0\References\CommonConfiguration\Neutral\Windows.winmd"; } }
        public AssemblyDefinition WindowsAssembly
        {
            get { return _windowsAssemblyDefinition ?? (_windowsAssemblyDefinition = Mono.Cecil.AssemblyDefinition.ReadAssembly(WindowsComponentPath)); }
        }
        public ModuleDefinition WindowsModule { get { return WindowsAssembly.MainModule; } }

        protected TypeDefinition GetNativeType(string name)
        {
            return NativeModule.Types.Where(s => s.FullName == "ToTypeScriptD.Native." + name).Single();
        }

        protected TypeDefinition GetWinNativeType(string name)
        {
            return WindowsModule.Types.Where(s => s.FullName == name).Single();
        }

        protected IEnumerable<TypeDefinition> GetWinNativeTypes(string name)
        {
            return WindowsModule.Types.Where(s => s.FullName.StartsWith(name));
        }
    }
}
