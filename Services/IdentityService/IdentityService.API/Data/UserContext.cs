using Common.Data.MongoDB;
using Common.Data.MongoDB.Models;
using IdentityService.API.Domain;

namespace IdentityService.API.Data
{
    public class UserContext : BaseContext<User>
    {
        public UserContext(IDatabaseSettings settings) : base(settings) { }
    }
}