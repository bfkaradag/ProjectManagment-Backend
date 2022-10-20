using DSPro.Models;
using DSPro.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DSPro.Services;

public class PersonTaskService:IPersonTaskService
{
    private readonly Context _context = null;

    public PersonTaskService(IOptions<Settings> settings)
    {
        _context = new Context(settings);
    }

    public async Task<List<PersonTaskResponse>> GetAllPersonTaskList(string userId, string role)
    {
        try
        {
            var query = _context.TaskAssigned.AsQueryable();
            if (role == "1")
            {
                query = query.Where(x => x.AssignedBy == userId);
            }
            var list = await query.Join(_context.User.AsQueryable(),
                ta => ta.AssignedBy,
                u => u.Id,
                (ta, u) => new
                {
                    AssignedTo = ta.AssignedTo ?? "-",
                    AssignedBy = ta.AssignedBy,
                    TaskId = ta.TaskId
                }
            ).Join(_context.PersonTasks.AsQueryable(),

                taU => taU.TaskId,
                pt => pt.Id,
                (taU, pt) => new PersonTaskResponse
                {
                    TaskId = taU.TaskId,
                    Title = pt.Title,
                    Description = pt.Description,
                    AssignedTo = taU.AssignedTo,
                    AssignedBy = taU.AssignedBy
                }
            ).ToListAsync();

            foreach (var x in list)
            {
                x.AssignedTo = await _context.User.AsQueryable().Where(y => y.Id == x.AssignedTo).Select(s => s.Username).FirstOrDefaultAsync();
            }
            
                // .Join(_context.User.AsQueryable(),
                //     taU2 => taU2.AssignedTo,
                //     u2 => u2.Id,
                //     (taU2, u2) => new PersonTaskResponse
                //     {
                //         AssignedTo = u2.Username,
                //         TaskId = taU2.TaskId,
                //         Title = taU2.Title,
                //         Description = taU2.Description,
                //         AssignedBy = taU2.AssignedBy
                //     }
                // ).ToListAsync();
       
            return list;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<bool> CreatePersonTask(PersonTask task, string userId)
    {
        try
        {
            task.Id = Guid.NewGuid().ToString();
            await _context.PersonTasks.InsertOneAsync(task);
            var taskAssigned = new TaskAssigned{AssignedBy = userId, AssignedTo = null, TaskId = task.Id};
            await _context.TaskAssigned.InsertOneAsync(taskAssigned);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> AssignPersonTask(string userId, string taskId)
    {
        try
        {
            var hasMaxTask = await _context.TaskAssigned.AsQueryable().CountAsync(x => x.AssignedTo == userId) >= 2;
            if (hasMaxTask)
            {
                return false;
            }
            var filter = Builders<TaskAssigned>.Filter.Eq("task_id", taskId);
            var update = Builders<TaskAssigned>.Update.Set("assigned_to", userId);


            var result = await _context.TaskAssigned.UpdateOneAsync(filter, update);
            if(result.ModifiedCount > 0) return true;
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

}