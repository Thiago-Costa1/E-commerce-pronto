using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace aspnetsite.Models
{
    public class Colaborador
    {
        
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "O senha é obrigatorio")]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 10 caracteres")]
        public string Senha { get; set; }

        /*
         * TIPO ColaboradorTipoConstant
        */
        public string? Tipo { get; set; }
    }
}
