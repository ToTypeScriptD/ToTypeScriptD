using ApprovalTests;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;
using ToTypeScriptD.Core;
using ToTypeScriptD.Core.TypeWriters;
using ToTypeScriptD.Core.WinMD;
using ToTypeScriptD.Tests.ExeTests;
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
            var config = new Config();

            new TypeWriterCollector(errors, typeCollection.TypeSelector)
                .Collect(value, typeCollection, config);
            var result = typeCollection.Render(filterRegex);
            var errorResult = errors.ToString();
            if (string.IsNullOrEmpty(errorResult))
            {
                return result;
            }
            return errorResult + Environment.NewLine + Environment.NewLine + result;
        }

        public static void Verify(this string item)
        {
            item = item.StripHeaderGarbageromOutput();
            Approvals.Verify(item);
        }

        public static string StripVersionFromOutput(this string value)
        {
            return System.Text.RegularExpressions.Regex.Replace(value, "v[0-9].[0-9].[0-9]{0,4}.[0-9]{0,4}[0-9]? - SHA1:[a-zA-Z0-9]{0,7} - (Debug|Release)", "v0.0.0000.0000 SHA1:0000000 - Debug");
        }

        public static string StripHeaderGarbageromOutput(this string value)
        {
            value = System.Text.RegularExpressions.Regex.Replace(value, "__ToTypeScriptD_([0-9a-z]){32}:", "__ToTypeScriptD_{RANDOM_GUIDishString}:");
            value = System.Text.RegularExpressions.Regex.Replace(value, @"//  Date:          (.*)(PM|AM)", "//  Date:          mm/dd/YYYY H:MM:SS PM");
            value = value.StripVersionFromOutput();
            return value;
        }

        public static void Verify(this ExeProcessResult item)
        {
            item.ToString().Verify();
        }

        public static void Verify<T>(this T item)
        {
            Approvals.Verify(item);
        }

        public static void DumpAndVerify(this string path)
        {
            var errors = new StringBuilderTypeNotFoundErrorHandler();
            var typeCollection = new ToTypeScriptD.Core.TypeWriters.TypeCollection(new ToTypeScriptD.Core.WinMD.WinMDTypeWriterTypeSelector());
            var config = new Config();
            var result = ToTypeScriptD.Render.FullAssembly(path, errors, typeCollection, string.Empty, config);
            Approvals.Verify(errors + result);
        }
    }

}
