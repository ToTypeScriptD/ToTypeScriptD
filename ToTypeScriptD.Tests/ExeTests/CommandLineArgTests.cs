using ApprovalTests;
using Xunit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ToTypeScriptD.Tests.ExeTests
{
    public class CommandLineArgTests : ExeTestBase
    {
        [Fact]
        public void ExeShouldGenerateHelpOnEmptyInput()
        {
            var result = Execute("").StdOut;
            result = Regex.Replace(result, "ToTypeScriptD 0.0.(.*)", "ToTypeScriptD 0.0.*");
            Approvals.Verify(result);
        }

        [Fact]
        public void ExeShouldGenerateOutputForMultipleWinmdFiles()
        {
            var result = Execute(@"C:\Windows\System32\WinMetadata\Windows.Foundation.winmd C:\Windows\System32\WinMetadata\Windows.Networking.winmd");

            (result.StdOut.Length > 100).ShouldBeTrue(result.StdOut.Length + " should be greater than 100");
        }

        [Fact]
        public void ExeDuplicateAssemblyShouldStillOnlyGenerateOne()
        {
            var resultDup = Execute(@"C:\Windows\System32\WinMetadata\Windows.Foundation.winmd C:\Windows\System32\WinMetadata\Windows.Foundation.winmd");
            var resultNonDup = Execute(@"C:\Windows\System32\WinMetadata\Windows.Foundation.winmd");

            // TODO: this test needs a good way to leverage the power of approvals diff capability (but it's not yet supported)

            resultNonDup.StdOut.Trim().ShouldEqual(resultDup.StdOut.Trim());
        }

        [Fact]
        public void ExeShouldBeAbleToGenerateSpecialTypes()
        {
            var resultDup = Execute("--specialTypes");

            resultDup.Verify();
        }
    }
}
