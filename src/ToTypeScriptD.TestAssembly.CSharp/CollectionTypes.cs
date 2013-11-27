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
}
