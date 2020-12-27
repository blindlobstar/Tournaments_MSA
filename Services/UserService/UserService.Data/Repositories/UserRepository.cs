using Common.Data.MongoDB.Repositories;
using System.Threading.Tasks;
using Common.Data.MongoDB;
using MongoDB.Driver;
using UserService.Core.Data;
using UserService.Core.Models;

namespace UserService.Data.Repositories
{
    public class UserRepository : BaseRepository<UserDto>, IUserRepository
    {
        public UserRepository(IBaseContext<UserDto> context) : base(context) { }
    }
}