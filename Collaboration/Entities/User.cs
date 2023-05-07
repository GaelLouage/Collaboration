using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Collaboration.Enums;

namespace Collaboration.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public UserRole Role { get; set; }
    }

}
