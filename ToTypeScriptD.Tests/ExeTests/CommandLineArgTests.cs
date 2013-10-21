using ApprovalTests;
using Xunit;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ToTypeScriptD.Tests.ExeTests
{
    public class CommandLineArgTests : ExeTestBase
    {
        [Fact(Timeout=3000)]
        public async void ExeShouldGenerateHelpOnEmptyInput()
        {
            var result = await Execute("");

            //Assert.IsEmpty(result.StdOut);
            Approvals.Verify(result.StdErr);
        }

        [Fact(Timeout = 3000)]
        public async void ExeShouldGenerateOutputForMultipleWinmdFiles()
        {
            var result = await Execute(@"C:\Windows\System32\WinMetadata\Windows.Foundation.winmd C:\Windows\System32\WinMetadata\Windows.Networking.winmd");

            Assert.Empty(result.StdErr);

            (result.StdOut.Length > 100).ShouldBeTrue(result.StdOut.Length + " should be greater than 100");
        }

        [Fact(Timeout=3000)]
        public async void ExeDuplicateAssemblyShouldStillOnlyGenerateOne()
        {
            var resultDup = await Execute(@"C:\Windows\System32\WinMetadata\Windows.Foundation.winmd C:\Windows\System32\WinMetadata\Windows.Foundation.winmd");
            var resultNonDup = await Execute(@"C:\Windows\System32\WinMetadata\Windows.Foundation.winmd");

            resultNonDup.ShouldEqual(resultDup);
        }
    }
}
