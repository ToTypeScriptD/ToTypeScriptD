using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ToTypeScriptD.Tests.ExeTests
{
    public class ExeTestBase
    {
        string TypeScriptDExePath = @"../../../bin/ToTypeScriptD.exe";

        public ExeProcessResult Execute(string args)
        {
            return Execute(new[] { args });
        }

        public ExeProcessResult Execute(IEnumerable<string> args)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = TypeScriptDExePath,
                Arguments = string.Join(" ", args),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            var process = Process.Start(processStartInfo);
            var stdErr = process.StandardError.ReadToEnd();
            var stdOut = process.StandardError.ReadToEnd();
            process.WaitForExit();

            return new ExeProcessResult
            {
                StdErr = stdErr,
                StdOut = stdOut,
                ExitCode = process.ExitCode,
            };
        }
    }
}
