namespace ToTypeScriptD.Tests
{
    public static class ApprovalsExtensions
    {
        public static void DiffWith(this string expected, string actual)
        {
            if (expected != actual)
            {
                var expectedFile = System.IO.Path.GetTempPath() + "Expected.Approvals.Temp.txt";
                var actualFile = System.IO.Path.GetTempPath() + "Actual.Approvals.Temp.txt";

                System.IO.File.WriteAllText(expectedFile, expected);
                System.IO.File.WriteAllText(actualFile, actual);

                var reporter = ApprovalTests.Approvals.GetReporter();
                reporter.Report(expectedFile, actualFile);
                Xunit.Assert.Equal(expected, actual);
            }
        }

        public static void Verify(this string item)
        {
            item = item.StripHeaderGarbageromOutput();
            ApprovalTests.Approvals.Verify(item);
        }

        public static void Verify(this ToTypeScriptD.Tests.ExeTests.ExeProcessResult item)
        {
            item.ToString().Verify();
        }

        public static void Verify<T>(this T item)
        {
            ApprovalTests.Approvals.Verify(item);
        }

        public static void DumpAndVerify(this string path)
        {
            var errors = new StringBuilderTypeNotFoundErrorHandler();
            var typeCollection = new ToTypeScriptD.Core.TypeWriters.TypeCollection(new ToTypeScriptD.Core.WinMD.WinMDTypeWriterTypeSelector());
            var config = new ToTypeScriptD.Core.Config();
            var result = ToTypeScriptD.Render.FullAssembly(path, errors, typeCollection, string.Empty, config);
            ApprovalTests.Approvals.Verify(errors + result);
        }
    }
}
