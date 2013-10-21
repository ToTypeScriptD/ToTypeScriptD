using ApprovalTests;
using ApprovalTests.Reporters;
using Mono.Cecil;
using Xunit;
using System.Collections.Generic;
using System.Text;
using ToTypeScriptD.Core.TypeWriters;

namespace ToTypeScriptD.Tests
{

    public class Spike : TestBase
    {
        [Fact]
        public void SpikeIt()
        {
            //Approvals.Verify(result);
        }

    }


    public static class Extensions
    {
        public static string ToTypeScript(this TypeDefinition value)
        {
            var typeCollection = new TypeCollection();
            new TypeWriterGenerator().Generate(value, typeCollection);
            return typeCollection.Render();
        }

        public static string ToTypeScript(this IEnumerable<TypeDefinition> value)
        {
            var typeCollection = new TypeCollection();
            new TypeWriterGenerator().Generate(value, typeCollection);
            return typeCollection.Render();
        }
    }

}
