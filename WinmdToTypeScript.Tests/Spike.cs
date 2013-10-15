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


    public static class Extensions
    {
        public static string ToTypeScript(this TypeDefinition value)
        {
            var typeCollection = new TypeCollection();
            new TypeWriterGenerator().Generate(value, typeCollection);
            return typeCollection.Render();
        }
    }

}
