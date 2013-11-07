﻿using Mono.Cecil;
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
                { "Windows.Foundation.DateTime", "Date"},
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

        public static bool ShouldIgnoreTypeByName(this string name)
        {
            if (name == "<Module>")
                return true;

            if (name.StartsWith("__I") && name.EndsWith("PublicNonVirtuals"))
                return true;

            if (name.StartsWith("__I") && name.EndsWith("ProtectedNonVirtuals"))
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
                .StripGenericTick()
                .StripOutParamSymbol();

            // hack the async type with a custom promise definition
            if (fromName.StartsWith("Windows.Foundation.IAsyncOperation<"))
            {
                fromName = fromName.Replace("Windows.Foundation.IAsyncOperation<", "ToTypeScriptD.WinRT.IPromise<");
            }

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

            // remove the generic bit
            return fromName;
        }

        public static string ToTypeScriptName(this string name)
        {
            if (name.ToUpper() == name)
            {
                return name.ToLower();
            }

            return Char.ToLowerInvariant(name[0]) + name.Substring(1);
        }
        public static string StripOutParamSymbol(this string value)
        {
            return value.Replace("&", "");
        }

        public static void NewLine(this System.IO.TextWriter textWriter)
        {
            textWriter.WriteLine("");
        }
    }
}