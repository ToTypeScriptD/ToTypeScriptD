using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToTypeScriptD.Core.TypeWriters
{
    public class TypeCollection
    {
        Dictionary<string, ITypeWriter> types = new Dictionary<string, ITypeWriter>();

        public bool Contains(string name)
        {
            return types.ContainsKey(name);
        }

        public void Add(string @namespace, string name, ITypeWriter typeWriterBase)
        {
            if (name == "<Module>")
                return;

            if (name.StartsWith("__I") && name.EndsWith("PublicNonVirtuals"))
                return;

            if (name.StartsWith("__I") && name.EndsWith("ProtectedNonVirtuals"))
                return;

            var fullname = @namespace + "." + name;

            if (!types.ContainsKey(fullname))
            {
                types.Add(fullname, typeWriterBase);
            }
        }

        public string Render()
        {
            Func<string, string> getNamespace = name =>
            {
                return name.Substring(0, name.LastIndexOf('.'));
            };
            var items = from t in types
                        orderby t.Key
                        group t by getNamespace(t.Key) into namespaces
                        select namespaces;

            var sb = new StringBuilder();
            var Indent = TypeWriterConfig.Instance.Indentation;
            foreach (var ns in items)
            {
                sb.AppendFormat("declare module {0} {{", ns.Key);
                sb.AppendLine();
                sb.AppendLine();

                foreach (var type in ns)
                {
                    type.Value.Write(sb);
                    sb.AppendLine();
                }

                sb.AppendLine("}");
            }
            return sb.ToString();
        }
    }

}
