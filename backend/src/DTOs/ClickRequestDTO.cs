namespace ShorterUrl.DTOs;

public sealed record ClickRequestDTO
{
    public string IpAdress { get; set; } = "";
    public string UserAgent { get; set; } = "";
    public string Location { get; set; } = "";
    public string Referrer { get; set; } = "";
    public string DeviceLanguage { get; set; } = "";

    public ClickRequestDTO() { }
    public ClickRequestDTO(HttpContext httpContext)
    {
        IpAdress = httpContext.Connection?.RemoteIpAddress?.ToString() ?? "";
        UserAgent = httpContext.Request.Headers["User-Agent"].ToString();
        Referrer = httpContext.Request.Headers["Referer"].ToString();
        DeviceLanguage = httpContext.Request.Headers["Accept-Language"].ToString().Split(',').FirstOrDefault() ?? "Unknown";
    }
}