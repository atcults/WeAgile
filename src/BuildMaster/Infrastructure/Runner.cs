using System;
using System.Diagnostics;
using BuildMaster.Model;

namespace BuildMaster.Infrastructure
{
    public class ProcessRunner
    {
        public static int RunProcess(string basePath, JobTask JobTask)
        {
            Process process = new Process();

            process.EnableRaisingEvents = true;
            process.OutputDataReceived += (sender, e) =>
            {
                //result.Output += e.Data + Environment.NewLine;
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                //result.ErrorOutput += e.Data + Environment.NewLine;
            };

            var exitCode = 0;

            process.Exited += (sender, e) =>
            {
                exitCode = process.ExitCode;
            };

            process.StartInfo.FileName = JobTask.CommandName;
            process.StartInfo.Arguments = JobTask.CommandAruments;
            process.StartInfo.WorkingDirectory = basePath + JobTask.RelativePath;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            process.WaitForExit();

            return exitCode;
        }
    }
}