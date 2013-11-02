using System.Text;

namespace ToTypeScriptD.Core.TypeWriters
{
    public interface ITypeWriter
    {
        void Write(StringBuilder sb);

        string TypeName { get; }
    }
}
