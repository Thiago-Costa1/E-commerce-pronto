using aspnetsite.Models;

namespace aspnetsite.Repository.Contract
{
    public interface INotebookRepository
    {
        // CRUD

        IEnumerable<Notebook> ObterTodosNotebooks();

        void Cadastrar(Notebook notebook);

        void Atualizar(Notebook notebook);

        Notebook ObterNotebooks(int Id);

        void Excluir(int Id);

    }
}
