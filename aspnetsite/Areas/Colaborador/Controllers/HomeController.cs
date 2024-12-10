using aspnetsite.Libraries.Filtro;
using aspnetsite.Libraries.Login;
using aspnetsite.Models.Constant;
using aspnetsite.Repository;
using aspnetsite.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace aspnetsite.Areas.Colaborador.Controllers
{

    [Area("Colaborador")]
    public class HomeController : Controller
    {
        private IColaboradorRepository _repositoryColaborador;
        private LoginColaborador _loginColaborador;
        private IColaboradorRepository _colaboradorRepository;


        public HomeController(IColaboradorRepository repositoryColaborador, LoginColaborador loginColaborador,
            IColaboradorRepository colaboradorRepository)

        {
            _repositoryColaborador = repositoryColaborador;
            _loginColaborador = loginColaborador;
            _colaboradorRepository = colaboradorRepository;
        }

        [ColaboradorAutorizacao]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PainelInicial()
        {
            return View();
        }
        public IActionResult Relatorio()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] Models.Colaborador colaborador)
        {
            Models.Colaborador colaboradorDB = _repositoryColaborador.Login(colaborador.Email, colaborador.Senha);

            if (colaboradorDB.Email != null && colaboradorDB.Senha != null)
            {
                _loginColaborador.Login(colaboradorDB);

                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                ViewData["MSG_E"] = "Usuário não localizado, verifique e-mail e senha digitado";
                return View();
            }
        }

        [ColaboradorAutorizacao]
        public IActionResult Painel()
        {

            return View(_colaboradorRepository.ObterTodosColaboradores());

        }

        [ColaboradorAutorizacao]
        public IActionResult Logout()
        {
            _loginColaborador.Logout();
            return RedirectToAction("Login", "Home");
        }

        public IActionResult PainelGerente()
        {
            ViewBag.Nome = _loginColaborador.GetColaborador().Nome;
            ViewBag.Tipo = _loginColaborador.GetColaborador().Tipo;
            ViewBag.Email = _loginColaborador.GetColaborador().Email;
            return View();
        }

        public IActionResult PainelComun()
        {
            ViewBag.Nome = _loginColaborador.GetColaborador().Nome;
            ViewBag.Tipo = _loginColaborador.GetColaborador().Tipo;
            ViewBag.Email = _loginColaborador.GetColaborador().Email;
            return View("PainelComun");
        }
    }
}