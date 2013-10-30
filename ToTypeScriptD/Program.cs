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
                bool wroteAnyTypes = ToTypeScriptD.Render.AllAssemblies(options.Files, options.IncludeSpecialTypeDefinitions, Console.Out, new ConsoleErrorTypeNotFoundErrorHandler());

                if (!wroteAnyTypes)
                {
                    Console.WriteLine(options.GetUsage());
                    Environment.ExitCode = 1;
                }
            }
        }
    }
}
