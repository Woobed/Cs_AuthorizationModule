using System.ComponentModel.DataAnnotations;
namespace AuthorizationKP.Models
{
    public class TwoFactAuthenticationViewModel
    {
        [Display(Name = "Код подтверждения")]
        public string userConfirmCode { get; set; }
    }
}
