namespace ShorterUrl.Models;

public class ShortUrlDAO
{
    public int Id { get; set; }
    public string ShortCode { get; set; } = "";
    public string OriginalUrl { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ExpiresAt { get; set; }
}
