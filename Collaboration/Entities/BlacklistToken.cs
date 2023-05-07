using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Collaboration.Entities
{
    public class BlacklistToken
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Token { get; set; }

        public DateTime ExpirationTime { get; set; }
    }
}
