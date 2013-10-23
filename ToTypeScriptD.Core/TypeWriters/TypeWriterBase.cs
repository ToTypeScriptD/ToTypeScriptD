using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToTypeScriptD.Core.TypeWriters
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


        public string IndentValue
        {
            get { return Config.Indentation.Dup(IndentCount); }
        }

        public void Indent(StringBuilder sb)
        {
            sb.Append(IndentValue);
        }

        internal void WriteOutMethodSignatures(StringBuilder sb, string exportType, string inheriterString)
        {
            Indent(sb); sb.AppendFormat("export {0} {1}", exportType, TypeDefinition.Name.StripGenericTick());

            if (TypeDefinition.HasGenericParameters)
            {
                sb.Append("<");
                TypeDefinition.GenericParameters.For((genericParameter, i, isLastItem) =>
                {
                    sb.AppendFormat("{0}{1}", genericParameter.ToTypeScriptType(), isLastItem ? "" : ",");
                });
                sb.Append(">");
            }

            sb.Append(" ");
            if (TypeDefinition.Interfaces.Any())
            {
                var interfaceTypes = TypeDefinition.Interfaces.Where(w => !w.Name.ShouldIgnoreTypeByName());
                if (interfaceTypes.Any())
                {
                    sb.Append(inheriterString);
                    interfaceTypes.For((item, i, isLast) =>
                    {
                        sb.AppendFormat(" {0}{1}", item.ToTypeScriptType(), isLast ? " " : ",");
                    });
                }
            }
            sb.AppendLine("{");

            // TODO: get specific types of EventListener types?
            if (TypeDefinition.HasEvents)
            {
                Indent(sb); Indent(sb); sb.AppendLine("addEventListener(type: string, listener: EventListener): void;");
                Indent(sb); Indent(sb); sb.AppendLine("removeEventListener(type: string, listener: EventListener): void;");

                TypeDefinition.Events.For((item, i, isLast) =>
                {
                    // TODO: events with multiple return types???
                    Indent(sb); Indent(sb); sb.AppendLine("on" + item.Name.ToLower() + "(ev: any);");
                });
            }

            TypeDefinition.Fields.Each(field =>
            {
                var fieldName = field.Name.ToTypeScriptName();
                Indent(sb); Indent(sb); sb.AppendFormat("{0}: {1};", fieldName, field.FieldType.ToTypeScriptType());
                sb.AppendLine();
            });

            var propNames = new HashSet<string>();
            TypeDefinition.Properties.Each(prop =>
            {
                // TODO: determine if property was already defined by interface?

                var propName = prop.Name.ToTypeScriptName();
                propNames.Add(propName);
                Indent(sb); Indent(sb); sb.AppendFormat("{0}: {1};", propName, prop.PropertyType.ToTypeScriptType());
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

                Indent(sb); Indent(sb);
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
