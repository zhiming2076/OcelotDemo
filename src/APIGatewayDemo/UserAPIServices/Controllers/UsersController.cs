using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UserAPIServices.Filters;

namespace UserAPIServices.Controllers
{
    public class UsersController : ApiController
    {
        [WapAuthorize]
        [HttpGet]
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
