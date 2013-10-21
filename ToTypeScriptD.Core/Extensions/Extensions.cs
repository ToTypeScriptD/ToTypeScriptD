using System;
using System.Collections.Generic;
using System.Linq;

namespace ToTypeScriptD
{

    public static class Extensions
    {
        public static string Dup(this string value, int count)
        {
            return string.Join("", Enumerable.Range(0, count).Select(s => value));
        }

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
                { "System.Guid",                 "string"}, // TODO: should this be a string or a System.Guid object?

        };
        static Dictionary<string, string> genericTypeMap = null;

        public static bool ShouldIgnoreType(this Mono.Cecil.TypeDefinition name)
        {
            if (!name.IsPublic)
                return true;

            // TODO: find a better way to detect inheritance of a specific type.
            //var baseType = name.BaseType;
            //while(baseType != null){
            //    baseType = name.BaseType;
            //}
            //if (name.BaseType != null && name.BaseType.FullName == "System.Attribute") return false;

            //return !name.IsNotPublic;
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

        public static string ToTypeScriptType(this Mono.Cecil.TypeReference typeReference)
        {
            if (genericTypeMap == null)
            {
                genericTypeMap = typeMap.ToDictionary(item => "<" + item.Key + ">", item => "<" + item.Value + ">");
            }

            var fromName = typeReference.FullName;

            if (typeMap.ContainsKey(fromName))
            {
                return typeMap[fromName];
            }

            if (fromName.Contains("&"))
            {
                // need to figure out out parameters
                return "TodoOutParameters";
            }

            var genericType = genericTypeMap.FirstOrDefault(x => fromName.Contains(x.Key));
            if (!genericType.Equals(default(System.Collections.Generic.KeyValuePair<string, string>)))
            {
                fromName = fromName.Replace(genericType.Key, genericType.Value);
            }

            // remove the generic bit
            return fromName.StripGenericTick();
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> items)
        {
            var hashset = new HashSet<T>();
            if (items != null)
            {
                foreach (var item in items)
                {
                    hashset.Add(item);
                }
            }
            return hashset;
        }

        public static string ToTypeScriptName(this string name)
        {
            if (name.ToUpper() == name)
            {
                return name.ToLower();
            }

            return Char.ToLowerInvariant(name[0]) + name.Substring(1);
        }

        /// <summary>
        /// For iterator extension method that also includes a bool with the 'isLastItem' value.
        /// </summary>
        /// <example>
        ///     new[] { 1, 2, 3 }.For((item, i, isLastItem) => { });
        /// </example>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">Items to iterate</param>
        /// <param name="action">generic action with T1=Item, T2=i</param>
        public static void For<T>(this IEnumerable<T> items, Action<T, int, bool> action)
        {
            if (items != null)
            {
                var count = items.Count();
                int i = 0;
                foreach (var item in items)
                {
                    action(item, i, i == (count - 1));
                    i++;
                }
            }
        }
        public static IEnumerable<int> Times(this int value)
        {
            return Enumerable.Range(0, value);
        }

        public static void Each<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    action(item);
                }
            }
        }
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(System.Globalization.CultureInfo.CurrentCulture, format, args);
        }

        public static string StripGenericTick(this string value)
        {
            4.Times().Each(x =>
            {
                value = value.Replace("`" + x, "");
            });
            return value;
        }
    }
}
