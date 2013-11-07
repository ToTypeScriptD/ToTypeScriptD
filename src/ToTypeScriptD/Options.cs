using CommandLine;
using CommandLine.Text;
using System.Collections.Generic;
using ToTypeScriptD.Core;

namespace ToTypeScriptD
{
    public class Options
    {
        [ValueList(typeof(List<string>))]
        public IList<string> Files { get; set; }

        [Option('s', "specialTypes", HelpText = "Writes the ToTypeScriptD special types to standard out")]
        public bool IncludeSpecialTypeDefinitions { get; set; }

        [Option('o', "outputType", Required = true, HelpText = "[WinRT | DotNet] - What .d.ts format would you like? EX: -o WinRT")]
        public OutputType OutputType { get; set; }

        private string _regexFilter;
        [Option('r', "regexFilter", HelpText = "A .net regular expression that can be used to filter the FullName of types exported. Picture this taking the FullName of the TypeScript type and running it through the .Net Regex.IsMatch(name, pattern)")]
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
