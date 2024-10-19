using System.ComponentModel.DataAnnotations;

namespace ShorterUrl.DTOs;

public sealed record LinkInsertRequestDTO
{
    [Required(ErrorMessage = "url is required")]
    public string Url { get; set; } = "";

    public bool IsValid()
    {
        return Uri.IsWellFormedUriString(Url, UriKind.Absolute);
    }
}