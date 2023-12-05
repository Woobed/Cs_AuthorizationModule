namespace AuthorizationKP.Domain.Service.Interfaces
{
    public interface ITwoFactAuthentication
    {
        string SendConfirmCode(string email);

    }
}
