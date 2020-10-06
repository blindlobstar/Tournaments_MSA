using System.Threading.Tasks;

namespace Common.Core.Auth
{
    public interface IAccessTokenService
    {
        Task<bool> IsActive(IJwtToken token);
        Task Deactivate(IJwtToken token);
    }
}