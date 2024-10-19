namespace ShorterUrl.Models;

public class AnalyticsDAO
{
    public int Id { get; set; }
    public ShortUrlDAO ShortUrlDAO { get; set; }
    public DateTime ClickDate { get; set; } = DateTime.Now;
    public string IpAdress { get; set; }
    public string UserAgent { get; set; }
    public string Location { get; set; }
    public string Referrer { get; set; }
}