using ApprovalTests;
using Mono.Cecil;
using System.Collections.Generic;
using ToTypeScriptD.Core.TypeWriters;
using Xunit;

namespace ToTypeScriptD.Tests
{

    public class Spike : WinmdTestBase
    {
        [Fact]
        public void SpikeIt()
        {
        }

    }


    public static class Extensions
    {
        public static string ToTypeScript(this TypeDefinition value)
        {
            var typeCollection = new TypeCollection();
            new TypeWriterCollector().Collect(value, typeCollection);
            return typeCollection.Render();
        }

        public static string ToTypeScript(this IEnumerable<TypeDefinition> value)
        {
            var typeCollection = new TypeCollection();
            new TypeWriterCollector().Collect(value, typeCollection);
            return typeCollection.Render();
        }

        public static void Verify<T>(this T item)
        {
            Approvals.Verify(item);
        }
    }

}
