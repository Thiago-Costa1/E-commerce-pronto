using System.ComponentModel.DataAnnotations;

namespace aspnetsite.Models
{
    public class Cliente
    {
        /* PK */
        [Display(Name = "Código", Description = "Código.")]
        public int Id { get; set; }

        [Display(Name = "Nome completo", Description = "Nome e Sobrenome.")]
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        public string Nome { get; set; }

        [Display(Name = "Nome da rua ou avenida", Description = "Nome de rua/avenida.")]
        [Required(ErrorMessage = "O nome do endereco é necessario.")]
        public string Endereco { get; set; }

        [Display(Name = "Nome da cidade", Description = "Nome da cidade.")]
        [Required(ErrorMessage = "O nome da cidade é obrigatório.")]
        public string Cidade { get; set; }

        [Display(Name = "Cep", Description = "Cep.")]
        [Required(ErrorMessage = "O nome do cep obrigatório.")]

        public string Cep { get; set; }

        [Display(Name = "Nascimento")]
        [Required(ErrorMessage = "A data é obrigatorio")]
        public DateTime Nascimento { get; set; }

        [Display(Name = "Sexo")]
        [Required(ErrorMessage = "Selecione uma opção")]
        [StringLength(1, ErrorMessage = "Deve conter 1 caracter")]
        public string Sexo { get; set; }

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "O CPF é obrigatorio")]
        public string CPF { get; set; }

        [Display(Name = "Celular")]
        [Required(ErrorMessage = "O Celular é obrigatorio")]
        public string Telefone { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = " O Email não é valido")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um email válido...")]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "O senha é obrigatorio")]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 10 caracteres")]
        public string Senha { get; set; }

        [Display(Name = "Situação")]
        public string? Situacao { get; set; }

        public List<Pedido> Pedidos { get; set; } = new List<Pedido>();

    }
}
