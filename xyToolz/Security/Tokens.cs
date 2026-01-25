using System.Security.Claims;

namespace xyToolz.Security
{
    /// <summary>
    /// Use-case facade for JWT token handling.
    /// Pure pass-through to internal RSA implementation.
    /// </summary>
    public static class Tokens
    {

        // Setup 

        public static Task<bool> LoadKeysAsync(string publicKeyPem, string privateKeyPem)
            => xyRsa.LoadKeysAsync(publicKeyPem, privateKeyPem);

        public static Task<bool> ConfigureAsync(string issuer, string audience)
            => xyRsa.ConfigureAsync(issuer, audience);


        // Token creation 

        public static Task<string> CreateAsync(
            IDictionary<string, object> claims,
            TimeSpan validFor)
            => xyRsa.GenerateJwtAsync(claims, validFor);


        // Token validation 

        public static Task<ClaimsPrincipal?> ValidateAsync(
            string token,
            bool validateLifetime = true)
            => xyRsa.ValidateJwtAsync(token, validateLifetime);



    }
}
