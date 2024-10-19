namespace ShorterUrl.Models;

public class ShortUrlDAO
{
    public int Id { get; set; }
    public string Token { get; set; } = "";
    public string Url { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ExpiresAt { get; set; }
}
