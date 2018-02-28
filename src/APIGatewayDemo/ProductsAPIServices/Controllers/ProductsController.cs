using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProductsAPIServices.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            throw new Exception("Get Products Error.");
            return new string[] { "Surface Book 2", "Mac Book Pro" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"You select a product: {id}-{DateTime.Now} with caching.";
        }

        [HttpGet("{name}/error")]
        public string Get(string name)
        {
            return $"error: {name}";
        }
    }
}
