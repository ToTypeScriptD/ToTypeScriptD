using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToTypeScriptD.Core.TypeWriters;

namespace ToTypeScriptD.Core.DotNet
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

            WriteFields(sb);
            WriteProperties(sb);
            Indent(sb); sb.AppendLine("}");

            WriteNestedTypes(sb);
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

        private void WriteProperties(StringBuilder sb)
        {
            TypeDefinition.Properties.Each(prop =>
            {
                var propName = prop.Name.ToTypeScriptName();
                Indent(sb); Indent(sb); sb.AppendFormat("{0}{1}: {2};", propName, prop.PropertyType.ToTypeScriptNullable(), prop.PropertyType.ToTypeScriptType());
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
