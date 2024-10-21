using System.Text.Json;

namespace ShorterUrl.Service;

public class LocationService
{
    private readonly HttpClient _httpClient;
    private const string ApiUrl = "https://ipinfo.io/{0}/json";

    public LocationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetLocationAsync(string ip)
    {
        if (string.IsNullOrEmpty(ip))
            return "IP address not provided.";

        var response = await _httpClient.GetStringAsync(string.Format(ApiUrl, ip));

        var jsonDoc = JsonDocument.Parse(response);

        var root = jsonDoc.RootElement;
        var city = root.GetProperty("city").GetString();
        var region = root.GetProperty("region").GetString();
        var country = root.GetProperty("country").GetString();

        return $"{city}, {region}, {country}";
    }
}