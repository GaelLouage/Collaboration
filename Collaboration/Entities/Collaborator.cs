using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Collaboration.Entities
{
    public class Collaborator
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public int ItineraryId { get; set; }
        public int UserId { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
