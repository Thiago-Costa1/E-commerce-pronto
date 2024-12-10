using Microsoft.AspNetCore.Mvc;

namespace aspnetsite.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View("login");
        }
    }
}
