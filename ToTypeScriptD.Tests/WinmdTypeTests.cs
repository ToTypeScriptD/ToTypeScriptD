using ApprovalTests;
using Mono.Cecil;
using Xunit;

namespace ToTypeScriptD.Tests
{

    public class WinmdTypeTests : WinmdTestBase
    {
        private TypeDefinition GetNativeType(string postfix)
        {
            return base.NativeAssembly.GetNativeType("ToTypeScriptD.Native." + postfix);
        }

        private TypeDefinition GetWinNativeType(string type)
        {
            return base.WindowsAssembly.GetNativeType(type);
        }

        [Fact]
        public void EnumType()
        {
            var result = GetNativeType("SampleEnum").ToTypeScript();
            Approvals.Verify(result);
        }

        [Fact]
        public void EnumTypeWithExplicitValues()
        {
            var result = GetNativeType("SampleEnumNumbered").ToTypeScript();
            Approvals.Verify(result);
        }

        [Fact]
        public void ClassWithEventHandler()
        {
            var result = GetNativeType("ClassWithEventHandler").ToTypeScript();
            Approvals.Verify(result);
        }

        [Fact]
        public void FullSampleAssembly()
        {
            var result = ToTypeScriptD.Render.FullAssembly(base.NativeAssembly.ComponentPath);
            Approvals.Verify(result);
        }

        [Fact]
        public void WindowsStorageClass()
        {
            var result = GetWinNativeType("Windows.Storage.StorageFile").ToTypeScript();
            Approvals.Verify(result);
        }
        [Fact]
        public void WindowsSystemUserProfileGlobalizationPreferencesClass()
        {
            var result = GetWinNativeType("Windows.System.UserProfile.GlobalizationPreferences").ToTypeScript();
            Approvals.Verify(result);
        }

        [Fact]
        public void IfEnumNamesAreAllCapsThenTheTranslateToAllLowerCase()
        {
            var result = GetWinNativeType("Windows.Foundation.Metadata.ThreadingModel")
                .ToTypeScript();
            Approvals.Verify(result);
        }

        [Fact]
        public void FullWindowsAssembly()
        {
            var file = @"C:\Windows\System32\WinMetadata\Windows.Foundation.winmd";
            var result = ToTypeScriptD.Render.FullAssembly(file);
            Approvals.Verify(result);
        }

        [Fact]
        public void AllWinmdFilesInWinMetadata()
        {

            var allFiles = System.IO.Directory.GetFiles(@"C:\Windows\System32\WinMetadata\", "*.winmd");

            var result = allFiles;
            Approvals.VerifyAll(result, "File: ");
            //.Verify();
        }

        // Some types in Windows.winmd are duplicated in Windows.Foundation.winmd (huh?)
        //[Fact]
        //public void DumpFullWindowsWinmd()
        //{
        //    var file = @"C:\Program Files (x86)\Windows Kits\8.0\References\CommonConfiguration\Neutral\Windows.winmd";
        //    var result = ToTypeScriptD.Render.FullAssembly(file);
        //    Approvals.Verify(result);
        //}

    }
}
