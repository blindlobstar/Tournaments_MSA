using System.Threading.Tasks;
using Common.Core.Data;
using IdentityService.API.Domain;

namespace IdentityService.API.Repositories
{
    public interface IUserRepository : IBaseRepository<User, string>
    {
        Task<User> Authenticate(string login, string password);
        Task<User> GetByLogin(string login);
    }
}