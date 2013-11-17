using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToTypeScriptD.Core.TypeWriters;
using ToTypeScriptD.Core.WinMD;

namespace ToTypeScriptD.Core.WinMD
{
    public abstract class TypeWriterBase : ITypeWriter
    {
        public TypeDefinition TypeDefinition { get; set; }
        public int IndentCount { get; set; }
        public TypeCollection TypeCollection { get; set; }

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
        public abstract void Write(StringBuilder sb);


        // TODO: pull tab out of Config
        public string IndentValue
        {
            get { return "    ".Dup(IndentCount); }
        }

        public void Indent(StringBuilder sb)
        {
            sb.Append(IndentValue);
        }

        internal void WriteOutMethodSignatures(StringBuilder sb, string exportType, string inheriterString)
        {
            Indent(sb); sb.AppendFormat("export {0} {1}", exportType, TypeDefinition.ToTypeScriptItemName());
            WriteGenerics(sb);
            sb.Append(" ");
            WriteExportedInterfaces(sb, inheriterString);
            sb.AppendLine("{");

            List<ITypeWriter> extendedTypes = WriteMethods(sb);
            WriteFields(sb);
            WriteProperties(sb);
            WriteEvents(sb);

            WriteAsyncPromiseMethods(sb);

            Indent(sb); sb.AppendLine("}");

            WriteExtendedTypes(sb, extendedTypes);
            WriteNestedTypes(sb);
        }

        private void WriteAsyncPromiseMethods(StringBuilder sb)
        {
            string genericTypeArgName;
            if (IsTypeAsync(out genericTypeArgName))
            {
                sb.AppendLine();
                Indent(sb); Indent(sb); sb.AppendFormatLine("// Promise Extension");
                Indent(sb); Indent(sb); sb.AppendFormatLine("then<U>(success?: (value: {0}) => ToTypeScriptD.WinRT.IPromise<U>, error?: (error: any) => ToTypeScriptD.WinRT.IPromise<U>, progress?: (progress: any) => void): ToTypeScriptD.WinRT.IPromise<U>;", genericTypeArgName);
                Indent(sb); Indent(sb); sb.AppendFormatLine("then<U>(success?: (value: {0}) => ToTypeScriptD.WinRT.IPromise<U>, error?: (error: any) => U, progress?: (progress: any) => void): ToTypeScriptD.WinRT.IPromise<U>;", genericTypeArgName);
                Indent(sb); Indent(sb); sb.AppendFormatLine("then<U>(success?: (value: {0}) => U, error?: (error: any) => ToTypeScriptD.WinRT.IPromise<U>, progress?: (progress: any) => void): ToTypeScriptD.WinRT.IPromise<U>;", genericTypeArgName);
                Indent(sb); Indent(sb); sb.AppendFormatLine("then<U>(success?: (value: {0}) => U, error?: (error: any) => U, progress?: (progress: any) => void): ToTypeScriptD.WinRT.IPromise<U>;", genericTypeArgName);
                Indent(sb); Indent(sb); sb.AppendFormatLine("done<U>(success?: (value: {0}) => any, error?: (error: any) => any, progress?: (progress: any) => void): void;", genericTypeArgName);
            }
        }

        private bool IsTypeAsync(out string genericTypeArgName)
        {
            var currType = TypeDefinition;

            if (IsTypeAsync(TypeDefinition, out genericTypeArgName))
            {
                return true;
            }

            foreach (var i in TypeDefinition.Interfaces)
            {
                if (IsTypeAsync(i, out genericTypeArgName))
                {
                    return true;
                }
            }
            genericTypeArgName = "";
            return false;
        }

        private bool IsTypeAsync(TypeReference typeReference, out string genericTypeArgName)
        {
            if (typeReference.FullName.StartsWith("Windows.Foundation.IAsyncOperation`1") ||
                typeReference.FullName.StartsWith("Windows.Foundation.IAsyncOperationWithProgress`2")
                )
            {
                var genericInstanceType = typeReference as GenericInstanceType;
                if (genericInstanceType == null)
                {
                    genericTypeArgName = "TResult";
                }
                else
                {
                    genericTypeArgName = genericInstanceType.GenericArguments[0].ToTypeScriptType();
                }
                return true;
            }

            genericTypeArgName = "";
            return false;
        }

        private void WriteGenerics(StringBuilder sb)
        {
            if (TypeDefinition.HasGenericParameters)
            {
                sb.Append("<");
                TypeDefinition.GenericParameters.For((genericParameter, i, isLastItem) =>
                {
                    StringBuilder constraintsSB = new StringBuilder();
                    genericParameter.Constraints.For((constraint, j, isLastItemJ) =>
                    {
                        // Not sure how best to deal with multiple generic constraints (yet)
                        // For now place in a comment
                        // TODO: possible generate a new interface type that extends all of the constraints?
                        var isFirstItem = j == 0;
                        var isOnlyItem = isFirstItem && isLastItemJ;
                        if (isOnlyItem)
                        {
                            constraintsSB.AppendFormat(" extends {0}", constraint.ToTypeScriptType());
                        }
                        else
                        {
                            if (isFirstItem)
                            {
                                constraintsSB.AppendFormat(" extends {0} /*TODO:{1}", constraint.ToTypeScriptType(), (isLastItemJ ? "*/" : ", "));
                            }
                            else
                            {
                                constraintsSB.AppendFormat("{0}{1}", constraint.ToTypeScriptType(), (isLastItemJ ? "*/" : ", "));
                            }
                        }
                    });

                    sb.AppendFormat("{0}{1}{2}", genericParameter.ToTypeScriptType(), constraintsSB.ToString(), (isLastItem ? "" : ", "));
                });
                sb.Append(">");
            }
        }

        private static void WriteExtendedTypes(StringBuilder sb, List<ITypeWriter> extendedTypes)
        {
            extendedTypes.Each(item => item.Write(sb));
        }

        private void WriteNestedTypes(StringBuilder sb)
        {
            TypeDefinition.NestedTypes.Where(type => type.IsNestedPublic).Each(type =>
            {
                var typeWriter = TypeCollection.TypeSelector.PickTypeWriter(type, IndentCount - 1, TypeCollection);
                sb.AppendLine();
                typeWriter.Write(sb);
            });
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

                if (method.IsSpecialName && !method.IsConstructor)
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

            // HACK: This not a sustainable approach (but working for now)
            //       The IWebSocket inherits from IClosable and the websocket's close 
            //       conflicts with the closable close so we have to hack this method
            //       onto the websocket interface.
            if (TypeDefinition.FullName == "Windows.Networking.Sockets.IWebSocket")
            {
                methodSignatures.Add(IndentValue + IndentValue + "close(): void;" + Environment.NewLine);
            }

            methodSignatures.Each(method => sb.Append(method));

            return extendedTypes;
        }

        private void WriteProperties(StringBuilder sb)
        {
            TypeDefinition.Properties.Each(prop =>
            {
                var propName = prop.Name.ToTypeScriptName();

                var propMethod = prop.GetMethod ?? prop.SetMethod;

                var staticText = propMethod.IsStatic ? "static " : "";

                Indent(sb); Indent(sb); sb.AppendFormat("{0}{1}: {2};", staticText, propName, prop.PropertyType.ToTypeScriptType());
                sb.AppendLine();
            });
        }

        private void WriteFields(StringBuilder sb)
        {
            TypeDefinition.Fields.Each(field =>
            {
                if (!field.IsPublic) return;
                var fieldName = field.Name.ToTypeScriptName();
                Indent(sb); Indent(sb); sb.AppendFormat("{0}: {1};", fieldName, field.FieldType.ToTypeScriptType());
                sb.AppendLine();
            });
        }

        private void WriteEvents(StringBuilder sb)
        {
            if (TypeDefinition.HasEvents)
            {
                Indent(sb); Indent(sb); sb.AppendLine("// Events");

                Indent(sb); Indent(sb); sb.AppendLine("addEventListener(eventName: string, listener: any): void;");
                Indent(sb); Indent(sb); sb.AppendLine("removeEventListener(eventName: string, listener: any): void;");
                var distinctListenerSignatures = new List<string>();

                TypeDefinition.Events.For((item, i, isLast) =>
                {
                    var eventListenerType = item.EventType.ToTypeScriptType();
                    var eventName = item.Name.ToLower();

                    var line = IndentValue + IndentValue + "addEventListener(eventName: \"{0}\", listener: {1}): void;".FormatWith(eventName, eventListenerType);
                    if (!distinctListenerSignatures.Contains(line))
                        distinctListenerSignatures.Add(line);

                    line = IndentValue + IndentValue + "removeEventListener(eventName: \"{0}\", listener: {1}): void;".FormatWith(eventName, eventListenerType);
                    if (!distinctListenerSignatures.Contains(line))
                        distinctListenerSignatures.Add(line);

                    line = IndentValue + IndentValue + "on{0}: (ev: {1}) => void;".FormatWith(eventName, eventListenerType);
                    if (!distinctListenerSignatures.Contains(line))
                    {
                        distinctListenerSignatures.Add(line);
                    }
                });

                distinctListenerSignatures.Each(item =>
                {
                    sb.AppendLine(item);
                });
                sb.AppendLine();
            }
        }

        private void WriteExportedInterfaces(StringBuilder sb, string inheriterString)
        {
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
        }


        public string FullName
        {
            get { return TypeDefinition.Namespace + "." + TypeDefinition.ToTypeScriptItemName(); }
        }
    }
}
