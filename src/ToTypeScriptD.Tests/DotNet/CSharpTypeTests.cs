using ApprovalTests;
using ToTypeScriptD.Core;
using ToTypeScriptD.Core.DotNet;
using Xunit;

namespace ToTypeScriptD.Tests.DotNet
{

    public class CSharpTypeTests : CSharpTestBase
    {
        [Fact]
        public void GenerateFullAssembly()
        {
            var path = base.CSharpAssembly.ComponentPath;
            path.DumpAndVerify(OutputType.DotNet);
        }

        [Fact]
        public void UpperCasePropertyName()
        {
            var path = base.CSharpAssembly.ComponentPath;
            path.DumpAndVerify(OutputType.DotNet, config =>
            {
                config.CamelCase = false;
            });
        }
    }
}
