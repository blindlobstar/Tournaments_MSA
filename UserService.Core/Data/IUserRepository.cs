using System.Threading.Tasks;
using Common.Core.Data;
using UserService.Core.Models;

namespace UserService.Core.Data
{
    public interface IUserRepository : IBaseRepository<User, string>
    {

    }
}