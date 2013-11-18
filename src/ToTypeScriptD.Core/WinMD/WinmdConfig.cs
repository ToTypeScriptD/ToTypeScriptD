
namespace ToTypeScriptD.Core.WinMD
{
    public class WinmdConfig : ConfigBase
    {
        public override TypeWriters.ITypeWriterTypeSelector GetTypeWriterTypeSelector()
        {
            return new WinMD.WinMDTypeWriterTypeSelector();
        }

        public override bool CamelCase
        {
            get { return true; }
            set { throw new System.NotSupportedException(); }
        }
    }
}
