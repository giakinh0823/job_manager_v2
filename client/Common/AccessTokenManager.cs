using client.Pages.Config;
using Client.Helper;
using server.Dto.Auth;
using server.Utils;

namespace client.Common
{
    public class AccessTokenManager
    {
        private const string AccessTokenKey = "AccessToken";
        private const string RefreshTokenKey = "RefreshToken";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ServerConfig _serverConfig;
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly ApiHelper _apiHelper = new ApiHelper(_httpClient);

        public AccessTokenManager(IHttpContextAccessor httpContextAccessor, ServerConfig serverConfig)
        {
            _httpContextAccessor = httpContextAccessor;
            _serverConfig = serverConfig;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            string accessToken = _httpContextAccessor.HttpContext.Session.GetString(AccessTokenKey);
            string refreshToken = _httpContextAccessor.HttpContext.Session.GetString(RefreshTokenKey);

            if (string.IsNullOrEmpty(accessToken) || !JwtUtils.ValidateToken(accessToken, true))
            {
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest
                    {
                        RefreshToken = refreshToken,
                    };

                    ApiResponse<RefreshTokenResponse> apiResponse = await _apiHelper.PostAsync<RefreshTokenResponse>($"{_serverConfig.Domain}/refresh", refreshTokenRequest);
                    accessToken = apiResponse.Data.AccessToken;
                }
                _httpContextAccessor.HttpContext.Session.SetString(AccessTokenKey, accessToken);
            }
            return accessToken;
        }
    }
}
