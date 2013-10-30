using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var tcs = new TaskCompletionSource<ExeProcessResult>();

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = TypeScriptDExePath,
                    Arguments = string.Join(" ", args),
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            };

            process.Start();

            var stdOut = process.StandardOutput.ReadToEnd();
            var stdError = process.StandardError.ReadToEnd();

            // this is a bit of a hack - but adding standard error to the final result;
            if (!string.IsNullOrEmpty(stdError))
            {
                stdOut += stdError;
            }

            process.WaitForExit();


            return new ExeProcessResult
                {
                    StdOut = stdOut,
                    ExitCode = process.ExitCode,
                };
        }
    }
}
