using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToTypeScriptD.TestAssembly.CSharp
{
    public class CrazyAmountOfNestedClasses
    {
        public class C1
        {
            public class C2
            {
                public class C3
                {
                    public class C4
                    {
                        public class C5
                        {
                            public class WAT
                            {
                                public static void TakesANestedParam(WAT wat)
                                {
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
