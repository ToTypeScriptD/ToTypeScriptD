using ApprovalTests;
using System.IO;
using Xunit;

namespace ToTypeScriptD.Tests
{
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
                Assert.Equal(expected, actual);
            }
        }
    }
}
