using DSPro.Models;

namespace DSPro.Services;

public interface IUserService
{
    public Task<List<UserResponse>> GetAllUsersByRole(int role);
}