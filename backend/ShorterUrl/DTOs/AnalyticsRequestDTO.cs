namespace ShorterUrl.DTOs;

public sealed record AnalyticsRequestDTO
{
    public string IpAdress { get; set; } = "";
    public string UserAgent { get; set; } = "";
    public string Location { get; set; } = "";
    public string Referrer { get; set; } = "";

    public AnalyticsRequestDTO() { }
    public AnalyticsRequestDTO(HttpContext httpContext)
    {
        IpAdress = httpContext.Connection?.RemoteIpAddress?.ToString() ?? "";
        UserAgent = httpContext.Request.Headers["User-Agent"].ToString();
        Referrer = httpContext.Request.Headers["Referer"].ToString();
    }
}