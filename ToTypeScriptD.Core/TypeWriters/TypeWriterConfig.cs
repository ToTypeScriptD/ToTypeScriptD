
namespace ToTypeScriptD.Core.TypeWriters
{
    public class TypeWriterConfig
    {
        public TypeWriterConfig()
        {
            Indentation = "    ";
        }

        public string Indentation { get; set; }

        static TypeWriterConfig()
        {
            Instance = new TypeWriterConfig();
        }
        public static TypeWriterConfig Instance { get; set; }
    }
}
