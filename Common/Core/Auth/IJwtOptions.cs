using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Core.Auth
{
    public interface IJwtOptions
    {
        string SecretKey { get; set; }
        string Issuer { get; set; }
        string Audience { get; set; }
        int Lifetime { get; set; }
    }
}
