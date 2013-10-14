
using System;
namespace WinmdToTypeScript
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: lost of validation/cleanup of args :)
            var file = args[0];

            var x = WinmdToTypeScript.Render.FullAssembly(file);

            Console.WriteLine(x);
        }
    }


}
