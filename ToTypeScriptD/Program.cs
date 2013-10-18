
using System;
namespace ToTypeScriptD
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: lost of validation/cleanup of args :)
            var file = args[0];

            var x = ToTypeScriptD.Render.FullAssembly(file);

            Console.WriteLine(x);
        }
    }


}
