using ApprovalTests;
using System.IO;
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
        [ApprovalTests.Reporters.UseReporter(typeof(ApprovalTests.Reporters.P4MergeReporter))]
        public void ExeDuplicateAssemblyShouldStillOnlyGenerateOne()
        {
            var resultDup = Execute(@"-o WinRT C:\Windows\System32\WinMetadata\Windows.Foundation.winmd C:\Windows\System32\WinMetadata\Windows.Foundation.winmd");
            var resultNonDup = Execute(@"-o WinRT C:\Windows\System32\WinMetadata\Windows.Foundation.winmd");

            resultNonDup.ToString().StripHeaderGarbageromOutput()
                .DiffWith(resultDup.ToString().StripHeaderGarbageromOutput());
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

    public static class ApprovalsExtensions
    {
        public static void DiffWith(this string expected, string actual)
        {
            if (expected != actual)
            {
                var expectedFile = Path.GetTempPath() + "Expected.Approvals.Temp.txt";
                var actualFile = Path.GetTempPath() + "Actual.Approvals.Temp.txt";

                File.WriteAllText(expectedFile, expected);
                File.WriteAllText(actualFile, actual);

                var reporter = Approvals.GetReporter();
                reporter.Report(expectedFile, actualFile);
            }
        }
    }

}
