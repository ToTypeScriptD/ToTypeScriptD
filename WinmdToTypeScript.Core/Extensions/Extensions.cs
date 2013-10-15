using System;
using System.Collections.Generic;
using System.Linq;

namespace WinmdToTypeScript
{

    public static class Extensions
    {
        public static string Dup(this string value, int count)
        {
            return string.Join("", Enumerable.Range(0, count).Select(s => value));
        }

        public static string ToTypeScriptType(this Mono.Cecil.TypeReference typeReference)
        {
            switch (typeReference.Name)
            {
                case "String": return "string";
                case "DateTime": return "Date";
                case "Void": return "void";
                    
                default:
                    return typeReference.Name;
            }
        }
        public static string ToTypeScriptName(this string name)
        {
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
    }
}
