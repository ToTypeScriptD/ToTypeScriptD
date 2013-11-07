
namespace ToTypeScriptD.Core
{
    public class Config
    {
        public OutputType OutputType { get; set; }

        internal TypeWriters.ITypeWriterTypeSelector GetTypeWriterTypeSelector()
        {
            if (this.OutputType == OutputType.DotNet)
            {
                return new DotNet.DotNetTypeWriterTypeSelector();
            }
            return new WinRT.WinRTTypeWriterTypeSelector();
        }
    }

    public enum OutputType
    {
        WinRT,
        DotNet,
    }
}
