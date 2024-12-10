namespace aspnetsite.Models
{
    public class Pedido
    {
        public int IdPedido { get; set; } // Identificador único do pedido
        public int IdCliente { get; set; }
        public DateTime DataPedido { get; set; }
        public decimal ValorTotal { get; set; } // Valor total do pedido
        public decimal NomeProduto { get; set; } // Valor total do pedido
        public List<Itens> Itens { get; set; } = new List<Itens>(); // Relacionamento: Um pedido pode ter vários produtos


    }
}
