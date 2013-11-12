using Mono.Cecil;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToTypeScriptD.Core.TypeWriters;

namespace ToTypeScriptD.Core.WinMD
{
    public class DelegateWriter : TypeWriterBase
    {
        public DelegateWriter(Mono.Cecil.TypeDefinition typeDefinition, int indentCount, TypeCollection typeCollection)
            : base(typeDefinition, indentCount, typeCollection)
        {
        }

        public override void Write(System.Text.StringBuilder sb)
        {
            ++IndentCount;
            Indent(sb); sb.AppendFormat("export interface {0}", TypeDefinition.ToTypeScriptItemName());

            if (TypeDefinition.HasGenericParameters)
            {
                sb.Append("<");
                TypeDefinition.GenericParameters.For((genericParam, i, isLastItem) =>
                {
                    sb.AppendFormat("{0}{1}", genericParam.ToTypeScriptType(), (isLastItem ? "" : ", "));
                });
                sb.Append(">");
            }

            sb.AppendLine(" {");
            IndentCount++;

            // 'ctor' is at index 0
            // 'invoke' is at index 1
            var invokeMethod = TypeDefinition.Methods[1];
            if (invokeMethod.Parameters.Any())
            {
                var target = invokeMethod.Parameters[0];
                Indent(sb); sb.AppendFormatLine("target: {0};", target.ParameterType.ToTypeScriptType());
            }
            else
            {
                Indent(sb); sb.AppendFormatLine("target: any;");
            }
            Indent(sb); sb.AppendFormatLine("detail: any[];");

            Indent(sb); sb.AppendLine("type: string;");
            IndentCount--;
            Indent(sb); sb.AppendLine("}");

        }

        private List<ITypeWriter> WriteMethods(StringBuilder sb)
        {
            List<ITypeWriter> extendedTypes = new List<ITypeWriter>();
            var methodSignatures = new HashSet<string>();
            foreach (var method in TypeDefinition.Methods)
            {
                var methodSb = new StringBuilder();
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
                    continue;
                }

                // Lowercase first char of the method
                methodName = methodName.ToTypeScriptName();

                Indent(methodSb); Indent(methodSb);
                if (method.IsStatic)
                {
                    methodSb.Append("static ");
                }
                methodSb.Append(methodName);

                var outTypes = new List<ParameterDefinition>();

                methodSb.Append("(");
                method.Parameters.Where(w => w.IsOut).Each(e => outTypes.Add(e));
                method.Parameters.Where(w => !w.IsOut).For((parameter, i, isLast) =>
                {
                    methodSb.AppendFormat("{0}{1}: {2}{3}",
                        (i == 0 ? "" : " "),                            // spacer
                        parameter.Name,                                 // argument name
                        parameter.ParameterType.ToTypeScriptType(),     // type
                        (isLast ? "" : ","));                           // last one gets a comma
                });
                methodSb.Append(")");

                // constructors don't have return types.
                if (!method.IsConstructor)
                {
                    string returnType;
                    if (outTypes.Any())
                    {
                        var outWriter = new OutParameterReturnTypeWriter(IndentCount, TypeDefinition, methodName, method.ReturnType, outTypes);
                        extendedTypes.Add(outWriter);
                        //TypeCollection.Add(TypeDefinition.Namespace, outWriter.TypeName, outWriter);
                        returnType = outWriter.TypeName;
                    }
                    else
                    {
                        returnType = method.ReturnType.ToTypeScriptType();
                    }

                    methodSb.AppendFormat(": {0}", returnType);
                }
                methodSb.AppendLine(";");

                var renderedMethod = methodSb.ToString();
                if (!methodSignatures.Contains(renderedMethod))
                    methodSignatures.Add(renderedMethod);
            }

            methodSignatures.Each(method => sb.Append(method));

            return extendedTypes;
        }
    }
}
