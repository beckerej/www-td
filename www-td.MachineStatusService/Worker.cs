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
            var last = _webApiContext.MachineStats.ToList().LastOrDefault();
            var index = last?.Id ?? default;
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    var machineStats = GetMachineStats(++index);
                    _webApiContext.MachineStats.Add(machineStats);
                    _logger.LogInformation($"Adding {machineStats}");
                    _webApiContext.SaveChanges();
                    await Task.Delay(10000, stoppingToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                ConsoleWriteException(e);
            }
        }

        private static MachineStats GetMachineStats(int index)
        {
            var freeOutput = UnixBinaryOutputParser.GetOutput("free -m").Split("\n");
            var diskOutput = UnixBinaryOutputParser.GetOutput("df").Split("\n");
            var cpuOutput = UnixBinaryOutputParser.GetOutput("mpstat").Split("\n");
            return CheckOutputs(freeOutput, diskOutput, cpuOutput) ? null : 
                CreateNewMachineStats(index, freeOutput, diskOutput, cpuOutput);
        }

        private static MachineStats CreateNewMachineStats(int index,
            IReadOnlyList<string> freeOutput,
            IReadOnlyList<string> diskOutput,
            IReadOnlyList<string> cpuOutput)
        {
            return new MachineStats
            {
                Id = index,
                MachineName = Process.GetCurrentProcess().MachineName,
                TotalMemory = UnixBinaryOutputParser.GetTotalMemory(freeOutput),
                UsedMemory = UnixBinaryOutputParser.GetUsedMemory(freeOutput),
                FreeMemory = UnixBinaryOutputParser.GetFreeMemory(freeOutput),
                SharedMemory = UnixBinaryOutputParser.GetSharedMemory(freeOutput),
                CacheMemory = UnixBinaryOutputParser.GetCacheMemory(freeOutput),
                AvailableMemory = UnixBinaryOutputParser.GetAvailableMemory(freeOutput),
                DiskUsage = UnixBinaryOutputParser.GetDiskUsage(diskOutput),
                CpuUsage = UnixBinaryOutputParser.GetCpuUsage(cpuOutput),
                CpuIdle = UnixBinaryOutputParser.GetCpuIdle(cpuOutput),
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
