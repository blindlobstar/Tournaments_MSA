using System.Threading.Tasks;
using Common.Data.MongoDB;
using Common.Data.MongoDB.Repositories;
using IdentityService.API.Domain;
using MongoDB.Driver;

namespace IdentityService.API.Repositories
{
    public class UserRepository : BaseRepository<User, string>, IUserRepository
    {
        public UserRepository(IBaseContext<User> context) : base(context) { }

        public Task<User> Authenticate(string login, string password)
        {
            var filter = Builders<User>.Filter.Eq(user => user.Login, login) 
                         & Builders<User>.Filter.Eq(user => user.Password, password);

            return Collection.FindSync(filter)
                .FirstOrDefaultAsync();
        }

        public Task<User> GetByLogin(string login)
        {
            var filter = Builders<User>.Filter.Eq(user => user.Login, login);

            return Collection.FindSync(filter)
                .FirstOrDefaultAsync();
        }
    }
}