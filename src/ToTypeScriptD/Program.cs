using System;
using ToTypeScriptD.Core;

namespace ToTypeScriptD
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigBase config = null;

            var options = new Options();

            string verbInvoked = null;

            if (CommandLine.Parser.Default.ParseArgumentsStrict(args, options, (verb, subOptions) =>
            {
                verb = (verb ?? "").ToLowerInvariant();
                verbInvoked = verb;
                if (verb == Options.DotNetCommandName)
                {
                    var dotNetSubOptions = subOptions as DotNetSubOptions;
                    if (dotNetSubOptions != null)
                    {
                        config = new ToTypeScriptD.Core.DotNet.DotNetConfig
                        {
                            AssemblyPaths = dotNetSubOptions.Files,
                            CamelCase = dotNetSubOptions.CamelCase,
                            IncludeSpecialTypes = dotNetSubOptions.IncludeSpecialTypeDefinitions,
                            IndentationType = dotNetSubOptions.IndentationType,
                            RegexFilter = dotNetSubOptions.RegexFilter,
                            TypeNotFoundErrorHandler = new ConsoleErrorTypeNotFoundErrorHandler(),
                        };
                    }
                }
                else if (verb == Options.WinmdCommandName)
                {
                    var winmdSubOptions = subOptions as WinmdSubOptions;
                    if (winmdSubOptions != null)
                    {
                        config = new ToTypeScriptD.Core.WinMD.WinmdConfig
                        {
                            AssemblyPaths = winmdSubOptions.Files,
                            IncludeSpecialTypes = winmdSubOptions.IncludeSpecialTypeDefinitions,
                            IndentationType = winmdSubOptions.IndentationType,
                            RegexFilter = winmdSubOptions.RegexFilter,
                            TypeNotFoundErrorHandler = new ConsoleErrorTypeNotFoundErrorHandler(),
                        };
                    }
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
