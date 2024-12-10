using Microsoft.AspNetCore.Mvc;
using aspnetsite.Models;
using aspnetsite.CarrinhoCompra;
using aspnetsite.Repository.Contract;
using aspnetsite.Libraries.Filtro;
using MySql.Data.MySqlClient;
using aspnetsite.Repository;
using aspnetsite.Libraries.Login;


namespace aspnetsite.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly CookieCarrinhoCompra _cookieCarrinhoCompra;
        private readonly INotebookRepository _notebookRepository;
        private readonly ILogger<CheckoutController> _logger;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly LoginCliente _loginCliente;


        private readonly string _conexaoMySQL;
        public CheckoutController(ILogger<CheckoutController> logger, CookieCarrinhoCompra cookieCarrinhoCompra,
                                  INotebookRepository notebookRepository, IPedidoRepository pedidoRepository, IClienteRepository clienteRepository, IConfiguration conf, LoginCliente loginCliente)
        {
            _logger = logger;
            _cookieCarrinhoCompra = cookieCarrinhoCompra;
            _notebookRepository = notebookRepository;
            _pedidoRepository = pedidoRepository;           
            _clienteRepository = clienteRepository;
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
            _loginCliente = loginCliente; 


        }

        public IActionResult Checkout()
        {
            var clienteLogado = _loginCliente.GetCliente();

            if (clienteLogado == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var carrinhoAtual = _cookieCarrinhoCompra.Consultar();

            if (carrinhoAtual == null || !carrinhoAtual.Any())
            {
                return RedirectToAction("Carrinho", "Carrinho");
            }

            // Recupera o desconto armazenado no TempData
            decimal descontoCupom = 0m;
            if (TempData["DescontoCupom"] != null)
            {
                decimal.TryParse(TempData["DescontoCupom"].ToString(), out descontoCupom);
            }

            ViewBag.DescontoCupom = descontoCupom;
            ViewBag.Cliente = clienteLogado;
            return View(carrinhoAtual);
        }


        public IActionResult Index()
        {
            var carrinho = _cookieCarrinhoCompra.Consultar(); // Obter carrinho atualizado
            var descontoCupom = TempData["DescontoCupom"] as decimal? ?? 0m; // Valor padrão 0 se não houver desconto

            // Aplicar o desconto ao último item do carrinho, se necessário
            if (carrinho.Any())
            {
                var ultimoItem = carrinho.Last();
                ultimoItem.precoNotebook -= descontoCupom; // Ajuste no último item
            }

            return View(carrinho);
        }




        [HttpPost]
        public IActionResult FinalizarPedido()
        {
            try
            {
                // Obter o cliente logado (substitua pelo ID real do cliente logado)
                int idCliente = 1; // Exemplo estático

                // Obter o carrinho atual do cookie
                var carrinho = _cookieCarrinhoCompra.Consultar();

                if (carrinho == null || !carrinho.Any())
                {
                    return BadRequest("O carrinho está vazio.");
                }

                // Calcular o valor total do pedido
                decimal descontoCupom = TempData["DescontoCupom"] != null
                    ? decimal.Parse(TempData["DescontoCupom"].ToString())
                    : 0m;

                decimal valorTotal = carrinho.Sum(p =>
                    (p.precoNotebook + p.GarantiaSelecionada) * p.quantidade
                ) - descontoCupom;

                // Criar o modelo de Pedido
                var pedido = new Pedido
                {
                    IdCliente = idCliente,
                    DataPedido = DateTime.Now,
                    ValorTotal = valorTotal, // Total com desconto
                    Itens = carrinho.Select(p => new Itens
                    {
                        IdProduto = p.codNotebook,
                        QtdItens = p.quantidade,
                        ValorParcial = p.precoNotebook, // Valor unitário do notebook
                        ValorTotal = (p.precoNotebook + p.GarantiaSelecionada) * p.quantidade // Valor total sem desconto
                    }).ToList()
                };

                // Salvar o pedido no banco de dados
                _pedidoRepository.CriarPedido(pedido);

                // Limpar o carrinho após finalizar o pedido
                _cookieCarrinhoCompra.RemoverTodos();

                // Redirecionar para o painel do cliente
                return RedirectToAction("PainelCliente", "Home");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro SQL: {ex.Message}");
                return StatusCode(500, "Erro ao processar o pedido. Tente novamente mais tarde.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro geral: {ex.Message}");
                return StatusCode(500, "Erro inesperado. Tente novamente mais tarde.");
            }
        }


        public IActionResult Remover(int id)
        {
            try
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();

                    // Iniciar uma transação para garantir que os itens e o pedido sejam excluídos juntos
                    using (var transaction = conexao.BeginTransaction())
                    {
                        try
                        {
                            // Remover os itens associados ao pedido
                            MySqlCommand cmdRemoverItens = new MySqlCommand(
                                "DELETE FROM Itens WHERE IdPedido = @IdPedido",
                                conexao,
                                transaction);
                            cmdRemoverItens.Parameters.AddWithValue("@IdPedido", id);
                            cmdRemoverItens.ExecuteNonQuery();

                            // Remover o pedido
                            MySqlCommand cmdRemoverPedido = new MySqlCommand(
                                "DELETE FROM Pedido WHERE IdPedido = @IdPedido",
                                conexao,
                                transaction);
                            cmdRemoverPedido.Parameters.AddWithValue("@IdPedido", id);
                            cmdRemoverPedido.ExecuteNonQuery();

                            // Confirmar a transação
                            transaction.Commit();
                        }
                        catch
                        {
                            // Reverter a transação em caso de erro
                            transaction.Rollback();
                            throw;
                        }
                    }

                    conexao.Close();
                }

                TempData["MensagemSucesso"] = "Pedido cancelado com sucesso!";
                return RedirectToAction("PainelCliente", "Home");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao cancelar o pedido: " + ex.Message;
                return RedirectToAction("PainelCliente", "Home");
            }
        }

    }
}