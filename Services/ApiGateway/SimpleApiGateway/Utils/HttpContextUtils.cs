using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SimpleApiGateway.Utils
{
    public static class HttpContextUtils
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            return identity.Claims.First(x => x.Type == ClaimTypes.Name).Value;
        }
    }
}
