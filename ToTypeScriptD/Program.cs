using System;

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

                try
                {
                    skipPrintingHelp = ToTypeScriptD.Render.AllAssemblies(options.Files, options.IncludeSpecialTypeDefinitions, Console.Out, new ConsoleErrorTypeNotFoundErrorHandler());
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
