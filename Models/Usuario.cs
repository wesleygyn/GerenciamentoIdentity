using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GerenciamentoIdentity.Models
{
    public class Usuario : IdentityUser
    {
        [Required]
        [Display(Name = "Matrícula")]
        [StringLength(10, ErrorMessage = "O campo {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 1)]
        public string Matricula { get; set; }

        public DateTime Admissao { get; set; }
        public DateTime Nascimento { get; set; }

        [Required]
        [Display(Name = "Documento")]
        [StringLength(19, ErrorMessage = "O campo {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 11)]
        public string Documento { get; set; }
        public bool Ativo { get; set; }
        public bool UsuarioSistema { get; set; }

        [Required]
        [Display(Name = "Nome")]
        [StringLength(50, ErrorMessage = "O campo {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 10)]
        public string Nome { get; set; }
        public bool Sexo { get; set; }

        [Required]
        [Display(Name = "Telefone")]
        [StringLength(15, ErrorMessage = "O campo {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 8)]
        public string Telefone { get; set; }

        [Required]
        [Display(Name = "Cep")]
        [StringLength(10, ErrorMessage = "O campo {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 9)]
        public string Cep { get; set; }

        [Required]
        [Display(Name = "Ibge")]
        [StringLength(10, ErrorMessage = "O campo {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 1)]
        public string Ibge { get; set; }

        [Required]
        [Display(Name = "Logradouro")]
        [StringLength(50, ErrorMessage = "O campo {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 5)]
        public string Logradouro { get; set; }


        [Display(Name = "Complemento")]
        [StringLength(50)]
        public string Complemento { get; set; }

        [Required]
        [Display(Name = "Número")]
        [StringLength(10, ErrorMessage = "O campo {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 1)]
        public string Numero { get; set; }

        [Required]
        [Display(Name = "Bairro")]
        [StringLength(50, ErrorMessage = "O campo {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        public string Bairro { get; set; }

        [Required]
        [Display(Name = "Município")]
        [StringLength(50, ErrorMessage = "O campo {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        public string Municipio { get; set; }

        [Required]
        [Display(Name = "Uf")]
        [StringLength(2, ErrorMessage = "O campo {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 2)]
        public string Uf { get; set; }
    }
}
