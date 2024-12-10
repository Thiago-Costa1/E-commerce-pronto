using aspnetsite.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace aspnetsite.Areas.Colaborador.Controllers
{
    public class PedidosController : Controller
    {
        private INotebookRepository _notebookRepository;
        public PedidosController(INotebookRepository notebookRepository)
        {
            _notebookRepository = notebookRepository;
        }
        public IActionResult Index()
        {

            return View(_notebookRepository.ObterTodosNotebooks());
        }
    }
}
