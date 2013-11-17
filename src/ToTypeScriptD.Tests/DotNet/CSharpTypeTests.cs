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
            var config = new Config
            {
                TypeNotFoundErrorHandler = new StringBuilderTypeNotFoundErrorHandler(),
                OutputType = OutputType.DotNet,
            };
            var typeCollection = new ToTypeScriptD.Core.TypeWriters.TypeCollection(config.GetTypeWriterTypeSelector());
            var result = ToTypeScriptD.Render.FullAssembly(path, typeCollection, config);
            (config.TypeNotFoundErrorHandler + result).Verify();
        }
    }
}
