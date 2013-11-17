﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ToTypeScriptD.Core;
using ToTypeScriptD.Core.TypeWriters;

namespace ToTypeScriptD
{
    public class Render
    {
        public static bool AllAssemblies(Config config, TextWriter w)
        {
            w.Write(GetHeader(config.AssemblyPaths));

            var typeCollection = new TypeCollection(config.GetTypeWriterTypeSelector());

            var wroteAnyTypes = WriteSpecialTypes(config.IncludeSpecialTypes, w, config);
            wroteAnyTypes |= WriteFiles(config.AssemblyPaths, w, config.TypeNotFoundErrorHandler, typeCollection, config.RegexFilter, config);
            return wroteAnyTypes;
        }

        private static bool WriteFiles(IEnumerable<string> assemblyPaths, TextWriter w, ITypeNotFoundErrorHandler typeNotFoundErrorHandler, TypeCollection typeCollection, string filterRegex, Config config)
        {
            var filesAlreadyProcessed = new HashSet<string>(new IgnoreCaseStringEqualityComparer());
            if (!assemblyPaths.Any())
                return false;

            assemblyPaths.Each(assemblyPath =>
            {
                if (filesAlreadyProcessed.Contains(assemblyPath))
                    return;

                filesAlreadyProcessed.Add(assemblyPath);
                CollectTypes(assemblyPath, typeNotFoundErrorHandler, typeCollection, config);
            });

            var renderedOut = typeCollection.Render(filterRegex);
            w.WriteLine(renderedOut);

            return true;
        }

        private static bool WriteSpecialTypes(bool includeSpecialTypes, TextWriter w, Config config)
        {
            if (!includeSpecialTypes)
                return false;

            w.NewLine();

            w.WriteLine(Resources.ToTypeScriptDSpecialTypes_d.Replace("    ", config.Indent));
            w.NewLine();
            w.NewLine();
            return true;
        }

        public static string FullAssembly(string assemblyPath, ITypeNotFoundErrorHandler typeNotFoundErrorHandler, TypeCollection typeCollection, string filterRegex, Config config)
        {
            CollectTypes(assemblyPath, typeNotFoundErrorHandler, typeCollection, config);
            return GetHeader(new[] { assemblyPath }) + typeCollection.Render(filterRegex);
        }

        private static string GetHeader(IEnumerable<string> assemblyPaths)
        {
            if (!assemblyPaths.All(File.Exists))
            {
                return "";
            }

            var sb = new StringBuilder();
            sb.AppendFormatLine("//****************************************************************");
            sb.AppendFormatLine("//  Generated by:  ToTypeScriptD");
            sb.AppendFormatLine("//  Website:       http://github.com/ToTypeScriptD/ToTypeScriptD");
            sb.AppendFormatLine("//  Version:       {0}", System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(Render).Assembly.Location).ProductVersion);
            sb.AppendFormatLine("//  Date:          {0}", DateTime.Now);
            if (assemblyPaths.Any())
            {
                sb.AppendFormatLine("//");
                sb.AppendFormatLine("//  Assemblies:");
                assemblyPaths
                    .Select(System.IO.Path.GetFileName)
                    .Distinct()
                    .OrderBy(s => s)
                    .Each(path =>
                    {
                        sb.AppendFormatLine("//    {0}", System.IO.Path.GetFileName(path));
                    });
                sb.AppendFormatLine("//");
            }
            sb.AppendFormatLine("//****************************************************************");
            sb.AppendFormatLine();
            sb.AppendFormatLine();
            sb.AppendFormatLine();
            return sb.ToString();
        }

        private static void CollectTypes(string assemblyPath, ITypeNotFoundErrorHandler typeNotFoundErrorHandler, TypeCollection typeCollection, Config config)
        {
            var assembly = Mono.Cecil.AssemblyDefinition.ReadAssembly(assemblyPath);

            typeCollection.AddAssembly(assembly);

            var typeWriterGenerator = new TypeWriterCollector(typeNotFoundErrorHandler, typeCollection.TypeSelector);
            foreach (var item in assembly.MainModule.Types)
            {
                typeWriterGenerator.Collect(item, typeCollection, config);
            }
        }

    }
}
