using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Playground.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FormatController : ControllerBase
    {
        private readonly ILogger<FormatController> _logger;

        public FormatController(ILogger<FormatController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public string Post([FromBody] string content)
        {
            return content;
        }
    }
}