using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using www_td.Database.Models;

namespace www_td.MachineStatusService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly WebApiContext _webApiContext;

        public Worker(WebApiContext webApiContext, ILogger<Worker> logger)
        {
            _webApiContext = webApiContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var last = _webApiContext.machinestats.ToList().LastOrDefault();
            var index = last?.id ?? default;
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    var machineStats = GetMachineStats(++index);
                    _webApiContext.machinestats.Add(machineStats);
                    _webApiContext.SaveChanges();
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                ConsoleWriteException(e);
            }
        }

        private static machinestats GetMachineStats(int index)
        {
            var freeOutput = UnixBinaryOutputParser.GetOutput("free -m").Split("\n");
            var diskOutput = UnixBinaryOutputParser.GetOutput("df").Split("\n");
            var cpuOutput = UnixBinaryOutputParser.GetOutput("mpstat").Split("\n");
            return CheckOutputs(freeOutput, diskOutput, cpuOutput) ? null : 
                CreateNewMachineStats(index, freeOutput, diskOutput, cpuOutput);
        }

        private static machinestats CreateNewMachineStats(int index,
            IReadOnlyList<string> freeOutput,
            IReadOnlyList<string> diskOutput,
            IReadOnlyList<string> cpuOutput)
        {
            return new machinestats
            {
                id = index,
                machinename = Process.GetCurrentProcess().MachineName,
                totalmemory = UnixBinaryOutputParser.GetTotalMemory(freeOutput),
                usedmemory = UnixBinaryOutputParser.GetUsedMemory(freeOutput),
                freememory = UnixBinaryOutputParser.GetFreeMemory(freeOutput),
                sharedmemory = UnixBinaryOutputParser.GetSharedMemory(freeOutput),
                cachememory = UnixBinaryOutputParser.GetCacheMemory(freeOutput),
                availablememory = UnixBinaryOutputParser.GetAvailableMemory(freeOutput),
                diskusage = UnixBinaryOutputParser.GetDiskUsage(diskOutput),
                cpuusage = UnixBinaryOutputParser.GetCpuUsage(cpuOutput),
                cpuidle = UnixBinaryOutputParser.GetCpuIdle(cpuOutput),
            };
        }

        private static bool CheckOutputs(IReadOnlyCollection<string> freeOutput,
            IReadOnlyCollection<string> diskOutput,
            IReadOnlyCollection<string> cpuOutput)
        {
            if (freeOutput.Count == 0)
            {
                ConsoleWriteError("free -m");
                return true;
            }

            if (diskOutput.Count == 0)
            {
                ConsoleWriteError("df");
                return true;
            }

            if (cpuOutput.Count == 0)
            {
                ConsoleWriteError("mpstat");
                return true;
            }

            return false;
        }

        private static void ConsoleWriteException(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Exception Message: {e.Message}");
            Console.WriteLine($"Exception StackTrace: {e.StackTrace}");
            Console.ResetColor();
        }

        private static void ConsoleWriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Error: /bin/bash -c \"{message}\" did not produce an output.");
            Console.ResetColor();
        }
    }
}
