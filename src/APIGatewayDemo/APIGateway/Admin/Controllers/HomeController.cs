using Microsoft.AspNetCore.Mvc;
using Ocelot.Configuration.Repository;

namespace APIGateway.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFileConfigurationRepository _fileConfigRepo;
        
        public HomeController(IFileConfigurationRepository fileConfigurationRepository)
        {
            _fileConfigRepo = fileConfigurationRepository;
        }
        public IActionResult Index()
        {
            var repo = _fileConfigRepo.Get();
            return View(repo.Result.Data);
        }
    }
}
