using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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
        private static RSA? _privateRsa;
        private static RSA? _publicRsa;
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
            const string success = "RSA keys were successfully loaded.";
            const string noPem = "Provided PEM string is null or empty.";
            const string fail = "Failed to load RSA keys from PEM input.";

            if (string.IsNullOrWhiteSpace(publicKeyPem) || string.IsNullOrWhiteSpace(privateKeyPem))
            {
                await xyLog.AsxLog(noPem);
                return false;
            }

            try
            {
                _privateRsa = RSA.Create();
                _privateRsa.ImportFromPem(privateKeyPem.ToCharArray());

                _publicRsa = RSA.Create();
                _publicRsa.ImportFromPem(publicKeyPem.ToCharArray());

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
        /// Konfiguriert Issuer und Audience für den Token.
        /// </summary>
        public static async Task ConfigureAsync(string issuer, string audience)
        {
            _issuer = issuer;
            _audience = audience;

            string configMessage = $"Konfiguration gesetzt: Issuer={issuer}, Audience={audience}";
            await xyLog.AsxLog(configMessage);
        }

        /// <summary>
        /// Erstellt ein signiertes JWT mit den angegebenen Claims und der Gültigkeitsdauer.
        /// </summary>
        public static async Task<string> GenerateJwtAsync(IDictionary<string, object> claims, TimeSpan validFor)
        {
           string success = "JWT erfolgreich erstellt.";
           string error = "PrivateKey nicht geladen – Token kann nicht erstellt werden.";
    

            if (_privateRsa == null)
            {
                await xyLog.AsxExLog(new InvalidOperationException(error));
                return "";
            }

            var credentials = new SigningCredentials(new RsaSecurityKey(_privateRsa), SecurityAlgorithms.RsaSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _issuer,
                Audience = _audience,
                Expires = DateTime.UtcNow.Add(validFor),
                Claims = claims,
                SigningCredentials = credentials
            };

            JwtSecurityTokenHandler tokenHandler = new ();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string jwt = tokenHandler.WriteToken(token);

            await xyLog.AsxLog(success);
            return jwt;
        }

        /// <summary>
        /// Validiert ein übergebenes JWT. Gibt null zurück, wenn ungültig.
        /// </summary>
        public static async Task<ClaimsPrincipal?> ValidateJwtAsync(string token, bool validateLifetime = true)
        {
            const string logErrorInit = "PublicKey nicht geladen – Tokenvalidierung nicht möglich.";
            const string logSuccess = "JWT erfolgreich validiert.";
            const string logFailure = "Fehler bei der JWT-Validierung.";
            const string invalidPublicKey = "PublicKey nicht geladen.";

            if (_publicRsa == null)
            {
                await xyLog.AsxLog(logErrorInit);
                throw new InvalidOperationException(invalidPublicKey);
            }

            JwtSecurityTokenHandler tokenHandler = new ();
            TokenValidationParameters validationParameters = new() 
            {
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(_publicRsa),
                ValidateLifetime = validateLifetime,
                ClockSkew = TimeSpan.FromMinutes(2)
            };

            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken? validatedToken);
                await xyLog.AsxLog(logSuccess);
                return principal;
            }
            catch (Exception ex)
            {
                await xyLog.AsxLog(logFailure);
                xyLog.ExLog(ex);
                return null;
            }
        }

        /// <summary>
        /// Gibt den öffentlichen Schlüssel im PEM-Format zurück.
        /// </summary>
        public static async Task<string> GetPublicKeyAsPemAsync()
        {
            const string logError = "PublicKey nicht geladen – kein PEM export möglich.";
            const string logSuccess = "PublicKey PEM exportiert.";
            const string pemBegin = "-----BEGIN PUBLIC KEY-----";
            const string pemEnd = "-----END PUBLIC KEY-----";

            if (_publicRsa == null)
            {
                await xyLog.AsxExLog(new Exception(logError));
                return "";
            }
            else
            {
                var builder = new StringBuilder();
                builder.AppendLine(pemBegin);
                builder.AppendLine(Convert.ToBase64String(_publicRsa.ExportSubjectPublicKeyInfo(), Base64FormattingOptions.InsertLineBreaks));
                builder.AppendLine(pemEnd);
                
                await xyLog.AsxLog(logSuccess);
                return builder.ToString();
            }


        }

    }
}
