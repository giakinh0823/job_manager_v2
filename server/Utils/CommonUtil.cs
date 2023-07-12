using Microsoft.AspNetCore.Authentication;
using server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace server.Utils
{
    public class CommonUtil
    {
        public static AccessTokenPayload GetPayload(HttpRequest request) {
            string? accessToken = GetTokenFromRequest(request);
            if (accessToken == null) return null;

            JwtSecurityToken jwtSecurityToken = JwtUtils.GetTokenClaims(accessToken);
            string? name = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
            string? email = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
            string? id = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            ICollection<string> roles = jwtSecurityToken.Claims.Where(claim => claim.Type == ClaimTypes.Role).Select(claim => claim.Value).ToList();

            var payload = new AccessTokenPayload();
            payload.UserId = id !=null ? int.Parse(id): null;
            payload.Name = name;
            payload.Email = email;
            payload.Roles = roles;
            return payload;
        }


        public static string? GetTokenFromRequest(HttpRequest request)
        {
            if (request.Headers.ContainsKey("Authorization"))
            {
                string authorizationHeader = request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
                {
                    string token = authorizationHeader.Substring("Bearer ".Length);
                    return token;
                }
            }
            return null;
        }
    }
}
