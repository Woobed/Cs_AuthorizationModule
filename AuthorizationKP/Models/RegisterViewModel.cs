

using System.ComponentModel.DataAnnotations;

namespace AuthorizationKP.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Заполните поле1")]
        [MinLength(2,ErrorMessage = "Введите не менее 2 символов")]
        [MaxLength(35, ErrorMessage = "Введите не более 35 символов")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Заполните поле2")]
        [MinLength(2, ErrorMessage = "Введите не менее 2 символов")]
        [MaxLength(35, ErrorMessage = "Введите не более 35 символов")]
        [Display(Name = "Фамилия")]
        public string SecondName { get; set; }


        [Required(ErrorMessage = "Заполните поле3")]
        [MinLength(2, ErrorMessage = "Введите не менее 2 символов")]
        [MaxLength(50, ErrorMessage = "Введите не более 50 символов")]
        [Display(Name = "Адрес электронной почты")]
        public string NormalizedEmail { get; set; }

        [Required(ErrorMessage = "Заполните поле5")]
        [MinLength(4, ErrorMessage = "Введите не менее 4 символов")]
        [MaxLength(35, ErrorMessage = "Введите не более 35 символов")]
        [Display(Name = "Логин")]
        public string Login { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Заполните поле6")]
        [MinLength(4, ErrorMessage = "Введите не менее 4 символов")]
        [MaxLength(35, ErrorMessage = "Введите не более 35 символов")]
        [Display(Name = "Пароль")]
        public string PasswordHash { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Подтвердите пароль")]
        [Compare("PasswordHash",ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтверждение пароля")]
        public string PasswordConfirm { get; set; }

        public bool EmailConfirmed { get; set; }

        public int systemConfirmCode { get; set; }
        //public int userConfirmCode { get; set; }



    }
}
