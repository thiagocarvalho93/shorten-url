namespace ShorterUrl.Models;

public class ShortenUrl
{
    public int Id { get; set; }
    public string Token { get; set; }
    public string LongUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}
