using Microsoft.AspNetCore.Mvc;

namespace aspnetsite.Controllers
{
    public class SobreController : Controller
    {
        public IActionResult Index()
        {
            return View("sobre");
        }
    }
}
