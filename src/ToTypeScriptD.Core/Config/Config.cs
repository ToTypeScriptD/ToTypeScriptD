using System.Collections.Generic;
using ToTypeScriptD.Core.Config;

namespace ToTypeScriptD.Core
{

    public abstract class ConfigBase
    {
        public ConfigBase()
        {
            this.IndentationType = IndentationFormatting.SpaceX4;
        }

        public abstract bool CamelCase { get; set; }
        public abstract TypeWriters.ITypeWriterTypeSelector GetTypeWriterTypeSelector();


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
    }
}
