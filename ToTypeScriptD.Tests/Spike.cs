using ApprovalTests;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;
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
            return ToTypeScript(new[] { value });
        }

        public static string ToTypeScript(this IEnumerable<TypeDefinition> value)
        {
            var typeCollection = new TypeCollection();
            var errors = new StringBuilderTypeNotFoundErrorHandler();
            new TypeWriterCollector(errors)
                .Collect(value, typeCollection);
            var result = typeCollection.Render();
            var errorResult = errors.ToString();
            if (string.IsNullOrEmpty(errorResult))
            {
                return result;
            }
            return errorResult + Environment.NewLine + Environment.NewLine + result;
        }

        public static void Verify<T>(this T item)
        {
            Approvals.Verify(item);
        }
    }

}
