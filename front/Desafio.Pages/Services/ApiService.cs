using System.Text;
using System.Text.Json;

namespace Desafio.Pages.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _baseUrl = "http://localhost:5049";
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}{endpoint}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                });
            }
            return default;
        }
        catch
        {
            return default;
        }
    }

    public async Task<HttpResponseMessage> PostAsync(string endpoint, object payload)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(payload), 
            Encoding.UTF8, 
            "application/json");
        
        return await _httpClient.PostAsync($"{_baseUrl}{endpoint}", content);
    }

    public async Task<HttpResponseMessage> PutAsync(string endpoint, object payload)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(payload), 
            Encoding.UTF8, 
            "application/json");
        
        return await _httpClient.PutAsync($"{_baseUrl}{endpoint}", content);
    }

    public async Task<string> GetErrorMessageAsync(HttpResponseMessage response)
    {
        try
        {
            var body = await response.Content.ReadAsStringAsync();
            return ErrorHandlingService.GetUserFriendlyMessage(body, (int)response.StatusCode);
        }
        catch
        {
            return ErrorHandlingService.GetUserFriendlyMessage(string.Empty, (int)response.StatusCode);
        }
    }
}
