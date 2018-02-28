using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using APIGatewayDemo.Common;

namespace CustomersAPIServices.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly ILogger _logger;

        public CustomersController(ILogger<CustomersController> logger)
        {
            this._logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            this._logger.LogInformation(LoggingEvents.GetItem, "Getting all items");
            //throw new Exception("this a new exception.");
            return new string[] { "Catcher Wong", "James Li" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            this._logger.LogInformation(LoggingEvents.GetItem, "Getting item {ID}", id);

            return $"Catcher Wong - {id}";
        }
    }
}
