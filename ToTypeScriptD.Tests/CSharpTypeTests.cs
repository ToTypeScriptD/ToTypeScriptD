using ApprovalTests;
using Xunit;

namespace ToTypeScriptD.Tests
{

    public class CSharpTypeTests : CSharpTestBase
    {
        [Fact]
        public void GenerateFullAssembly()
        {
            var path = base.CSharpAssembly.ComponentPath;
            var result = ToTypeScriptD.Render.FullAssembly(path);
            Approvals.Verify(result);
        }
    }
}
