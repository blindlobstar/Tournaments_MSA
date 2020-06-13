using Common.Data.MongoDB.Repositories;
using System.Threading.Tasks;
using Common.Data.MongoDB;
using MongoDB.Driver;
using UserService.Core.Data;
using UserService.Core.Models;

namespace UserService.Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IBaseContext<User> context) : base(context) { }
    }
}