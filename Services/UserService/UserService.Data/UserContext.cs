using Common.Data.MongoDB;
using Common.Data.MongoDB.Models;
using UserService.Core.Models;

namespace UserService.Data
{
    public class UserContext : BaseContext<UserDto>
    {
        public UserContext(IDatabaseSettings settings) : base(settings) { }
    }
}