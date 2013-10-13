using Mono.Cecil;
using System;
using System.Text;

namespace WinmdToTypeScript.TypeWriters
{
    public abstract class TypeWriterBase //: ITypeWriter
    {
        public TypeDefinition TypeDefinition { get; set; }
        public int IndentCount { get; set; }

        public static TypeWriterConfig config;
        public static TypeWriterConfig Config
        {
            get { return config ?? TypeWriterConfig.Instance; }
            set { config = value; }
        }

        public TypeWriterBase(TypeDefinition typeDefinition, int indentCount)
        {
            this.TypeDefinition = typeDefinition;
            this.IndentCount = indentCount;
        }

        public virtual void Write(StringBuilder sb, Action midWrite)
        {
            midWrite();
        }
        public virtual void Write(StringBuilder sb)
        {
            //noop
        }


        public string Indent
        {
            get { return Config.Indentation.Dup(IndentCount); }
        }
    }
}
