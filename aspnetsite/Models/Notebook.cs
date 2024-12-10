using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace aspnetsite.Models
{
    public class Notebook
    {
        [Display(Name = "Cod")]
        public int codNotebook { get; set; }

        [Display(Name = "Nome")]
        public string nomeNotebook { get; set; }
        public int quantidade {  get; set; }
        public string imagemNotebook1 { get; set; } // Caminho da imagem principal
        public string imagemNotebook2 { get; set; } 
        public string imagemNotebook3 { get; set; }
        public string imagemNotebook4 { get; set; }

        [Display(Name = "Preço")]
        public decimal precoNotebook { get; set; }
        public decimal descontoNotebook { get; set; }
        public decimal? valorGarantiaNotebook { get; set; }
        public decimal GarantiaSelecionada { get; set; } // Valor da garantia escolhida
        public string corNotebook {  get; set; }
        public string sistemaOperacionalNotebook { get; set; }
        public string processadorNotebook { get; set; }
        public string placaVideoNotebook { get; set; }
        public string telaNotebook { get; set; }

        [Display(Name = "Memória")]
        public string memoriaNotebook { get; set; }
        public string armazenamentoNotebook { get; set; }
        public string portasConexaoNotebook { get; set; }
        public string cameraNotebook { get; set; }
        public string audioNotebook { get; set; }
        public string redeComunicacaoNotebook { get; set; }
        public string bateriaNotebook { get; set; }
        public string fonteAlimentacaoNotebook { get; set; }
        public string pesoNotebook { get; set; }
        public string incluidoNaCaixaNotebook { get; set; }

        public List<Notebook> Carrinho { get; set; } // Produtos sendo comprados
        public decimal Total => (precoNotebook + GarantiaSelecionada) * quantidade;

    }
}