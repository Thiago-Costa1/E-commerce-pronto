using aspnetsite.Models;
using Newtonsoft.Json;

namespace aspnetsite.Libraries.Login
{
    public class LoginCliente
    {
        private string Key = "Login.Cliente";
        private Sessao.Sessao _sessao;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginCliente(Sessao.Sessao sessao, IHttpContextAccessor httpContextAccessor)
        {
            _sessao = sessao;
            _httpContextAccessor = httpContextAccessor;
        }
        public void Login(Cliente cliente)
        {
            // Serializar
            string clienteJSONString = JsonConvert.SerializeObject(cliente);
            
            _sessao.Cadastrar(Key, clienteJSONString);

            _httpContextAccessor.HttpContext.Session.SetString("ClienteLogado", "true");

        }
        // LoginCli
        //Reverter Json para objeto cliente ** Deserializar **
        public Cliente GetCliente()
        {
            // Deserializar
            if (_sessao.Existe(Key))
            {
                string clienteJSONString = _sessao.Consultar(Key);
                return JsonConvert.DeserializeObject<Cliente>(clienteJSONString);
            }
            else
            {
                return null;
            }
        }
        //Remove a sessão e desloga o Cliente
        public void Logout()
        {
            _sessao.RemoverTodos();
            _httpContextAccessor.HttpContext.Session.Remove("ClienteLogado");  // Remover indicador de login

        }
        public bool IsClienteLogado()
        {
            // Verifica se a sessão "ClienteLogado" existe
            return _httpContextAccessor.HttpContext.Session.GetString("ClienteLogado") != null;
        }
    }
}