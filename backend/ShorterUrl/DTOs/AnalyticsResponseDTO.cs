using ShorterUrl.Models;

namespace ShorterUrl.DTOs;

public sealed record AnalyticsResponseDTO
{
    public string? ShortCode { get; set; }
    public string? OriginalUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ExpiresAt { get; set; }
    public int TotalClicks { get; set; }
    public List<AnalyticsDAO> Clicks { get; set; } = new();
}