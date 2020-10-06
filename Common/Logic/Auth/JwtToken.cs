using Common.Core.Auth;

namespace Common.Logic.Auth
{
    public class JwtToken : IJwtToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Id { get; set; }
    }
}