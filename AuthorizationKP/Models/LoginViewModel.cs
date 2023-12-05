using System.ComponentModel.DataAnnotations;

namespace AuthorizationKP.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Заполните поле")]
        [MinLength(4, ErrorMessage = "Введите не менее 4 символов")]
        [MaxLength(35, ErrorMessage = "Введите не более 35 символов")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Заполните поле")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Введите не менее 4 символов")]
        [MaxLength(35, ErrorMessage = "Введите не более 35 символов")]
        [Display(Name = "Пароль")]

        public string PasswordHash { get; set; }
    }
}
