namespace Services.Authentication
{
    public interface IAuthenticationService
    {
        string GetToken(LoginModel loginModel);
    }
}
