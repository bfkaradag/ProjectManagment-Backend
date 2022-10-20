using DSPro.Models;
using MongoDB.Driver;

namespace DSPro.Tools;

public class SeedData
{
    public static async Task EnsureSeedData(IServiceProvider service)
    {


        try
    {
        IMongoDatabase _database = null;
        var settings = service.GetRequiredService<IConfiguration>();
        var client = new MongoClient(settings.GetSection("MongoConnection:ConnectionString").Value);
        if (client != null)
            _database= client.GetDatabase(settings.GetSection("MongoConnection:Database").Value);

        var collectionList = await _database.ListCollections().ToListAsync();
        if (collectionList.Count < 2)
        {
            _database.CreateCollection("person_task");
            _database.CreateCollection("user");
            _database.CreateCollection("task_assigned");

            var userCollection = _database.GetCollection<User>("user");
            var users = new List<User>
            {
                new User {Id = "6693076c-3904-4b50-ae0e-c52c35e5f4c0", Username = "admin", Password = "21232f297a57a5a743894a0e4a801fc3", Role = 1},
                new User {Id = "cf7322dc-d59f-4b65-93dc-4adbafc51d7b", Username = "user1", Password = "24c9e15e52afc47c225b757e7bee1f9d", Role = 0},
                new User {Id = "7be7f0b0-1bc0-4fc9-8dcf-fe1e27df33ef", Username = "user2", Password = "7e58d63b60197ceb55a1c487989a3720", Role = 0},
                new User {Id = "77af8bd0-e47d-44b0-b640-16cab8abf9fb", Username = "user3", Password = "92877af70a45fd6a2ed7fe81e1236b78", Role = 0},

            };
            userCollection.InsertMany(users);
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
    
        
    }
}