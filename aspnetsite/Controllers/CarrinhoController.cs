using aspnetsite.CarrinhoCompra;
using aspnetsite.Models;
using aspnetsite.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace aspnetsite.Controllers
{
    public class CarrinhoController : Controller
    {
        private readonly CookieCarrinhoCompra _cookieCarrinho;
        private readonly INotebookRepository _notebookRepository;
        private readonly CookieCarrinhoCompra _cookieCarrinhoCompra;
        private readonly ILogger<CarrinhoController> _logger;

        public CarrinhoController(ILogger<CarrinhoController> logger, CookieCarrinhoCompra cookieCarrinho,
                                     CookieCarrinhoCompra cookieCarrinhoCompra, INotebookRepository notebookRepository)
        {
            _logger = logger;
            _cookieCarrinho = cookieCarrinho;
            _notebookRepository = notebookRepository;
            _cookieCarrinhoCompra = cookieCarrinhoCompra;
        }
        public IActionResult AdicionarAoCarrinho(int id)
        {
            var notebook = _notebookRepository.ObterNotebooks(id);
            if (notebook == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var item = new Notebook()
                {
                    codNotebook = id,
                    quantidade = 1,
                    imagemNotebook1 = notebook.imagemNotebook1,
                    nomeNotebook = notebook.nomeNotebook,
                    precoNotebook = notebook.precoNotebook,
                    valorGarantiaNotebook = notebook.valorGarantiaNotebook
                };
                _cookieCarrinhoCompra.Cadastrar(item);
                return RedirectToAction("Carrinho");
            }
        }

        [HttpGet]
        public IActionResult Carrinho()
        {
            decimal valorCupom = TempData["DescontoCupom"] as decimal? ?? 0m;            
            ViewData["ValorCupom"] = valorCupom; // Passa o valor do cupom para a View

            return View(_cookieCarrinhoCompra.Consultar());
        }

        public IActionResult Remover(int id)
        {
            var produto = new Notebook { codNotebook = id };
            _cookieCarrinho.Remover(produto);
            return RedirectToAction("Carrinho");
        }

        [HttpPost]
        public IActionResult AtualizarQuantidade(int codNotebook, int quantidade)
        {
            var carrinho = _cookieCarrinhoCompra.Consultar();
            var produto = carrinho.FirstOrDefault(p => p.codNotebook == codNotebook);

            if (produto != null)
            {
                produto.quantidade = quantidade;

                // Salva o estado atualizado do carrinho no cookie
                _cookieCarrinhoCompra.Salvar(carrinho);

                return Ok(new { mensagem = "Quantidade atualizada." });
            }

            return NotFound(new { mensagem = "Produto não encontrado." });
        }



        [HttpPost]
        public IActionResult AtualizarGarantia([FromBody] GarantiaViewModel garantiaModel)
        {
            var carrinho = _cookieCarrinhoCompra.Consultar(); // Método para obter o carrinho
            var item = carrinho.FirstOrDefault(p => p.codNotebook == garantiaModel.id);
            if (item != null)
            {
                item.GarantiaSelecionada = garantiaModel.garantia;
                _cookieCarrinhoCompra.Salvar(carrinho); // Salva o carrinho atualizado nos cookies
            }
            return Json(carrinho);
        }

        // ViewModel para receber os dados:
        public class GarantiaViewModel
        {
            public int id { get; set; }
            public decimal garantia { get; set; }
        }


        public IActionResult AtualizarCarrinho()
        {
            var carrinhoAtual = _cookieCarrinhoCompra.Consultar();
            _cookieCarrinhoCompra.Salvar(carrinhoAtual);
            return Ok(new { mensagem = "Carrinho atualizado com sucesso!" });
        }

        // Lista simulada de cupons com seus valores
        private static readonly Dictionary<string, decimal> Cupons = new Dictionary<string, decimal>
        {
            { "DESCONTO200", 200m },
            { "DESCONTO300", 300m }
        };
        [HttpPost]
        public JsonResult ValidarCupom([FromBody] CupomRequest request)
        {
            string codigo = request.Codigo?.Trim();

            // Verifica se o código de cupom é vazio
            if (string.IsNullOrEmpty(codigo))
            {
                return Json(new { sucesso = false, mensagem = "Por favor, insira um código de cupom." });
            }

            // Verifica se o cupom está na lista de cupons válidos
            if (Cupons.TryGetValue(codigo, out decimal desconto))
            {
                // Log para verificar o que foi encontrado
                _logger.LogInformation($"Cupom {codigo} encontrado com valor {desconto}.");

                // Armazena o desconto como string em TempData
                TempData["DescontoCupom"] = desconto.ToString("F2"); // Corrigido para string
                return Json(new { sucesso = true, desconto });
            }

            // Se o cupom não for encontrado
            _logger.LogWarning($"Cupom {codigo} não encontrado.");

            return Json(new { sucesso = false, mensagem = "Cupom inválido." });
        }
    }
}