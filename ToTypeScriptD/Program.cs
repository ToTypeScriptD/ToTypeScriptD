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
                var filesAlreadyProcessed = new HashSet<string>(new IgnoreCaseStringEqualityComparer());
                if (options.Files.Any())
                {
                    // Values are available here
                    //if (options.Verbose) Console.WriteLine("Filename: {0}", options.InputFile);

                    options.Files.Each(file =>
                    {
                        if (filesAlreadyProcessed.Contains(file))
                            return;

                        filesAlreadyProcessed.Add(file);
                        var x = ToTypeScriptD.Render.FullAssembly(file);
                        System.Diagnostics.Debug.WriteLine(x);
                        Console.WriteLine("");
                        Console.WriteLine(x);
                    });
                }
                else
                {
                    Console.Error.WriteLine(options.GetUsage());
                    Environment.Exit(1);
                }
            }
            else
            {
                Console.Error.WriteLine(options.GetUsage());
                Environment.Exit(1);
            }
        }
    }


    public class IgnoreCaseStringEqualityComparer: EqualityComparer<string>
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
