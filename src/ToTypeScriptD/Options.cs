using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using ToTypeScriptD.Core;
using ToTypeScriptD.Core.Config;

namespace ToTypeScriptD
{
    public class Options
    {
        public const string DotNetCommandName = "dotnet";
        public const string WinmdCommandName = "winmd";

        [VerbOption(Options.DotNetCommandName, HelpText = "Generate .d.ts based on .Net conventions")]
        public DotNetSubOptions DotNet { get; set; }

        [VerbOption(Options.WinmdCommandName, HelpText = "Generate .d.ts based on WinJS/WinMD conventions")]
        public WinmdSubOptions WinMD { get; set; }

        [HelpVerbOption]
        public string GetUsage(string verb)
        {
            var help = new HelpText
            {
                Heading = HeadingInfo.Default,
                Copyright = CopyrightInfo.Default,
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };

            object optionsObject = null;
            if (verb == DotNetCommandName)
            {
                help.AddPreOptionsLine(Environment.NewLine + "Usage: ToTypeScriptD dotnet [--specialTypes] [File1.dll]...[FileN.dll]");
                optionsObject = new DotNetSubOptions();
            }
            else if (verb == WinmdCommandName)
            {
                help.AddPreOptionsLine(Environment.NewLine + "Usage: ToTypeScriptD winmd [--specialTypes] [File1.winmd]...[FileN.winmd]");
                optionsObject = new WinmdSubOptions();
            }

            if (optionsObject != null)
            {
                help.AddOptions(optionsObject); ;
            }
            else
            {
                help.AddDashesToOption = false;
                help.AddOptions(this);
            }

            return help;
        }
    }

    public class BaseSubOption
    {
        public BaseSubOption()
        {
            IndentationType = IndentationFormatting.SpaceX4;
        }

        [ValueList(typeof(List<string>))]
        public IList<string> Files { get; set; }

        [Option('s', "specialTypes", HelpText = "Writes the ToTypeScriptD special types to standard out")]
        public bool IncludeSpecialTypeDefinitions { get; set; }

        [Option('i', "indentWith", HelpText = "Override default indentation of SpaceX4 (four spaces). Possible options: [None, TabX1, TabX2, SpaceX1,...SpaceX8]")]
        public IndentationFormatting IndentationType { get; set; }

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
    }

    public class DotNetSubOptions : BaseSubOption
    {
        [Option('p', "camelCase", DefaultValue = true, HelpText = "Changes properties from .Net name to camel cased names. EX: (Foo: string) becomes (foo: string)")]
        public bool CamelCase { get; set; }
    }

    public class WinmdSubOptions:  BaseSubOption
    {
    }

}
