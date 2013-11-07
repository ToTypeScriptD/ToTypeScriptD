using ApprovalTests;
using Xunit;

namespace ToTypeScriptD.Tests.ExeTests
{
    public class CommandLineArgTests : ExeTestBase
    {
        [Fact]
        public void ExeShouldGenerateHelpOnEmptyInput()
        {
            var result = Execute("").StdOut.StripVersionFromOutput();
            Approvals.Verify(result);
        }

        [Fact]
        public void ExeShouldGenerateHelpWithHelpArgs()
        {
            var result = Execute("--help").StdOut.StripVersionFromOutput();
            Approvals.Verify(result);
        }

        [Fact]
        public void ExeShouldGenerateOutputForMultipleWinmdFiles()
        {
            var result = Execute(@"-o WinRT C:\Windows\System32\WinMetadata\Windows.Foundation.winmd C:\Windows\System32\WinMetadata\Windows.Networking.winmd");

            (result.StdOut.Length > 100).ShouldBeTrue(result.StdOut.Length + " should be greater than 100");
        }

        [Fact]
        public void VerifyExeGeneratedOutputForMultipleWinmdFiles()
        {
            var result = Execute(@"-o WinRT C:\Windows\System32\WinMetadata\Windows.Foundation.winmd C:\Windows\System32\WinMetadata\Windows.System.winmd");

            result.StdOut.Verify();
        }

        [Fact]
        public void ExeDuplicateAssemblyShouldStillOnlyGenerateOne()
        {
            var resultDup = Execute(@"-o WinRT C:\Windows\System32\WinMetadata\Windows.Foundation.winmd C:\Windows\System32\WinMetadata\Windows.Foundation.winmd");
            var resultNonDup = Execute(@"-o WinRT C:\Windows\System32\WinMetadata\Windows.Foundation.winmd");

            // TODO: this test needs a good way to leverage the power of approvals diff capability (but it's not yet supported)

            resultNonDup.StdOut.Trim().ShouldEqual(resultDup.StdOut.Trim());
        }

        [Fact]
        public void ExeShouldBeAbleToGenerateSpecialTypes()
        {
            var resultDup = Execute("-o WinRT --specialTypes");

            resultDup.Verify();
        }

        [Fact]
        public void ExeShouldGiveHelpfulErrorWhenFilesNotFoundInUnknownDirectory()
        {
            var resultDup = Execute(@"-o WinRT C:\TypeScriptD\TypeScriptD\TypeScriptD\Foo.dll C:\TypeScriptD\TypeScriptD\TypeScriptD\Foo.dll");

            resultDup.Verify();
        }

        [Fact]
        public void ExeShouldGiveHelpfulErrorWhenFilesNotFound()
        {
            var resultDup = Execute(@"-o WinRT C:\TypeScriptD_FileNotFound_ThisShouldNotExistOnYourSystem.dll");

            resultDup.Verify();
        }

        [Fact]
        public void ExeShouldApplyRegexFilterOnTypes()
        {
            var resultDup = Execute(@"-o DotNet ..\..\..\bin\ToTypeScriptD.TestAssembly.CSharp.dll --regexFilter 'ToTypeScriptD.TestAssembly.CSharp.NamespaceSample'");

            resultDup.Verify();
        }

    }

    public static class Extensions
    {
        public static string StripVersionFromOutput(this string value)
        {
            return System.Text.RegularExpressions.Regex.Replace(value, "^ToTypeScriptD v[0-9].[0-9].[0-9]{0,4}.[0-9]{0,4}[0-9]? - SHA1:[a-zA-Z0-9]{0,7} - (Debug|Release)", "ToTypeScriptD - v0.0.0000.0000 SHA1:0000000 - Debug");
        }
    }
}
