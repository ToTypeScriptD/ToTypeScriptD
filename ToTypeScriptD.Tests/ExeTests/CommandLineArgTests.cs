using ApprovalTests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ToTypeScriptD.Tests.ExeTests
{
    [Timeout(3000)]
    public class CommandLineArgTests : ExeTestBase
    {
        [Test]
        public void ExeShouldGenerateHelpOnEmptyInput()
        {
            var result = Execute("");

            Assert.IsEmpty(result.StdOut);
            Approvals.Verify(result.StdErr);
        }
    }
}
