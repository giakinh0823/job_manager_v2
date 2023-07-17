using client.Common;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.Helper
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ErrorResponse
    {
        public string Message { get; set; }
    }

    public class ApiHelper
    {
        private readonly AccessTokenManager _accessTokenManager;
        private readonly HttpClient _httpClient;

        public ApiHelper(AccessTokenManager accessTokenManager)
        {
            _httpClient = new HttpClient();
            _accessTokenManager = accessTokenManager;
        }

      
        public async Task<ApiResponse<TResponse>> GetAsync<TResponse>(string url, bool? isAuthen)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if(isAuthen != null & isAuthen == true)
            {
                string accessToken = await _accessTokenManager.GetAccessTokenAsync();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                var data = await JsonSerializer.DeserializeAsync<TResponse>(stream, options);
                return new ApiResponse<TResponse> { IsSuccess = true, Data = data };
            }
            else
            {
                using var errorStream = await response.Content.ReadAsStreamAsync();
                var errorResponse = await JsonSerializer.DeserializeAsync<ErrorResponse>(errorStream, options);
                return new ApiResponse<TResponse> { IsSuccess = false, ErrorMessage = errorResponse?.Message };
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

        // Multiform-data
        public async Task<ApiResponse<TResponse>> PostMultiFormAsync<TResponse>(string url, object data)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using var content = new MultipartFormDataContent();

            foreach (var property in data.GetType().GetProperties())
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(data)?.ToString();

                if (!string.IsNullOrEmpty(propertyName) && !string.IsNullOrEmpty(propertyValue))
                {
                    content.Add(new StringContent(propertyValue), propertyName);
                }
            }

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

        public async Task<ApiResponse<TResponse>> PutAsync<TResponse>(string url, object data)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            string json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, content);

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

        public async Task<ApiResponse<TResponse>> DeleteAsync<TResponse>(string url)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var response = await _httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                var data = await JsonSerializer.DeserializeAsync<TResponse>(stream, options);
                return new ApiResponse<TResponse> { IsSuccess = true, Data = data };
            }
            else
            {
                using var errorStream = await response.Content.ReadAsStreamAsync();
                var errorResponse = await JsonSerializer.DeserializeAsync<ErrorResponse>(errorStream, options);
                return new ApiResponse<TResponse> { IsSuccess = false, ErrorMessage = errorResponse?.Message };
            }
        }

        public async Task<ApiResponse<TResponse>> SearchAsync<TResponse>(string url, IDictionary<string, string> parameters)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var queryString = new FormUrlEncodedContent(parameters).ReadAsStringAsync().Result;
            var fullUrl = $"{url}?{queryString}";

            var response = await _httpClient.GetAsync(fullUrl);

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                var data = await JsonSerializer.DeserializeAsync<TResponse>(stream, options);
                return new ApiResponse<TResponse> { IsSuccess = true, Data = data };
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
