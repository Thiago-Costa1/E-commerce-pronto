using aspnetsite.GerenciaArquivos;
using aspnetsite.Models;
using aspnetsite.Repository;
using aspnetsite.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace aspnetsite.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    public class ProdutoController : Controller
    {
        private INotebookRepository _notebookRepository;

        public ProdutoController(INotebookRepository notebookRepository)
        {

            _notebookRepository = notebookRepository;
        }

        [HttpPost]
        public IActionResult CadNotebook(Notebook notebook, IFormFile imagemNotebook1, IFormFile imagemNotebook2, IFormFile imagemNotebook3, IFormFile imagemNotebook4)
        {
            var caminhos = GerenciadorArquivo.CadastrarImagemProduto(imagemNotebook1, imagemNotebook2, imagemNotebook3, imagemNotebook4);

            // Atribui os caminhos retornados aos respectivos campos do notebook
            notebook.imagemNotebook1 = caminhos[0];
            notebook.imagemNotebook2 = caminhos[1];
            notebook.imagemNotebook3 = caminhos[2];
            notebook.imagemNotebook4 = caminhos[3];

            _notebookRepository.Cadastrar(notebook);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index()
        {
            return View(_notebookRepository.ObterTodosNotebooks());
        }
       
    }
}