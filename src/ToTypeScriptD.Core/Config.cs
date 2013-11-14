using System.Collections.Generic;

namespace ToTypeScriptD.Core
{

    public enum OutputType
    {
        WinRT,
        DotNet,
    }

    public class Config
    {
        private IEnumerable<string> _assemblyPaths = new List<string>();

        public OutputType OutputType { get; set; }

        public bool IncludeSpecialTypes { get; set; }

        public IEnumerable<string> AssemblyPaths
        {
            get
            {
                return _assemblyPaths ?? new string[0];
            }
            set
            {
                _assemblyPaths = value ?? new string[0];
            }
        }

        internal TypeWriters.ITypeWriterTypeSelector GetTypeWriterTypeSelector()
        {
            if (this.OutputType == OutputType.DotNet)
            {
                return new DotNet.DotNetTypeWriterTypeSelector();
            }
            return new WinMD.WinMDTypeWriterTypeSelector();
        }

        private TypeWriters.ITypeNotFoundErrorHandler _typeNotfoundErrorHandler;
        public TypeWriters.ITypeNotFoundErrorHandler TypeNotFoundErrorHandler
        {
            get
            {
                return _typeNotfoundErrorHandler ?? (_typeNotfoundErrorHandler = new ConsoleErrorTypeNotFoundErrorHandler());
            }
            set
            {
                _typeNotfoundErrorHandler = value;
            }
        }

        private string _regexFilter = "";
        public string RegexFilter
        {
            get { return _regexFilter ?? ""; }
            set { _regexFilter = value ?? ""; }
        }
    }

}
