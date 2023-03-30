using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GerenciamentoIdentity.Models.ViewModels
{
    public class UserManagerViewModel
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

        [Required]
        //[StringLength(100, ErrorMessage = "O {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmação da senha")]
        [Compare("Password", ErrorMessage = "A senha e a senha de confirmação não correspondem.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Confirmar email")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Confirmar telefone")]
        public bool PhoneNumberConfirmed { get; set; }

        public bool LockoutEnabled { get; set; }

        public DateTime? LockoutEnd { get; set; }
    }
}