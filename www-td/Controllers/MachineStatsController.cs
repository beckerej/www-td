using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using www_td.Models;

namespace www_td.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineStatsController : ControllerBase
    {
        private readonly WebApiContext _webApiContext;
        private ILogger<WeatherForecastController> _logger;

        public MachineStatsController(WebApiContext webApiContext, ILogger<WeatherForecastController> logger)
        {
            _webApiContext = webApiContext;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<MachineStats> Get()
        {
            return _webApiContext.MachineStats.ToList();
        }
    }
}