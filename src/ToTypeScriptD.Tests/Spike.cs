using ApprovalTests;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;
using ToTypeScriptD.Core.TypeWriters;
using ToTypeScriptD.Core.WinMD;
using Xunit;

namespace ToTypeScriptD.Tests
{

    public class Spike
    {
        [Fact]
        public void SpikeIt()
        {
            //@"C:\Windows\System32\WinMetadata\Windows.System.winmd".DumpAndVerify();
        }
    }


    public static class Extensions
    {
        public static string ToTypeScript(this TypeDefinition value)
        {
            return ToTypeScript(new[] { value });
        }

        public static string ToTypeScript(this IEnumerable<TypeDefinition> value, string filterRegex = null)
        {
            var typeCollection = new TypeCollection(new WinMDTypeWriterTypeSelector());
            var errors = new StringBuilderTypeNotFoundErrorHandler();
            new TypeWriterCollector(errors, typeCollection.TypeSelector)
                .Collect(value, typeCollection);
            var result = typeCollection.Render(filterRegex);
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

        public static void DumpAndVerify(this string path)
        {
            var errors = new StringBuilderTypeNotFoundErrorHandler();
            var typeCollection = new ToTypeScriptD.Core.TypeWriters.TypeCollection(new ToTypeScriptD.Core.WinMD.WinMDTypeWriterTypeSelector());
            var result = ToTypeScriptD.Render.FullAssembly(path, errors, typeCollection, string.Empty);
            Approvals.Verify(errors + result);
        }
    }

}
