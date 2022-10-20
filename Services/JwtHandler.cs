using System.Security.Claims;
using DSPro.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DSPro.Services;

public class JwtHandler: IJwtHandler
{
    private readonly IConfiguration _configuration;
    public JwtHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public JwtTokenResource CreateAccessToken(string userId,string username, int role)
    {
        var now = DateTime.UtcNow;
        var claims = new Claim[]
        {
           new Claim(Constants.UserIdPref, userId.ToString()),
            new Claim(Constants.Username, username),
            new Claim(Constants.Role, role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = now.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"]));

        var jwt = new JwtSecurityToken(
            _configuration["Jwt:Issuer"], 
            _configuration["Jwt:Audience"],
            claims,
            now,
            expiry,
            credentials
            );
        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        var unixTimestamp = (int)now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        
        return new JwtTokenResource
        {
            Token = token,
            Expiry = unixTimestamp
        };;

    }

    public JwtTokenResource CreateRefreshToken(string userId,string username)
    {
        var now = DateTime.UtcNow;
        var claims = new Claim[]
        {
            new Claim(Constants.UserIdPref, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, username),
            new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = now.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"]));

        var jwt = new JwtSecurityToken(
            _configuration["Jwt:Issuer"], 
            _configuration["Jwt:Audience"],
            claims,
            now,
            expiry,
            credentials
        );
        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        var unixTimestamp = (int)now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        return new JwtTokenResource
        {
            Token = token,
            Expiry = unixTimestamp
        };
    }
}