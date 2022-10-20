using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DSPro.Models;


public class User
{
    [BsonId]
    // [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("username")]
    public string Username { get; set; }
    [BsonElement("password")]
    public string Password { get; set; }
    [BsonElement("role")]
    public int Role { get; set; }
}

public enum AppRole
{
    Admin = 1,
    User = 0
}

public class LoginRequest
{
    public string username { get; set; }
    public string password { get; set; }
}

public class UserResponse
{
    public string Id { get; set; }
    public string Username { get; set; }
    public bool IsAble { get; set; }

    // public UserResponse(
    //     int id,
    //     string username,
    //     bool isAble)
    // {
    //     Id = id;
    //     Username = username;
    //     IsAble = isAble;
    // }
}