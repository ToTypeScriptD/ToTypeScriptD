using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToTypeScriptD.Tests.ExeTests
{
    public class ExeProcessResult
    {
        public string StdOut { get; set; }
        public int ExitCode { get; set; }

        public override string ToString()
        {
            return StdOut;
        }
    }
}
