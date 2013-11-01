using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToTypeScriptD.TestAssembly.CSharp
{
    public interface IAmAnInterface
    {
        string SomeProperty { get; set; }

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

        public string SomeProperty { get; set; }
    }
}
