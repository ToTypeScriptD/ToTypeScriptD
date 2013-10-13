using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Reporters;
using Mono.Cecil;
using WinmdToTypeScript.TypeWriters;

namespace WinmdToTypeScript.Tests
{
    [UseReporter(typeof(ApprovalTests.Reporters.VisualStudioReporter))]
    [TestFixture]
    public class Spike
    {
        Mono.Cecil.ModuleDefinition mainModule;

        [SetUp]
        public void setup()
        {
            var winMD = @"C:\Program Files (x86)\Windows Kits\8.0\References\CommonConfiguration\Neutral\Windows.winmd";
            Mono.Cecil.AssemblyDefinition assemblyDefninition = Mono.Cecil.AssemblyDefinition.ReadAssembly(winMD);
            mainModule = assemblyDefninition.MainModule;
        }

        [Test]
        public void EnumType()
        {
            var type = mainModule.Types.Where(s => s.FullName == "Windows.Networking.NetworkOperators.NetworkDeviceStatus").Single();
            var result = (new g()).Generate(type);

            Approvals.Verify(result);
        }
    }


    public class g
    {
        public string Generate(Mono.Cecil.TypeDefinition td)
        {
            TypeWriterBase typeWriter = null;
            if (td.IsEnum)
            {
                typeWriter = new EnumWriter(td, 0);
            }
            var sb = new StringBuilder();
            typeWriter.Write(sb);
            return sb.ToString();
        }
    }

}
