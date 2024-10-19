namespace ShorterUrl.DTOs;

public sealed record GeneralAnalyticsResponseDTO
{
    public int TotalUrls { get; set; }
    public int TotalClicks { get; set; }
    public Dictionary<string, int> ClicksByLocations { get; set; } = new();
}