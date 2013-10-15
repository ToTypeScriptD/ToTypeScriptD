using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinmdToTypeScript.Core.TypeWriters
{
    public class InterfaceWriter: ITypeWriter
    {

        public InterfaceWriter(Mono.Cecil.TypeReference typeDefinition, int indentCount, TypeCollection typeCollection)
        {
            this.TypeDefinition = typeDefinition;
        }

        public void Write(StringBuilder sb)
        {
            var Indent = TypeWriterConfig.Instance.Indentation; // TODO: fix me
            sb.AppendLine(Indent + "interface " + TypeDefinition.Name + " {");

            //var propNames = new HashSet<string>();
            //TypeDefinition.Properties.Each(prop =>
            //{
            //    var propName = prop.Name.ToTypeScriptName();
            //    propNames.Add(propName);
            //    step(sb); step(sb); sb.AppendFormat("{0}: {1};", propName, prop.PropertyType.ToTypeScriptType());
            //    sb.AppendLine();
            //});

            //foreach (var method in TypeDefinition.Methods)
            //{
            //    var methodName = method.Name;

            //    // ignore special event handler methods
            //    if (method.HasParameters &&
            //        method.Parameters[0].Name.StartsWith("__param0") &&
            //        (methodName.StartsWith("add_") || methodName.StartsWith("remove_")))
            //        continue;

            //    // already handled properties
            //    if (method.IsGetter || method.IsSetter)
            //        continue;

            //    // translate the constructor function
            //    if (method.IsConstructor)
            //    {
            //        methodName = "constructor";
            //    }

            //    // Lowercase first char of the method
            //    methodName = methodName.ToTypeScriptName();

            //    step(sb); step(sb); sb.Append(methodName);

            //    sb.Append("(");
            //    method.Parameters.For((parameter, i, isLast) =>
            //    {
            //        sb.Append(parameter.Name);
            //        sb.Append(": ");
            //        sb.Append(parameter.ParameterType.ToTypeScriptType());
            //        if (isLast) sb.Append(", ");
            //    });
            //    sb.Append(")");

            //    // constructors don't have return types.
            //    if (!method.IsConstructor)
            //    {
            //        sb.AppendFormat(": {0}", method.ReturnType.ToTypeScriptType());
            //    }
            //    sb.AppendLine(";");
            //}

            sb.AppendLine(Indent + "}");
        }

        public Mono.Cecil.TypeReference TypeDefinition { get; set; }
    }
}
