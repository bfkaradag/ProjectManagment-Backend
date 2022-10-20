using System.Net;
using DSPro.Models;
using DSPro.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DSPro.Controllers;

[Route("api/auth")]

public class AuthController
{
    private readonly IJwtHandler _jwtHandler;
    private readonly IAuthService _authService;

    public AuthController(IJwtHandler jwtHandler, IAuthService authService)
    {
        _jwtHandler = jwtHandler;
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<BaseResponse<JwtTokenResource>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var response = new BaseResponse<JwtTokenResource>(
                payload: null,
                statusCode: HttpStatusCode.OK,
                friendlyMessage: null
            );
            var user = await _authService.Authenticate(request);
            if (user == null)
            {
                var friendlyMessage = new FriendlyMessage {Title = "Not Found", Message = "User Not Found"};
                response.StatusCode = HttpStatusCode.NotFound;
                response.FriendlyMessage = friendlyMessage;
            }

            else if (user.Password != request.password)
            {
                var friendlyMessage = new FriendlyMessage {Title = "Wrong", Message = "Username or password is wrong"};
                response.StatusCode = HttpStatusCode.Forbidden;
                response.FriendlyMessage = friendlyMessage;
            }

            else
            {
                var friendlyMessage = new FriendlyMessage {Title = "Success", Message = "You login sucessfully."};
                response.Payload = _jwtHandler.CreateAccessToken(user.Id, user.Username, user.Role);
                response.FriendlyMessage = friendlyMessage;
            }
      
       
            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<JwtTokenResource>(payload: null,
                statusCode: HttpStatusCode.InternalServerError,
                friendlyMessage: new FriendlyMessage {Title = "Error", Message = "Error"});
        }
    }
    
}