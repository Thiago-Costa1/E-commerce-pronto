using aspnetsite.Models;

namespace aspnetsite.Repository.Contract
{
    public interface IClienteRepository
    {
        // Login Cliente
        Cliente Login(string Email, string Senha);

        //CRUD
        void Cadastrar(Cliente cliente);
        void Atualizar(Cliente cliente);
        void Ativar(int id);
        void Desativar(int id);
        void Excluir(int Id);
        Cliente ObterCliente(int Id);

        Cliente BuscaEmailCliente(string email);

        Cliente BuscaCpfCliente(string CPF);

        IEnumerable<Cliente> ObterTodosClientes();
        // IPagedList<Cliente> ObterTodosClientes(int? pagina, string pesquisa);

    }
}
