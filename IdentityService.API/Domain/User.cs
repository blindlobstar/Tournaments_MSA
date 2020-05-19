using Common.Core.Data;

namespace IdentityService.API.Domain
{
    public class User : IEntity<string>
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}