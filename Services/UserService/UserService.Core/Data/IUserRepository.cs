using Common.Data.MongoDB.Repositories;
using UserService.Core.Models;

namespace UserService.Core.Data
{
    public interface IUserRepository : IMongoRepository<UserDto>
    {

    }
}