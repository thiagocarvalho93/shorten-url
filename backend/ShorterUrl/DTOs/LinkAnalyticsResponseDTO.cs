using ShorterUrl.Models;

namespace ShorterUrl.DTOs;

public sealed record LinkAnalyticsResponseDTO
{
    public string? ShortCode { get; set; }
    public string? OriginalUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ExpiresAt { get; set; }
    public int TotalClicks { get; set; }
    public List<AnalyticsModel> Clicks { get; set; } = new();
}