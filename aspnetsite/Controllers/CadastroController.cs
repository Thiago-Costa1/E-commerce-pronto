using Microsoft.AspNetCore.Mvc;

namespace aspnetsite.Controllers
{
    public class CadastroController : Controller
    {
        public IActionResult Index()
        {
            return View("Cadastro");
        }
    }
}
