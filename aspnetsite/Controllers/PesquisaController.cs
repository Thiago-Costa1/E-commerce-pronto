using aspnetsite.Models;
using aspnetsite.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace aspnetsite.Controllers
{
    public class PesquisaController : Controller
    {

        private INotebookRepository _notebookRepository;

        public PesquisaController(INotebookRepository notebookRepository)
        {
            _notebookRepository = notebookRepository;
        }

        public IActionResult Index()
        {
            return View("Pesquisa");
        }

        public IActionResult Pesquisa(string category)
        {
            // Obter todos os notebooks
            var notebooks = _notebookRepository.ObterTodosNotebooks();

            IEnumerable<Notebook> notebooksFiltrados;
            switch (category.ToLower())
            {
                case "estudante":
                    notebooksFiltrados = notebooks.Where(p => p.precoNotebook <= 3000);
                    break;
                case "gamer":
                    notebooksFiltrados = notebooks.Where(p => p.precoNotebook > 3000 && p.precoNotebook <= 10000);
                    break;
                case "trabalho":
                    notebooksFiltrados = notebooks.Where(p => p.precoNotebook > 10000);
                    break;
                default:
                    return BadRequest("Categoria inválida");
            }
            return View("Pesquisa", notebooksFiltrados);
        }
    }
}