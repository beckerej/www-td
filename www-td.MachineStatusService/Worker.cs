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
            var index = _webApiContext.machinestats.ToList().Last().id;
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                _webApiContext.machinestats.Add(new machinestats
                {
                    id = ++index,
                    cputime = Process.GetCurrentProcess().TotalProcessorTime.Seconds
                });
                var last = _webApiContext.machinestats.ToList().Last();
                _logger.LogInformation($"Adding machine stat {{id: {last.id}, cpu: {last.cputime}}}");
                _webApiContext.SaveChanges();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
