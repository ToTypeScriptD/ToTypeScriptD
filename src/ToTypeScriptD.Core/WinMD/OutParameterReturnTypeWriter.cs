using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToTypeScriptD.Core.TypeWriters;
using ToTypeScriptD.Core.WinMD;

namespace ToTypeScriptD.Core.WinMD
{
    public class OutParameterReturnTypeWriter : ITypeWriter
    {
        static int versionCounter = 0;

        private int version;
        private TypeReference ReturnTypeReference;
        private List<ParameterDefinition> OutTypes;
        private int IndentCount;
        private TypeDefinition TypeDefinition;
        private string MethodName;
        private ConfigBase config;

        public OutParameterReturnTypeWriter(ConfigBase config, int indentCount, Mono.Cecil.TypeDefinition TypeDefinition, string methodName, TypeReference retrunTypeReference, List<ParameterDefinition> outTypes)
        {
            this.version = ++versionCounter;
            this.config = config;
            this.IndentCount = indentCount;
            this.TypeDefinition = TypeDefinition;
            this.MethodName = methodName;
            this.ReturnTypeReference = retrunTypeReference;
            this.OutTypes = outTypes;
        }

        public string IndentValue
        {
            get { return config.Indent.Dup(IndentCount); }
        }

        public void Write(StringBuilder sb)
        {
            sb.AppendLine();
            sb.Append(IndentValue); sb.AppendFormat("interface {0} {{{1}", TypeName, Environment.NewLine);

            // return type
            if (!(ReturnTypeReference.FullName == "System.Void"))
            {
                sb.AppendFormat("{0}{0}__returnValue: {1};{2}", IndentValue, ReturnTypeReference.ToTypeScriptType(), Environment.NewLine);
            }

            // out parameter values
            OutTypes.Each(item =>
            {
                sb.AppendFormat("{0}{0}{1}: {2};{3}", IndentValue, item.Name, item.ParameterType.ToTypeScriptType(), Environment.NewLine);
            });

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
                return TypeDefinition.ToTypeScriptItemName() + "_" + MethodName + "_OUT_" + this.version + genericParams;
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
