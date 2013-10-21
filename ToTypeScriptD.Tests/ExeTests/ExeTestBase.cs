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

        public async Task<ExeProcessResult> Execute(string args)
        {
            return await Execute(new[] { args });
        }

        public Task<ExeProcessResult> Execute(IEnumerable<string> args)
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
            var stdOut = process.StandardOutput.ReadToEnd();
            var stdErr = process.StandardError.ReadToEnd();

            var tcs = new TaskCompletionSource<ExeProcessResult>();

            process.Exited += (sender, argsX) =>
            {
                tcs.SetResult(new ExeProcessResult
                {
                    StdErr = stdErr,
                    StdOut = stdOut,
                    ExitCode = process.ExitCode,
                });
                process.Dispose();
            };

            process.Start();

            return tcs.Task;
        }
    }
}
