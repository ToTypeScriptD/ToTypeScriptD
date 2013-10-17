
using System;
namespace NeedAProjectName
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: lost of validation/cleanup of args :)
            var file = args[0];

            var x = NeedAProjectName.Render.FullAssembly(file);

            Console.WriteLine(x);
        }
    }


}
