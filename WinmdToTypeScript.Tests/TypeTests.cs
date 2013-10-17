using ApprovalTests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinmdToTypeScript.Tests
{

    public class TypeTests : TestBase
    {
        [Test]
        public void EnumType()
        {
            var result = GetNativeType("SampleEnum").ToTypeScript();
            Approvals.Verify(result);
        }

        [Test]
        public void EnumTypeWithExplicitValues()
        {
            var result = GetNativeType("SampleEnumNumbered").ToTypeScript();
            Approvals.Verify(result);
        }

        [Test]
        public void ClassWithEventHandler()
        {
            var result = GetNativeType("ClassWithEventHandler").ToTypeScript();
            Approvals.Verify(result);
        }

        [Test]
        public void FullSampleAssembly()
        {
            var result = WinmdToTypeScript.Render.FullAssembly(base.NativeComponentPath);
            Approvals.Verify(result);
        }

        [Test]
        public void WindowsStorageClass()
        {
            var result = GetWinNativeType("Windows.Storage.StorageFile").ToTypeScript();
            Approvals.Verify(result);
        }
        [Test]
        public void WindowsSystemUserProfileGlobalizationPreferencesClass()
        {
            var result = GetWinNativeType("Windows.System.UserProfile.GlobalizationPreferences").ToTypeScript();
            Approvals.Verify(result);
        }
        
        //[Test]
        //public void FullWindowsAssembly()
        //{
        //    var file = @"C:\Windows\System32\WinMetadata\Windows.Foundation.winmd";
        //    var result = WinmdToTypeScript.Render.FullAssembly(file);
        //    Approvals.Verify(result);
        //}
    }
}
