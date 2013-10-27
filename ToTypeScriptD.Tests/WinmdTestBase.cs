
namespace ToTypeScriptD.Tests
{
    public class WinmdTestBase
    {
        public TestAssembly NativeAssembly = new TestAssembly(@"ToTypeScriptD.Native.winmd");
        public TestAssembly WindowsAssembly = new TestAssembly(@"C:\Program Files (x86)\Windows Kits\8.0\References\CommonConfiguration\Neutral\Windows.winmd");
    }
}
