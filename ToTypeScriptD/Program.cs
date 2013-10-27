using System;
using System.Collections.Generic;
using System.Linq;

namespace ToTypeScriptD
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                bool wroteAnyTypes = ToTypeScriptD.Render.AllAssemblies(options.Files, options.IncludeSpecialTypeDefinitions, Console.Out);

                if (!wroteAnyTypes)
                {
                    Console.WriteLine(options.GetUsage());
                    Environment.ExitCode = 1;
                }
            }
            else
            {
                Console.WriteLine(options.GetUsage());
                Environment.ExitCode = 1;
            }
        }
    }


}
