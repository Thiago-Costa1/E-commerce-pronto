using aspnetsite.Libraries.Filtro;
using aspnetsite.Models.Constant;
using aspnetsite.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace aspnetsite.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacao(ColaboradorTipoConstant.Gerente)]
    public class ColaboradorController : Controller
    {
        private IColaboradorRepository _colaboradorRepository;
        public ColaboradorController(IColaboradorRepository colaboradorRepository)
        {
            _colaboradorRepository = colaboradorRepository;
        }
        public IActionResult Index()
        {
            return View(_colaboradorRepository.ObterTodosColaboradores());
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(Models.Colaborador colaborador)
        {
            colaborador.Tipo = ColaboradorTipoConstant.Comum;

            _colaboradorRepository.Cadastrar(colaborador);

            TempData["MSG_S"] = "Registro salvo com sucesso!";

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Atualizar(int id)
        {
            Models.Colaborador colaborador = _colaboradorRepository.ObterColaborador(id);

            if (colaborador == null)
            {
                return NotFound("Colaborador não encontrado.");
            }

            return View(colaborador);
        }

        [HttpPost]
        public IActionResult Atualizar([FromForm] Models.Colaborador colaborador)
        {
            if (ModelState.IsValid)
            {
                _colaboradorRepository.Atualizar(colaborador);

                TempData["MSG_S"] = "Registro salvo com sucesso!";

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult AtualizarParcial(int id)
        {
            var colaborador = _colaboradorRepository.ObterColaborador(id);
            if (colaborador == null)
                return NotFound();

            // Retorna a view do formulário
            return PartialView("_FormAtualizarColaborador", colaborador);
        }

        [ValidateHttpReferer]
        public IActionResult Excluir(int id)
        {
            _colaboradorRepository.Excluir(id);
            return RedirectToAction(nameof(Index));
        }

    }
}