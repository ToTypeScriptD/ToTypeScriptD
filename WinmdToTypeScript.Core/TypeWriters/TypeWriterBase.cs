using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinmdToTypeScript.Core.TypeWriters
{
    public abstract class TypeWriterBase : ITypeWriter
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

        public void step(StringBuilder sb)
        {
            sb.Append(Indent);
        }

        internal void WriteOutMethodSignatures(StringBuilder sb, string exportType, string inheriterString)
        {
            step(sb); sb.AppendFormat("export {0} {1}", exportType, TypeDefinition.Name.StripGenericTick());

            if (TypeDefinition.HasGenericParameters)
            {
                sb.Append("<");
                TypeDefinition.GenericParameters.For((genericParameter, i, isLastItem) =>
                {
                    sb.AppendFormat("{0}{1}", genericParameter.Name, isLastItem ? "" : ",");
                });
                sb.Append(">");
            }

            sb.Append(" ");
            if (TypeDefinition.Interfaces.Any())
            {
                sb.Append(inheriterString);
                TypeDefinition.Interfaces.For((item, i, isLast) =>
                {
                    sb.AppendFormat(" {0}{1}", item.FullName.StripGenericTick(), isLast ? " " : ",");
                });
            }
            sb.AppendLine("{");

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
                // TODO: determine if property was already defined by interface?

                var propName = prop.Name.ToTypeScriptName();
                propNames.Add(propName);
                step(sb); step(sb); sb.AppendFormat("{0}: {1};", propName, prop.PropertyType.ToTypeScriptType());
                sb.AppendLine();
            });

            foreach (var method in TypeDefinition.Methods)
            {
                // TODO: determine if method was already defined by interface?

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

                step(sb); step(sb);
                if (method.IsStatic)
                {
                    sb.Append("static ");
                }
                sb.Append(methodName);

                sb.Append("(");
                method.Parameters.For((parameter, i, isLast) =>
                {
                    sb.Append(i == 0 ? "" : " ");
                    sb.Append(parameter.Name);
                    sb.Append(": ");
                    sb.Append(parameter.ParameterType.ToTypeScriptType());
                    sb.Append(isLast ? "" : ",");
                });
                sb.Append(")");

                // constructors don't have return types.
                if (!method.IsConstructor)
                {
                    sb.AppendFormat(": {0}", method.ReturnType.ToTypeScriptType());
                }
                sb.AppendLine(";");
            }
        }
    }
}
