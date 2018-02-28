using Microsoft.AspNetCore.Mvc;
using Ocelot.Configuration.Repository;

namespace APIGateway.Controllers
{
    public class DiagnosticController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
