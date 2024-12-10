namespace aspnetsite.Models
    {
        public class Itens
        {
            public int IdItem { get; set; } // ID do item no banco de dados
            public int IdPedido { get; set; } // ID do pedido ao qual este item pertence
            public int IdProduto { get; set; } // ID do produto adicionado ao pedido
            public decimal Preco { get; set; } // Preço do produto no momento da compra
            public decimal Garantia { get; set; } // Valor da garantia escolhida
            public decimal ValorParcial { get; set; }
            public decimal ValorTotal { get; set; } // Valor total deste item (quantidade * valor unitário)
            public int QtdItens { get; set; } // Quantidade comprada deste item
            public string NomeProduto { get; set; } // Nome do produto
        }
    }   