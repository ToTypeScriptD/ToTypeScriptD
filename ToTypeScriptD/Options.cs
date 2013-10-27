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

        //[Option('v', "verbose", DefaultValue = true, HelpText = "Prints all messages to standard output.")]
        //public bool Verbose { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
