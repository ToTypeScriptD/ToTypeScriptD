using Mono.Cecil;
using System;

namespace WinmdToTypeScript.Core.TypeWriters
{
    public class ClassWriter : TypeWriterBase
    {
        private TypeDefinition typeDefinition;
        private int indentCount;

        public ClassWriter(Mono.Cecil.TypeDefinition typeDefinition, int indentCount)
            : base(typeDefinition, indentCount)
        {
            this.typeDefinition = typeDefinition;
            this.indentCount = indentCount;
        }

        public override void Write(System.Text.StringBuilder sb)
        {
            var namespaceWriter = new NamespaceWriter(TypeDefinition, IndentCount);
            namespaceWriter.Write(sb, () =>
            {
                ++IndentCount;

                /*
                    export interface IUriRuntimeClass {
                        absoluteUri: string;
                        displayUri: string;
                        domain: string;
                        extension: string;
                        fragment: string;
                        host: string;
                        password: string;
                        path: string;
                        port: number;
                        query: string;
                        queryParsed: Windows.Foundation.WwwFormUrlDecoder;
                        rawUri: string;
                        schemeName: string;
                        suspicious: bool;
                        userName: string;
                        equals(pUri: Windows.Foundation.Uri): bool;
                        combineUri(relativeUri: string): Windows.Foundation.Uri;
                    }
                    export class Uri implements Windows.Foundation.IUriRuntimeClass, Windows.Foundation.IUriRuntimeClassWithAbsoluteCanonicalUri {
                        constructor (uri: string);
                        absoluteUri: string;
                        displayUri: string;
                        domain: string;
                        extension: string;
                        fragment: string;
                        host: string;
                        password: string;
                        path: string;
                        port: number;
                        query: string;
                        queryParsed: Windows.Foundation.WwwFormUrlDecoder;
                        rawUri: string;
                        schemeName: string;
                        suspicious: bool;
                        userName: string;
                        absoluteCanonicalUri: string;
                        displayIri: string;
                        equals(pUri: Windows.Foundation.Uri): bool;
                        combineUri(relativeUri: string): Windows.Foundation.Uri;
                        static new(baseUri: string, relativeUri: string): Uri;
                        static unescapeComponent(toUnescape: string): string;
                        static escapeComponent(toEscape: string): string;
                    }
                 */
                sb.AppendLine(Indent + "export class " + TypeDefinition.Name + "{");

                // TODO: get specific types of EventListener types?
                if (TypeDefinition.HasEvents)
                {
                    sb.Append(Indent); sb.Append(Indent); sb.AppendLine("addEventListener(type: string, listener: EventListener): void;");
                    sb.Append(Indent); sb.Append(Indent); sb.AppendLine("removeEventListener(type: string, listener: EventListener): void;");

                    for (int i = 0; i < TypeDefinition.Events.Count; i++)
                    {
                        var item = TypeDefinition.Events[i];
                        sb.Append(Indent); sb.Append(Indent); sb.AppendLine("on" + item.Name.ToLower() + "(ev: any);");
                    }
                }

                foreach (var method in TypeDefinition.Methods)
                {
                    var methodName = method.Name;

                    if (methodName.StartsWith("add_") && method.HasParameters && method.Parameters[0].Name.StartsWith("__param0"))
                        continue;
                    if (methodName.StartsWith("remove_") && method.HasParameters && method.Parameters[0].Name.StartsWith("__param0"))
                        continue;

                    if (methodName == ".ctor")
                    {
                        methodName = "constructor";
                    }

                    methodName = Char.ToLowerInvariant(methodName[0]) + methodName.Substring(1);

                    sb.Append(Indent); sb.Append(Indent); sb.Append(methodName);
                    sb.Append("(");
                    for (int iparameter = 0; iparameter < method.Parameters.Count; iparameter++)
                    {
                        var parameter = method.Parameters[iparameter];
                        sb.Append(parameter.Name);
                        sb.Append(": ");
                        sb.Append(parameter.ParameterType.ToTypeScriptType());
                        if (iparameter < method.Parameters.Count - 1)
                        {
                            sb.Append(", ");
                        }
                    }
                    sb.Append(");");
                    sb.AppendLine();
                }

                sb.AppendLine(Indent + "}");
            });
        }
    }
}
