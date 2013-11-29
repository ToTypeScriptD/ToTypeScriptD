using System;
using System.Threading.Tasks;

namespace ToTypeScriptD.TestAssembly.CSharp
{
    public class CollectionTypes
    {
        public int[] IntegerArray { get; set; }
        public System.Collections.Generic.IList<int> IListOfIntegers { get; set; }
        public System.Collections.Generic.List<int> ListOfIntegers { get; set; }
        public System.Collections.Generic.ICollection<int> ICollectionOfIntegers { get; set; }
        public System.Collections.ObjectModel.Collection<int> CollectionOfIntegers { get; set; }

        public System.Collections.Generic.IEnumerable<int> IEnumerableOfIntegers { get; set; }
    }

    public class SampleEnumerableClass<T> : System.Collections.Generic.IEnumerable<T>
    {

        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public class SampleCollectionClass<T> : System.Collections.Generic.ICollection<T>
    {
        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
