using Common.Core.Auth;

namespace Common.Logic.Auth
{
    public class JwtOptions : IJwtOptions
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Lifetime { get; set; }
    }
}
