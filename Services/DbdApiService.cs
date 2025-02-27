using DbdRandomazoAPI.Models;

namespace DbdRandomazoAPI.Services
{
    public class DbdApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public DbdApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = "https://dbd-api.herokuapp.com"; // Default URL if not in configuration
        }

        public async Task<List<Killer>> GetAllKillersAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Killer>>($"{_baseUrl}/killers");
            return response;
        }

        public async Task<List<Perk>> GetKillerPerksAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Perk>>($"{_baseUrl}/perks?role=killer");
            return response;
        }

        public async Task<List<Addon>> GetKillerAddonsAsync(string killerName)
        {
            var response = await _httpClient.GetFromJsonAsync<List<Addon>>($"{_baseUrl}/addons?killer={killerName}");
            return response;
        }
    }
}
