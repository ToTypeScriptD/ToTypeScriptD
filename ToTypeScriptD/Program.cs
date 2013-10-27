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
                bool wroteAnyTypes = false;
                if (options.IncludeSpecialTypeDefinitions)
                {
                    wroteAnyTypes = true;
                    WriteOutSpecialTypes();
                }

                ToTypeScriptD.Render.AllAssemblies(options.Files, Console.Out);

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

        private static void WriteOutSpecialTypes()
        {
            Console.WriteLine("");
            Console.WriteLine(Resources.ToTypeScriptDSpecialTypes_d);
            Console.WriteLine("");
            Console.WriteLine("");
        }
    }


}
