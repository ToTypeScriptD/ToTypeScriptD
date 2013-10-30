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
            var errors = new StringBuilderTypeNotFoundErrorHandler();
            var result = ToTypeScriptD.Render.FullAssembly(path, errors);
            Approvals.Verify(errors + result);
        }
    }
}
