namespace BlazorEcommerce.Client.Services.AuthenticationService;

public interface IAuthService
{
    Task<ServiceResponse<int>> Register(UserRegister request);
}
