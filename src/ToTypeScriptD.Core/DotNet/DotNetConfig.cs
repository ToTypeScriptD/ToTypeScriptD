
namespace ToTypeScriptD.Core.DotNet
{
    public class DotNetConfig : ConfigBase
    {
        public DotNetConfig()
            : base()
        {
            CamelBackCase = true;
        }

        public override bool CamelBackCase { get; set; }

        public override TypeWriters.ITypeWriterTypeSelector GetTypeWriterTypeSelector()
        {
            return new DotNet.DotNetTypeWriterTypeSelector();
        }
    }
}
