using ApprovalTests;
using ApprovalTests.Reporters;
using Mono.Cecil;
using NUnit.Framework;
using System.Text;
using WinmdToTypeScript.Core.TypeWriters;

namespace WinmdToTypeScript.Tests
{

    [TestFixture]
    public class Spike : TestBase
    {
        [Test]
        public void SpikeIt()
        {
            //Approvals.Verify(result);
        }

    }

    public class TypeTests : TestBase
    {
        [Test]
        public void EnumType()
        {
            var result = GetNativeType("SampleEnum").ToTypeScript();
            Approvals.Verify(result);
        }

        [Test]
        public void EnumTypeWithExplicitValues()
        {
            var result = GetNativeType("SampleEnumNumbered").ToTypeScript();
            Approvals.Verify(result);
        }
    }

    public static class Extensions
    {
        public static string ToTypeScript(this TypeDefinition value)
        {
            return new TypeWriterGenerator().Generate(value);
        }
    }

}
