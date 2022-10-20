using DSPro.Models;

namespace DSPro.Services;

public interface IPersonTaskService
{
    public Task<List<PersonTaskResponse>> GetAllPersonTaskList(string userId, string role);
    public Task<bool> CreatePersonTask(PersonTask task, string userId);
    public Task<bool> AssignPersonTask(string userId,string taskId);
    
}