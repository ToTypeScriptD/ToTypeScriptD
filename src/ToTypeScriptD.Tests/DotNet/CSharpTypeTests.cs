﻿using ApprovalTests;
using ToTypeScriptD.Core;
using ToTypeScriptD.Core.DotNet;
using Xunit;

namespace ToTypeScriptD.Tests.DotNet
{

    public class CSharpTypeTests : CSharpTestBase
    {
        [Fact]
        public void GenerateFullAssembly()
        {
            var path = base.CSharpAssembly.ComponentPath;
            var typeSelector = new DotNetTypeWriterTypeSelector();
            var errors = new StringBuilderTypeNotFoundErrorHandler();
            var typeCollection = new ToTypeScriptD.Core.TypeWriters.TypeCollection(typeSelector);
            var config = new Config();
            var result = ToTypeScriptD.Render.FullAssembly(path, errors, typeCollection, string.Empty, config);
            (errors + result).Verify();
        }
    }
}
