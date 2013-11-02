using CommandLine;
using CommandLine.Text;
using System.Collections.Generic;

namespace ToTypeScriptD
{
    public class Options
    {
        [ValueList(typeof(List<string>))]
        public IList<string> Files { get; set; }

        [Option('s', "specialTypes", HelpText = "Writes the ToTypeScriptD special types to standard out")]
        public bool IncludeSpecialTypeDefinitions { get; set; }

        private string _regexFilter;
        [Option('r', "regexFilter", HelpText = "A .net regular expression that can be used to filter the FullName of types exported.")]
        public string RegexFilter
        {
            get
            {
                return _regexFilter;
            }
            set
            {
                var v = value ?? "";
                if (v.StartsWith("'") && v.EndsWith("'"))
                {
                    v = v.Substring(1, v.Length - 2);
                }
                if (v.StartsWith("\"") && v.EndsWith("\""))
                {
                    v = v.Substring(1, v.Length - 2);
                }

                _regexFilter = v;
            }
        }

        //[Option('v', "verbose", DefaultValue = true, HelpText = "Prints all messages to standard output.")]
        //public bool Verbose { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current =>
            {

                current.AddPreOptionsLine(" "); // blank line
                current.AddPreOptionsLine("Usage: TypeScriptD.exe [--specialTypes] [<file1.winmd> ...<fileN.winmd>]");

                HelpText.DefaultParsingErrorsHandler(this, current);
            });
        }
    }
}
