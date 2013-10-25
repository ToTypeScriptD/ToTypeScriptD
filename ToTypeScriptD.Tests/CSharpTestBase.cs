using System.Linq;
using System.IO;
using Mono.Cecil;
using ApprovalTests.Reporters;
using System.Collections.Generic;
using Xunit;

namespace ToTypeScriptD.Tests
{
    public class CSharpTestBase
    {
        public TestAssembly CSharpAssembly { get; private set; }
        public CSharpTestBase()
        {
            var path = typeof(ToTypeScriptD.TestAssembly.CSharp.IAmAnInterface).Assembly.Location;
            path = Path.Combine(Path.GetDirectoryName(path), Path.GetFileName(path));
            CSharpAssembly = new TestAssembly(path);
        }
    }
}
