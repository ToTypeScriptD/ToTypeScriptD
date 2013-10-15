using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinmdToTypeScript.Core.TypeWriters
{
    public class ClassWriter : TypeWriterBase
    {
        private TypeDefinition typeDefinition;
        private int indentCount;

        public ClassWriter(Mono.Cecil.TypeDefinition typeDefinition, int indentCount, TypeCollection typeCollection)
            : base(typeDefinition, indentCount, typeCollection)
        {
            this.typeDefinition = typeDefinition;
            this.indentCount = indentCount;
        }

        public override void Write(System.Text.StringBuilder sb)
        {
            Action step = () => { sb.Append(Indent); };
            ++IndentCount;
            sb.AppendLine(Indent + "export class " + TypeDefinition.Name + " {");

            // TODO: get specific types of EventListener types?
            if (TypeDefinition.HasEvents)
            {
                step(); step(); sb.AppendLine("addEventListener(type: string, listener: EventListener): void;");
                step(); step(); sb.AppendLine("removeEventListener(type: string, listener: EventListener): void;");

                TypeDefinition.Events.For((item, i, isLast) =>
                {
                    // TODO: events with multiple return types???
                    step(); step(); sb.AppendLine("on" + item.Name.ToLower() + "(ev: any);");
                });
            }

            var propNames = new HashSet<string>();
            TypeDefinition.Properties.Each(prop =>
            {
                var propName = prop.Name.ToTypeScriptName();
                propNames.Add(propName);
                step(); step(); sb.AppendFormat("{0}: {1};", propName, prop.PropertyType.ToTypeScriptType());
                sb.AppendLine();
            });

            foreach (var method in TypeDefinition.Methods)
            {
                var methodName = method.Name;

                // ignore special event handler methods
                if (method.HasParameters &&
                    method.Parameters[0].Name.StartsWith("__param0") &&
                    (methodName.StartsWith("add_") || methodName.StartsWith("remove_")))
                    continue;

                // already handled properties
                if (method.IsGetter || method.IsSetter)
                    continue;

                // translate the constructor function
                if (method.IsConstructor)
                {
                    methodName = "constructor";
                }

                // Lowercase first char of the method
                methodName = methodName.ToTypeScriptName();

                step(); step(); sb.Append(methodName);

                sb.Append("(");
                method.Parameters.For((parameter, i, isLast) =>
                {
                    sb.Append(parameter.Name);
                    sb.Append(": ");
                    sb.Append(parameter.ParameterType.ToTypeScriptType());
                    if (isLast) sb.Append(", ");
                });
                sb.Append(")");

                // constructors don't have return types.
                if (!method.IsConstructor)
                {
                    sb.AppendFormat(": {0}", method.ReturnType.ToTypeScriptType());
                }
                sb.AppendLine(";");
            }

            sb.AppendLine(Indent + "}");
        }
    }

    public class TypeCollection : IEnumerable<string>
    {
        Dictionary<string, string> types = new Dictionary<string, string>();

        public bool Contains(string name)
        {
            return types.ContainsKey(name);
        }

        public void Add(string name, string body)
        {
            if (types.ContainsKey(name))
            {
                if (types[name] != body)
                {
                    throw new ArgumentException(
                        "Duplicate type [{0}] with different bodies {1}{2}{2}{3}"
                        .FormatWith(name, Environment.NewLine, body, types[name]));
                }
            }
            else
            {
                types.Add(name, body);
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return this.types.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
                    sb.AppendLine(type.Value);
                }

                sb.AppendLine("}");
            }
            return sb.ToString();
        }
    }
}
