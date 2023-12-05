using System.ComponentModel.DataAnnotations;

namespace AuthorizationKP.Models
{
    public class ChangePasswordViewModel
    {
        
        public string Login {  get; set; }

        [Required(ErrorMessage = "Заполните поле")]
        [MinLength(2, ErrorMessage = "Введите не менее 5 символов")]
        [MaxLength(50, ErrorMessage = "Введите не более 50 символов")]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }
    }
}
