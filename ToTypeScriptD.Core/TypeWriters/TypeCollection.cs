using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToTypeScriptD.Core.TypeWriters
{
    public class TypeCollection
    {
        Dictionary<string, ITypeWriter> types = new Dictionary<string, ITypeWriter>();
        HashSet<string> typesRendered = new HashSet<string>();
        HashSet<AssemblyDefinition> assemblies = new HashSet<AssemblyDefinition>();

        public bool Contains(string name)
        {
            return types.ContainsKey(name);
        }

        public void Add(string @namespace, string name, ITypeWriter typeWriterBase)
        {
            if (name.ShouldIgnoreTypeByName())
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
                        where !typesRendered.Contains(t.Key)
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
                    typesRendered.Add(type.Key);
                    type.Value.Write(sb);
                    sb.AppendLine();
                }

                sb.AppendLine("}");
            }
            return sb.ToString();
        }

        internal void AddAssembly(Mono.Cecil.AssemblyDefinition assembly)
        {
            assemblies.Add(assembly);
        }

        internal TypeDefinition LookupType(TypeReference item)
        {
            string lookupName = item.FullName;
            if (item.IsGenericInstance)
            {
                lookupName = item.GetElementType().FullName;
            }
            var foundType = item.Module.Types.SingleOrDefault(w => w.FullName == lookupName);
            return foundType;
        }
    }

}
