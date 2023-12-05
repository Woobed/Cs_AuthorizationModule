namespace AuthorizationKP.Domain.Entity
{
    public class Users
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string NormalizedEmail { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }

        public bool EmailConfirm { get; set; }
        public string Role { get; set; }
    }

}
