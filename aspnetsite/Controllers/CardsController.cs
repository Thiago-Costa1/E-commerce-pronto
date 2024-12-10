using aspnetsite.Models;
using aspnetsite.Repository.Contract;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace aspnetsite.Controllers
{
    public class CardsController : Controller
    {
        private readonly INotebookRepository _notebookRepository;

        public CardsController(INotebookRepository notebookRepository)
        {
            _notebookRepository = notebookRepository;
        }

        public IActionResult DetalhesProduto(int id)
        {
            // Usa o método ObterNotebooks para buscar o notebook pelo ID.
            var notebook = _notebookRepository.ObterNotebooks(id);

            decimal precoAVista = notebook.precoNotebook - (notebook.descontoNotebook);
            ViewBag.PrecoAVista = precoAVista;
            ViewBag.PrecoTotal = notebook.precoNotebook;  // Garante que não é nulo
            ViewBag.NumeroParcelas = 12;

            ViewBag.DescricaoDinamica = $"{notebook.nomeNotebook}, {notebook.placaVideoNotebook}, {notebook.processadorNotebook}, {notebook.sistemaOperacionalNotebook}, {notebook.armazenamentoNotebook}, {notebook.audioNotebook}";

            ViewBag.ValorGarantia = notebook.valorGarantiaNotebook; // Passa o valor da garantia, pode ser nulo
            ViewBag.GarantiaDisponivel = notebook.valorGarantiaNotebook != null; // Verifica se tem garantia


            // Retorna a view com os detalhes do notebook.
            return View(notebook);
        }
    }
}