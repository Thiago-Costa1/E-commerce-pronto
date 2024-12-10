using System.Collections.Generic;
using aspnetsite.Models;


namespace aspnetsite.Repository.Contract
{
    public interface IPedidoRepository
    {        
        void CriarPedido(Pedido pedido); // Salvar um novo pedido
        List<Pedido> ObterPedidosPorCliente(int idCliente); // Obter histórico de pedidos de um cliente      
    }
}
