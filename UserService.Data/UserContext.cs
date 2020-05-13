using Common.Data.MongoDB;
using Common.Data.MongoDB.Models;
using UserService.Core.Models;

namespace UserService.Data
{
    public class UserContext : BaseContext<User>
    {
        public UserContext(IDatabaseSettings settings) : base(settings) { }
    }
}