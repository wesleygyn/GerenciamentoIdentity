using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GerenciamentoIdentity.Models.ViewModels
{
    public class UpdateUserManagerViewModel
    {
        [Display(Name = "Código")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Usuário")]
        [StringLength(100, ErrorMessage = "O {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100, ErrorMessage = "O {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone]
        [StringLength(15, ErrorMessage = "O {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 8)]
        [Display(Name = "Telefone")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Confirmar email")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Confirmar telefone")]
        public bool PhoneNumberConfirmed { get; set; }

        public bool LockoutEnabled { get; set; }

        public DateTime? LockoutEnd { get; set; }
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