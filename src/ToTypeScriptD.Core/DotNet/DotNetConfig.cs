
namespace ToTypeScriptD.Core.DotNet
{
    public class DotNetConfig : ConfigBase
    {
        public DotNetConfig()
            : base()
        {
            CamelCase = true;
        }

        public override bool CamelCase { get; set; }

        public override TypeWriters.ITypeWriterTypeSelector GetTypeWriterTypeSelector()
        {
            return new DotNet.DotNetTypeWriterTypeSelector();
        }
    }
}
