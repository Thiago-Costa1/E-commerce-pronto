namespace aspnetsite.Models
{
    public class CheckoutViewModel
    {
        public string NomeCliente { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string Cep { get; set; }

        
        public List<Notebook> Carrinho { get; set; } // Lista de notebooks no carrinho
        public decimal Total { get; set; } // Total calculado
        public decimal Desconto { get; set; } // Valor do desconto do cupom
        

       
        public decimal TotalFinal { get; set; } // Total final com desconto
        public decimal QtdItens { get; set; } // Quantidade comprada deste item
        public string NomeNotebook { get; set; }
        public string ImagemNotebook1 { get; set; }
        public int Quantidade { get; set; }
        public decimal GarantiaSelecionada { get; set; }
        public decimal PrecoTotal { get; set; }
        public string FormaPagamento { get; set; } // Exemplo: Cartão, Boleto
        public List<Notebook> Produtos { get; set; } // Produtos sendo comprados
    }
}

///
// Pedido 
//IdPedidod
// IdCliente
// DataPedido

//Itens
// IdInten
// IdPedido
// IdPorduto
// ValorParcial (Soma dos itens)
// ValorTotal (Soma con desconto)
// QtdItens (10)

