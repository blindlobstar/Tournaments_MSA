using Common.Core.Data;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserService.Core.Models
{
    public class User : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public User()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }
    }
}