using Common.Data.MongoDB.Repositories;
using System.Threading.Tasks;
using Common.Data.MongoDB;
using MongoDB.Driver;
using UserService.Core.Data;
using UserService.Core.Models;

namespace UserService.Data.Repositories
{
    public class UserRepository : BaseRepository<User, string>, IUserRepository
    {
        public UserRepository(IBaseContext<User> context) : base(context) { }

        public async Task<User> Authenticate(string login, string password) =>
            await Collection.Find(u => string.Compare(u.Login, login, false) == 0
                                       && string.Compare(u.Password, password, false) == 0)
                .FirstOrDefaultAsync();
    }
}