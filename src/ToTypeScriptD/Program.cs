using System;
using ToTypeScriptD.Core;

namespace ToTypeScriptD
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (CommandLine.Parser.Default.ParseArgumentsStrict(args, options))
            {
                bool skipPrintingHelp = false;
                var config = new Config
                {
                    OutputType = options.OutputType,
                    AssemblyPaths = options.Files,
                    IncludeSpecialTypes = options.IncludeSpecialTypeDefinitions,
                    TypeNotFoundErrorHandler = new ConsoleErrorTypeNotFoundErrorHandler(),
                    RegexFilter = options.RegexFilter,
                };

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
                    Console.WriteLine(options.GetUsage());
                    Environment.ExitCode = 1;
                }
            }
        }
    }
}
