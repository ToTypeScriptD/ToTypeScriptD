using System;
using System.Text;
using ToTypeScriptD.Core.TypeWriters;

namespace ToTypeScriptD
{
    public class StringBuilderTypeNotFoundErrorHandler : ITypeNotFoundErrorHandler
    {
        private StringBuilder sb = new StringBuilder();
        public void Handle(Mono.Cecil.TypeReference typeReference)
        {
            sb.AppendLine("Hmm, don't seem to have this type lying around: " + typeReference.FullName);
        }

        public override string ToString()
        {
            return sb.ToString();
        }
    }

    public class ConsoleErrorTypeNotFoundErrorHandler : ITypeNotFoundErrorHandler
    {
        public void Handle(Mono.Cecil.TypeReference typeReference)
        {
            Console.Error.WriteLine("Warning: Don't seem to have this type lying around: " + typeReference.FullName);
        }
    }
}
