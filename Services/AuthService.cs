using System.Net;
using DSPro.Models;
using DSPro.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DSPro.Services;

public class AuthService: IAuthService
{
    private readonly Context _context;

    public AuthService(IOptions<Settings> settings)
    {
        _context = new Context(settings);
    }

    public async Task<User> Authenticate(LoginRequest request)    {
        return await _context.User.Find(x => x.Username == request.username).FirstOrDefaultAsync();
    }
}