using ApprovalTests;
using ApprovalTests.Reporters;
using Mono.Cecil;
using NUnit.Framework;
using System.Text;
using WinmdToTypeScript.TypeWriters;

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


    public class g
    {
        public string Generate(Mono.Cecil.TypeDefinition td)
        {
            TypeWriterBase typeWriter = null;
            if (td.IsEnum)
            {
                typeWriter = new EnumWriter(td, 0);
            }
            var sb = new StringBuilder();
            typeWriter.Write(sb);
            return sb.ToString();
        }
    }


    public static class Extensions
    {
        public static string ToTypeScript(this TypeDefinition value)
        {
            return new g().Generate(value);
        }
    }

}
