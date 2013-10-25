using ApprovalTests;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToTypeScriptD.Tests
{

    public class WinmdTypeTests : WinmdTestBase
    {
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
            var result = ToTypeScriptD.Render.FullAssembly(base.NativeComponentPath);
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
        public void IfEnumNamesAreAllCapsThenTheTranslateToAllLowerCase() {
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
    }
}
