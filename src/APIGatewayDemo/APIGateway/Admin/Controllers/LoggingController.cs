using Microsoft.AspNetCore.Mvc;
using Ocelot.Configuration.Repository;

namespace APIGateway.Controllers
{
    public class LoggingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
