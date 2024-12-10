using aspnetsite.Models;
using Newtonsoft.Json;

namespace aspnetsite.Libraries.Login
{
    public class LoginColaborador
    {
        private string Key = "Login.Colaborador";
        private Sessao.Sessao _sessao;
        private readonly IHttpContextAccessor _httpContextAccessor;

        //Injetar sessão na classe LoginColaborador
        public LoginColaborador(Sessao.Sessao sessao, IHttpContextAccessor httpContextAccessor)
        {
            _sessao = sessao;
            _httpContextAccessor = httpContextAccessor;
        }
        //Converte o objeto Colaborador para Json ** Serializar **
        public void Login(Colaborador colaborador)
        {
            //Serializar
            string colaboradorJSONString = JsonConvert.SerializeObject(colaborador);

            _sessao.Cadastrar(Key, colaboradorJSONString);

            _httpContextAccessor.HttpContext.Session.SetString("ColaboradorLogado", "true");

        }

        // LoginColaborador GetCliente()
        //Reverter Json para objeto Colaborador ** Deserializar **
        public Colaborador GetColaborador()
        {
            //Deserializar
            if (_sessao.Existe(Key))
            {
                string colaboradorJSONString = _sessao.Consultar(Key);
                return JsonConvert.DeserializeObject<Colaborador>(colaboradorJSONString);
            }
            else
            {
                return null;
            }
        }
        //Remove a sessão e desloga o Colaborador
        public void Logout()
        {
            _sessao.RemoverTodos();
            _httpContextAccessor.HttpContext.Session.Remove("ColaboradorLogado");  // Remover indicador de login

        }

        public bool IsColaboradorLogado()
        {
            // Verifica se a sessão "ColaboradorLogado" existe
            return _httpContextAccessor.HttpContext.Session.GetString("ColaboradorLogado") != null;
        }
    }
}