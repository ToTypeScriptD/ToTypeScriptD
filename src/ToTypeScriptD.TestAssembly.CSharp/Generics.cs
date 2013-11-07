using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToTypeScriptD.TestAssembly.CSharp
{
    public class GenericClass<T>
    {
        public T GetItem(T input)
        {
            return input;
        }
    }

    public class GenericClassWithConstraint<T>
        where T : IAmAnInterface
    {
        public T GetItem(T item)
        {
            return item;
        }
    }


    public class GenericClassWithMultipleTypesConstrained<T, K>
        where T : IAmAnInterface
        where K : IAmAnInterface
    {
        public K GetItemK(K item)
        {
            return item;
        }

        public T GetItemT(T item)
        {
            return item;
        }
    }


    public interface IAmAnotherInterfaceButGeneric1<T> { }
    public interface IAmAnotherInterfaceButGeneric2<T> { }
    public interface IAmAnotherInterfaceButGeneric3<T> : IAmAnotherInterfaceButGeneric1<T>, IAmAnotherInterfaceButGeneric2<T> { }
    public class GenericClassIsGettingALittleCrazy<T> where T : IAmAnotherInterfaceButGeneric3<T>
    {

    }



    public interface IAmAnotherInterface1 { }
    public interface IAmAnotherInterface2 { }
    public interface IAmAnotherInterface3<T, K> { }
    public class GenericClassWithOneTypeConstraintMultipleTimes<T, K>
        where T : IAmAnInterface, IAmAnotherInterface1, IAmAnotherInterface2, IAmAnotherInterface3<T, K>
    {
        public T GetItemT(T item)
        {
            return item;
        }
    }


    public class GenericClassWith<T, K>
    {
        public void GetSomething(T inT, K inK, out IAmAnotherInterface3<T, K> outParam1)
        {
            throw new NotImplementedException();
        }
    }
}
