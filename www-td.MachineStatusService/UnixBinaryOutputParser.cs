using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace www_td.MachineStatusService
{
    public static class UnixBinaryOutputParser
    {
        public static string GetOutput(string unixProgram)
        {
            string output;

            var info = CreateProcess(unixProgram);

            using (var process = Process.Start(info))
            {
                output = process != null ? process.StandardOutput.ReadToEnd() : string.Empty;
            }

            return output;

            ProcessStartInfo CreateProcess(string unixProgramCommand)
            {
                var processInfo = new ProcessStartInfo(unixProgramCommand)
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{unixProgramCommand}\"",
                    RedirectStandardOutput = true
                };
                return processInfo;
            }
        }

        public static float GetCpuIdle(IReadOnlyList<string> cpuOutput)
        {
            return GetCpuFromIndex(cpuOutput, 11);
        }

        public static float GetCpuUsage(IReadOnlyList<string> cpuOutput)
        {
            return Enumerable.Range(2, 9).Sum(i => GetCpuFromIndex(cpuOutput, i));
        }

        public static int GetDiskUsage(IReadOnlyList<string> diskOutput)
        {
            return int.Parse(diskOutput[3].Split(" ", StringSplitOptions.RemoveEmptyEntries)[4].Replace("%", ""));
        }

        public static int GetTotalMemory(IReadOnlyList<string> freeOutput)
        {
            return GetFreeFromIndex(freeOutput, 1);
        }

        public static int GetUsedMemory(IReadOnlyList<string> freeOutput)
        {
            return GetFreeFromIndex(freeOutput, 2);
        }

        public static int GetFreeMemory(IReadOnlyList<string> freeOutput)
        {
            return GetFreeFromIndex(freeOutput, 3);
        }

        public static int GetSharedMemory(IReadOnlyList<string> freeOutput)
        {
            return GetFreeFromIndex(freeOutput, 4);
        }

        public static int GetCacheMemory(IReadOnlyList<string> freeOutput)
        {
            return GetFreeFromIndex(freeOutput, 5);
        }

        public static int GetAvailableMemory(IReadOnlyList<string> freeOutput)
        {
            return GetFreeFromIndex(freeOutput, 6);
        }

        public static float GetCpuFromIndex(IReadOnlyList<string> cpuOutput, int i)
        {
            return float.Parse(cpuOutput[3].Split(" ", StringSplitOptions.RemoveEmptyEntries)[i]);
        }

        public static int GetFreeFromIndex(IReadOnlyList<string> freeOutput, int i)
        {
            return int.Parse(freeOutput[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)[i]);
        }
    }
}
