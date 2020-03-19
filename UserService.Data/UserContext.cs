using Common.Data.MongoDB;
using Common.Data.MongoDB.Models;
using MongoDB.Driver;
using UserService.Core.Models;

namespace UserService.Data
{
    public class UserContext : BaseContext<User>, IBaseContext<User>
    {
        public UserContext(IDatabaseSettings settings) : base(settings) { }
    }
}