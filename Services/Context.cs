using DSPro.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DSPro.Repositories;

public class Context
{
    private readonly IMongoDatabase _database = null;

    public Context(IOptions<Settings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        if (client != null)
            _database = client.GetDatabase(settings.Value.Database);
    }

    public IMongoCollection<PersonTask> PersonTasks
    {
        get
        {
            return _database.GetCollection<PersonTask>("person_task");
        }
    }

    public IMongoCollection<User> User
    {
        get { return _database.GetCollection<User>("user"); }
    }

    public IMongoCollection<TaskAssigned> TaskAssigned
    {
        get { return _database.GetCollection<TaskAssigned>("task_assigned"); }
    }
}
