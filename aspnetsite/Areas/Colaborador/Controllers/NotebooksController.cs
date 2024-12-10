using aspnetsite.GerenciaArquivos;
using aspnetsite.Models;
using aspnetsite.Repository.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace aspnetsite.Areas.Colaborador.Controllers
{

    [Area("Colaborador")]


    public class NotebooksController : Controller
    {
        private INotebookRepository _notebookRepository;

        public NotebooksController(INotebookRepository notebookRepository)
        {

            _notebookRepository = notebookRepository;
        }
        public IActionResult Index()
        {
            return View(_notebookRepository.ObterTodosNotebooks());
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

            return RedirectToAction("Painel", "Home");
        }

        public IActionResult CadNotebook()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Edit(Notebook notebook)
        {
            _notebookRepository.Atualizar(notebook);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Atualizar(int id)
        {
            Models.Notebook notebook = _notebookRepository.ObterNotebooks(id);
            return View(notebook);
        }
        [HttpPost]
        public IActionResult Atualizar(Notebook notebook, IFormFile imagemNotebook1, IFormFile imagemNotebook2, IFormFile imagemNotebook3, IFormFile imagemNotebook4)
        {
            var caminhos = GerenciadorArquivo.CadastrarImagemProduto(imagemNotebook1, imagemNotebook2, imagemNotebook3, imagemNotebook4);

            // Assumindo que você deseja atualizar as propriedades de imagem no notebook
            if (caminhos.Count > 0) notebook.imagemNotebook1 = caminhos[0];
            if (caminhos.Count > 1) notebook.imagemNotebook2 = caminhos[1];
            if (caminhos.Count > 2) notebook.imagemNotebook3 = caminhos[2];
            if (caminhos.Count > 3) notebook.imagemNotebook4 = caminhos[3];

            _notebookRepository.Atualizar(notebook);
            return RedirectToAction(nameof(Index));
        }



        public IActionResult Delete(int id)
        {
            _notebookRepository.Excluir(id);
            return RedirectToAction("Index", "Notebooks");
        }

        // Usando o repositório para buscar notebooks do banco de dados
        [HttpGet]
        [Route("api/notebooks/filter")]
        public IActionResult FiltrarNotebooks(string category)
        {
            var notebooks = _notebookRepository.ObterTodosNotebooks();

            // Filtrar os notebooks com base na categoria recebida
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
            // Retornar a lista filtrada como JSON
            return Ok(notebooksFiltrados);
        }
    }
}