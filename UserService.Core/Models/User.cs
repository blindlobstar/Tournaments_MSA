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
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int TournamentWinsCount { get; set; }
        public int TournamentsCount { get; set; }
         
        public User()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }
    }
}