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

                var filesAlreadyProcessed = new HashSet<string>(new IgnoreCaseStringEqualityComparer());
                if (options.Files.Any())
                {
                    wroteAnyTypes = true;
                    // Values are available here
                    //if (options.Verbose) Console.WriteLine("Filename: {0}", options.InputFile);

                    options.Files.Each(file =>
                    {
                        if (filesAlreadyProcessed.Contains(file))
                            return;

                        filesAlreadyProcessed.Add(file);
                        var x = ToTypeScriptD.Render.FullAssembly(file);
                        Console.WriteLine("");
                        Console.WriteLine(x);
                    });
                }
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


    public class IgnoreCaseStringEqualityComparer : EqualityComparer<string>
    {

        public override bool Equals(string x, string y)
        {
            if (x == null && y == null) return true;
            if (x == null) return false;
            return x.Equals(y);
        }

        public override int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}
