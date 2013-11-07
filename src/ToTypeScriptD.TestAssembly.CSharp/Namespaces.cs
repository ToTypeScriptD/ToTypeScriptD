
namespace ToTypeScriptD.TestAssembly.CSharp.NamespaceSample
{
    public class TestClass
    {
        public static void Go() { }
    }

    namespace Sample
    {
        public class TestClassA
        {
            public static void Go() { }
        }

        namespace SubSample
        {
            public class TestClassB
            {
                public static void Go() { }
            }
        }
    }

    namespace Sample2
    {
        public class TestClassC
        {
            public static void Go() { }
        }
    }

}
