using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToTypeScriptD.Core.TypeWriters;

namespace ToTypeScriptD.Core.WinRT
{
    public class OutParameterReturnTypeWriter : ITypeWriter
    {
        private TypeReference ReturnTypeReference;
        private List<ParameterDefinition> OutTypes;
        private int IndentCount;
        private TypeDefinition TypeDefinition;
        private string MethodName;

        public OutParameterReturnTypeWriter(int IndentCount, Mono.Cecil.TypeDefinition TypeDefinition, string methodName, TypeReference retrunTypeReference, List<ParameterDefinition> outTypes)
        {
            // TODO: Complete member initialization
            this.IndentCount = IndentCount;
            this.TypeDefinition = TypeDefinition;
            this.MethodName = methodName;
            this.ReturnTypeReference = retrunTypeReference;
            this.OutTypes = outTypes;
        }

        // TODO: pull out of config
        public string IndentValue
        {
            get { return "    ".Dup(IndentCount); }
        }

        public void Write(StringBuilder sb)
        {
            sb.AppendLine();
            sb.Append(IndentValue); sb.AppendFormat("interface {0} {{{1}", TypeName, Environment.NewLine);
            IndentCount++;

            // return type
            if (!(ReturnTypeReference.FullName == "System.Void"))
            {
                sb.AppendFormat("{0}__returnValue: {1};{2}", IndentValue, ReturnTypeReference.ToTypeScriptType(), Environment.NewLine);
            }

            // out parameter values
            OutTypes.Each(item =>
            {
                sb.AppendFormat("{0}{1}: {2};{3}", IndentValue, item.Name, item.ParameterType.ToTypeScriptType(), Environment.NewLine);
            });

            IndentCount--;
            sb.Append(IndentValue + "}" + Environment.NewLine);
        }

        public string TypeName
        {
            get
            {
                string genericParams = "";
                if (TypeDefinition.HasGenericParameters)
                {
                    genericParams = "<" + TypeDefinition.GenericParameters.Select(s => s.FullName).Join(", ") + ">";
                }
                return TypeDefinition.ToTypeScriptItemName() + "_" + MethodName + "_OUT" + genericParams;
            }
        }

        public string FullName
        {
            get
            {
                return TypeDefinition.Name + "." + TypeName;
            }
        }
    }
}
