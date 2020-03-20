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

        public async Task<User> Authenticate(string login, string password)
        {
            var filter = Builders<User>.Filter.Eq(user => user.Login, login) 
                         & Builders<User>.Filter.Eq(user => user.Password, password);

            return await Collection.FindSync(filter)
                .FirstOrDefaultAsync();
        }
            
    }
}