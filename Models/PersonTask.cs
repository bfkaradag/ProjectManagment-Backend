using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DSPro.Models
{
    public class PersonTask
    {
        [BsonId]
        // [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("title")] public string Title { get; set; }

        [BsonElement("description")] public string Description { get; set; }

        [BsonElement("created_at")] public DateTime? CreatedAt { get; set; }
        [BsonElement("deadline_at")] public DateTime? DeadlineAt { get; set; }

        [BsonElement("task_status")] public string TaskStatus { get; set; }
    }

    public class PersonTaskRequest
    {

        public string Title { get; set; }

        public string Description { get; set; }

    }

    public class PersonTaskResponse
    {

        public string TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        // public string TaskStatus { get; set; }
        public string AssignedBy { get; set; }
        public string AssignedTo { get; set; }
    }
}