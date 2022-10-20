using DSPro.Models;

namespace DSPro.Services;

public interface IAuthService
{
    public Task<User> Authenticate(LoginRequest request);

}