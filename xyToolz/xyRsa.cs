using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using xyToolz.Helper.Logging;

namespace xyToolz
{


    /// <summary>
    /// Utility class for handling JWT creation and validation using RSA public/private key encryption.
    ///
    /// <para><b>Available Features:</b></para>
    /// <list type="bullet">
    ///   <item><description>Asymmetric key loading from PEM (public/private).</description></item>
    ///   <item><description>JWT generation with arbitrary claims and expiration.</description></item>
    ///   <item><description>Token validation with issuer, audience, signature and lifetime checks.</description></item>
    ///   <item><description>Public key export in PEM format for client distribution.</description></item>
    /// </list>
    ///
    /// <para><b>Thread Safety:</b></para>
    /// Not thread-safe due to internal static RSA references. Must not be used concurrently across multiple threads.
    ///
    /// <para><b>Limitations:</b></para>
    /// No support for rotating keys or refresh tokens out of the box.
    ///
    /// <para><b>Example Usage:</b></para>
    /// <code>
    /// await xyRsa.LoadKeysAsync(pubKey, privKey);
    /// await xyRsa.ConfigureAsync("https://myapi", "myAudience");
    /// string jwt = await xyRsa.GenerateJwtAsync(claims, TimeSpan.FromHours(1));
    /// var user = await xyRsa.ValidateJwtAsync(jwt);
    /// </code>
    /// </summary>

    public static class xyRsa
    {
        private static RSA? _privateRsaKey;
        private static RSA? _publicRsaKey;
        private static string? _issuer;
        private static string? _audience;

        /// <summary>
        /// Loads RSA public and private Keys from PEM-formatted strings and initializes internal key containers.
        /// </summary>
        /// <param name="publicKeyPem">PEM-formatted public key string.</param>
        /// <param name="privateKeyPem">PEM-formatted private key string.</param>
        /// <returns>True if keys were loaded successfully; otherwise, false.</returns>
        public static async Task<bool> LoadKeysAsync(string publicKeyPem, string privateKeyPem)
        {
            string success = "RSA keys were successfully loaded.";
            string noPem = "Provided PEM string is null or empty.";
            string fail = "Failed to load RSA keys from PEM input.";

            if (string.IsNullOrWhiteSpace(publicKeyPem) || string.IsNullOrWhiteSpace(privateKeyPem))
            {
                await xyLog.AsxLog(noPem);
                return false;
            }

            try
            {
                _privateRsaKey = RSA.Create();
                _privateRsaKey.ImportFromPem(privateKeyPem.ToCharArray());

                _publicRsaKey = RSA.Create();
                _publicRsaKey.ImportFromPem(publicKeyPem.ToCharArray());

                await xyLog.AsxLog(success);
                return true;
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                await xyLog.AsxLog(fail);
                return false;
            }
        }


        /// <summary>
        /// Configures the issuer and audience values for JWT generation and validation.
        /// </summary>
        /// <param name="issuer">The issuer identifier to use in the token.</param>
        /// <param name="audience">The audience identifier for the token.</param>
        /// <returns>True if both values were set successfully; otherwise, false.</returns>
        public static async Task<bool> ConfigureAsync(string issuer, string audience)
        {
            string logSuccess = "Issuer and Audience configured successfully.";
            string logError = "Issuer or Audience input was null or empty.";

            if (string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
            {
                await xyLog.AsxLog(logError);
                return false;
            }

            _issuer = issuer;
            _audience = audience;

            await xyLog.AsxLog($"{logSuccess} Issuer = {_issuer}, Audience = {_audience}");
            return true;
        }

        /// <summary>
        /// Generates a signed JSON Web Token (JWT) using the configured RSA private key and provided claims.
        /// </summary>
        /// <param name="claims">A dictionary containing the custom claims to include in the token payload.</param>
        /// <param name="validFor">The duration for which the token should remain valid.</param>
        /// <returns>
        /// The encoded JWT string if successful; otherwise, an empty string if key is missing or an error occurs.
        /// </returns>
        public static async Task<string> GenerateJwtAsync(IDictionary<string, object> claims, TimeSpan validFor)
        {
            string success = "JWT generated successfully.";
            string noKey = "Private RSA key not loaded. Cannot generate token.";
            string logError = "An error occurred while generating the JWT.";

            if (_privateRsaKey == null)
            {
                await xyLog.AsxLog(noKey);
                return string.Empty;
            }

            try
            {
                SigningCredentials credentials = new SigningCredentials(new RsaSecurityKey(_privateRsaKey), SecurityAlgorithms.RsaSha256);

                SecurityTokenDescriptor tokenDescriptor = new()
                {
                    Issuer = _issuer,
                    Audience = _audience,
                    Expires = DateTime.UtcNow.Add(validFor),
                    Claims = claims,
                    SigningCredentials = credentials
                };

                JwtSecurityTokenHandler tokenHandler = new();
                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
                string jwt = tokenHandler.WriteToken(token);

                await xyLog.AsxLog(success);
                return jwt;
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                await xyLog.AsxLog(logError);
                return string.Empty;
            }
        }


        /// <summary>
        /// Validates a JWT using the configured RSA public key and returns the extracted ClaimsPrincipal if valid.
        /// </summary>
        /// <param name="token">The JWT string to be validated.</param>
        /// <param name="validateLifetime">Optional flag to enforce expiration check (default: true).</param>
        /// <returns>
        /// The authenticated <see cref="ClaimsPrincipal"/> if the token is valid; otherwise, <c>null</c>.
        /// </returns>
        /// <exception cref="InvalidOperationException">Thrown if the public key has not been loaded.</exception>
        public static async Task<ClaimsPrincipal?> ValidateJwtAsync(string token, bool validateLifetime = true)
        {
            string initError = "Public RSA key is not loaded. Token validation cannot proceed.";
            string success = "JWT successfully validated.";
            string fail = "JWT validation failed.";

            if (_publicRsaKey == null)
            {
                await xyLog.AsxLog(initError);
                return null;
            }

            JwtSecurityTokenHandler tokenHandler = new ();
            TokenValidationParameters validationParameters = new ()
            {
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(_publicRsaKey),
                ValidateLifetime = validateLifetime,
                ClockSkew = TimeSpan.FromMinutes(2)
            };

            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                await xyLog.AsxLog(success);
                return principal;
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                await xyLog.AsxLog(fail);
                return null;
            }
        }

        /// <summary>
        /// Exports the configured public RSA key as a PEM-formatted string.
        /// </summary>
        /// <returns>
        /// A PEM string containing the public key, or an empty string if the key is not initialized.
        /// </returns>
        public static async Task<string> GetPublicKeyAsPemAsync()
        {
            string noKey = "Public key not loaded – unable to export.";
            string success = "Public key exported as PEM.";
            string pemStart = "-----BEGIN PUBLIC KEY-----";
            string pemEnd = "-----END PUBLIC KEY-----";
            

            if (_publicRsaKey is null)
            {
                await xyLog.AsxExLog(new Exception(noKey));
                return string.Empty;
            }

            try
            {
                byte[] keyBytes = _publicRsaKey.ExportSubjectPublicKeyInfo();
                string base64 = Convert.ToBase64String(keyBytes, Base64FormattingOptions.InsertLineBreaks);

                StringBuilder builder = new();
                builder.AppendLine(pemStart).AppendLine(base64).AppendLine(pemEnd);

                await xyLog.AsxLog(success);
                return builder.ToString();
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                return string.Empty;
            }
        }


    }
}
