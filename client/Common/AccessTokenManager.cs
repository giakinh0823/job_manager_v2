using client.Pages.Config;
using Client.Helper;
using server.Dto.Auth;
using server.Utils;
using System.Text;
using System.Text.Json;

namespace client.Common
{
    public class AccessTokenManager
    {
        private const string AccessTokenKey = "AccessToken";
        private const string RefreshTokenKey = "RefreshToken";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ServerConfig _serverConfig;
        private readonly HttpClient _httpClient;

        public AccessTokenManager(IHttpContextAccessor httpContextAccessor, ServerConfig serverConfig)
        {
            _httpContextAccessor = httpContextAccessor;
            _serverConfig = serverConfig;
            _httpClient = new HttpClient();
        }

        public async Task<string> GetAccessTokenAsync()
        {
            string accessToken = _httpContextAccessor.HttpContext.Session.GetString(AccessTokenKey);

            if (string.IsNullOrEmpty(accessToken) || !JwtUtils.ValidateToken(accessToken, true))
            {
                string refreshToken = _httpContextAccessor.HttpContext.Session.GetString(RefreshTokenKey);

                if (!string.IsNullOrEmpty(refreshToken))
                {
                    RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest
                    {
                        RefreshToken = refreshToken,
                    };

                    ApiResponse<RefreshTokenResponse> apiResponse = await PostAsync<RefreshTokenResponse>($"{_serverConfig.Domain}/refresh", refreshTokenRequest);
                    accessToken = apiResponse.Data.AccessToken;
                }
                _httpContextAccessor.HttpContext.Session.SetString(AccessTokenKey, accessToken);
            }
            return accessToken;
        }

        public async Task<ApiResponse<TResponse>> PostAsync<TResponse>(string url, object data)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            string json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                var responseData = await JsonSerializer.DeserializeAsync<TResponse>(stream, options);
                return new ApiResponse<TResponse> { IsSuccess = true, Data = responseData };
            }
            else
            {
                using var errorStream = await response.Content.ReadAsStreamAsync();
                var errorResponse = await JsonSerializer.DeserializeAsync<ErrorResponse>(errorStream, options);
                return new ApiResponse<TResponse> { IsSuccess = false, ErrorMessage = errorResponse?.Message };
            }
        }
    }
}
