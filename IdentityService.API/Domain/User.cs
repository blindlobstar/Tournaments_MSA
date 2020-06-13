using Common.Core.Data;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IdentityService.API.Domain
{
    public class User : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}