using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using client.Common;
using Client.Helper;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using client.Pages.Config;
using server.Utils;
using server.Dto.Auth;

namespace client.Config.Security
{
    public class FptAuthorize : Attribute, IAuthorizationFilter
    {

        private const string AccessTokenKey = "AccessToken";
        private const string RefreshTokenKey = "RefreshToken";
        private readonly ServerConfig _serverConfig;
        private readonly HttpClient _httpClient;

        public FptAuthorize()
        {
            _serverConfig = new ServerConfig();
            _httpClient = new HttpClient();
        }
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            string? accessToken = context.HttpContext.Session.GetString(AccessTokenKey);

            if (string.IsNullOrEmpty(accessToken) || !JwtUtils.ValidateToken(accessToken, true))
            {
                string refreshToken = context.HttpContext.Session.GetString(RefreshTokenKey);

                if (!string.IsNullOrEmpty(refreshToken))
                {
                    RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest
                    {
                        RefreshToken = refreshToken,
                    };

                    ApiResponse<RefreshTokenResponse> apiResponse = await PostAsync<RefreshTokenResponse>($"{_serverConfig.Domain}/refresh", refreshTokenRequest);
                    accessToken = apiResponse.Data.AccessToken;
                    context.HttpContext.Session.SetString(AccessTokenKey, accessToken);
                }
                else
                {
                    context.Result = new RedirectToPageResult("/Auth/Login");
                }
            }
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
