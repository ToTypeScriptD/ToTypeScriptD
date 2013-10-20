using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToTypeScriptD.Tests.ExeTests
{
    public class ExeProcessResult
    {
        public string StdErr { get; set; }
        public string StdOut { get; set; }
        public int ExitCode { get; set; }
    }
}
