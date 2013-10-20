using System;
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
                if (options.Files.Any())
                {
                    // Values are available here
                    //if (options.Verbose) Console.WriteLine("Filename: {0}", options.InputFile);

                    options.Files.Each(file =>
                    {
                        var x = ToTypeScriptD.Render.FullAssembly(file);
                        Console.WriteLine("");
                        Console.WriteLine(x);
                    });
                }
                else
                {
                    Console.Error.WriteLine(options.GetUsage());
                }
            }
            else
            {
                Console.Error.WriteLine(options.GetUsage());
            }
        }
    }


}
