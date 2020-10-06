using System.Collections.Generic;

namespace Common.Core.Auth
{
    public interface IJwtService
    {
        IJwtToken CreateToken(string userId, string role, Dictionary<string, string> claims = null);
    }
}