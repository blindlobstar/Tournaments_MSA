using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;

namespace IdentityService.API.Utils
{
    public static class HashUtils
    {
        public static string CreateHash(this string password, byte[] salt) =>
            Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
    }
}
