using System.Linq;
using Mono.Cecil;
using ApprovalTests.Reporters;
using System.Collections.Generic;
using Xunit;

namespace ToTypeScriptD.Tests
{
    public class CSharpTestBase
    {
        public TestAssembly CSharpAssembly = new TestAssembly(typeof(ToTypeScriptD.TestAssembly.CSharp.IAmAnInterface).Assembly.CodeBase);
    }
}
