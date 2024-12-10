namespace aspnetsite.Models
{
    public class ProdutoPedido
    {
            public int Id { get; set; } // Identificador único do item no pedido
            public int ProdutoId { get; set; } // Relaciona ao notebook cadastrado
            public string NomeProduto { get; set; } // Nome do notebook
            public int Quantidade { get; set; } // Quantidade comprada
            public decimal Preco { get; set; } // Preço unitário do produto
            public decimal GarantiaSelecionada { get; set; } // Valor da garantia escolhida

            // Relacionamento: Vincula este item ao Pedido
            public int PedidoId { get; set; } // ID do pedido ao qual pertence
            public Pedido Pedido { get; set; }
    }
}   