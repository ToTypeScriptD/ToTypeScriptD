using ApprovalTests;
using Xunit;

namespace ToTypeScriptD.Tests.DotNet
{

    public class CSharpTypeTests : CSharpTestBase
    {
        [Fact]
        public void GenerateFullAssembly()
        {
            var path = base.CSharpAssembly.ComponentPath;
            var errors = new StringBuilderTypeNotFoundErrorHandler();
            var typeCollection = new ToTypeScriptD.Core.TypeWriters.TypeCollection();
            var result = ToTypeScriptD.Render.FullAssembly(path, errors, typeCollection, string.Empty);
            Approvals.Verify(errors + result);
        }
    }
}
