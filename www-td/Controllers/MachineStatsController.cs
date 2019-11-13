using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using www_td.Database.Models;

namespace www_td.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineStatsController : ControllerBase
    {
        private readonly WebApiContext _webApiContext;
        private ILogger<MachineStatsController> _logger;

        public MachineStatsController(WebApiContext webApiContext, ILogger<MachineStatsController> logger)
        {
            _webApiContext = webApiContext;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<machinestats> Get()
        {
            return _webApiContext.machinestats.ToList();
        }
    }
}