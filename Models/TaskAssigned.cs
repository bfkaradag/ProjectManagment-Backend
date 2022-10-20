using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DSPro.Models;

public class TaskAssigned
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonElement("task_id")] 
    public string TaskId { get; set; }

    [BsonElement("assigned_by")]
    public string AssignedBy { get; set; }
    [BsonElement("assigned_to")]
    public string AssignedTo { get; set; }
}