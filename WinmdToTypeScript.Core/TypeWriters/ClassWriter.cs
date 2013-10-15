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

                    TypeDefinition.Events.For((item, i, isLast) =>
                    {
                        // TODO: events with multiple return types???
                        sb.Append(Indent); sb.Append(Indent); sb.AppendLine("on" + item.Name.ToLower() + "(ev: any);");
                    });
                }

                foreach (var method in TypeDefinition.Methods)
                {
                    var methodName = method.Name;

                    // ignore special event handler methods
                    if (method.HasParameters &&
                        method.Parameters[0].Name.StartsWith("__param0") &&
                        (methodName.StartsWith("add_") || methodName.StartsWith("remove_")))
                        continue;

                    // translate the constructor function
                    if (methodName == ".ctor")
                    {
                        methodName = "constructor";
                    }

                    // Lowercase first char of the method
                    methodName = Char.ToLowerInvariant(methodName[0]) + methodName.Substring(1);

                    sb.Append(Indent); sb.Append(Indent); sb.Append(methodName);
                    sb.Append("(");

                    method.Parameters.For((parameter, i, isLast) =>
                    {
                        sb.Append(parameter.Name);
                        sb.Append(": ");
                        sb.Append(parameter.ParameterType.ToTypeScriptType());
                        if (isLast) sb.Append(", ");
                    });

                    sb.Append(");");
                    sb.AppendLine();
                }

                sb.AppendLine(Indent + "}");
            });
        }
    }
}
