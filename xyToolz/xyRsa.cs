using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz
{


    /// <summary>
    /// Hilfsklasse zur Erzeugung und Validierung von JWTs mit RSA-Schlüsseln.
    /// Wiederverwendbar in verschiedenen Projekten.
    /// </summary>
    public static class xyRsa
    {
        private static RSA? _privateRsa;
        private static RSA? _publicRsa;
        private static string? _issuer;
        private static string? _audience;

        /// <summary>
        /// Initialisiert die internen RSA-Objekte mit PEM-Schlüsseln.
        /// </summary>
        public static async Task LoadKeysAsync(string publicKeyPem, string privateKeyPem)
        {
            const string logSuccess = "RSA-Schlüssel erfolgreich geladen.";
            const string logError = "Fehler beim Laden der RSA-Schlüssel.";

            try
            {
                _privateRsa = RSA.Create();
                _privateRsa.ImportFromPem(privateKeyPem.ToCharArray());

                _publicRsa = RSA.Create();
                _publicRsa.ImportFromPem(publicKeyPem.ToCharArray());

                await xyLog.AsxLog(logSuccess);
            }
            catch (Exception ex)
            {
                await xyLog.AsxLog(logError);
                xyLog.ExLog(ex);
                throw;
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
            const string logError = "PrivateKey nicht geladen – Token kann nicht erstellt werden.";
            const string logSuccess = "JWT erfolgreich erstellt.";
            const string invalidPrivateKey = "PrivateKey nicht geladen.";

            if (_privateRsa == null)
            {
                await xyLog.AsxExLog(new InvalidOperationException(logError));
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

            await xyLog.AsxLog(logSuccess);
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
