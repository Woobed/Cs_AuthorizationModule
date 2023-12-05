namespace AuthorizationKP.Domain.ViewModels.Users
{
    public class UsersViewModel
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string NormalizedEmail { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }

    }
}
