namespace ShorterUrl.DTOs;

public sealed record AnalyticsOverallResponseDTO
{
    public int TotalUrls { get; set; }
    public int TotalClicks { get; set; }
    public Dictionary<string, int> ClicksByLocations { get; set; } = new();
}