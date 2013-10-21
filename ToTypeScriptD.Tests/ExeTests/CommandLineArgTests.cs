using ApprovalTests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ToTypeScriptD.Tests.ExeTests
{
    public class CommandLineArgTests : ExeTestBase
    {
        [Test]
        [Timeout(3000)]
        public void ExeShouldGenerateHelpOnEmptyInput()
        {
            var result = Execute("");

            Assert.IsEmpty(result.StdOut);
            Approvals.Verify(result.StdErr);
        }

        [Test]
        [Timeout(3000)]
        [Ignore("This is hanging on the Console.WriteLine(...)")]
        public void ExeShouldGenerateOutputForMultipleWinmdFiles()
        {
            var result = Execute(@"C:\Windows\System32\WinMetadata\Windows.Foundation.winmd C:\Windows\System32\WinMetadata\Windows.Networking.winmd");

            Assert.IsEmpty(result.StdErr);

            result.StdOut.Length.ShouldBeGreaterThan(100);
        }

        [Test]
        [Timeout(3000)]
        [Ignore("This is hanging on the Console.WriteLine(...)")]
        public void ExeDuplicateAssemblyShouldStillOnlyGenerateOne()
        {
            var resultDup = Execute(@"C:\Windows\System32\WinMetadata\Windows.Foundation.winmd C:\Windows\System32\WinMetadata\Windows.Foundation.winmd");
            var resultNonDup = Execute(@"C:\Windows\System32\WinMetadata\Windows.Foundation.winmd");

            resultNonDup.ShouldEqual(resultDup);
        }
    }
}
