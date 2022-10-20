using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using DSPro.Models;
using DSPro.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DSPro.Controllers;

[Authorize]
[Route("api/task")]

public class TaskController
{
    private readonly IPersonTaskService _personTaskService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TaskController(IPersonTaskService personTaskService, IHttpContextAccessor httpContextAccessor)
    {
        _personTaskService = personTaskService;
        _httpContextAccessor = httpContextAccessor;
    }
    [HttpPost("create")]
    public async Task<BaseResponse<bool>> Create([FromBody] PersonTaskRequest request)
    {
        var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(Constants.UserIdPref).Value;
        try
        {
            var response = new BaseResponse<bool>(
                payload: false,
                statusCode: HttpStatusCode.OK,
                friendlyMessage: null
            );

            var personTask = new PersonTask
            {
                Title = request.Title,
                Description = request.Description,
                CreatedAt = DateTime.Now,
                DeadlineAt = DateTime.Now.AddDays(2),
                TaskStatus = "unassigned",

            };
            var result = await _personTaskService.CreatePersonTask(personTask, currentUserId);
            if (result)
            {
                var friendlyMessage = new FriendlyMessage {Title = "Created", Message = "Created Successfully."};
                response.Payload = true;
                response.FriendlyMessage = friendlyMessage;
            }
            else
            {
                var friendlyMessage = new FriendlyMessage {Title = "Not Created", Message = "Create Failed."};
                response.Payload = true;
                response.FriendlyMessage = friendlyMessage;
            }

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<bool>(payload: false,
                statusCode: HttpStatusCode.InternalServerError,
                friendlyMessage: new FriendlyMessage {Title = "Error", Message = "Error"});
        }
       
    }

    [HttpGet("all")]
    public async Task<BaseResponse<List<PersonTaskResponse>>> GetAll()
    {

        try
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(Constants.UserIdPref).Value;
            var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirst(Constants.Role).Value;

            var response = new BaseResponse<List<PersonTaskResponse>>(
                payload: null,
                statusCode: HttpStatusCode.OK,
                friendlyMessage: null
            );


            var result = await _personTaskService.GetAllPersonTaskList(currentUserId, currentUserRole);
            if (result.Count > 0)
            {
                var friendlyMessage = new FriendlyMessage
                    {Title = "Success", Message = "All data fetched successfuly."};
                response.Payload = result;
                response.FriendlyMessage = friendlyMessage;
            }
            else
            {
                var friendlyMessage = new FriendlyMessage {Title = "Not Found", Message = "Not Found"};
                response.FriendlyMessage = friendlyMessage;
                response.StatusCode = HttpStatusCode.NotFound;
            }

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<List<PersonTaskResponse>>(payload: null,
                statusCode: HttpStatusCode.InternalServerError,
                friendlyMessage: new FriendlyMessage {Title = "Error", Message = "Error"});
        }

    }

    [HttpPut("assign-task/{taskId}")]
    public async Task<BaseResponse<bool>> AssignPersonTask(string taskId)
    {
        try
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(Constants.UserIdPref).Value;

            var result = await _personTaskService.AssignPersonTask(currentUserId, taskId);
            if (result)
            {
                return new BaseResponse<bool>(
                    payload: true,
                    statusCode: HttpStatusCode.OK,
                    friendlyMessage: new FriendlyMessage {Title = "Updated", Message = "Updated Succeessfully"});
            }

            return new BaseResponse<bool>(
                payload: false,
                statusCode: HttpStatusCode.NoContent,
                friendlyMessage: new FriendlyMessage {Title = "Failed", Message = "Update Failed"});

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<bool>(payload: false,
                statusCode: HttpStatusCode.InternalServerError,
                friendlyMessage: new FriendlyMessage {Title = "Error", Message = "Error"});
        }
    }
}