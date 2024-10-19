namespace ShorterUrl.Models;

public class ClickModel
{
    public int Id { get; set; }
    public int LinkId { get; set; }
    public LinkModel LinkModel { get; set; }
    public DateTime ClickDate { get; set; } = DateTime.Now;
    public string IpAdress { get; set; }
    public string UserAgent { get; set; }
    public string Location { get; set; }
    public string Referrer { get; set; }
}