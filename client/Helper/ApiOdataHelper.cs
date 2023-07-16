using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Client.Helper
{
    public class ApiOdataHelper : ApiHelper
    {
        public ApiOdataHelper(HttpClient httpClient) : base(httpClient)
        {
        }

        // chỉ cần thằng này là đủ
        public async Task<TResponse?> GetWithODataAsync<TResponse>(string url, IDictionary<string, string> parameters)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };


            var queryString = BuildODataQueryString(parameters);
            var fullUrl = $"{url}?{queryString}";

            var response = await _httpClient.GetAsync(fullUrl);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<TResponse>(stream, options);
        }

        public async Task<TResponse?> GetOneWithODataAsync<TResponse>(string url, string id, IDictionary<string, string> parameters)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var queryString = BuildODataQueryString(parameters);
            var fullUrl = $"{url}/{id}";
            if (queryString != null)
            {
                fullUrl += $"?{queryString}";
            }

            var response = await _httpClient.GetAsync(fullUrl);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<TResponse>(stream, options);
        }

        public async Task<TResponse?> SearchWithODataAsync<TResponse>(string url, string searchTerm, IDictionary<string, string> parameters)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var encodedSearchTerm = Uri.EscapeDataString(searchTerm);
            parameters["$search"] = encodedSearchTerm;

            var queryString = BuildODataQueryString(parameters);
            var fullUrl = $"{url}?{queryString}";

            var response = await _httpClient.GetAsync(fullUrl);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<TResponse>(stream, options);
        }

        public async Task<TResponse?> PostWithODataAsync<TResponse>(string url, Object data)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            string json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<TResponse>(stream, options);
        }

        // multiform-data
        public async Task<TResponse?> PostMultiFormWithODataAsync<TResponse>(string url, object data)
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
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<TResponse>(stream, options);
        }

        public async Task<TResponse?> PutWithODataAsync<TResponse>(string url, string id, Object data)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            string json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{url}/{id}", content);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<TResponse>(stream, options);
        }

        public async Task<TResponse?> DeleteWithODataAsync<TResponse>(string url, string id)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var response = await _httpClient.DeleteAsync($"{url}/{id}");
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<TResponse>(stream, options);
        }


        private string BuildODataQueryString(IDictionary<string, string> parameters)
        {
            if (parameters == null) return null;
            var queryStringParams = new List<string>();
            foreach (var parameter in parameters)
            {
                var encodedKey = Uri.EscapeDataString(parameter.Key);
                var encodedValue = Uri.EscapeDataString(parameter.Value);
                var queryStringParam = $"{encodedKey}={encodedValue}";
                queryStringParams.Add(queryStringParam);
            }

            return string.Join("&", queryStringParams);
        }
    }
}
