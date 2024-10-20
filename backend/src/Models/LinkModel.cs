using System.Text.Json.Serialization;

namespace ShorterUrl.Models;

public class LinkModel
{
    public int Id { get; set; }
    public string ShortCode { get; set; } = "";
    public string Alias { get; set; } = "";
    public string OriginalUrl { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ExpiresAt { get; set; }
    [JsonIgnore]
    public List<ClickModel> Clicks { get; set; } = new();
    public UserModel User { get; set; }
    public int UserId { get; set; }
}
