using aspnetsite.CarrinhoCompra;
using aspnetsite.Libraries.Filtro;
using aspnetsite.Libraries.Login;
using aspnetsite.Models;
using aspnetsite.Models.Constant;
using aspnetsite.Repository;
using aspnetsite.Repository.Contract;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using System.Diagnostics;

namespace aspnetsite.Controllers
{
    public class HomeController : Controller
    {
        //Injeção de dependência
        private readonly IClienteRepository _clienteRepository;
        private readonly INotebookRepository _notebookRepository;
        private readonly CookieCarrinhoCompra _cookieCarrinhoCompra;
        private readonly IColaboradorRepository _colaboradorRepository;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly LoginCliente _loginCliente;
        private readonly LoginColaborador _loginColaborador;

        public HomeController(
            IPedidoRepository pedidoRepository,
            IColaboradorRepository colaboradorRepository,
            IClienteRepository clienteRepository,
            INotebookRepository notebookRepository,
            CookieCarrinhoCompra cookieCarrinhoCompra,
            LoginCliente loginCliente,
            LoginColaborador loginColaborador)
        {
            // Injeção das dependências
            _pedidoRepository = pedidoRepository;
            _colaboradorRepository = colaboradorRepository;
            _clienteRepository = clienteRepository;
            _notebookRepository = notebookRepository;
            _cookieCarrinhoCompra = cookieCarrinhoCompra;
            _loginCliente = loginCliente;
            _loginColaborador = loginColaborador;
        }



        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm] Cliente cliente)
        {
            cliente.Situacao = SituacaoConstant.Ativo;

            _clienteRepository.Cadastrar(cliente);

            TempData["CadastroSucesso"] = true;
            return RedirectToAction("Login", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login([FromForm] Cliente cliente)
        {
            Cliente clienteDB = _clienteRepository.Login(cliente.Email, cliente.Senha);

            if (clienteDB.Email != null && clienteDB.Senha != null)
            {
                _loginCliente.Login(clienteDB);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //Erro na sessão
                ViewData["MSG_E"] = "Usuário não localizado, por favor verifique e-mail e senha digitado";
                return View();
            }
        }
        [ClienteAutorizacao]
        public IActionResult PainelCliente()
        {

            int idCliente = 1; // Exemplo: obtém o cliente logado
            var pedidos = _pedidoRepository.ObterPedidosPorCliente(idCliente);

            Cliente cliente = _clienteRepository.ObterCliente(idCliente);

            var viewModel = new PainelClienteViewModel
            {
                Cliente = cliente,
                Pedidos = pedidos
            };

            // Passar o PainelClienteViewModel para o layout via ViewData
            ViewData["ClienteLogado"] = viewModel;

            return View(viewModel);
        }



        public IActionResult LogoutCliente()
        {
            _loginCliente.Logout();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index() // Exibe os notebooks na Index

        {
            // Verificar se o cliente está logado e passar para a view
            ViewData["IsClienteLogado"] = _loginCliente.IsClienteLogado();

            // Verificar se o colaborador está logado e passar para a view
            ViewData["IsColaboradorLogado"] = _loginColaborador.IsColaboradorLogado();



            var notebooks = _notebookRepository.ObterTodosNotebooks();
            return View(notebooks);
        }

        public IActionResult Carrinho() // Carrinho de compra

        {
            var produtosCarrinho = _cookieCarrinhoCompra.Consultar();
            return View("/Views/Carrinho/Carrinho.cshtml", produtosCarrinho);

        }

        public IActionResult RemoverItem(int id) // Remover itens do carrinho
        {
            _cookieCarrinhoCompra.Remover(new Notebook() { codNotebook = id });
            return RedirectToAction(nameof(Carrinho));
        }

        public IActionResult PainelComum()
        {
            // Obter cliente logado
            var cliente = _loginCliente.GetCliente();

            if (cliente == null)
            {
                // Se não houver cliente logado, redirecionar para a página de login
                return RedirectToAction("Login", "Home");
            }

            // Preencher o ViewModel
            var viewModel = new PainelClienteViewModel
            {
                Cliente = cliente
            };

            // Retornar a view com o ViewModel
            return View(viewModel);
        }



        public IActionResult Relatorio()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            // Obtém o cliente pelo ID
            Cliente cliente = _clienteRepository.ObterCliente(id);

            if (cliente == null)
            {
                Console.WriteLine($"Cliente com ID {id} não encontrado.");
                return NotFound();
            }

            var viewModel = new PainelClienteViewModel
            {
                Cliente = cliente,
                Pedidos = new List<Pedido>() // Se necessário, preencha com os pedidos reais do cliente
            };

            // Passa o cliente para a view
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Editar(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                // Retorna a View com o modelo esperado (PainelClienteViewModel)
                var viewModel = new PainelClienteViewModel
                {
                    Cliente = cliente,
                    Pedidos = new List<Pedido>() // Preencha os pedidos reais, se necessário
                };

                return View(viewModel); // Passa o modelo correto
            }

            try
            {
                Console.WriteLine($"Atualizando cliente com ID {cliente.Id} e Nome {cliente.Nome}");
                _clienteRepository.Atualizar(cliente); // Atualiza o cliente no banco
                TempData["MensagemSucesso"] = "Informações atualizadas com sucesso!";
                return RedirectToAction("PainelComum", "Home"); // Redireciona para o painel do cliente
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar cliente: {ex.Message}");
                TempData["MensagemErro"] = "Ocorreu um erro ao atualizar as informações. Tente novamente.";

                // Retorna a View com o modelo esperado
                var viewModel = new PainelClienteViewModel
                {
                    Cliente = cliente,
                    Pedidos = new List<Pedido>() // Preencha os pedidos reais, se necessário
                };

                return View(viewModel);
            }
        }
    }
}