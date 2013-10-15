using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinmdToTypeScript.Core.TypeWriters
{
    public class ClassWriter : TypeWriterBase
    {
        public ClassWriter(Mono.Cecil.TypeDefinition typeDefinition, int indentCount, TypeCollection typeCollection)
            : base(typeDefinition, indentCount, typeCollection)
        {
        }

        public override void Write(System.Text.StringBuilder sb)
        {
            ++IndentCount;
            sb.AppendLine(Indent + "export class " + TypeDefinition.Name + " {");

            // TODO: get specific types of EventListener types?
            if (TypeDefinition.HasEvents)
            {
                step(sb); step(sb); sb.AppendLine("addEventListener(type: string, listener: EventListener): void;");
                step(sb); step(sb); sb.AppendLine("removeEventListener(type: string, listener: EventListener): void;");

                TypeDefinition.Events.For((item, i, isLast) =>
                {
                    // TODO: events with multiple return types???
                    step(sb); step(sb); sb.AppendLine("on" + item.Name.ToLower() + "(ev: any);");
                });
            }

            var propNames = new HashSet<string>();
            TypeDefinition.Properties.Each(prop =>
            {
                var propName = prop.Name.ToTypeScriptName();
                propNames.Add(propName);
                step(sb); step(sb); sb.AppendFormat("{0}: {1};", propName, prop.PropertyType.ToTypeScriptType());
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

                step(sb); step(sb); sb.Append(methodName);

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
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
