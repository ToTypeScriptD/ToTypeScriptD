using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using ToTypeScriptD;

namespace ToTypeScriptD.Core.DotNet
{

    public static class DotNetExtensions
    {

        static Dictionary<string, string> typeMap = new Dictionary<string, string>{
                { "System.String",               "string"},
                { "System.Int16",                "number"},
                { "System.Int32",                "number"},
                { "System.Int64",                "number"},
                { "System.UInt16",               "number"},
                { "System.UInt32",               "number"},
                { "System.UInt64",               "number"},
                { "System.Object",               "any"},
                { "System.DateTime", "Date"},
                { "System.Void",                 "void"},
                { "System.Boolean",              "boolean"},
                { "System.IntPtr",               "number"},
                { "System.Byte",                 "number"}, // TODO: Confirm if this is the correct representation?
                { "System.Single",               "number"},
                { "System.Double",               "number"},
                { "System.Char",                 "number"}, // TODO: should this be a string or number?
                { "System.Guid",                 "string"},
                { "System.Byte[]",               "any"},
        };
        static Dictionary<string, string> genericTypeMap = null;

        public static bool ShouldIgnoreType(this Mono.Cecil.TypeDefinition name)
        {
            if (!name.IsPublic)
                return true;

            return false;
        }

        public static bool ShouldIgnoreTypeByName(this TypeReference typeReference)
        {
            if (typeReference.FullName == "System.Collections.IEnumerable")
                return true;

            return false;
        }

        private static bool IsNullable(this Mono.Cecil.TypeReference typeReference)
        {
            // TODO: is there a better way to determine if it's a Nullable?
            if (typeReference.Namespace == "System" && typeReference.Name == "Nullable`1")
            {
                return true;
            }
            return false;
        }

        public static string ToTypeScriptNullable(this Mono.Cecil.TypeReference typeReference)
        {
            return IsNullable(typeReference) ? "?" : "";
        }

        private static TypeReference GetNullableType(TypeReference typeReference)
        {
            if (IsNullable(typeReference))
            {
                var genericInstanceType = typeReference as GenericInstanceType;
                if (genericInstanceType != null)
                {
                    typeReference = genericInstanceType.GenericArguments[0];
                }
                else
                {
                    throw new NotImplementedException("For some reason this Nullable didn't have a generic parameter type? " + typeReference.FullName);
                }
            }

            return typeReference;
        }

        public static string ToTypeScriptItemName(this Mono.Cecil.TypeReference typeReference)
        {
            // Nested classes don't report their namespace. So we have to walk up the 
            // DeclaringType tree to find the root most type to grab it's namespace.
            var parentMostType = typeReference;
            while (parentMostType.DeclaringType != null)
            {
                parentMostType = parentMostType.DeclaringType;
            }

            var mainTypeName = typeReference.FullName;

            // trim namespace off of the front.
            mainTypeName = mainTypeName.Substring(parentMostType.Namespace.Length + 1);

            // replace the nested class slash with an underscore
            mainTypeName = mainTypeName.Replace("/", "_").StripGenericTick();

            return mainTypeName.StripGenericTick();
        }

        public static string ToTypeScriptType(this Mono.Cecil.TypeReference typeReference)
        {
            typeReference = GetNullableType(typeReference);

            if (genericTypeMap == null)
            {
                genericTypeMap = typeMap
                    .ToDictionary(item => "<" + item.Key + ">", item => "<" + item.Value + ">");

                typeMap
                    .ToDictionary(item => item.Key + "[]", item => item.Value + "[]")
                    .Each(x => genericTypeMap.Add(x.Key, x.Value));
            }

            var fromName = typeReference.FullName;

            // translate / in nested classes into underscores
            fromName = fromName.Replace("/", "_");

            if (typeMap.ContainsKey(fromName))
            {
                return typeMap[fromName];
            }

            var genericType = genericTypeMap.FirstOrDefault(x => fromName.Contains(x.Key));
            if (!genericType.Equals(default(System.Collections.Generic.KeyValuePair<string, string>)))
            {
                fromName = fromName.Replace(genericType.Key, genericType.Value);
            }

            fromName = fromName
                .StripGenericTick();

            // To lazy to figure out the Mono.Cecil way (or if there is a way), but do 
            // some string search/replace on types for example:
            //
            // turn
            //      Windows.Foundation.Collections.IMapView<System.String,System.Object>;
            // into
            //      Windows.Foundation.Collections.IMapView<string,any>;
            // 
            typeMap.Each(item =>
            {
                fromName = fromName.Replace(item.Key, item.Value);
            });

            // If it's an array type return it as such.
            var genericTypeArgName = "";
            if (IsTypeArray(typeReference, out genericTypeArgName))
            {
                return genericTypeArgName + "[]";
            }

            // remove the generic bit
            return fromName;
        }


        private static bool IsTypeArray(TypeReference typeReference, out string genericTypeArgName)
        {
            genericTypeArgName = "";

            if (!typeReference.IsGenericInstance)
                return false;

            if (IsTypeTypeArray(typeReference, out genericTypeArgName))
            {
                return true;
            }

            return false;
        }
        private static bool IsTypeTypeArray(TypeReference typeReference, out string genericTypeArgName)
        {
            var genericTypeInstanceReference = typeReference as GenericInstanceType;
            if (genericTypeInstanceReference != null)
            {
                if (genericTypeInstanceReference == null)
                {
                    genericTypeArgName = "T";
                }
                else
                {
                    genericTypeArgName = genericTypeInstanceReference.GenericArguments[0].ToTypeScriptType();
                }

                var enumerableNamePrefix = "System.Collections.IEnumerable";

                // is this IEnumerable?
                if (typeReference.FullName.StartsWith(enumerableNamePrefix))
                {
                    return true;
                }

                // does it have an interface that implements IEnumerable?
                var possibleListType = GetTypeDefinition(genericTypeInstanceReference.ElementType);
                if (possibleListType != null)
                {
                    if (possibleListType.Interfaces.Any(x => x.FullName.StartsWith(enumerableNamePrefix)))
                    {
                        return true;
                    }
                }

                // TODO: do we need to work harder at inspecting interface items?
                // TODO: write tests to prove it..
            }



            genericTypeArgName = "";
            return false;
        }

        public static TypeDefinition GetTypeDefinition(TypeReference typeReference)
        {
            if (typeReference == null)
                return null;

            try
            {
                var resolver = new DefaultAssemblyResolver();
                var ass = resolver.Resolve(typeReference.Scope.Name);

                var result = ass.Modules
                    .SelectMany(x => x.Types)
                    .FirstOrDefault(td => td.FullName == typeReference.FullName);
                return result;
            }
            catch (AssemblyResolutionException)
            {
                // for now ignore...
                // TODO: figure out a better way to handle non-framework assemblies...
            }

            return null;
        }
    }
}
