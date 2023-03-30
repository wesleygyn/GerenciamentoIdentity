using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GerenciamentoIdentity.Models.ViewModels
{
    public class ResetPasswordViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Informe uma nova senha")]
        //[StringLength(100, ErrorMessage = "O {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova senha")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Informe a confirmação da senha")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmação da senha")]
        [Compare("Password", ErrorMessage = "A senha e a senha de confirmação não correspondem.")]
        public string Verify { get; set; }
    }
}
