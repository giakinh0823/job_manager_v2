using BusinessObject;
using DataAccess.Repository;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using server.Dto.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace server.Utils
{
    public class JwtUtils
    {
        public static AccessTokenResponse GenerateAccessToken(User user)
        {
            var expiresTime = DateTime.Now.AddMinutes(15);
            List<string> roles = user.UserRoles != null ? user.UserRoles.ToList().Select(r => r.Role.Name).ToList() : new List<string?>();
            string accessToken = GenerateToken(expiresTime, GetClaims(true, user.Name, user.Email, roles));
            string refreshToken = GenerateToken(DateTime.Now.AddDays(30), GetClaims(false, user.Name, user.Email, null));

            AccessTokenResponse accessTokenResponse = new AccessTokenResponse
            {
                AccessToken = accessToken,
                ExpiresTime = expiresTime,
                Roles = roles,
                RefreshToken = refreshToken,
            };

            return accessTokenResponse;
        }

        public static RefreshTokenResponse Refreshtoken(string? refreshToken)
        {
            bool isRefreshTokenValid = ValidateToken(refreshToken, false);

            if (isRefreshTokenValid)
            {
                var refreshTokenClaims = GetTokenClaims(refreshToken);
                var refreshTokenEmail = refreshTokenClaims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                IUserRepository userRepository = new UserRepository();
                User? user = userRepository.FindByEmail(refreshTokenEmail);
                if (user == null)
                {
                    throw new ApplicationException("Không tìm thấy thông tin user");
                }

                var expiresTime = DateTime.Now.AddMinutes(15);
                List<string> roles = user.UserRoles != null ? user.UserRoles.ToList().Select(r => r.Role.Name).ToList() : new List<string?>();
                string accessToken = GenerateToken(expiresTime, GetClaims(true, user.Name, user.Email, roles));

                RefreshTokenResponse refreshTokenResponse = new RefreshTokenResponse
                {
                    AccessToken = accessToken,
                    ExpiresTime = expiresTime,
                    Roles = roles,
                };

                return refreshTokenResponse;
            }

            throw new ApplicationException("Refresh token không hợp lệ");
        }

        static string GenerateToken(DateTime expirationTime, IEnumerable<Claim> claims)
        {
            var _config = getConfig();
            var secretKey = _config["Jwt:Key"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: claims,
                expires: expirationTime,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        static bool ValidateToken(string token, bool validateLifetime)
        {
            var _config = getConfig();
            var secretKey = _config["Jwt:Key"];

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = validateLifetime,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out _);
                return true;
            }
            catch
            {
                return false;
            }
        }

        static IEnumerable<Claim> GetClaims(bool includeRoles, string name, string email, ICollection<String>? roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Email, email)
            };

            if (includeRoles && roles != null)
            {
                roles.ToList().FindAll(r => r !=null).ForEach(role =>
                {
                    claims.AddRange(new[]
                    {
                        new Claim(ClaimTypes.Role, role),
                    });
                });
            }

            return claims;
        }

        static JwtSecurityToken GetTokenClaims(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            return jwtToken;
        }


        private static IConfiguration getConfig()
        {
            var _config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json").Build();
            return _config;
        }
    }
}
