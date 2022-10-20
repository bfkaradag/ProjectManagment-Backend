using System.Net;
using DSPro.Models;
using DSPro.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DSPro.Services;

public class UserService: IUserService
{
    private readonly Context _context = null;

    public UserService(IOptions<Settings> settings)
    {
        _context = new Context(settings);
    }
    public async Task<List<UserResponse>> GetAllUsersByRole(int role)
    {
        try
        {
            var users =  _context.User.AsQueryable()
                .Where(x => x.Role == role)
                .Select(x => new UserResponse
                {
                    Id = x.Id,
                    Username = x.Username,

                }).ToList();
            

            foreach (var usr in users)
            {
                var isAble = _context.TaskAssigned.AsQueryable().Where(x => x.AssignedTo == usr.Id).Count() < 2;
                usr.IsAble = isAble;
            }

            return users;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}