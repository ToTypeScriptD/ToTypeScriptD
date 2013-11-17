using System;
using ToTypeScriptD.Core;

namespace ToTypeScriptD
{
    class Program
    {
        static void Main(string[] args)
        {
            Config config = null;

            var options = new Options();

            string verbInvoked = null;

            if (CommandLine.Parser.Default.ParseArgumentsStrict(args, options, (verb, subOptions) =>
            {
                verb = (verb ?? "").ToLowerInvariant();
                verbInvoked = verb;
                if (verb == "dotnet")
                {
                    var dotNetSubOptions = (DotNetSubOptions)subOptions;
                    config = new DotNetConfig
                    {
                        AssemblyPaths = dotNetSubOptions.Files,
                        CamelCase = dotNetSubOptions.CamelCase,
                        IncludeSpecialTypes = dotNetSubOptions.IncludeSpecialTypeDefinitions,
                        IndentationType = dotNetSubOptions.IndentationType,
                        RegexFilter = dotNetSubOptions.RegexFilter,
                        TypeNotFoundErrorHandler = new ConsoleErrorTypeNotFoundErrorHandler(),
                    };
                }
                else if (verb == "winmd")
                {
                    var winmdSubOptions = (WinmdSubOptions)subOptions;
                    config = new WinmdConfig
                    {
                        AssemblyPaths = winmdSubOptions.Files,
                        IncludeSpecialTypes = winmdSubOptions.IncludeSpecialTypeDefinitions,
                        IndentationType = winmdSubOptions.IndentationType,
                        RegexFilter = winmdSubOptions.RegexFilter,
                        TypeNotFoundErrorHandler = new ConsoleErrorTypeNotFoundErrorHandler(),
                    };
                }
            }))
            {
                bool skipPrintingHelp = false;

                try
                {
                    skipPrintingHelp = ToTypeScriptD.Render.AllAssemblies(config, Console.Out);
                }
                catch (Exception ex)
                {
                    if (ex is System.IO.DirectoryNotFoundException || ex is System.IO.FileNotFoundException)
                    {
                        skipPrintingHelp = true;
                        Console.Error.WriteLine("Error: " + ex.Message);
                    }
                    else
                    {
                        throw;
                    }
                }

                if (!skipPrintingHelp)
                {
                    Console.WriteLine(options.GetUsage(verbInvoked));
                    Environment.ExitCode = 1;
                }
            }
        }
    }
}
