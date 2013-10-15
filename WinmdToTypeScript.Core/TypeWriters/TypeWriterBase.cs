using Mono.Cecil;
using System;
using System.Text;

namespace WinmdToTypeScript.Core.TypeWriters
{
    public abstract class TypeWriterBase //: ITypeWriter
    {
        public TypeDefinition TypeDefinition { get; set; }
        public int IndentCount { get; set; }
        public TypeCollection TypeCollection { get; set; }

        public static TypeWriterConfig config;
        public static TypeWriterConfig Config
        {
            get { return config ?? TypeWriterConfig.Instance; }
            set { config = value; }
        }

        public TypeWriterBase(TypeDefinition typeDefinition, int indentCount, TypeCollection typeCollection)
        {
            this.TypeDefinition = typeDefinition;
            this.IndentCount = indentCount;
            this.TypeCollection = typeCollection;
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
