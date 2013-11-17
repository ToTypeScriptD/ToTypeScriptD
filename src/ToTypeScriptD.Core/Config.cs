using System.Collections.Generic;

namespace ToTypeScriptD.Core
{

    public enum OutputType
    {
        WinRT,
        DotNet,
    }


    public enum IndentationFormatting
    {
        None,
        TabX1,
        TabX2,
        SpaceX1,
        SpaceX2,
        SpaceX3,
        SpaceX4, // Default
        SpaceX5,
        SpaceX6,
        SpaceX7,
        SpaceX8,
    }

    public class Config
    {
        public Config()
        {
            this.IndentationType = IndentationFormatting.SpaceX4;
        }

        public OutputType OutputType { get; set; }

        public bool IncludeSpecialTypes { get; set; }

        private IEnumerable<string> _assemblyPaths = new List<string>();
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

        public IndentationFormatting IndentationType { get; set; }
        public string Indent
        {
            get
            {
                switch (IndentationType)
                {
                    case IndentationFormatting.None: return "";
                    case IndentationFormatting.TabX1: return "\t";
                    case IndentationFormatting.TabX2: return "\t\t";
                    case IndentationFormatting.SpaceX1: return " ";
                    case IndentationFormatting.SpaceX2: return "  ";
                    case IndentationFormatting.SpaceX3: return "   ";
                    case IndentationFormatting.SpaceX4: return "    ";
                    case IndentationFormatting.SpaceX5: return "     ";
                    case IndentationFormatting.SpaceX6: return "      ";
                    case IndentationFormatting.SpaceX7: return "       ";
                    case IndentationFormatting.SpaceX8: return "        ";
                    default:
                        return "    ";
                }
            }
        }


        public TypeWriters.ITypeWriterTypeSelector GetTypeWriterTypeSelector()
        {
            if (this.OutputType == OutputType.DotNet)
            {
                return new DotNet.DotNetTypeWriterTypeSelector();
            }
            return new WinMD.WinMDTypeWriterTypeSelector();
        }
    }

}
