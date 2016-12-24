using System;
using System.Threading.Tasks;

namespace ToTypeScriptD.TestAssembly.CSharp
{
    public interface IAmAnInterface
    {
        string SomeProperty { get; set; }
        int? NullableProperty { get; set; }

        void GetNothing();
        Task<int> LoadAsync();
    }

    public class SomeClass : IAmAnInterface
    {
        public void GetNothing()
        {
            throw new NotImplementedException();
        }

        public Task<int> LoadAsync()
        {
            throw new NotImplementedException();
        }

        public void MethodWithNullableParameter(int? p1)
        {
        }

        public void MethodThatTakesNestedValueTypeAsNullable(NestedEnumValueType? nullableEnum)
        {
        }

        public void MethodWithStrangeParameterNames(Action function)
        {
        }

        public string SomeProperty { get; set; }


        public int? NullableProperty { get; set; }

        public enum NestedEnumValueType
        {
            a,
            b,
            c
        }
    }
}
